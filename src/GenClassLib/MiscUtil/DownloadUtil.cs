using System;
using System.IO;
using System.Web;

namespace MindHarbor.GenClassLib.MiscUtil {
	public abstract class DownloadUtil {
		/// <summary>
		/// Send the file in the filepath to the HttpResponse and end it
		/// </summary>
		/// <param name="filePath"></param>
		public static void DownLoadByStream(string filePath) {
			if (!File.Exists(filePath))
				throw new ArgumentException("Cannot find the file " + filePath);
			HttpContext.Current.Response.ContentType = "application/octet-stream";
			HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + GetFileName(filePath));
			HttpContext.Current.Response.TransmitFile(filePath);
			HttpContext.Current.Response.End();
			//FileStream stream = new FileStream(filePath, FileMode.Open,
			//FileAccess.Read, FileShare.Read);
			//try
			//{
			//    int bufSize = (int)stream.Length;
			//    byte[] buf = new byte[bufSize];

			//    int bytesRead = stream.Read(buf, 0, bufSize);

			//    HttpContext.Current.Response.OutputStream.Write(buf, 0, bytesRead);
			//    HttpContext.Current.Response.End();
			//}
			//finally
			//{
			//    stream.Close();
			//}
		}

		/// <summary>
		/// only get fileName(non-include path);
		/// </summary>
		/// <param name="fullpath">fullpath</param>
		/// <returns></returns>
		public static string GetFileName(string fullpath) {
			int index = fullpath.LastIndexOf('\\');
			string fileName = fullpath.Substring(index + 1);
			return fileName;
		}
	}
}