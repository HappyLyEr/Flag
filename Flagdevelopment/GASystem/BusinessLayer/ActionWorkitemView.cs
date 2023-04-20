using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace GASystem.BusinessLayer
{
    public class ActionWorkitemView : GenericBusinnesClass
    {
        Hashtable _workitemMappings = new Hashtable();

        public  ActionWorkitemView() : base(GASystem.DataModel.GADataClass.GAActionWorkitemView)
        {
            //create workitemmappings
            _workitemMappings.Add("workitemtype", "TextFree1");
            _workitemMappings.Add("PriorityListsRowId", "IntFree1");
            _workitemMappings.Add("DateEndEstimated", "DateTimeFree1");
        }
        

   
        public override void UpdateFromList(System.Collections.Hashtable updatedValues)
        {
            int rowid = 0;
            try
            {
                string rowidColumn = this.DataClass.ToString().Substring(2) + "rowid";
                rowid = (int)updatedValues[rowidColumn];
            }
            catch (Exception ex)
            {
                throw new GAExceptions.GAException("Error getting actionworkitemrowid");
            }

            //if rowid is -1, then the workflow is not started yet, update gaaction
            if (rowid == -1)
                updateActionFromList(updatedValues);
            else 
                updateWorkitemFromList(updatedValues, rowid);   //update workitem

            //any other values, ignore
        }



        private void updateWorkitemFromList(System.Collections.Hashtable updatedValues, int WorkitemRowId)
        {
            BusinessClass wbc = Utils.RecordsetFactory.Make(GASystem.DataModel.GADataClass.GAWorkitem);
            
            //remove keyvalues from hashtable
            if (updatedValues.ContainsKey("Actionrowid"))
                updatedValues.Remove("Actionrowid");
            if (updatedValues.ContainsKey(this.DataClass.ToString().Substring(2) + "rowid"))
                updatedValues.Remove(this.DataClass.ToString().Substring(2) + "rowid");

        
            System.Data.DataSet ds = wbc.GetByRowId(WorkitemRowId);
           
            foreach (DictionaryEntry entry in updatedValues)
            {
                
                    string columnName = entry.Key.ToString();
                    if (_workitemMappings.ContainsKey(columnName))
                        columnName = _workitemMappings[columnName].ToString();

                    if (ds.Tables[0].Columns.Contains(columnName))
                    {

                        if (entry.Value != null && entry.Value.ToString() != string.Empty)
                            ds.Tables[0].Rows[0][columnName] = entry.Value;
                        else
                            ds.Tables[0].Rows[0][columnName] = DBNull.Value;
                    }
               
            }
            wbc.CommitDataSet(ds);
        }

        private void updateActionFromList(System.Collections.Hashtable updatedValues)
        {

            int actionRowId = (int)updatedValues["Actionrowid"];
            
            BusinessClass abc = Utils.RecordsetFactory.Make(GASystem.DataModel.GADataClass.GAAction);

            //remove keyvalues from hashtable
            if (updatedValues.ContainsKey("Actionrowid"))
                updatedValues.Remove("Actionrowid");
            if (updatedValues.ContainsKey(this.DataClass.ToString().Substring(2) + "rowid"))
                updatedValues.Remove(this.DataClass.ToString().Substring(2) + "rowid");


            System.Data.DataSet ds = abc.GetByRowId(actionRowId);

            foreach (DictionaryEntry entry in updatedValues)
            {

                string columnName = entry.Key.ToString();

                if (ds.Tables[0].Columns.Contains(columnName))
                {

                    if (entry.Value != null && entry.Value.ToString() != string.Empty)
                        ds.Tables[0].Rows[0][columnName] = entry.Value;
                    else
                        ds.Tables[0].Rows[0][columnName] = DBNull.Value;
                }

            }
            abc.CommitDataSet(ds);
        }
    }
}
