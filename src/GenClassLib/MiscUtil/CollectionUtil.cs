using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MindHarbor.GenClassLib.MiscUtil {
	/// <summary>
	/// An Utility class that provide a number of helper methods arround collections. 
	/// </summary>
	public class CollectionUtil {
		private static Random r = new Random(12);
		private CollectionUtil() {}

		/// <summary>
		/// Adds all items from one dictionary to another 
		/// </summary>
		/// <param name="original">The original.</param>
		/// <param name="toAdd">To add.</param>
		public static void DictionaryAddRange(IDictionary original, IDictionary toAdd) {
			foreach (object key in toAdd.Keys) {
				if (original.Contains(key))
					throw new NotSupportedException("Two dictionaries have overlap keys");
				original.Add(key, toAdd[key]);
			}
		}

		/// <summary>
		/// Add a collection of items into a dictionary, using a collection of keys and one default value for all the items
		/// </summary>
		/// <param name="original">The original.</param>
		/// <param name="newKeys">The new keys.</param>
		/// <param name="defaultValue">The default value.</param>
		public static void DictionaryAddRange(IDictionary original, IEnumerable newKeys, object defaultValue) {
			foreach (object key in newKeys) {
				if (original.Contains(key))
					throw new NotSupportedException("Two dictionaries have overlap keys");
				original.Add(key, defaultValue);
			}
		}

		/// <summary>
		/// Sorts the Ilist.
		/// </summary>
		/// <param name="collection">The collection.</param>
		///<returns>A new sorted IList that has all the items in the <paramref name="collection"/></returns>
		public static IList Sort(ICollection collection) {
			ArrayList temp = new ArrayList(collection);
			temp.Sort();
			return temp;
		}

		///<summary>
		///</summary>
		///<param name="collection"></param>
		///<typeparam name="T"></typeparam>
		///<returns>A new sorted IList that has all the items in the <paramref name="collection"/></returns>
		public static IList<T> Sort<T>(IEnumerable<T> collection) {
			List<T> retVal = new List<T>(collection);
			retVal.Sort();
			return retVal;
		}

		///<summary>
		///</summary>
		///<param name="collection"></param>
		///<typeparam name="T"></typeparam>
		///<returns>A new sorted IList that has all the items in the <paramref name="collection"/></returns>
		///<param name="sortExpression"></param>
		public static IList<T> Sort<T>(IEnumerable<T> collection, string sortExpression) {
			List<T> retVal = new List<T>(collection);
			retVal.Sort(new GenericComparer<T>(sortExpression));
			return retVal;
		}

		/// <summary>
		/// Get the last item from an IEnumerable
		/// </summary>
		/// <param name="collection"></param>
		/// <returns>return null if the collection is empty</returns>
		/// <remarks>
		/// This method depends on the sequence of the collection, 
		/// if the collection does not have a sequence, then it is unpreditable that which item will be returned.
		/// </remarks>
		public static object GetLast(IEnumerable collection) {
			object retVal = null;
			IEnumerator en = collection.GetEnumerator();
			while (en.MoveNext()) retVal = en.Current;
			return retVal;
		}

		/// <summary>
		/// Get the first item from an IEnumerable
		/// </summary>
		/// <param name="collection"></param>
		/// <returns>return null if the collection is empty</returns>
		/// <remarks>
		/// This method depends on the sequence of the collection, 
		/// if the collection does not have a sequence, then it is unpreditable that which item will be returned.
		/// </remarks>
		public static object GetFirst(IEnumerable collection) {
			object retVal = null;
			IEnumerator en = collection.GetEnumerator();
			if (en.MoveNext()) retVal = en.Current;

			return retVal;
		}

		/// <summary>
		/// Get an item from an IEnumerable (the first one returned by its enumerator
		/// </summary>
		/// <param name="collection"></param>
		/// <returns>returns null if the collection is empty</returns>
		public static T GetOneItem<T>(IEnumerable<T> collection) {
			return (T) GetFirst(collection);
		}

		/// <summary>
		/// Get random item from an ICollection  
		/// </summary>
		/// <param name="collection"></param>
		/// <returns>returns null if the collection is empty</returns>
		public static T RandomItem<T>(ICollection<T> collection) {
			int index = r.Next(0, collection.Count);
			IEnumerator<T> en = collection.GetEnumerator();
			for (int i = 0; i <= index; i++)
				en.MoveNext();
			return en.Current;
		}

		/// <summary>
		/// Builds a string reprensentation for a <paramref name="collection"/>
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="seperator">the string used to seperate strings of different items</param>
		/// <param name="emptyText">the string to be return if the <paramref name="collection"/> is empty</param>
		/// <returns></returns>
		/// <remarks>
		/// Note that null tiems and items with an empty ToString() won't show up in this representation
		/// To control this behavior use <see cref="ToString(IEnumerable , string , string , string ) "/>
		/// </remarks>
		public static string ToString(IEnumerable collection, string seperator, string emptyText) {
			return ToString(collection, seperator, emptyText, string.Empty);
		}

		/// <summary>
		/// Builds a string reprensentation for a <paramref name="collection"/>
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="seperator">the string used to seperate strings of different items</param>
		/// <param name="emptyText">the string to be return if the <paramref name="collection"/> is empty</param>
		/// <param name="nullText">the string to use when the item is null or its ToString() returns an empty string</param>
		/// <returns></returns>
		public static string ToString(IEnumerable collection, string seperator, string emptyText, string nullText) {
			StringBuilder sb = new StringBuilder();
			foreach (object o in collection) {
				string s = string.Empty;
				if (o != null) s = o.ToString();
				if (string.IsNullOrEmpty(s)) s = nullText;
				if (!string.IsNullOrEmpty(s)) sb.Append(s + seperator);
			}
			if (sb.Length > 0) {
				sb.Remove(sb.Length - seperator.Length, seperator.Length);
				return sb.ToString();
			}
			else
				return emptyText;
		}

		/// <summary>
		/// Builds a string reprensentation for a <paramref name="collection"/>
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="seperator">the string used to seperate strings of different items</param>
		/// <returns>returns <see cref="String.Empty"/> if the <paramref name="collection"/> is empty.</returns>
		/// <remarks>
		/// Note that item with an empty ToString() won't show up in this representation.
		/// To control this behavior use <see cref="ToString(IEnumerable , string , string , string ) "/>
		/// </remarks>
		public static string ToString(IEnumerable collection, string seperator) {
			return ToString(collection, seperator, string.Empty);
		}
	}
}