using System;
using System.Reflection;
using System.Text;
using MindHarbor.GenInterfaces;

namespace MindHarbor.GenClassLib.MiscUtil {
	public abstract class StringUtil {
		/// <summary>
		/// split a name to lastname and firstname
		/// </summary>
		/// <param name="name">the name format can be either "Lastname, firstname" or "firstname lastname"</param>
		/// <returns>name[0] : LastName ; name[1] FirstName
		/// if the name is only one word, it will be returned as the lastname, while the firstname will be null
		/// </returns>
		public static string[] SplitName(string name) {
			string[] retVal = new string[2];
			int i;
			if ((i = name.IndexOf(',')) >= 0) {
				retVal[0] = name.Substring(0, i).Trim();
				retVal[1] = name.Substring(i + 1).Trim();
			}
			else if ((i = name.LastIndexOf(' ')) >= 0) {
				retVal[0] = name.Substring(i + 1).Trim();
				retVal[1] = name.Substring(0, i).Trim();
			}
			else
				retVal[0] = name;
			return retVal;
		}

		public static string BreakCamelCase(string camelCaseString) {
			int i = 1;
			bool inWord = true;
			StringBuilder sb = new StringBuilder(camelCaseString);
			while (i < sb.Length) {
				if (Char.IsUpper(sb[i]))
					if (Char.IsLower(sb[i - 1])
					    || (i < sb.Length - 1 && Char.IsLower(sb[i + 1]))) {
						sb.Insert(i, " ");
						i++;
					}
				i++;
			}
			return sb.ToString().Trim();
		}

		///<summary>
		/// Parses a string to <typeparamref name="T"/>
		///</summary>
		///<param name="value"></param>
		///<typeparam name="T"></typeparam>
		///<returns></returns>
		public static T Parse<T>(string value) {
			if (string.IsNullOrEmpty(value) || value == "NULL")
				return default(T);
			if ((typeof (T)).IsEnum)
				return (T) Enum.Parse(typeof (T), value);
			return (T) Convert.ChangeType(value, typeof (T));
		}

		public static string ToString(IAddress a, bool includeName) {
			StringBuilder sb = new StringBuilder();
			if (includeName)
				sb.AppendLine(a.Name);
			sb.AppendLine(a.Street);
			if (!string.IsNullOrEmpty(a.Street2))
				sb.AppendLine(a.Street2);
			sb.Append(a.City)
				.Append(" ")
				.Append(a.State)
				.Append(" ")
				.Append(a.Zip);
			return sb.ToString();
		}

		public static string ToHtml(string plainText) {
			StringBuilder sb = new StringBuilder(plainText);
			sb.Replace("<", "&lt;");
			sb.Replace(">", "&gt;");
			sb.Replace("\r\n", "<br />");
			sb.Replace("\t", "    ");
			sb.Replace("  ", " &nbsp;");
			return sb.ToString();
		}

        public static string ToString(object o , string formatString) {
            if(o == null)
                return string.Empty;
            if(!string.IsNullOrEmpty(formatString)) {
                Type t = o.GetType();
                MethodInfo mi = t.GetMethod("ToString", new Type[] {typeof (string)});
                if (mi != null) 
                    return    (string) mi.Invoke(o, new object[] {formatString});
            }
            return o.ToString();
        }
	}
}