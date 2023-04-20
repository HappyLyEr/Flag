using System;
using GASystem.DataModel;
using GASystem.AppUtils;
using System.Collections;
using GASystem.BusinessLayer;
using System.Collections.Generic;
using GASystem.BusinessLayer.Utils;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for FilterBuilder.
	/// </summary>
	public class FilterBuilder
	{
		private GADataClass _dataClass;
		private System.Collections.ArrayList filterElements;
		private FilterWebControl _filterWebControl;
	//	private System.Web.HttpContext _context;

        public event EventHandler FilterChanged;

		public FilterBuilder(GADataClass DataClass)
		{
			//
			// TODO: Add constructor logic here
			//
		//	_context = Context;
			_dataClass = DataClass;
			_filterWebControl = new FilterWebControl();
			_filterWebControl.DataClass = DataClass;
			filterElements = new ArrayList();
			FieldDescription[] fds = FieldDefintion.GetFieldDescriptions(_dataClass);
            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);

			foreach (FieldDescription fd in fds) 
			{
				if (!fd.HideInSummary) 
				{
					IFilterElement filterElement = FilterFactory.MakeFilterElement(fd);
					filterElements.Add(filterElement);
					_filterWebControl.AddEditElement(filterElement.EditWebControl);
					_filterWebControl.AddViewElement(filterElement.ViewWebControl);

                    //add filterperiode element after to and from date fields
                    if (fd.FieldId.Trim().ToUpper() == cd.DateToField.Trim().ToUpper())
                    {
                        IFilterElement filterPeriodeElement = new DatePeriodeFilterElement(fd);
                        filterElements.Add(filterPeriodeElement);
                        _filterWebControl.AddEditElement(filterPeriodeElement.EditWebControl);
                        _filterWebControl.AddViewElement(filterPeriodeElement.ViewWebControl);
                    }


				}

                ////check for datefield
                //if (!fd.HideInSummary && fd.FieldId.Trim().ToUpper() == cd.DateField.Trim().ToUpper())
                //{
                //    IFilterElement filterElement = FilterFactory.MakeFilterElement(fd);
                //    filterElements.Add(filterElement);
                //    _filterWebControl.AddEditElement(filterElement.EditWebControl);
                //    _filterWebControl.AddViewElement(filterElement.ViewWebControl);
                //    IFilterElement filterElement2 = FilterFactory.MakeFilterElement(fd);
                //    filterElements.Add(filterElement2);
                //    _filterWebControl.AddEditElement(filterElement2.EditWebControl);
                //    _filterWebControl.AddViewElement(filterElement2.ViewWebControl);
                //}

			}

            //add possible datefield

			//handle set filter clicked event for gui;
			_filterWebControl.SetFilterClicked += new EventHandler(_filterWebControl_SetFilterClicked);
		//	_filterWebControl.ExportToExcelClicked += new EventHandler(_filterWebControl_ExportToExcelClicked);
            _filterWebControl.ResetFilterClicked += new EventHandler(_filterWebControl_ResetFilterClicked);
            _filterWebControl.SaveFilterClicked += new EventHandler(_filterWebControl_SaveFilterClicked);
            _filterWebControl.DropDownFilterSelected += new EventHandler<GASystem.GAGUI.GAGUIEvents.GAEventArgs<int>>(_filterWebControl_DropDownFilterSelected);
            _filterWebControl.DeleteFilterClicked += new EventHandler<GASystem.GAGUI.GAGUIEvents.GAEventArgs<int>>(_filterWebControl_DeleteFilterClicked);
		}

        void _filterWebControl_DeleteFilterClicked(object sender, GASystem.GAGUI.GAGUIEvents.GAEventArgs<int> e)
        {

            if (e.Value != -1)
            {
                BusinessClass bc = RecordsetFactory.Make(GADataClass.GAClassFilter);
                bc.DeleteRow(e.Value);

                //set default filter
                _filterWebControl_ResetFilterClicked(sender, e);



            }
        }

        void _filterWebControl_DropDownFilterSelected(object sender, GASystem.GAGUI.GAGUIEvents.GAEventArgs<int> e)
        {
            BusinessClass bc = RecordsetFactory.Make(GADataClass.GAClassFilter);
            int selectedValue = e.Value;

            ClassFilterDS ds = (ClassFilterDS)bc.GetByRowId(selectedValue);

            _filterWebControl.FilterDropDownSelectedValue = selectedValue;

            clearAllFilters();

            if (ds.GAClassFilter.Rows.Count > 0)
            {
                

                string queryString = ((ClassFilterDS.GAClassFilterRow)ds.GAClassFilter.Rows[0]).FlagWhereStatement;
                string[] queryStringElements = queryString.Split('&');
                //ArrayList elements = new ArrayList();
               
                
                Dictionary<string, string> elements = new Dictionary<string, string>();
                foreach (string element in queryStringElements)
                {
                    if (element.Contains("="))
                    {
                        string[] elementPair = element.Split('=');
                        if (elementPair.Length == 2)
                            elements.Add(elementPair[0], elementPair[1]);                        
                    }
                }

                int maxFilter = 20;
               for (int t = 0; t < maxFilter; t++)
                {
                    if (elements.ContainsKey("ffield" + t.ToString()))
                    {
                        string fName = !elements.ContainsKey("ffield" + t.ToString())  ? string.Empty : elements["ffield" + t.ToString()].ToString();
                        string fCondition = !elements.ContainsKey("fCondition" + t.ToString()) ? string.Empty : elements["fCondition" + t.ToString()].ToString();
                        string fOperator = !elements.ContainsKey("fOperator" + t.ToString())? string.Empty : elements["fOperator" + t.ToString()].ToString();

                        this.SetQueryStringFilter(fName, fOperator, fCondition);
                    }
                }
               


            }
            if (FilterChanged != null)
                FilterChanged(this, EventArgs.Empty);

        }

       

        void _filterWebControl_SaveFilterClicked(object sender, EventArgs e)
        {
            foreach (IFilterElement fe in filterElements)
            {
                fe.GenerateFilterString();
            }

            string viewName = string.Empty;

            foreach (IFilterElement fe in filterElements)
            {
                fe.GenerateFilterString();
                if (fe.ViewWebControl.enabledElement)
                {
                    if (viewName != string.Empty)
                        viewName += ", ";
                    viewName += ((GASystem.GUIUtils.DataFilter.ViewElements.GeneralViewWebControl)fe.ViewWebControl).DisplayString;
                }
            }


            string queryString = this.getQueryStringFilter();
            BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAClassFilter);
            GADataRecord owner = new GADataRecord(ClassDefinition.GetClassDescriptionByGADataClass(_filterWebControl.DataClass).RowId  , GADataClass.GAClass);
            GASystem.DataModel.ClassFilterDS ds = (ClassFilterDS)bc.GetNewRecord(owner);
            ClassFilterDS.GAClassFilterRow row = (ClassFilterDS.GAClassFilterRow)ds.Tables[0].Rows[0];
            row.Comment = viewName;
            row.PersonnelRowId = GASystem.GAGUI.GUIUtils.SessionData.SessionInfo.UserPersonnelRowId;
            row.FlagWhereStatement = queryString;

            bc.SaveNew(ds, owner);

            _filterWebControl.FilterDropDownSelectedValue = ((ClassFilterDS.GAClassFilterRow)ds.GAClassFilter.Rows[0]).ClassFilterRowId;
            
        }

        void _filterWebControl_ResetFilterClicked(object sender, EventArgs e)
        {
            foreach (IFilterElement filterElement in filterElements)
            {
                filterElement.ViewWebControl.SetSession(false, FilterOperator.Equal, string.Empty);
                filterElement.EditWebControl.ResetToDefaulFilter();
                filterElement.EditWebControl.SetDefaultFilter();
                //filterElement.GenerateFilterString();
            }
            SetDefaultFilter();
            if (FilterChanged != null)
                FilterChanged(this, EventArgs.Empty);
        }

		public void SetDefaultFilter() 
		{
			foreach (IFilterElement filterElement in filterElements)
			{
				
				filterElement.EditWebControl.SetDefaultFilter();
				filterElement.GenerateFilterString();
			}

		}


		public void SetQueryStringFilter(string FieldName, string Operator, String Condition) 
		{
			foreach (IFilterElement filterElement in filterElements)
			{
				if (filterElement.EditWebControl.FieldName.ToUpper() == FieldName.ToUpper()) 
				{ 
					filterElement.EditWebControl.SetDefaultFilter(Operator, Condition);
					filterElement.GenerateFilterString();
				}
				
			}

		}


		public string GetFilterString() 
		{
			//TODO iterate through filterelements and generate filter sql
			//return string.Empty;
			string sqlfilter = string.Empty;
			foreach (IFilterElement fe in filterElements) 
			{
				string elementSQL = fe.GetFilterString();
				if (elementSQL != null && elementSQL != string.Empty) 
				{
					if (sqlfilter != string.Empty)
						sqlfilter += " and ";
                    // tor 20150211 added code to replace too many ' when creating DateEqualTo filter - removed again 20150205 after removing start and end ' with space
                    //if (fe.EditWebControl.ConditionOperator == FilterOperator.DateEqualTo.ToString()
                    //    || elementSQL.IndexOf("BETWEEN  ''") > -1)
                    //{ 
                    //    sqlfilter += elementSQL.Replace("''", "'"); 
                    //}
                    //else
                    //{
                        sqlfilter += elementSQL;
                    //}
				}
			}
			return sqlfilter;
		}

        public string getQueryStringFilter()
        {
            int counter = 0;
            string qString = string.Empty;
            foreach (IFilterElement fe in filterElements)
            {
                if (fe.EditWebControl.enabledElement)
                {
                    qString += "&ffield" + counter.ToString() +  "=" + fe.EditWebControl.FieldName;
                    qString += "&fCondition" + counter.ToString() + "=" + fe.EditWebControl.ConditionText;
                    qString += "&fOperator" + counter.ToString() + "=" + fe.EditWebControl.ConditionOperator;
                    counter++;
                }
            }
            if (qString == string.Empty)
                qString = "&nofilter=true";  //no filter is specified, set a flag in the url indicating this
            return qString;
        }

		public FilterWebControl FilterControl 
		{
			get  {return _filterWebControl;}
		}

        private void clearAllFilters()
        {
            foreach (IFilterElement fe in filterElements)
            {
                fe.EditWebControl.enabledElement = false;
                fe.GenerateFilterString();
            }
        }

		private void _filterWebControl_SetFilterClicked(object sender, EventArgs e)
		{
			foreach (IFilterElement fe in filterElements) 
			{
				fe.GenerateFilterString();
			}
		}

		private void _filterWebControl_ExportToExcelClicked(object sender, EventArgs e)
		{
			_filterWebControl.ExportToExcel(GetFilterString(), _dataClass);
		}
	}
}
