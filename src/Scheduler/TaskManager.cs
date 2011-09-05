using System.Collections.Generic;
using System.Collections.ObjectModel;
using MindHarbor.Scheduler.Configuration;
using MindHarbor.Scheduler.Exceptions;

namespace MindHarbor.Scheduler {
	/// <summary>
	/// A class for loading and managing the tasks
	/// </summary>
	public sealed class TaskManager {
		private static readonly IList<ITask> tasks = new List<ITask>();
		private static SchedulerConfigurationSection cfgSec = SchedulerConfigurationSection.GetInstance();
		private static ITask existingTask;

		static TaskManager() {
			LoadTasks(false);
		}

		public static IList<ITask> Tasks {
			get { return new ReadOnlyCollection<ITask>(tasks); }
		}

		public static bool IsShutDown {
			get { return Scheduler.IsShutDown; }
		}

		private static void LoadTasks(bool skipDuplicates) {
			CheckShutdown();
			if (cfgSec != null)
				foreach (TaskElement te in cfgSec.TaskElements)
					if (!skipDuplicates || GetTaskByName(te.Task.Name) == null) {
						te.Task.ErrorAlertFrom = cfgSec.ErrorAlertFrom.Value;
						te.Task.ErrorAlertEmails = cfgSec.ErrorAlertEmails.Emails;
						AddTask(te.Task, te.StartImmediately);
					}
		}

		private static IInterceptor GetInterceptorForTask(ITask t) {
			if (cfgSec == null)
				return null;
			TaskElement te = cfgSec.FindTaskElementByType(t.GetType());
			if (te == null || !te.UserInterceptor)
				return null;
			IInterceptor retVal = te.InterceptorElement.Interceptor;
			if (retVal == null)
				retVal = cfgSec.DefaultInterceptor;
			return retVal;
		}

		public static void ReloadTasks() {
			LoadTasks(true);
		}

		/// <summary>
		/// Adds a task to the manager
		/// </summary>
		/// <param name="t"></param>
		/// <param name="startImmediately"></param>
		/// <remarks>
		/// If there is already a task with the same name in the manager, it will
		/// 1) remove it if it's finished 
		/// 2) or throw an TaskWithDuplicatedNameException
		/// </remarks>
		public static void AddTask(ITask t, bool startImmediately) {
			ClearupOutDatedTask();
			existingTask = GetTaskByName(t.Name);
			if (existingTask != null)
				if (existingTask.Status == TaskStatus.Finished)
					RemoveTask(existingTask);
				else
					throw new TaskWithDuplicatedNameException();
			tasks.Add(t);
			if (startImmediately)
				StartTask(t);
		}

		private static void ClearupOutDatedTask() {
			ITask[] ts = new ITask[tasks.Count];
			tasks.CopyTo(ts, 0);
			foreach (ITask t in ts)
				if (t.IsOutDated)
					RemoveTask(t);
		}

		public static void StartAll() {
			CheckShutdown();
			foreach (ITask task in tasks) StartTask(task);
		}

		private static void CheckShutdown() {
			if (IsShutDown)
				throw new SchedulerException("Scheduler is already shutdown");
		}

		public static void StopAll() {
			foreach (ITask task in tasks) StopTask(task);
		}

		public static void StopTask(ITask task) {
			if (TaskIsScheduled(task))
				Scheduler.RemoveSchedule(task.Schedule);
		}

		public static bool RemoveTask(ITask task) {
			if (!tasks.Contains(task))
				return false;
			StopTask(task);
			tasks.Remove(task);
			return true;
		}

		public static void StartTask(ITask task) {
			if (!tasks.Contains(task))
				throw new SchedulerException("The task is not in the task list.");
			else if (!TaskIsScheduled(task)) Scheduler.AddSchedule(task.Schedule, GetInterceptorForTask(task));
		}

		public static void ShutDown() {
			tasks.Clear();
			Scheduler.Shutdown();
		}

		public static bool TaskIsScheduled(ITask t) {
			return Scheduler.Contains(t.Schedule);
		}

		public static ITask GetTaskByName(string name) {
			foreach (ITask task in tasks)
				if (task.Name == name)
					return task;
			return null;
		}
	}
}