using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using GASystem.AppUtils;

namespace GASystem.WebControls.ListControls
{
    class GAColumnExcelItemFactory
    {
        public static GridColumn Make(FieldDescription fd)
        {
            string ControlType = fd.ControlType.ToUpper().TrimEnd();
            string ControlFormat = fd.Dataformat.ToUpper().TrimEnd();
            
            ////checkboc
            //if (ControlType == "CHECKBOX")
            //{
            //    Telerik.WebControls.GridCheckBoxColumn chkboxColumn = new GridCheckBoxColumn();
            //    chkboxColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
            //    chkboxColumn.DataField = fd.FieldId;
            //    chkboxColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
            //    chkboxColumn.SortExpression = fd.FieldId;
            //    return chkboxColumn;
            //}

            //date column
            if (ControlType == "DATE" )
            {
                //Telerik.WebControls.GridBoundColumn dateColumn = CreateBoundColumn(fd);
                //dateColumn.DataFormatString = "{0:d}";
                //return dateColumn;
              //  return new GridDateBoundColumn(fd.FieldId);


                GASystem.WebControls.ListControls.GridDateBoundColumn dateColumn = new GASystem.WebControls.ListControls.GridDateBoundColumn(fd.FieldId);
                dateColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
                dateColumn.DataField = fd.FieldId;
                dateColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                dateColumn.SortExpression = fd.FieldId;
                return dateColumn;

            }

            //date column
            if (ControlType == "DATETIME")
            {
                GASystem.WebControls.ListControls.GridDateTimeBoundColumn dateColumn = new GASystem.WebControls.ListControls.GridDateTimeBoundColumn(fd.FieldId);
                dateColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
                dateColumn.DataField = fd.FieldId;
                dateColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                dateColumn.SortExpression = fd.FieldId;
                return dateColumn;

            }

            //lookup, line edit of lookup columns is currently not allowed
            if (ControlType == "LOOKUPFIELD" || ControlType == "LOOKUPFIELDMULTIPLE")
            {
                Telerik.WebControls.GridBoundColumn luColumn = CreateBoundColumn(fd);
                luColumn.ReadOnly = true;
                return luColumn;
            }
          
            //dropdown
            if (ControlType == "DROPDOWNLIST" || ControlType == "POSTBACKDROPDOWNLIST"
                // Tor 20171215 added DROPDOWNLISTMULTIPLE
                //|| ControlType == "DROPDOWNLISTMULTIPLE"
                )
            {
                GridDropDownColumn ddColumn = new GridDropDownColumn();
                ddColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
                ddColumn.DataField = fd.FieldId;
                ddColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                //ddColumn.l
                //ddColumn.DataSourceID
                ddColumn.ListDataMember = "galists_" + fd.ListCategory;
                ddColumn.ListValueField = "listsrowid";
                ddColumn.ListTextField = "galistdescription";
                ddColumn.SortExpression = fd.FieldId + "_displayfield";
                // Tor 20141215 Jan Ove 2 lines below added to avoid getting first list element if element empty
                ddColumn.EnableEmptyListItem = true;
                ddColumn.EmptyListItemText = "";
                return ddColumn;
            }

            if (ControlType == "WORKITEMRESPONSIBLE")
            {
                FlagGUILibrary.WebControls.ListControls.GridBoundWorkitemParticipantColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundWorkitemParticipantColumn();
                tmpColumn.Separator = ", ";
                tmpColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = fd.IsReadOnly;
                return tmpColumn;
            }

            if (ControlType == "GADATACLASS")
            {
                Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundGADataClassColumn();
                tmpColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = fd.IsReadOnly;
                return tmpColumn;
            }


            if (ControlFormat == "DECIMAL2")
            {
                Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GABoundDecimal2Column();
                tmpColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = true;
                return tmpColumn;
            }

            if (ControlFormat == "MINUTES")
            {
                Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GABoundMinutesColumn();
                tmpColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
                tmpColumn.DataField = fd.FieldId;
                tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
                tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = true;
                return tmpColumn;
            }


            //default control

            //if (fd.DataLength == 0 || fd.DataLength == -1 || fd.DataLength > 50)
            //    return CreateBoundLimitColumn(fd);

            return CreateBoundColumn(fd);
           // return CreateHyperLinkColumn(fd);
        }


        private static GridHyperLinkColumn CreateHyperLinkColumn(FieldDescription fd)
        {
            GridHyperLinkColumn hypColumn = new GridHyperLinkColumn();
            hypColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
            hypColumn.DataTextField = fd.FieldId;
            hypColumn.DataNavigateUrlFields = new string[] { fd.TableId.Substring(2) + "rowid" };
            hypColumn.DataNavigateUrlFormatString = "http://alink/a?{0}";
            hypColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
            hypColumn.SortExpression = fd.FieldId;
           // hypColumn.ReadOnly = fd.IsReadOnly;
            return hypColumn;
        }


        private static GridBoundColumn CreateBoundLimitColumn(FieldDescription fd)
        {
            Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundLimitColumn();
            tmpColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
            tmpColumn.DataField = fd.FieldId;
            tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
            tmpColumn.SortExpression = fd.FieldId;
            tmpColumn.ReadOnly = fd.IsReadOnly;
            return tmpColumn;
        }

        private static GridBoundColumn CreateBoundColumn(FieldDescription fd)
        {
            Telerik.WebControls.GridBoundColumn tmpColumn = new GridBoundColumn();
            tmpColumn.HeaderText = AppUtils.Localization.GetCaptionText(fd.DataType);
            tmpColumn.DataField = fd.FieldId;
            tmpColumn.UniqueName = "gridcolumn_" + fd.TableId + "_" + fd.FieldId;
            tmpColumn.SortExpression = fd.FieldId;
            tmpColumn.ReadOnly = fd.IsReadOnly;
            return tmpColumn;
        }

    }

    
}
