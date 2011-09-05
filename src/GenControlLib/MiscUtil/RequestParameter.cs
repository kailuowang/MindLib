using System;
using System.Web;
using MindHarbor.GenClassLib.MiscUtil;

namespace MindHarbor.GenControlLib.MiscUtil {
	public static class RequestParameter {
		private const string encryptionKey = "MHada34ei2cmd*8kd84ch3cc";

		private static HttpRequest Request {
			get { return HttpContext.Current.Request; }
		}

		public static T Get<T>(string paraName) {
			return Get<T>(paraName, false);
		}

		public static T Get<T>(string paraName, bool encrypted) {
			string stringValInRequest = Request[paraName];
			if (string.IsNullOrEmpty(stringValInRequest))
				throw new RequestParameterException("Cannot find parameter named " + paraName + " in the request.");
			if (encrypted)
				stringValInRequest = EncryptionUtil.Decrypt(stringValInRequest, encryptionKey);
			return (T) Convert.ChangeType(stringValInRequest, typeof (T));
		}

		/// <summary>
		/// Geneates a url string for encrypted parameter
		/// </summary>
		/// <param name="paraName"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static string EncryptedParameterURLString(string paraName, object val) {
			return paraName + "=" + HttpUtility.UrlEncode(EncryptionUtil.Encrypt(val.ToString(), encryptionKey));
		}

		/// <summary>
		/// Try get the parameter value
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="paraName"></param>
		/// <param name="val"></param>
		/// <param name="encrypted">whether this parameter is encrypted</param>
		/// <returns></returns>
		public static bool TryGet<T>(string paraName, out T val, bool encrypted) {
			try {
				val = Get<T>(paraName, encrypted);
				return true;
			}
			catch (RequestParameterException) {}
			catch (InvalidCastException) {}

			val = default(T);
			return false;
		}
	}

	public class RequestParameterException : Exception {
		public RequestParameterException(string msg) : base(msg) {}
	}
}