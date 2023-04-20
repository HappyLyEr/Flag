using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GASystem.AppUtils;

namespace GASystem.WebControls.ListControls
{
	/// <summary>
	/// Displays a value as a checkbox
	/// </summary>
	public class GADataRecordIdentifierTemplate : ITemplate
	{
			private Literal lbl;
			private string _columnName;
            // Tor 201611 Security 20161122 (never referenced) private string _formattingString = "{0}";
		
			public GADataRecordIdentifierTemplate(string columnName)
			{
				_columnName = columnName;
			}

			#region ITemplate Members

			public void InstantiateIn(Control container)
			{
				lbl = new Literal();
				lbl.DataBinding += new EventHandler(lbl_DataBinding);
				container.Controls.Add(lbl);
			}

			#endregion

			protected void lbl_DataBinding(object sender, EventArgs e)
			{
				Literal l = (Literal)sender;
				DataGridItem dgi = (DataGridItem)l.NamingContainer;
				string val = ((DataRowView)dgi.DataItem)[_columnName].ToString();
				
				string[] valparts = val.Split('-');
				if (valparts.Length == 2)
				{
					lbl.Text = Localization.GetGuiElementText(valparts[0].Trim()) + " " + GASystem.BusinessLayer.Utils.RecordSetUtils.GetDataRecordName(valparts[0].Trim(), valparts[1]);
				} else
				{
					lbl.Text = string.Empty;
				}
					
				
				
			}

		}
	}