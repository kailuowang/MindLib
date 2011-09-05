using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MindHarbor.GenControlLib.MiscUtil {
	/// <summary>
	/// 
	/// </summary>
	public class ExcelExportUtil {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName">no extension needed here</param>
		/// <param name="gv"></param>
		public static void ExportGridView(string fileName, GridView gv) {
			Table table = new Table();
			if (gv.HeaderRow != null) {
				PrepareControlForExport(gv.HeaderRow);
				table.Rows.Add(gv.HeaderRow);
			}
			foreach (GridViewRow row in gv.Rows) {
				PrepareControlForExport(row);
				table.Rows.Add(row);
			}
			if (gv.FooterRow != null) {
				PrepareControlForExport(gv.FooterRow);
				table.Rows.Add(gv.FooterRow);
			}
			ExportControl(fileName, table, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName">no extension needed here</param>
		/// <param name="ctl"></param>
		/// <param name="preProcess">whether to process the control for those elements that is not supported in excel</param>
		public static void ExportControl(string fileName, Control ctl, bool preProcess) {
			using (StringWriter sw = new StringWriter())
			using (HtmlTextWriter htw = new HtmlTextWriter(sw)) {
				if (preProcess)
					PrepareControlForExport(ctl);
				ctl.RenderControl(htw);
				ExportString(fileName, sw.ToString());
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName">no extension needed here</param>
		/// <param name="content"></param>
		public static void ExportString(string fileName, string content) {
			HttpResponse response = HttpContext.Current.Response;
			response.Clear();
			response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
			response.Charset = "";

			// If you want the option to open the Excel file without saving then
			// comment out the line below
			// Response.Cache.SetCacheability(HttpCacheability.NoCache);
			response.ContentType = "application/vnd.ms-excel";
			response.Write(content);
			response.End();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName">no extension needed here</param>
		/// <param name="ctl"></param>
		public static void ExportControl(string fileName, Control ctl) {
			ExportControl(fileName, ctl, true);
		}

		private static void PrepareControlForExport(Control control) {
			for (int i = 0; i < control.Controls.Count; i++) {
				Control current = control.Controls[i];
				if (current is LinkButton) {
					control.Controls.Remove(current);
					control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
				}
				else if (current is ImageButton) {
					control.Controls.Remove(current);
					control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
				}
				else if (current is HyperLink) {
					control.Controls.Remove(current);
					control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
				}
				else if (current is DropDownList) {
					control.Controls.Remove(current);
					control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
				}
				else if (current is CheckBox) {
					control.Controls.Remove(current);
					control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "Y" : "N"));
				}
				else if (current is TextBox) {
					control.Controls.Remove(current);
					control.Controls.AddAt(i, new LiteralControl((current as TextBox).Text));
				}

				if (current.HasControls())
					PrepareControlForExport(current);
			}
		}
	}
}