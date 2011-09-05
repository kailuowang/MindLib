using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using MindHarbor.URLRewriter.Config;

namespace MindHarbor.URLRewriter
{
    public class URLRewriterModule:IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication) sender;
            Rewrite(app);
        }

        #endregion


        /// <summary>
        /// This method is called during the module's BeginRequest event.
        /// </summary>
        /// <param name="app">The HttpApplication instance.</param>
        private void Rewrite(System.Web.HttpApplication app)
        {
            RewriterRuleCollection rules = RewriterConfiguration.GetConfig().Rules;
            for (int i = 0; i < rules.Count; i++){
                string url;
                if(IsMatch(rules[i].Redirection.Pattern,rules[i].Redirection.Replace,app,out url)) {
                    app.Response.Redirect(url);
                }

                if (IsMatch(rules[i].Rewriter.Pattern, rules[i].Rewriter.Replace, app,out url))
                {
                    RewriterUtils.RewriteUrl(app.Context, url);
                    break;
                }
            }
            
        }

        private bool IsMatch(string pattern,string replace, HttpApplication app,out string url) {
            string resovePattern= "^" + RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, pattern) + "$";
            Regex regex= new Regex(resovePattern, RegexOptions.IgnoreCase);
            url = RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, regex.Replace(app.Request.Url.PathAndQuery, replace));
            return regex.IsMatch(app.Request.Url.PathAndQuery);
        }
    }
}
