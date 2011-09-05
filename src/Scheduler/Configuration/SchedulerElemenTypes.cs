using System;
using System.Configuration;
using MindHarbor.SingletonUtil;

namespace MindHarbor.Scheduler.Configuration {
	public class SchedulerNameElement : ConfigurationElement {
		/// <summary>
		/// 
		/// </summary>
		[ConfigurationProperty("value",
			DefaultValue = "Scheduler",
			IsRequired = true,
			IsKey = true)]
		public string Value {
			get { return (string) this["value"]; }
			set { this["value"] = value; }
		}
	}
	public class ErrorAlertFromElement : ConfigurationElement
	{
		/// <summary>
		/// 
		/// </summary>
		[ConfigurationProperty("value",
			DefaultValue = "")]
		public string Value {
			get { return (string) this["value"]; }
			set { this["value"] = value; }
		}
	}
	
	public class ErrorAlertEmailsElement : ConfigurationElement {
		/// <summary>
		/// 
		/// </summary>
		[ConfigurationProperty("value",
			DefaultValue = "",
			IsRequired = false,
			IsKey = false)]
		public string Value {
			get { return (string) this["value"]; }
			set { this["value"] = value; }
		}

		public string[] Emails {
			get { return string.IsNullOrEmpty(Value) ? new string[0] : Value.Split(new char[] {',', ' ', ';'}); }
		}
	}

	public class InterceptorElement : ConfigurationElement {
		/// <summary>
		/// 
		/// </summary>
		[ConfigurationProperty("name",
			DefaultValue = "",
			IsRequired = true,
			IsKey = true)]
		public string Name {
			get { return (string) this["name"]; }
			set { this["name"] = value; }
		}

		public IInterceptor Interceptor {
			get {
				if (string.IsNullOrEmpty(Name)) return null;
				return (IInterceptor) SingletonInstanceLoader.Load(Type.GetType(Name, true));
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class TaskElement : ConfigurationElement {
		/// <summary>
		/// 
		/// </summary>
		[ConfigurationProperty("name",
			DefaultValue = "",
			IsRequired = true,
			IsKey = true)]
		public string Name {
			get { return (string) this["name"]; }
			set { this["name"] = value; }
		}

		[ConfigurationProperty("startImmediately",
			DefaultValue = "true",
			IsRequired = false,
			IsKey = false)]
		public bool StartImmediately {
			get { return (bool) this["startImmediately"]; }
			set { this["startImmediately"] = value; }
		}

		/// <summary>
		/// whether or not use <see cref="IInterceptor"/>
		/// </summary>
		/// <remarks>
		/// if set to true then will use the <see cref="InterceptorElement"/> or the Scheduler's DefaultInterceptor
		/// </remarks>
		[ConfigurationProperty("useInterceptor",
			DefaultValue = "true",
			IsRequired = false,
			IsKey = false)]
		public bool UserInterceptor {
			get { return (bool) this["useInterceptor"]; }
			set { this["useInterceptor"] = value; }
		}

		[ConfigurationProperty("interceptor",
			IsDefaultCollection = false)]
		public InterceptorElement InterceptorElement {
			get { return (InterceptorElement) base["interceptor"]; }
		}

		public ITask Task {
			get { return (ITask) SingletonInstanceLoader.Load(TaskType); }
		}

		public Type TaskType {
			get { return Type.GetType(Name, true); }
		}
	}
}