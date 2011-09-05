using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using log4net;
using MindHarbor.GenClassLib.MiscUtil;

namespace MindHarbor.GenControlLib.MiscUtil {
	/// <summary>
	/// Handle the http error by logging the error msg into EventLog as well as sending it to a list of Emails
	/// </summary>
	/// <remarks>
	/// The distribution list should be specified with the key "HttpApplicationErrorMonitorEmails" in the appsetting section in the .config file.
	/// Emails should be seperate by either comma, space or semicolon.
	/// You can set the From address of the Email report by setting "HttpApplicationErrorReporterEmail" in the appsetting section in the .config file
	/// Sample Configuration File:
	/// <example>
	/// <code>
	/// <![CDATA[
	/// <appSettings>
	///    <add key="MindHarbor.HttpApplicationErrorMonitorEmails" value="Maintainer@SoftwareCompany.com,Manager@ClientCompany.com"/>
	///    <add key="MindHarbor.HttpApplicationErrorReporterEmail" value="AuthenticatedUser@ClientCompany.com"/> 
	///    <add key="MindHarbor.HttpApplicationErrorIgnoredExceptions" value="System.FormatException"/> 
	/// </appSettings>
	/// <system.net>
	///   <mailSettings>
	///        <smtp deliveryMethod="Network">
	///            <network host="mailking.MindHarbor.com" port="25" defaultCredentials="false"/>
	///        </smtp>
	///    </mailSettings>
	/// </system.net>
	/// <system.web>
	///    <httpModules>
	///        <add name="ErrorHandlerHTTPModule" type="MindHarbor.GenControlLib.MiscUtil.ErrorHandlerHTTPModule,MindHarbor.GenControlLib"/>
	///    </httpModules>
	///    <customErrors mode="RemoteOnly" defaultRedirect="GenericErrorPage.htm">
	///        <error statusCode="403" redirect="NoAccess.htm"/>
	///        <error statusCode="404" redirect="FileNotFound.htm"/>
	///    </customErrors>
	/// </system.web>
	/// ]]>
	/// </code>
	/// </example>
	/// </remarks>
    public class ErrorHandlerHTTPModule : IHttpModule
    {
		private string[] ignoredExceptions;

		/// <summary>
        /// The key of the  setting of the distribution list of Emails to report 
        /// </summary>
        public const string ErrorDistributionEmailListSettingName = "MindHarbor.HttpApplicationErrorMonitorEmails";

        /// <summary>
        /// The key of the setting of the Ignored Exceptions
        /// </summary>
        public const string IgnoredExceptionsCfgKey = "MindHarbor.HttpApplicationErrorIgnoredExceptions";

        /// <summary>
        ///  The key of the  setting of the Email report from
        /// </summary>
        public const string ReportFromSettingName = "MindHarbor.HttpApplicationErrorReporterEmail";

        #region IHttpModule Members

        public void Init(HttpApplication app)
        {
            //  if (!Debugger.IsAttached)
            app.Error += new EventHandler(PostBackHandleError);
            app.PostMapRequestHandler +=
                    new EventHandler(context_PostMapRequestHandler);
        }

        void context_PostMapRequestHandler(object sender, EventArgs e)
        {

            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
                page.Error += new EventHandler(AjaxHandleError);

        }

        public void Dispose() { }

        #endregion


        private void HandleError()
        {
            HttpContext ctx = HttpContext.Current;
            Exception objErr = ctx.Server.GetLastError().GetBaseException();
        	if (ExceptionShouldBeIgnored(objErr))
                return;
            string err = "Error(s) Caught in Application_Error event at " + DateTime.Now + "\n" +
                         "Error in: " + ctx.Request.Url.ToString() + "\n" + BuildClientRequetInfo() + "\n"+
						 BuildExceptionMessage(objErr);

            try
            {
                EventLog.WriteEntry(ConfigHelperBase.RootPath(), err, EventLogEntryType.Error);
            }
            catch (Exception) {} 
			
			try
            {
            	Logger().Error(err,objErr);
            }
            catch (Exception) {}
			
			try
            {
				EmailError(err);
            }
            catch (Exception e) {
            	Logger().Error("Failed to sent Error notification", e);	
            } 

          
        }

		private ILog Logger() {
			return log4net.LogManager.GetLogger("MindHarbor.GenControlLib.ErrorHandlerHTTPModule.Logger");
		}

		/// <summary>
        /// Handle when error occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjaxHandleError(object sender, EventArgs e)
        {
            HandleError();
        }

        /// <summary>
        /// Handle when error occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PostBackHandleError(object sender, EventArgs e)
        {
            if (HttpContext.Current.Handler is Page) return;
            HandleError();
        }

        private bool ExceptionShouldBeIgnored(Exception objErr)
        {
			InitIgnoredExceptions();
        	foreach (string s in ignoredExceptions)
                if (!string.IsNullOrEmpty(s)&&s.Trim().Length>0&&objErr.GetType().FullName.Contains(s))
                    return true;
            return false;
        }

		private void InitIgnoredExceptions() {
			if(ignoredExceptions == null) {
				string ignoredExceptionNames = "System.Web.HttpException;" 
												+ ConfigHelperBase.GetSetting(IgnoredExceptionsCfgKey, string.Empty);
				ignoredExceptions = ignoredExceptionNames.Split(new char[] { ';' });
			}
		}


		private string BuildExceptionMessage(Exception e)
        {
            StringBuilder message =
                new StringBuilder("\r\n\r\nUnhandledException logged by UnhandledExceptionModule.dll:\r\n\r\n");
            for (Exception currentException = e;
                 currentException != null;
                 currentException = currentException.InnerException)
                message.AppendFormat("\r\n\r\ntype={0}\r\n\r\nmessage={1}\r\n\r\nstack=\r\n{2}\r\n\r\n",
                                     currentException.GetType().FullName,
                                     currentException.Message,
                                     currentException.StackTrace);
            return message.ToString();
        }

        private string BuildClientRequetInfo()
        {
            HttpRequest request = HttpContext.Current.Request;
            string info =
                string.Format("IP:{0}\r\nBrower:{1}", request.UserHostAddress,
                              request.UserAgent);
            if (HttpContext.Current.User != null
                && HttpContext.Current.User.Identity != null
                && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                info += "\nUser identity:" + HttpContext.Current.User.Identity.Name;
            return info;
        }

        private void EmailError(string err)
        {
            string distributionList = ConfigHelperBase.GetSetting(ErrorDistributionEmailListSettingName);
            if (String.IsNullOrEmpty(distributionList)) return;

            SmtpClient smtp = new SmtpClient();
            if (string.IsNullOrEmpty(smtp.Host)) return;

            try
            {
                foreach (string s in distributionList.Split(new char[] { ',', ' ', ';' }))
                    smtp.Send(ErrorMessageFromEmail(), s, "An error occured on " + ConfigHelperBase.RootPath(), err);
            }
            catch (Exception e)
            {
                //Catch the exception so that sending error notification won't affect other error handeling logic
            }
        }

        private string ErrorMessageFromEmail()
        {
            string retVal = ConfigHelperBase.GetSetting(ReportFromSettingName);
            if (!string.IsNullOrEmpty(retVal))
                return retVal;
            return "HttpErrorHandler@" + HttpContext.Current.Request.Url.Host;
        }
    }
}