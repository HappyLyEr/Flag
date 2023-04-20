using System;
using System.Collections.Generic;
using System.Text;
using GASystem.DataModel;
using GASystem.BusinessLayer.Utils;
using System.Data;
using System.Text.RegularExpressions;

namespace GASystem.BusinessLayer
{
    public class ProcedureSMTPTemplate
    {
        private string _templateText = string.Empty;
        private int _actionRowId;
        private string _templateName;
        private int _procedureRowId;
      
        private System.Collections.Generic.Dictionary<GADataClass, System.Data.DataRow> valueTables = new Dictionary<GADataClass, System.Data.DataRow>();   
        
        public ProcedureSMTPTemplate(int ActionRowId, string TemplateName)
        {
            _actionRowId = ActionRowId;
            _templateName = TemplateName;
            init();
        }

        private void init() 
        {
            BusinessClass abc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAAction);
            ActionDS ads = (ActionDS)abc.GetByRowId(_actionRowId);

            if (ads.GAAction.Rows.Count == 0)
                throw new GAExceptions.GAException("Can not find action with rowid " + _actionRowId.ToString());
            
            ActionDS.GAActionRow _actionRow;
            _actionRow = (ActionDS.GAActionRow)ads.GAAction.Rows[0];
            if (_actionRow.IsProcedureRowIdNull())
                throw new GAExceptions.GAException("Can not find action with rowid " + _actionRowId.ToString() + " does not have a procedure");

            _procedureRowId = _actionRow.ProcedureRowId;
          //  valueTables.Add(GADataClass.GAAction, _actionRow);
        }

        /// <summary>
        /// get template text from GAProcedureTemplate
        /// </summary>
        public string getTemplateText()
        {
            //opimization, do not reload text if already retrived
            if (_templateText != string.Empty)   
                return _templateText;

            //get template text
            BusinessClass bc = RecordsetFactory.Make(GADataClass.GAProcedureTemplate);
            ProcedureTemplateDS ptDS =  (ProcedureTemplateDS) bc.GetByOwner(new GADataRecord(_procedureRowId, GADataClass.GAProcedure), null);

            foreach (ProcedureTemplateDS.GAProcedureTemplateRow row in ptDS.GAProcedureTemplate.Rows)
                if (row.Name.ToUpper() == _templateName.ToUpper())
                    _templateText = row.Text;

            return _templateText;

        }

        /// <summary>
        /// get template with field values added
        /// </summary>
        public string getExpandedText()
        {
            string template = getTemplateText();

            List<string> elements = findElementReferences(template);

            foreach (string element in elements)
                template = template.Replace("!" + element, getTableValue(element));

            return template;
        }

        /// <summary>
        /// Find all elements of type ![table].[field] in the template
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        private System.Collections.Generic.List<string> findElementReferences(string InputString)
        {
            System.Collections.Generic.List<string> results = new List<string>();

            Match m;
            string HRefPattern = "!(\\[[ a-zA-Z_0-9]*\\].\\[[ a-zA-Z_0-9]*\\])";

            m = Regex.Match(InputString, HRefPattern,
                            RegexOptions.IgnoreCase | RegexOptions.Compiled);
            while (m.Success)
            {
                results.Add(m.Groups[1].Value);
                m = m.NextMatch();
            }
            return results;
        }



       /// <summary>
        /// get value for single table field
        /// </summary>
        /// <param name="Element"></param>
        /// <returns></returns>
        private string getTableValue(string Element) 
        {
            string myElement = Element.Replace("[", string.Empty);
            myElement = myElement.Replace("]", string.Empty);

            string[] elementPair = myElement.Split('.');
            if (elementPair.Length != 2)
                return string.Empty;

            GADataClass currentDataClass = GADataRecord.ParseGADataClass(elementPair[0]);

            try
            {
                if (!valueTables.ContainsKey(currentDataClass))
                    addClassToValueTable(currentDataClass);
            } catch (Exception ex )
            {
                return string.Empty;
            }


            if (valueTables[currentDataClass] == null)
                return string.Empty;

            if (valueTables[currentDataClass].Table.Columns.Contains(elementPair[1]))
                if (valueTables[currentDataClass][elementPair[1]] != DBNull.Value)
                {
                    GASystem.AppUtils.FieldDescription fd = GASystem.AppUtils.FieldDefintion.GetFieldDescription(elementPair[1], currentDataClass.ToString());

                    if (fd == null)
                        return valueTables[currentDataClass][elementPair[1]].ToString();

                   valueTables[currentDataClass][elementPair[1]].ToString();

                    //format date file
                    if (fd.Dataformat.ToUpper().Equals("DATE"))
                        //stringContent = DateTime.Parse(stringContent).ToShortDateString();
                        try
                        {
                            if (fd.ControlType.ToUpper() == "DATETIME")
                                //display date and time
                                return Convert.ToDateTime(valueTables[currentDataClass][elementPair[1]]).ToLongDateString() + " " + Convert.ToDateTime(valueTables[currentDataClass][elementPair[1]]).ToShortTimeString();
                            else
                                return Convert.ToDateTime(valueTables[currentDataClass][elementPair[1]]).ToLongDateString();
                        }
                        catch (System.InvalidCastException e)
                        {
                            //could not cast to datetime, just display standard tostring();
                            return valueTables[currentDataClass][elementPair[1]].ToString();
                        }

                    //check for boolean
                    if (fd.ControlType.ToUpper().Equals("CHECKBOX"))
                    {
                        try
                        {
                            bool boolValue = (bool)valueTables[currentDataClass][elementPair[1]];

                            if (boolValue)
                                return GASystem.AppUtils.Localization.GetGuiElementText("yes");
                            else
                                return GASystem.AppUtils.Localization.GetGuiElementText("no");

                        } catch (System.InvalidCastException e)
                        {
                            //could not cast to datetime, just display standard tostring();
                            return valueTables[currentDataClass][elementPair[1]].ToString();
                        }

                    }


                    //default
                    return valueTables[currentDataClass][elementPair[1]].ToString();

                    
                }
                   
            
            
            return string.Empty;
        }

        private void addClassToValueTable(GADataClass DataClass)
        {
            int MAXNUMBEROFLEVELS = 20;
            //int curretLevel = 0;
            GADataRecord owner = new GADataRecord(_actionRowId, GADataClass.GAAction); //start at known action
            //while (!DataClass.ToString().ToUpper().Equals(owner.DataClass.ToString().ToUpper()))
            //{
            //    owner = DataClassRelations.GetOwner(owner);
            //    curretLevel++;
            //    if (owner == null)
            //        throw new GASystem.GAExceptions.GAException("Owner of type " + DataClass.ToString() + " not found");
            //    if (curretLevel >= MAXNUMBEROFLEVELS)
            //        throw new GASystem.GAExceptions.GAException("Maximun number of recursive searches for ownerclass exceeded");
            //}
            owner = findDataRecordInPathByDataClass(owner, DataClass, MAXNUMBEROFLEVELS);

            if (owner == null)
            {
                valueTables.Add(owner.DataClass, null);
                return;
            }

           // BusinessClass bc = RecordsetFactory.Make(owner.DataClass);
            DataSet ds = RecordsetFactory.GetRecordSetAllDetailsByDataRecord(owner); // bc.GetByRowId(owner.RowId);
            if (ds.Tables[0].Rows.Count == 0)
            {
                valueTables.Add(owner.DataClass, null);
                return;
            }

            valueTables.Add(owner.DataClass, ds.Tables[0].Rows[0]);
        }

        private GADataRecord findDataRecordInPathByDataClass(GADataRecord Owner, GADataClass DataClass, int maxNumbersOfIterations)
        {
            GADataRecord myOwner = new GADataRecord(Owner.RowId, Owner.DataClass);
            List<GADataRecord> linkedRecords = new List<GADataRecord>();
            while (!DataClass.ToString().ToUpper().Equals(myOwner.DataClass.ToString().ToUpper()))
            {
                
                foreach (GADataRecord linkedOwner in ManyToManyLinks.GetManyToManyOwnerDataRecordsByMemberDataRecordAndDate(myOwner, System.DateTime.Now))
                    linkedRecords.Add(linkedOwner);
                myOwner = DataClassRelations.GetOwner(myOwner);
                maxNumbersOfIterations--;
           
                if (maxNumbersOfIterations <= 0)
                    throw new GASystem.GAExceptions.GAException("Maximun number of recursive searches for ownerclass exceeded");

                if (myOwner == null)
                {
                    //if owner is not found in direct path, check linked owner
                    {
                        if (linkedRecords.Count == 0)
                            return null;
                        foreach (GADataRecord linkedOwner in linkedRecords)
                            myOwner = findDataRecordInPathByDataClass(linkedOwner, DataClass, maxNumbersOfIterations);
                    }
                    //throw new GASystem.GAExceptions.GAException("Owner of type " + DataClass.ToString() + " not found");
                }
               
            }
            return myOwner;
        }

    }
}
