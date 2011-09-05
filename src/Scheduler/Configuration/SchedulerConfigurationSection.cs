using System;
using System.Configuration;

namespace MindHarbor.Scheduler.Configuration {
	/// <summary>
	/// Root Section for MindHarbor.DomainTemplate Configuration
	/// </summary>
	public class SchedulerConfigurationSection : ConfigurationSection {
		/// <summary>
		/// 
		/// </summary>
		public const string SectionName = "MindHarbor.Scheduler";

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// Declare a collection element represented 
		/// in the configuration file by the sub-section
		/// <![CDATA[
		/// <urls> <add .../> </urls> 
		/// Note: the "IsDefaultCollection = false" 
		/// instructs the .NET Framework to build a nested 
		/// section like <urls> ...</urls>.
		/// ]]>
		/// </remarks>
		[ConfigurationProperty("tasks",
			IsDefaultCollection = false)]
		public TaskElementCollection TaskElements {
			get { return (TaskElementCollection) base["tasks"]; }
		}

		[ConfigurationProperty("schedulerName",
			IsDefaultCollection = false, IsRequired = true)]
		public SchedulerNameElement SchedulerNameElement {
			get { return (SchedulerNameElement) base["schedulerName"]; }
		}

		[ConfigurationProperty("defaultInterceptor",
			IsDefaultCollection = false)]
		public InterceptorElement DefaultInterceptorElement {
			get { return (InterceptorElement) base["defaultInterceptor"]; }
		}
		
		[ConfigurationProperty("errorAlertEmails",
			IsDefaultCollection = false)]
		public ErrorAlertEmailsElement ErrorAlertEmails
		{
			get { return (ErrorAlertEmailsElement)base["errorAlertEmails"]; }
		}
		
		[ConfigurationProperty("errorAlertFrom",
			IsDefaultCollection = false)]
		public ErrorAlertEmailsElement ErrorAlertFrom
		{
			get { return (ErrorAlertEmailsElement)base["errorAlertFrom"]; }
		}

		public IInterceptor DefaultInterceptor {
			get { return DefaultInterceptorElement.Interceptor; }
		}

		public string SchedulerName {
			get { return SchedulerNameElement.Value; }
		}

		/// <summary>
		/// Get the instance from the current application's config file
		/// </summary>
		/// <returns></returns>
		public static SchedulerConfigurationSection GetInstance() {
			SchedulerConfigurationSection section =
				ConfigurationManager.GetSection(SectionName) as SchedulerConfigurationSection;
			//comment out by kai to allow scheduler running without this section.
			//if (section == null)
			//    throw new SchedulerException("Section \"" + SectionName + "\" is not found in the configuration file");
			return section;
		}

		public TaskElement FindTaskElementByType(Type taskType) {
			foreach (TaskElement te in TaskElements)
				if (taskType.Equals(te.TaskType))
					return te;
			return null;
		}
	}
}