namespace MindHarbor.GenClassLib.XMLUtil {
	/// <summary>
	/// Summary description for GeneralCompositeXmlMapper.
	/// </summary>
	public class GeneralCompositeXmlMapper : CompositeXmlMapperBase {
		public GeneralCompositeXmlMapper() {}

		public object RootCompositeObject {
			get { return DataSource; }
			set { DataSource = value; }
		}
	}
}