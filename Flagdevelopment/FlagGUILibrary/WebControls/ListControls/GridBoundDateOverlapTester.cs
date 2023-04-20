using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using System.Data;
using GASystem.DataModel;
using GASystem.BusinessLayer;

namespace FlagGUILibrary.WebControls.ListControls
{
    class GridBoundDateOverlapTester : GridBoundColumn
    {
        private string _separator = "<br/>";
        const int MAXPARTICIPANTSINLIST = 5;

        public string Separator
        {
            get
            {
                return _separator;
            }
            set
            {
                _separator = value;
            }
        }

        public override bool ReadOnly
        {
            get
            {
                return true;
            }
            set
            {
                base.ReadOnly = value;
            }
        }

        private string dataClass;

        public string DataClass
        {
            get { return dataClass; }
            set { dataClass = value; }
        }

        private GASystem.AppUtils.ClassDescription cd;

        public GASystem.AppUtils.ClassDescription CD
        {
            get { return cd; }
            set { cd = value; }
        }


        private GADataRecord _owner;

        public GADataRecord Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }


        protected override string FormatDataValue(object dataValue, GridItem item)
        {
            if (cd.DateFromField == string.Empty || cd.DateToField == string.Empty)
                return string.Empty;

            int rowid = -1;

            try
            {
                rowid = (int)dataValue;
            }
            catch
            {
                return string.Empty;
            }

            GADataClass myDataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(cd.DataClassName);

            GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GASystem.DataModel.GADataRecord.ParseGADataClass(cd.DataClassName));
            DataSet ds = bc.GetByRowId(rowid);

            if (!(ds.Tables[0].Rows.Count == 1 && ds.Tables[0].Columns.Contains(cd.DateFromField) && ds.Tables[0].Columns.Contains(cd.DateToField)))
                return string.Empty;

           GADataRecord myOwner = GASystem.BusinessLayer.DataClassRelations.GetOwner(new GADataRecord(rowid, myDataClass));

            System.DateTime fromdt = ds.Tables[0].Rows[0][cd.DateFromField] == DBNull.Value ? new System.DateTime(1800,1,1) : (System.DateTime)ds.Tables[0].Rows[0][cd.DateFromField];
            System.DateTime todt = ds.Tables[0].Rows[0][cd.DateToField] == DBNull.Value ? new System.DateTime(9000,1,1) : (System.DateTime)ds.Tables[0].Rows[0][cd.DateToField];

            int numberOfRecords = 0;

            if (ds.Tables[0].Rows[0]["DepartmentCategoryListsRowId"] != DBNull.Value)
            {
                int departmentCategory = ds.Tables[0].Rows[0]["DepartmentCategoryListsRowId"] == DBNull.Value ? -1 : (int)ds.Tables[0].Rows[0]["DepartmentCategoryListsRowId"];
                int vertical = ds.Tables[0].Rows[0]["bunksinroom"] == DBNull.Value ? -1 : (int)ds.Tables[0].Rows[0]["bunksinroom"];

                //create filter
                string filter = "DepartmentCategoryListsRowId = " + departmentCategory.ToString();
                filter = filter + " AND bunksinroom = " + vertical.ToString();

                numberOfRecords = bc.GetNumberOfRowsByOwnerAndTimeSpan(myOwner, fromdt, todt, filter, null);
            }
            if (numberOfRecords > 1)
                return "<img src=\"images/warning.png\" title=\"This record overlaps with one or more other records\" >";
            else
                return string.Empty;

            
            
        }

       
    }
}
