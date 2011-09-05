using System;
using System.Web;
using System.Web.Caching;
using System.Configuration;
using System.Xml.Serialization;

namespace MindHarbor.URLRewriter.Config
{
	/// <summary>
	/// Specifies the configuration settings in the Web.config for the RewriterRule.
	/// </summary>
	/// <remarks>This class defines the structure of the Rewriter configuration file in the Web.config file.
	/// Currently, it allows only for a set of rewrite rules; however, this approach allows for customization.
	/// For example, you could provide a ruleset that <i>doesn't</i> use regular expression matching; or a set of
	/// constant names and values, which could then be referenced in rewrite rules.
	/// <p />
	/// The structure in the Web.config file is as follows:
	/// <code>
	/// &lt;configuration&gt;
	/// 	&lt;configSections&gt;
	/// 		&lt;section name="RewriterConfig" 
	/// 		            type="URLRewriter.Config.URLRewriterSerializerSectionHandler, URLRewriter" /&gt;
	///		&lt;/configSections&gt;
	///		
	///		&lt;RewriterConfig&gt;
	///			&lt;Rules&gt;
	///				&lt;RewriterRule&gt;
    ///                 &lt;Rewriter&gt;
    ///					    &lt;Pattern&gt;<i>pattern</i>&lt;/Pattern&gt;
    ///					    &lt;Replace&gt;<i>replace with</i>&lt;/Replace&gt;
    ///                 &lt;/Rewriter&gt;
    ///                 &lt;Redirection&gt;
    ///					    &lt;Pattern&gt;<i>pattern</i>&lt;/Pattern&gt;
    ///					    &lt;Replace&gt;<i>replace with</i>&lt;/Replace&gt;
    ///                 &lt;/Redirection&gt;
	///				&lt;/RewriterRule&gt;
    ///				&lt;RewriterRule&gt;
    ///                 &lt;Rewriter&gt;
    ///					    &lt;Pattern&gt;<i>pattern</i>&lt;/Pattern&gt;
    ///					    &lt;Replace&gt;<i>replace with</i>&lt;/Replace&gt;
    ///                 &lt;/Rewriter&gt;
    ///                 &lt;Redirection&gt;
    ///					    &lt;Pattern&gt;<i>pattern</i>&lt;/Pattern&gt;
    ///					    &lt;Replace&gt;<i>replace with</i>&lt;/Replace&gt;
    ///                 &lt;/Redirection&gt;
    ///				&lt;/RewriterRule&gt;
	///				...
    ///				&lt;RewriterRule&gt;
    ///                 &lt;Rewriter&gt;
    ///					    &lt;Pattern&gt;<i>pattern</i>&lt;/Pattern&gt;
    ///					    &lt;Replace&gt;<i>replace with</i>&lt;/Replace&gt;
    ///                 &lt;/Rewriter&gt;
    ///                 &lt;Redirection&gt;
    ///					    &lt;Pattern&gt;<i>pattern</i>&lt;/Pattern&gt;
    ///					    &lt;Replace&gt;<i>replace with</i>&lt;/Replace&gt;
    ///                 &lt;/Redirection&gt;
    ///				&lt;/RewriterRule&gt;
	///			&lt;/Rules&gt;
	///		&lt;/RewriterConfig&gt;
	///		
	///		&lt;system.web&gt;
	///			...
	///		&lt;/system.web&gt;
	///	&lt;/configuration&gt;
	/// </code>
	/// </remarks>
	[Serializable()]
	[XmlRoot("URLRewriter")]
	public class RewriterConfiguration
	{
		private RewriterRuleCollection rules;

		/// <summary>
		/// GetConfig() returns an instance of the <b>RewriterConfiguration</b> class with the values populated from
		/// the Web.config file.  It uses XML deserialization to convert the XML structure in Web.config into
		/// a <b>RewriterConfiguration</b> instance.
		/// </summary>
		/// <returns>A <see cref="RewriterConfiguration"/> instance.</returns>
		public static RewriterConfiguration GetConfig()
		{
            if (HttpContext.Current.Cache["URLRewriter"] == null)
                HttpContext.Current.Cache.Insert("URLRewriter", ConfigurationSettings.GetConfig("URLRewriter"));

            return (RewriterConfiguration)HttpContext.Current.Cache["URLRewriter"];
		}

		#region Public Properties
		/// <summary>
		/// A <see cref="RewriterRuleCollection"/> instance that provides access to a set of <see cref="RewriterRule"/>s.
		/// </summary>
		public RewriterRuleCollection Rules
		{
			get
			{
				return rules;
			}
			set
			{
				rules = value;
			}
		}
		#endregion
	}
}
