using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using GASystem.GAControls.UtilityControls;
using GASystem.GAControls.EditForm;
using GASystem.BusinessLayer.Utils;
using GASystem.AppUtils;

namespace GASystem.GAControls.EditForm
{
    class ExposedHoursGroupViewDetailsForm : GeneralDetailsForm
    {
        private DateTime dateFrom = DateTime.MinValue;
        private DateTime dateTo = DateTime.MaxValue;
        private int departmentCategory;
        private int vertical;
        private bool fromIsValid = true;
        private PlaceHolder warningPlaceholder;
        private string headerText = "<br/>" + AppUtils.Localization.GetGuiElementText("WarningPersonnelCountOverlaps");

        public ExposedHoursGroupViewDetailsForm(GADataRecord DataRecord)
            : base(DataRecord)
		{
		
		}


        protected override void EditRecordSave(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
        {
            DataSet dataSet = e.CommandDataSetArgument;
            AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(GADataClass.GAExposedHoursGroupView);



            if (!dataSet.Tables[0].Columns.Contains(cd.DateFromField) || !dataSet.Tables[0].Columns.Contains(cd.DateToField))
            {
                throw new GAExceptions.GAException("ExposedHoursGroupView definition is missing date columns");
            }
            
             //get dates
            dateFrom = dataSet.Tables[0].Rows[0][cd.DateFromField] == DBNull.Value ? DateTime.MinValue : (DateTime)dataSet.Tables[0].Rows[0][cd.DateFromField];
            dateTo = dataSet.Tables[0].Rows[0][cd.DateToField] == DBNull.Value ? new DateTime(9000,1,1) : (DateTime)dataSet.Tables[0].Rows[0][cd.DateToField];
            departmentCategory = dataSet.Tables[0].Rows[0]["DepartmentCategoryListsRowId"] == DBNull.Value ? -1 : (int)dataSet.Tables[0].Rows[0]["DepartmentCategoryListsRowId"];
            vertical = dataSet.Tables[0].Rows[0]["bunksinroom"] == DBNull.Value ? -1 : (int)dataSet.Tables[0].Rows[0]["bunksinroom"];
            
            string filter = "DepartmentCategoryListsRowId = " + departmentCategory.ToString();
            filter = filter + " AND bunksinroom = " + vertical.ToString();
            

            BusinessClass bc = RecordsetFactory.Make(GADataClass.GAExposedHoursGroupView);
            int numberOfOverlappingRecords = bc.GetNumberOfRowsByOwnerAndTimeSpan(this.OwnerRecord, dateFrom, dateTo, filter, null);

            if (DataRecord.RowId > 0)               //edit of existing record. own record included in count
				numberOfOverlappingRecords--;

            if (numberOfOverlappingRecords > 0) {
                fromIsValid = false;
                headerText = "<br/>" + AppUtils.Localization.GetGuiElementText("CannotSavePersonnelCountOverlaps");

                
                return;
            }
            fromIsValid = true;
            base.EditRecordSave(sender, e);
        }

        protected override void OnPreRender(EventArgs e)
        {
           
            base.OnPreRender(e);
            if (this.DataRecord.RowId > 0 || !fromIsValid)
                ShowOverLappingRecords();
        }


        private void ShowOverLappingRecords()
        {
            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(DataRecord.DataClass);
            if (!this.Page.IsPostBack)
            {
                BusinessLayer.BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(DataRecord.DataClass);
                DataSet dataSet = bc.GetByRowId(DataRecord.RowId);

                if (!dataSet.Tables[0].Columns.Contains(cd.DateFromField) || !dataSet.Tables[0].Columns.Contains(cd.DateFromField))
                    return;

                //get dates
                dateFrom = dataSet.Tables[0].Rows[0][cd.DateFromField] == DBNull.Value ? DateTime.MinValue : (DateTime)dataSet.Tables[0].Rows[0][cd.DateFromField];
                dateTo = dataSet.Tables[0].Rows[0][cd.DateToField] == DBNull.Value ? new DateTime(9000, 1, 1) : (DateTime)dataSet.Tables[0].Rows[0][cd.DateToField];
                departmentCategory = dataSet.Tables[0].Rows[0]["DepartmentCategoryListsRowId"] == DBNull.Value ? -1 : (int)dataSet.Tables[0].Rows[0]["DepartmentCategoryListsRowId"];
           
            }

            
                
            
            //get raw datarecord
           
            //create filter
            string filter = "DepartmentCategoryListsRowId = " + departmentCategory.ToString();
            filter += " and " + DataRecord.DataClass.ToString().Substring(2) + "rowid <> " + DataRecord.RowId.ToString();
            //check owner
            if (this.OwnerRecord == null)
                OwnerRecord = BusinessLayer.DataClassRelations.GetOwner(DataRecord, null);



            ListData.ListClassByOwnerAndTimeSpan overlappingList = new GASystem.GAControls.ListData.ListClassByOwnerAndTimeSpan(DataRecord.DataClass, this.OwnerRecord, dateFrom, dateTo, filter);

            overlappingList.HeaderText = headerText;


            this.warningPlaceholder.Controls.Add(overlappingList);
            overlappingList.GenerateControl();
            this.warningPlaceholder.Visible = overlappingList.ContainsRecords;
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);


            warningPlaceholder = new PlaceHolder();
            warningPlaceholder.Visible = false;
            this.Controls.Add(warningPlaceholder);
        }

    }
}
