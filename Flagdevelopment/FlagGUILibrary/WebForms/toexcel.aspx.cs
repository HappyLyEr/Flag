using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem.GAControls;
using GASystem;
using GASystem.AppUtils;



namespace GASystem.WebForms
{
	/// <summary>
	/// Summary description for toexcel.
	/// </summary>
	public class toexcel : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DataGrid DataGrid1;
		
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		//	Response.ContentType = "application/excel";

			//get dataclass
			GADataClass dataClass = (GADataClass)this.Context.Items["dataclass"];


			GADataRecord owner = SessionManagement.GetCurrentDataContext().SubContextRecord;

			

			//myListDataRecords.RecordsDataSet.Tables[0].DefaultView.RowFilter = myFilterBuilder.GetFilterString();

			ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(dataClass);

			DataSet ds;
			if (cd.IsTop)
				ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(dataClass, null);
			else 
			{
				BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(dataClass);
				ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner); //.Tables[0];
			}

			//ds.Tables[0].DefaultView.RowFilter = this.Context.Items["exportfilter"].ToString();



			//setup datagrid
			
			DataGrid1.Columns.Clear();
			DataGrid1.AutoGenerateColumns = false;
			DataGrid1.EnableViewState= true;
			
		//	DataGrid1.DataKeyField = ds.Tables[DataClass].PrimaryKey[0].ColumnName;

            // Tor 20140320 added ownerclass (2nd parameter in call below) to hide fields that should not show in current memberclass, ownerclass record)
            FieldDescription[] fds = GASystem.AppUtils.FieldDefintion.GetFieldDescriptionsDetailsForm(dataClass.ToString(),"");
			foreach (FieldDescription fd in fds) 
			{
				if (!fd.HideInDetail && ds.Tables[0].Columns.Contains(fd.FieldId))
				{
					BoundColumn column = new BoundColumn();
					column.DataField = fd.FieldId;
					column.HeaderText = AppUtils.Localization.GetCaptionText( fd.DataType);
					//column.DataFormatString =setFormatting(c);
					DataGrid1.Columns.Add(column);
				}
			}
			




			DataGrid1.DataSource = ds.Tables[0];
			
			DataGrid1.DataBind();


			Response.Clear();
			Response.Buffer= true;
			Response.ContentType = "application/vnd.ms-excel";
			Response.AddHeader("Content-Disposition", "attachment; filename=export.xls");
			//Response.OutputStream.Write(buffer, 0, buffer.Length);
			Response.Charset = "UTF-8";
			Response.Cache.SetCacheability(HttpCacheability.Private);
			this.EnableViewState = false;

			System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

			this.ClearControls(DataGrid1);
			DataGrid1.RenderControl(oHtmlTextWriter);

			Response.Write(oStringWriter.ToString());


			Response.End();


		}

		
		private void ClearControls(Control control)
		{
			for (int i=control.Controls.Count -1; i>=0; i--)
			{
				ClearControls(control.Controls[i]);
			}

			if (!(control is TableCell))
			{
				if (control.GetType().GetProperty("SelectedItem") != null)
				{
					LiteralControl literal = new LiteralControl();
					control.Parent.Controls.Add(literal);
					try
					{
						literal.Text = (string)control.GetType().GetProperty("SelectedItem").GetValue(control,null);
					}
					catch

					{

					}

					control.Parent.Controls.Remove(control);
				}

				else

					if (control.GetType().GetProperty("Text") != null)
				{
					LiteralControl literal = new LiteralControl();
					control.Parent.Controls.Add(literal);
					literal.Text = (string)control.GetType().GetProperty("Text").GetValue(control,null);
					control.Parent.Controls.Remove(control);
				}
			}
			return;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
