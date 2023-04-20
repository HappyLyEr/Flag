using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.AppUtils;
using GASystem.GAExceptions;
using GASystem.BusinessLayer.Utils;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using log4net;

namespace GASystem.GAControls.EditForm
{
	/// <summary>
	/// Summary description for CreateMultipleRecordsForm.
	/// </summary>
	public class CreateMultipleRecordsForm : GeneralDetailsForm
	{
		public CreateMultipleRecordsForm(GADataRecord DataRecord) : base(DataRecord)
		{
		//
		// TODO: Add constructor logic here
		//
				
		}

		protected override EditDataRecord GetEditFormControl()
		{
			return (EditDataRecord)this.Page.LoadControl("~/gagui/UserControls/CreateMultipleRecords.ascx");
		}


		
	}
}
