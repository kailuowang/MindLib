using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;

namespace MindHarbor.GenClassLib.XMLUtil {
	/// <summary>
	/// This mapper will map a composite patterned object into an xml file
	/// </summary>
	public abstract class CompositeXmlMapperBase {
		#region private fields

		private string[] customProperties = new string[] {};
		private object dataSource;
		private string idField = "Id";
		private bool ignoreRoot = false;
		private string[] mapperManagedProperties = new string[] {};
		private string nameField = "Name";
		private string subComponentsField;
		private string valueField = "Value";
		private XmlDocument xd = new XmlDocument();

		#endregion

		#region Properties

		/// <summary>
		/// The names of the Properties that should be managed by the inheritance mapper
		/// by using getMapperManagedProperty(...)
		/// </summary>
		protected string[] MapperManagedProperties {
			get { return mapperManagedProperties; }
			set { mapperManagedProperties = value; }
		}

		/// <summary>
		/// the names of the other properties(string) needs to be mapped into an element
		/// the mapper will call the ToString() method on these properties and map the value to xml files
		/// </summary>
		public string[] CustomProperties {
			get { return customProperties; }
			set { customProperties = value; }
		}

		protected object DataSource {
			get { return dataSource; }
			set { dataSource = value; }
		}

		public string NameField {
			get { return nameField; }
			set { nameField = value; }
		}

		public string ValueField {
			get { return valueField; }
			set { valueField = value; }
		}

		public string IdField {
			get { return idField; }
			set { idField = value; }
		}

		public string SubComponentsField {
			get {
				if (subComponentsField == "" && DataSource != null)
					subComponentsField = guessSubComponentsField(DataSource);
				return subComponentsField;
			}
			set { subComponentsField = value; }
		}

		/// <summary>
		/// when mapping the composite whether map the root component name/property and so on
		/// </summary>
		public bool IgnoreRoot {
			get { return ignoreRoot; }
			set { ignoreRoot = value; }
		}

		#endregion Properties

		#region public methods

		public XmlDocument MapToXmlDoc() {
			resetXd();
			if (IgnoreRoot)
				foreach (object component in getSubComponents(DataSource))
					xd.DocumentElement.AppendChild(createXmlElement(component));
			else
				xd.DocumentElement.AppendChild(createXmlElement(DataSource));

			return xd;
		}

		public XmlElement CreateXmlElement() {
			return createXmlElement(dataSource);
		}

		public void MapToXmlFile(string filePath) {
			FileInfo fi = new FileInfo(filePath);
			if (fi.Exists) fi.Delete();
			FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
			XmlTextWriter xw = new XmlTextWriter(fs, null);
			xw.Formatting = Formatting.Indented;
			xw.Indentation = 2;
			xw.IndentChar = ' ';
			MapToXmlDoc().WriteTo(xw);
			xw.Flush();
			xw.Close();
			fs.Close();
		}

		#endregion public methods

		#region private methods

		private void resetXd() {
			xd = new XmlDocument();
			XmlDeclaration decl = xd.CreateXmlDeclaration("1.0", "utf-8", "");
			xd.InsertBefore(decl, xd.DocumentElement);
			XmlElement root = xd.CreateElement("root");
			xd.AppendChild(root);
		}

		protected IEnumerable getSubComponents(object composite) {
			PropertyInfo pi = composite.GetType().GetProperty(SubComponentsField);
			if (pi == null || pi.GetValue(composite, null) == null) return new ArrayList();
			return (IEnumerable) pi.GetValue(composite, null);
		}

		protected string getStringProperty(object component, string propertyName) {
			PropertyInfo pi = component.GetType().GetProperty(propertyName);
			if (pi == null || pi.GetValue(component, null) == null) return null;
			return pi.GetValue(component, null).ToString();
		}

		protected XmlElement createXmlElement(object composite) {
			XmlElement xe = initializeXmlElement(composite);

			foreach (XmlElement elem in createXmlElementsOfSubs(composite))
				xe.AppendChild(elem);
			return xe;
		}

		/// <summary>
		/// this method is for inheritance of this class to override to
		/// write their own way of creating xmlElements for the components of a composite
		/// </summary>
		/// <param name="composite"></param>
		/// <returns></returns>
		protected virtual IEnumerable createXmlElementsOfSubs(object composite) {
			IList retVal = new ArrayList();

			foreach (object component in getSubComponents(composite))
				retVal.Add(createXmlElement(component));
			return retVal;
		}

		protected XmlElement initializeXmlElement(object composite) {
			XmlElement xe = xd.CreateElement("Composite");

			XmlElement nameElem = creatPropertyXMLElement(composite, NameField, "Name");
			if (nameElem != null)
				xe.AppendChild(nameElem);

			XmlElement idElem = creatPropertyXMLElement(composite, idField, "Id");
			if (idElem != null)
				xe.AppendChild(idElem);

			foreach (string propertyName in CustomProperties) {
				XmlElement propertyElem = creatPropertyXMLElement(composite, propertyName);
				if (propertyElem != null)
					xe.AppendChild(propertyElem);
			}

			foreach (string propertyName in MapperManagedProperties) {
				XmlElement propertyElem =
					createXMLElementWithStringInnerText(propertyName, getMapperManagedProperty(composite, propertyName));
				if (propertyElem != null)
					xe.AppendChild(propertyElem);
			}

			return xe;
		}

		/// <summary>
		/// this method is to get a mapper managed property that is calculated by the inheritance mapper
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		protected virtual string getMapperManagedProperty(object composite, string propertyName) {
			return string.Empty;
		}

		private XmlElement creatPropertyXMLElement(object composite, string propertyName, string elementName) {
			string propertyValue = getStringProperty(composite, propertyName);
			return createXMLElementWithStringInnerText(elementName, propertyValue);
		}

		private XmlElement createXMLElementWithStringInnerText(string elementName, string innerText) {
			if (innerText == string.Empty || elementName == string.Empty) return null;

			XmlElement elem = xd.CreateElement(elementName);
			elem.InnerText = innerText;
			return elem;
		}

		private XmlElement creatPropertyXMLElement(object composite, string propertyName) {
			return creatPropertyXMLElement(composite, propertyName, propertyName);
		}

		private string guessSubComponentsField(object target) {
			string targetClassName = target.GetType().Name;
			return "sub" + targetClassName;
		}

		#endregion private methods
	}
}