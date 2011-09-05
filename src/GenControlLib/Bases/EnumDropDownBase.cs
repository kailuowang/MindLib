using System;
using System.Collections.Generic;
using System.Text;

namespace MindHarbor.GenControlLib.Bases{
    public enum EnumDataFields{
        Values,
        Names
    }

    public abstract class EnumDropDownBase<EnumT> : SpecializedDDLBase where EnumT : struct{
        protected virtual EnumDataFields DefaultSourceField{
            get { return EnumDataFields.Names; }
        }

        /// <summary>
        /// Whether to use the int values or the Names of the Enum to populate the texts in DropDownList
        /// this property can only be set in page file
        /// </summary>
        public EnumDataFields ShowValueOrName{
            get{
                if (ViewState["SourceField"] == null)
                    ViewState["SourceField"] = DefaultSourceField;
                return (EnumDataFields) ViewState["SourceField"];
            }
            set { ViewState["SourceField"] = value; }
        }

        public override sealed string DataValueField{
            get { return "Value"; }
        }

        public override sealed string DataTextField{
            get{
                if (ShowValueOrName == EnumDataFields.Names)
                    return "Key";
                else
                    return "Value";
            }
        }

        public override sealed object DataSource{
            get { return GetValues(); }
        }

        public EnumT Selected{
            get{
                if (SelectedValue == PLZ_SELECT_ITEM_VALUE)
                    throw new Exception("This dropdown has not been selected yet");
                return (EnumT) Enum.Parse(typeof (EnumT), SelectedValue);
            }
            set { SelectedValue = (Convert.ToInt32(value).ToString()); }
        }

      protected ICollection<EnumT> GetHideEnumData(){
            ICollection<EnumT> temp = new List<EnumT>();


            if (ViewState["HideEnumData"] == null || string.IsNullOrEmpty(ViewState["HideEnumData"].ToString()))

                return temp;


            foreach (string enumData in ViewState["HideEnumData"].ToString().Split(new char[] { ',', ';' }))
            {
                if (string.IsNullOrEmpty(enumData))
                    continue;
                temp.Add((EnumT)Enum.Parse(typeof(EnumT), enumData));
            }
            return temp;
        }

        /// <summary>
        /// Set some Enum options hidden.
        /// this property can only be set in page file
        /// </summary>
        public void SetHideEnumData(ICollection<EnumT> enumTs){
            StringBuilder sb = new StringBuilder();
            foreach (EnumT enumData in enumTs)
            {
                sb.Append(enumData.ToString());
                sb.Append(";");
            }
            ViewState["HideEnumData"] = sb.ToString();
        }

        private object GetValues(){
            IDictionary<string, int> dict = new Dictionary<string, int>();
            ICollection<EnumT> temp = GetHideEnumData();
            foreach (EnumT et in Enum.GetValues(typeof (EnumT))){
                if (!temp.Contains(et))
                    dict.Add(et.ToString(), Convert.ToInt32(et));
            }
            return dict;
        }
    }
}