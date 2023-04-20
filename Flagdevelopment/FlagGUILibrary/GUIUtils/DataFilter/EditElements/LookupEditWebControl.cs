using System;
using System.Web.UI.WebControls;
using GASystem.AppUtils;
using GASystem.GUIUtils;
using System.Web.UI;

namespace GASystem.GUIUtils.DataFilter.EditElements
{
	/// <summary>
	/// Controls for editing filters on general textboxes
	/// </summary>
	public class LookupEditWebControl : GeneralEditWebControl, System.Web.UI.INamingContainer
	{
        string _alias;

        public LookupEditWebControl(FieldDescription fd) : base(fd)
		{
            
            _alias = fd.FieldId;
            this.FieldName = fd.TableId + "." + fd.FieldId; // GenerateCombinedColumns(fd);
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
            //resetting id of checkbox;
            conditionCheckBox.ID = _alias + "checkbox";
            _conditionText.Attributes.Remove("onFocus");
            _conditionText.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID + "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");
			
			
		}

        /// <param name="FieldDescriptionInfo"></param>
        /// <param name="CurrentTableAlias"></param>
        /// <returns></returns>
        private string GenerateCombinedColumns(GASystem.AppUtils.FieldDescription FieldDescriptionInfo)
        {

            string columnSelect = "''";
            foreach (String columnName in FieldDescriptionInfo.GetLookupTableDisplayColumns())
            {
                GASystem.AppUtils.FieldDescription fd = GASystem.AppUtils.FieldDefintion.GetFieldDescription(columnName, FieldDescriptionInfo.LookupTable);
                if (fd.LookupTable == string.Empty)
                    columnSelect += " + " + columnName + " + ' ' ";
                else
                {
                    columnSelect += " + " + GenerateCombinedColumns(fd);
                }
            }

            return columnSelect;
        }
		
	}
}
