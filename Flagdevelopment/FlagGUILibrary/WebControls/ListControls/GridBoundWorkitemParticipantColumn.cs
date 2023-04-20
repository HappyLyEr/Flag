using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using System.Data;

namespace FlagGUILibrary.WebControls.ListControls
{
    class GridBoundWorkitemParticipantColumn : GridBoundColumn
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

        public override bool ReadOnly    //enforce readonly
        {
            get
            {
                return true;
            }
            set
            {
                base.ReadOnly = true;
            }
        }

        protected override string FormatDataValue(object dataValue, GridItem item)
        {
            
            
            string val = base.FormatDataValue(dataValue, item);


            string participantIds = string.Empty;
            if (val.Length > 2)
            {
                participantIds = val.Substring(1, val.Length - 2);
                participantIds = participantIds.Replace(";", ",");
            }
            DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForRowIds(GASystem.DataModel.GADataClass.GAPersonnel, participantIds);

            string resp = string.Empty;
            if (ds.Tables[0].Rows.Count == 1)
            {
                resp = ds.Tables[0].Rows[0]["FamilyName"] + " " + ds.Tables[0].Rows[0]["GivenName"];
            } 
            else
            {
                for (int t = 0; t < ds.Tables[0].Rows.Count && t < MAXPARTICIPANTSINLIST; t++)
                {
                    DataRow row = ds.Tables[0].Rows[t];
                    resp += row["FamilyName"] + " " + row["GivenName"] + _separator;
                }
            }
            if (ds.Tables[0].Rows.Count > MAXPARTICIPANTSINLIST)
                resp += "...";
            
            
            return resp;
        }

       
    }
}
