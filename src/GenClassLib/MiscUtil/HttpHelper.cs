using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace MindHarbor.GenClassLib.MiscUtil {
	public class HttpHelper {
		private string contentType = "application/x-www-form-urlencoded";

		public HttpHelper() {}

		public string ContentType {
			get { return contentType; }
			set { contentType = value; }
		}

		public XmlDocument RequestForXMLResponse(string postData, string url) {
			XmlDocument doc = new XmlDocument();

			HttpWebResponse response = DoHttpRequest(postData, url);
			using (XmlTextReader xtr = new XmlTextReader(response.GetResponseStream())) {
				doc.Load(xtr);
				xtr.Close();
			}
			response.Close();
			return doc;
		}

		public string RequestForPlainTextResponse(string postData, string url) {
			string result;
			HttpWebResponse response = DoHttpRequest(postData, url);
			using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.ASCII)) {
				result = sr.ReadToEnd();

				// Close and clean up the StreamReader
				sr.Close();
			}
			response.Close();
			return result;
		}

		public HttpWebResponse DoHttpRequest(string postData, string url) {
			HttpWebRequest r = PrepareRequest(postData, url);

			return (HttpWebResponse) r.GetResponse();
		}

		private static HttpWebRequest PrepareRequest(string postData, string url) {
			HttpWebRequest r = (HttpWebRequest) HttpWebRequest.Create(url);

			r.Method = "Post";
			r.ContentType = "application/x-www-form-urlencoded";
			r.Credentials = CredentialCache.DefaultCredentials;

			if (!string.IsNullOrEmpty(postData)) {
				ASCIIEncoding encoding = new ASCIIEncoding();
				byte[] byte1 = encoding.GetBytes(postData);
				r.ContentLength = byte1.Length;
				Stream stream = r.GetRequestStream();
				stream.Write(byte1, 0, byte1.Length);
				stream.Close();
			}

			return r;
		}

		/// <summary>
		/// Do a http request without getting response
		/// </summary>
		/// <param name="postData"></param>
		/// <param name="url"></param>
		/// <remarks> 
		/// due to some unknow bug, this method cannot be performed over 24 times in one thread. 
		/// </remarks>
		public void UnidirectionHttpRequest(string postData, string url) {
			HttpWebRequest r = PrepareRequest(postData, url);
			IAsyncResult result = r.BeginGetResponse(new AsyncCallback(AsyncCallback), null);
			//r.EndGetResponse(result);
		}

		/// <summary>
		/// Generate para string in the url
		/// </summary>
		/// <param name="paraName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public string AddPara(string paraName, string value) {
			return "&" + Para(paraName, value);
		}

		public string AddPara(string paraName, object value) {
			return AddPara(paraName, value.ToString());
		}

		public string Para(string paraName, string value) {
			return paraName + "=" + HttpUtility.UrlEncode(value);
		}

		public string Para(string paraName, object value) {
			return Para(paraName, value.ToString());
		}

		public void AsyncCallback(IAsyncResult ar) {
			//dononthing
		}
	}
}