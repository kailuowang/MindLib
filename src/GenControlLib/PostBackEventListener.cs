using System.Web.UI.WebControls;

namespace MindHarbor.GenControlLib {
	public class PostBackEventListener : LinkButton {
		private const string DefaultArgPrefix = "PostBackEventListenerEventArgs";

		public string EventArgPrefix {
			get {
				if (ViewState["EventArgPrefix"] == null)
					ViewState["EventArgPrefix"] = DefaultArgPrefix;
				return (string) ViewState["EventArgPrefix"];
			}
			set { ViewState["EventArgPrefix"] = value; }
		}

		public event CommandEventHandler PostBackCatched;

		protected override void RaisePostBackEvent(string eventArgument) {
			if (!string.IsNullOrEmpty(eventArgument) &&
			    eventArgument.IndexOf(EventArgPrefix) == 0 && PostBackCatched != null)
				PostBackCatched(this, new CommandEventArgs(CommandName, eventArgument.Replace(EventArgPrefix, string.Empty)));
		}

		public string PostBackJSCode(string commandArg) {
			return Page.ClientScript.GetPostBackEventReference(
				this, EventArgPrefix + commandArg);
		}
	}
}