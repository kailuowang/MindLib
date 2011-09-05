using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace MindHarbor.GenControlLib.MiscUtil {
	/// <summary>
	/// a base class for config helper in projects
	/// </summary>
	public abstract class CookieHelperBase {
		private static HttpContext ctx {
			get { return HttpContext.Current; }
		}

		public static string Get(string key) {
			return Get<string>(key);
		}

	 

		public static T Get<T>(string key) {
			return Get(key, default(T));
		}

		/// <summary>
		/// Try get the setting in the confirguration file, if not set there, return the defaultValue
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T Get<T>(string key, T defaultValue) {
			string setting = ctx.Request.Cookies[key] != null ? ctx.Request.Cookies[key].Value : string.Empty;
			if (string.IsNullOrEmpty(setting))
			{
				return defaultValue;
			}
			return (T)Convert.ChangeType(setting, typeof(T));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="persists">whether this cookie value persists</param>
		public static void Set<T>(string key, T value, bool persists) {
			bool isEmpty = object.Equals( value , default(T));
			ctx.Request.Cookies.Remove(key);
			HttpCookie ck = new HttpCookie(key,isEmpty ? string.Empty : value.ToString());
			if (isEmpty)
				ck.Expires = DateTime.Now;
			else if (persists)
				ck.Expires = DateTime.Now.AddMonths(12);
				
			ctx.Response.Cookies.Add(ck);
			ctx.Request.Cookies.Add(ck);
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