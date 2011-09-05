using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace MindHarbor.GenClassLib.Data
{
	/// <summary>
	/// This storage wrapper can be used as static field and store data in Session, it's key safe and it can be used outside of the HttpContext
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SessionData<T>
	{
		[ThreadStatic]
		private static IDictionary context;

		private readonly Guid gid = Guid.NewGuid();

		private IDictionary Context
		{
			get
			{
				if (context == null)
				{
					context = new Hashtable();
				}
				return context;
			}
		}

		private bool InHttpContext {
			get { return HttpContext.Current != null; }
		}

		private HttpSessionState Session {
			get { return HttpContext.Current.Session; }
		}

		private bool Exists {
			get {
				if(InHttpContext)
					return  Session[gid.ToString()] != null;
				return Context.Contains(gid);
					
			}
		} 

		public T Value
		{
			get
			{
				if (Exists)
				{
					if (InHttpContext)
						return (T)  Session[gid.ToString()];
					return (T)Context[gid];
				}
				else
				{
					return default(T);
				}
			}
			set
			{
				if (InHttpContext)
					 Session[gid.ToString()] = value;
				else
					 Context[gid] = value;
			}
		}
	}
}
