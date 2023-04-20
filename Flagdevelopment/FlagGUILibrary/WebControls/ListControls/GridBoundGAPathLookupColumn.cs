using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using GASystem.AppUtils;
using GASystem.DataModel;
using System.Data;

namespace FlagGUILibrary.WebControls.ListControls
{
    class GridBoundGAPathLookupColumn : GridBoundColumn
    {
        private int characterLimit = 50;
        FieldDescription _fd;
        GADataClass targetDataClass = GADataClass.NullClass;
        List<GADataClass> targetDataClasses = new List<GADataClass>();

        
        public GridBoundGAPathLookupColumn(FieldDescription fd) : base() 
        {
            _fd = fd;
            try
            {
                targetDataClass = GADataRecord.ParseGADataClass(fd.LookupTable);
            }
            catch (System.ArgumentException ex)
            {
                targetDataClass = GADataClass.NullClass;
            }
            targetDataClasses.Add(targetDataClass);
            if (targetDataClass == GADataClass.GALocation)
                targetDataClasses.Add(GADataClass.GACrew);     //hack, consider gacrew a location, TODO support multiple gadataclasses defined in fielddef
        }

        public int CharacterLimit
        {
            get { return characterLimit; }
            set { characterLimit = value; }
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



        protected override string FormatDataValue(object dataValue, GridItem item)
        {
            if (dataValue == null)
                return string.Empty;

            string[] myPaths = dataValue.ToString().Split('/');

            foreach(string myPath in myPaths)
                foreach (GADataClass target in targetDataClasses)
                {
                    if (myPath.Contains(target.ToString()))
                    {
                        string[] locationinfo = myPath.Split('-');
                        if (locationinfo.Length == 2)
                        {
                            try
                            {
                                return getNameForDataRecord(new GADataRecord(int.Parse(locationinfo[1]), GADataRecord.ParseGADataClass(locationinfo[0])));
                            }
                            catch (Exception ex)
                            {
                                return string.Empty;
                            }
                        }
                        else return string.Empty;

                    }

                }
                
                
              

           return string.Empty;
        }


        /// <summary>
        /// Get name of given datarecord. 
        /// TODO this method is copied from datacontectinfo.ascx.cs. refactor into a seperate business class
        /// </summary>
        /// <param name="DataRecord"></param>
        /// <returns></returns>
        private string getNameForDataRecord(GADataRecord DataRecord)
        {
            string ownerName;
            DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsByDataRecord(DataRecord);
            GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataRecord.DataClass);

            if (cd.ObjectName != string.Empty)
            {
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains(cd.ObjectName))
                    ownerName = ds.Tables[0].Rows[0][cd.ObjectName].ToString();
                else
                    ownerName = "";

            }
            else
            {
                ownerName = "";
            }
            return ownerName;
        }

    }
}
