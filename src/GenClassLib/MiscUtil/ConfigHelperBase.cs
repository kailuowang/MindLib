using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace MindHarbor.GenClassLib.MiscUtil {
	/// <summary>
	/// a base class for config helper in projects
	/// </summary>
	public abstract class ConfigHelperBase {
		private static IDictionary<string, object> appSettingDict = new Dictionary<string, object>();

		public static string GetSetting(string key) {
			return GetSetting<string>(key);
		}

		public static void SetSetting(string key, object value) {
			appSettingDict[key] = value;
		}

		public static T GetSetting<T>(string key) {
			return GetSetting(key, default(T), true);
		}

		/// <summary>
		/// Try get the setting in the confirguration file, if not set there, return the defaultValue
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T GetSetting<T>(string key, T defaultValue) {
			return GetSetting(key, defaultValue, false);
		}

		private static T GetSetting<T>(string key, T defaultValue, bool throwException) {
			object retVal = null;
			if (appSettingDict.TryGetValue(key, out retVal)) return (T) retVal;
			string setting = ConfigurationManager.AppSettings[key];
			if (string.IsNullOrEmpty(setting)) {
				if (throwException)
					throw new Exception("The setting with key \"" + key + "\" must be set, check your web.config file");
				return defaultValue;
			}
			return (T) Convert.ChangeType(setting, typeof (T));
		}

		/// <summary>
		/// The insecure root application path of the current http application
		/// </summary>
		/// <returns></returns>
		public static string RootPath() {
			return RootPath(HttpContext.Current.Request.IsSecureConnection);
		}

		/// <summary>
		/// The root application path of the current http application
		/// </summary>
		/// <param name="secure">whether or not return a secure link to the root</param>
		/// <returns></returns>
		public static string RootPath(bool secure) {
			HttpRequest r = HttpContext.Current.Request;
			string port = r.Url.Port == 80 ? string.Empty : ":" + r.Url.Port;
			return "Http" + (secure ? "s" : "") + "://" + r.Url.Host + port + r.ApplicationPath + "/";
		}
	}
}