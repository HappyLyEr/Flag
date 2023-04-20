using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using GASystem.AppUtils;
using FlagGUILibrary.WebControls.ListControls;
using GASystem.GUIUtils;
using System.Web.UI;

namespace GASystem.WebControls.ListControls
{
    class GAColumnItemFactory
    {
        

        public static GridColumn Make(FieldDescription fd, bool readOnly, String baseURL)
        {
            string ControlType = fd.ControlType.ToUpper().TrimEnd();
            string ControlFormat = fd.Dataformat.ToUpper().TrimEnd();
            
            //checkboc
            if (ControlType == "CHECKBOX")
            {
                if (fd.ColumnType == "int")
                {
                    FlagGUILibrary.WebControls.ListControls.GridBoundIntToBoolColumn int2boolColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundIntToBoolColumn();
                    int2boolColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                    int2boolColumn.DataField = fd.FieldId;
                    int2boolColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                    int2boolColumn.SortExpression = fd.FieldId;
                    int2boolColumn.ReadOnly = readOnly;
                    return int2boolColumn;
                }
                else
                {
                    Telerik.WebControls.GridCheckBoxColumn chkboxColumn = new GridCheckBoxColumn();
                    chkboxColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                    chkboxColumn.DataField = fd.FieldId;
                    chkboxColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                    chkboxColumn.SortExpression = fd.FieldId;
                    chkboxColumn.ReadOnly = readOnly;
                    return chkboxColumn;
                }
            }

            //date column
            if (ControlType == "DATE" )
            {
                if (fd.IsReadOnly || readOnly)
                    return CreateBoundColumn(fd, readOnly);
                
                //Telerik.WebControls.GridBoundColumn dateColumn = CreateBoundColumn(fd);
                //dateColumn.DataFormatString = "{0:d}";
                //return dateColumn;
              //  return new GridDateBoundColumn(fd.FieldId);


                GASystem.WebControls.ListControls.GridDateBoundColumn dateColumn = new GASystem.WebControls.ListControls.GridDateBoundColumn(fd.FieldId);
                dateColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                dateColumn.DataField = fd.FieldId;
                dateColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                dateColumn.SortExpression = fd.FieldId;
                
       
                return dateColumn;

            }

            //date column
            if (ControlType == "DATETIME")
            {

                if (fd.IsReadOnly || readOnly)
                    return CreateBoundColumn(fd, readOnly);


                GASystem.WebControls.ListControls.GridDateTimeBoundColumn dateColumn = new GASystem.WebControls.ListControls.GridDateTimeBoundColumn(fd.FieldId);
                dateColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                dateColumn.DataField = fd.FieldId;
                dateColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                dateColumn.SortExpression = fd.FieldId;
                return dateColumn;

            }

            /*
            //lookup, line edit of lookup columns is currently not allowed
            if (ControlType == "LOOKUPFIELD" || ControlType == "LOOKUPFIELDMULTIPLE")
            {
                Telerik.WebControls.GridBoundColumn luColumn = CreateBoundColumn(fd, true);
                luColumn.ReadOnly = true;
                return luColumn;
            }
             */

            //lookup, line edit of lookup columns is currently not allowed
            if (ControlType == "LOOKUPFIELD" || ControlType == "LOOKUPFIELDMULTIPLE")
            {
                // Tor 20151130 JOF assistance - do not show/generate hyperlink if lookuptable="GALists"
                if (fd.LookupTable.ToString()!="GALists")
                    return CreateHyperLinkColumn(fd, false);
            }

            if (ControlType == "LOOKUPFIELDEDIT")
            {
                // Tor 20151130 JOF assistance - do not show/generate hyperlink if lookuptable="GALists"
                if (fd.LookupTable.ToString() != "GALists")
                    return CreateHyperLinkColumn(fd, true);
            }

            //dropdown
            if (ControlType == "DROPDOWNLIST" || ControlType == "POSTBACKDROPDOWNLIST")
            {
                //if (fd.IsReadOnly || readOnly)
                //{
                //    return CreateBoundColumn(fd, readOnly);                
                //}
                //else
                //{
                    GridDropDownColumn ddColumn = new GridDropDownColumn();
                    ddColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                    ddColumn.DataField = fd.FieldId;
                    ddColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                    //ddColumn.l
                    //ddColumn.DataSourceID
                    ddColumn.ListDataMember = "galists_" + fd.ListCategory;
                    ddColumn.ListValueField = "listsrowid";
                    ddColumn.ListTextField = "galistdescription";
                    ddColumn.SortExpression = fd.FieldId + "_displayfield";
                    ddColumn.EnableEmptyListItem = true;
                    ddColumn.EmptyListItemText = string.Empty;
                    ddColumn.EmptyListItemValue = string.Empty;
                    if (fd.IsReadOnly || readOnly)
                    {
                        ddColumn.ReadOnly = true;
                    }
                    return ddColumn;
                //}
            }

           //GridBoundSelectListColumn
            //selectlist
            if (ControlType == "SELECTLIST")
            {
                GridBoundSelectListColumn ddColumn = new GridBoundSelectListColumn();
                ddColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                ddColumn.DataField = fd.FieldId;
                ddColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                ddColumn.DataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(fd.TableId); ;
                //ddColumn.l
                //ddColumn.DataSourceID
                //ddColumn.ListDataMember = "galists_" + fd.ListCategory;
                //ddColumn.ListValueField = "listsrowid";
                //ddColumn.ListTextField = "galistdescription";
                //ddColumn.SortExpression = fd.FieldId + "_displayfield";
                //ddColumn.EnableEmptyListItem = true;
                //ddColumn.EmptyListItemText = string.Empty;
                //ddColumn.EmptyListItemValue = string.Empty;
                ddColumn.ReadOnly = readOnly;
                return ddColumn;
            }


            if (ControlType == "WORKITEMRESPONSIBLE")
            {
                Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundWorkitemParticipantColumn();
                tmpColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = true;  // fd.IsReadOnly && readOnly;
                return tmpColumn;
            }

            if (ControlType == "WORKITEMRESPONSIBLEROLE")
            {
                Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundWorkitemRoleParticipantColumn();
                tmpColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = true; // fd.IsReadOnly && readOnly;
                return tmpColumn;
            }

            if (ControlType == "GADATACLASS")
            {
                Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundGADataClassColumn();
                tmpColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = fd.IsReadOnly && readOnly;
                return tmpColumn;
            }

            if (ControlType == "LOCALIZEDLABEL")
            {
                Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundLocalizedLabelColumn();
                tmpColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = fd.IsReadOnly && readOnly;
                return tmpColumn;
            }



            if (ControlType == "GENERALURL")
            {
                return CreateURLHyperLinkColumn(fd, baseURL);
            }

            if (ControlType == "PATHLOOKUP")
            {
                Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundGAPathLookupColumn(fd);
                tmpColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = fd.IsReadOnly && readOnly;
                return tmpColumn;
            }
            
            if (ControlType == "DATARECORDNAME")
            {
                Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundGADataRecordNameColumn(fd);
                tmpColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = fd.IsReadOnly && readOnly;
                return tmpColumn;
            }

            if (ControlType == "WORKITEMACKNOWLEDGE")
            {
                Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundAcknowledgeTester(fd);
                tmpColumn.HeaderText =  "&nbsp;";
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = true;
                return tmpColumn;
            }

            if (ControlFormat == "DECIMAL2")
            {
                Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GABoundDecimal2Column();
                tmpColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = true;
                return tmpColumn;
            }

            if (ControlFormat == "MINUTES")
            {
                Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GABoundMinutesColumn();
                tmpColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = true;
                return tmpColumn;
            }

            //default control

            if (fd.DataLength == 0 || fd.DataLength == -1 || fd.DataLength > 50)
                return CreateBoundLimitColumn(fd, readOnly);

            return CreateBoundColumn(fd, readOnly);
           // return CreateHyperLinkColumn(fd);
        }
        

        //private static GridHyperLinkColumn CreateHyperLinkColumn(FieldDescription fd)
        //{
        //    GridHyperLinkColumn hypColumn = new GridHyperLinkColumn();
        //    hypColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
        //    hypColumn.DataTextField = fd.FieldId;
        //    hypColumn.DataNavigateUrlFields = new string[] { fd.TableId.Substring(2) + "rowid" };
        //    hypColumn.DataNavigateUrlFormatString = "http://alink/a?{0}";
        //    hypColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
        //    hypColumn.SortExpression = fd.FieldId;
        //    hypColumn.ReadOnly = _readonly;
        //    return hypColumn;
        //}

        private static GridBoundColumn CreateURLHyperLinkColumn(FieldDescription fd, String baseURL)
        {
            FlagGUILibrary.WebControls.ListControls.GridBoundURLColumn tmpColumn = new GridBoundURLColumn();
            tmpColumn.BaseURL = baseURL;
            tmpColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
            tmpColumn.DataField = fd.FieldId;
            tmpColumn.UniqueName = "gridurlcolumn_" + fd.TableId + "_" + fd.FieldId;
            tmpColumn.SortExpression = fd.FieldId;
           // tmpColumn.ReadOnly = fd.IsReadOnly || readOnly;
            return tmpColumn;
            
            
            
            //GridHyperLinkColumn hypColumn = new GridHyperLinkColumn();
            //hypColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
            //hypColumn.DataTextField = fd.FieldId;
            //hypColumn.DataTextFormatString = getURLDescPart("{0}");
            ////hypColumn.DataTextFormatString = "this is a link";
            //hypColumn.DataNavigateUrlFields = new string[] { fd.FieldId };
            //hypColumn.DataNavigateUrlFormatString = getURLPart("{0}");
            ////hypColumn.NavigateUrl =getURLPart("{0}");
            //hypColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
            //hypColumn.SortExpression = fd.FieldId;
        
            //return hypColumn;
        }

        private static string getURLPart(string href) {
            return "urlpart";
            //string tmparef = href.Replace("<a url=\"", "");
            //tmparef = tmparef.Replace("</a>", "");
            //tmparef = tmparef.Replace("\">", "#");
            //string[] aref = tmparef.Split("#".ToCharArray(), StringSplitOptions.None);
            //return tmparef;
            //if (aref.Length == 2)
            //{
            //    return  aref[0];
            //}
            //return href;
            
        }

        private static string getURLDescPart(string href)
        {
            return "descriptpart";
            //string tmparef = href.Replace("<a url=\"", "");
            //tmparef = tmparef.Replace("</a>", "");
            //tmparef = tmparef.Replace("\">", "#");
            //string[] aref = tmparef.Split("#".ToCharArray(), StringSplitOptions.None);
            //if (aref.Length == 2)
            //{
            //    return aref[1];
                
            //}
            //return href;

        }


        private static GridHyperLinkColumn CreateHyperLinkColumn(FieldDescription fd, Boolean editMode)
        {
            GridHyperLinkColumn hypColumn = new GridHyperLinkColumn();
            hypColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
            hypColumn.DataTextField = fd.FieldId;
            hypColumn.DataNavigateUrlFields = new string[] { fd.FieldId + "_keyid" };
            
            if (editMode)
            {
                hypColumn.DataNavigateUrlFormatString = LinkUtils.GenerateURLForSingleRecordDetails(fd.LookupTable, "{0}");
            }
            else
            {
                hypColumn.DataNavigateUrlFormatString = LinkUtils.GenerateSimpleURLForSingleRecordView(fd.LookupTable, "{0}");
            }
//            hypColumn.DataNavigateUrlFormatString = LinkUtils.GenerateSimpleURLForSingleRecordView(fd.LookupTable, "{0}");
            hypColumn.Target = "_blank";
            hypColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
            hypColumn.SortExpression = fd.FieldId;
            //hypColumn.ReadOnly = _readonly;
            return hypColumn;
        }


        private static GridBoundColumn CreateBoundLimitColumn(FieldDescription fd, bool readOnly)
        {
            Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundLimitColumn();
            tmpColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
            tmpColumn.DataField = fd.FieldId;
            tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
            tmpColumn.SortExpression = fd.FieldId;
            tmpColumn.ReadOnly = fd.IsReadOnly || readOnly; ;
            return tmpColumn;
        }

        private static GridBoundColumn CreateBoundColumn(FieldDescription fd, bool readOnly)
        {
            Telerik.WebControls.GridBoundColumn tmpColumn = new GridBoundColumn();
            tmpColumn.HeaderText = breakHeaderLabel(AppUtils.Localization.GetCaptionText(fd.DataType));
            tmpColumn.DataField = fd.FieldId;
            tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
            tmpColumn.SortExpression = fd.FieldId;
            tmpColumn.ReadOnly = fd.IsReadOnly || readOnly; 
            return tmpColumn;
        }



        /// <summary>
        /// Tries to insert a single break <br/> in header label so that long labels uses two or more lines.
        /// </summary>
        /// <param name="tabLabel"></param>
        /// <returns></returns>
        private static string breakHeaderLabel(string tabLabel)
        {
           
            
            if (tabLabel.Length < 15)       //do not break short labels
                return tabLabel;


            tabLabel = tabLabel.Trim();
            string[] words = tabLabel.Split(' ');
            //return if there are no spaces in the string;
            if (words.Length == 1)
                return tabLabel;

            //simple break if two words only
            if (words.Length == 2)
                return tabLabel.Replace(" ", "<br/>");

            //find the middle space
            int indexOfMiddle = tabLabel.Length / 2;
            int currentSpace = words[0].Length;
            int currentBreak = 0;
            int currentBreakDistance = indexOfMiddle;
            int currentIndex = 0;
            for (int t = 0; t < words.Length - 1; t++)
            {
                currentIndex += words[t].Length + 1; //adding 1 in order to compensate for space character;
                if (Math.Abs(indexOfMiddle - currentIndex) < currentBreakDistance)
                {
                    currentBreakDistance = Math.Abs(indexOfMiddle - currentIndex);
                    currentBreak = t;
                }
            }

            //combine new string;
            tabLabel = string.Empty;
            for (int t = 0; t < words.Length; t++)
            {
                tabLabel += words[t];
                if (t == currentBreak)
                    tabLabel += "<br/>";
                else
                    tabLabel += " ";
            }

            return tabLabel;

        }

    }

    
}
