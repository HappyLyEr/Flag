namespace GASystem.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem;
	using GASystem.AppUtils;
	using GASystem.BusinessLayer;
	using GASystem.GAExceptions;
	using GASystem.DataModel;
	using GASystem.GAGUIEvents;


	/// <summary>
	///		Summary description for SelectTemplate.
	/// </summary>
	public class SelectTemplate : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DropDownList TemplateDropDown;
		protected System.Web.UI.WebControls.Button SelectTemplateButton;
		protected System.Web.UI.WebControls.Label SelectTemplateLabel;
		private GADataRecord _templateRootRecord = new GADataRecord(1, GADataClass.GATemplate);
		private GADataClass _templateDataClass = GADataClass.GACrisis;

		public event GACommandEventHandler TemplateSelected;

		private void Page_Load(object sender, System.EventArgs e)
		{
			SelectTemplateLabel.Text = Localization.GetGuiElementText("SelectTemplate");
			SelectTemplateButton.Text = Localization.GetGuiElementText("SelectTemplate");

			BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(TemplateDataClass);

			if (_templateRootRecord == null)
				_templateRootRecord = new GADataRecord(1, GADataClass.GATemplate);
			DataSet dataSet = bc.GetByOwner(TemplateRootRecord,null);
			if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
			{
				ClassDescription classDesc = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(TemplateDataClass);

				TemplateDropDown.DataSource = dataSet;
				TemplateDropDown.DataTextField = classDesc.ObjectName;
				TemplateDropDown.DataValueField = TemplateDataClass.ToString().Substring(2) + "RowId";
				TemplateDropDown.DataBind();
			}
			
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SelectTemplateButton.Click += new System.EventHandler(this.SelectTemplateButton_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void SelectTemplateButton_Click(object sender, System.EventArgs e)
		{
			if (null!=TemplateSelected)
			{
				GACommandEventArgs args = new GACommandEventArgs();
				int rowId = 0;
				try
				{
					rowId = int.Parse(TemplateDropDown.SelectedItem.Value);
					args.CommandStringArgument = TemplateDropDown.SelectedItem.Value;
					args.CommandDataRecordArgument = new GADataRecord(rowId, TemplateDataClass);
					args.CommandName = "SelectTemplate";
				}
				catch (Exception ex)
				{
					throw new GAException("Error selecting template. Selected template in dropdownlist may not contain a numeric RowId. "+ex.Message, ex);
				}
				TemplateSelected(sender, args);
			}

		}

		public GADataRecord TemplateRootRecord
		{
			get { return _templateRootRecord; }
			set { _templateRootRecord = value; }
		}

		public GADataClass TemplateDataClass
		{
			get { return _templateDataClass; }
			set { _templateDataClass = value; }
		}
	}
}
