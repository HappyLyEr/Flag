using GASystem.AppUtils;
using GASystem.DataAccess.Utils;
using GASystem.DataModel;
using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using GASystem.BusinessLayer.Utils;

namespace GASystem.BusinessLayer
{
    public class ExcelImport
    {
        private GADataClass _gaDataClass;
        private GADataRecord _OwnerDataRecord;

        private string _strDataClassDS
        {
            get
            {
                return this._gaDataClass
                    .ToString()
                    .Substring(2, this._gaDataClass.ToString().Length - 2) + "DS";
            }
        }

        private string[] avoidCtrlTypes =
            {"FORMPAGE", "LABELROW", "LABELGROUPROW", "LOCALIZEDLABEL", "MULTIPARTCHECKLIST", "HAZARDMATRIX"};

        public ExcelImport(GADataClass gacDataClass, GADataRecord ownerDataRecord)
        {
            _gaDataClass = gacDataClass;
            _OwnerDataRecord = ownerDataRecord;
        }

        private ExcelPackage GetExcelPackage(string fileName)
        {
            try
            {
                string NewFileName = GetTempName();
                System.IO.File.Move(fileName, NewFileName);

                FileStream stream = System.IO.File.OpenRead(NewFileName);

                return new ExcelPackage(stream);
            }
            catch (Exception e)
            {
                throw new ImportExcelException(Localization.GetErrorText("ImportExcelWrongFile"));
            }
        }

        private string GetTempName()
        {
            return Path.Combine(File.TemporaryPath, Guid.NewGuid().ToString("N") + ".ImpXlsx.tmp");
        }

        private void ValidateExcelFile(ExcelPackage package)
        {
            if (package == null || package.Workbook.Worksheets.Count <= 0)
            {
                throw new ImportExcelException(Localization.GetErrorText("ImportExcelDamagedFile"));
            }

            ExcelWorksheet sheetXlSX = package.Workbook.Worksheets[1];

            if (sheetXlSX.Dimension.End.Row <= 1)
            {
                throw new ImportExcelException(Localization.GetErrorText("ImportExcelNoRows"));
            }

            //other validations
        }

        private bool ValidateDataExcelFile(DataTable dataTable, FieldDescription fd, ref object value, int rowNum)
        {
            if ((!dataTable.Columns[fd.FieldId].AllowDBNull && value == null) ||
                (fd.RequiredField && value == null))
            {
                throw new ImportExcelException(
                    string.Format(
                        Localization.GetErrorText("ImportExcelFieldValueIsNull"), fd.DataType, rowNum + 1));
            }

            if (value != null)
            {
                if (dataTable.Columns[fd.FieldId].DataType != value.GetType())
                {
                    try
                    {
                        value = Convert.ChangeType(value, dataTable.Columns[fd.FieldId].DataType);
                    }
                    catch
                    {
                        throw new ImportExcelException(
                            string.Format(
                                Localization.GetErrorText("ImportExcelDataTypeMismatch"), fd.DataType,
                                fd.FieldId + ": " + fd.ColumnType, rowNum + 1));
                    }
                }
            }

            if (fd.ColumnType.Contains("char") && fd.DataLength > 0 && value != null && ((string)value).Length > fd.DataLength)
            {
                throw new ImportExcelException(
                    string.Format(
                        Localization.GetErrorText("ImportExcelDataLength"), fd.DataType, rowNum + 1));
            }

            return true;
        }

        private DataSet GetDynamicDataSet()
        {
            Type type =
                Type.GetType("GASystem.DataModel." + this._strDataClassDS + ", GASystem.DataModel");

            if (type == null)
                throw new Exception("No " + this._strDataClassDS + " DataSet is defined");

            return (DataSet) Activator.CreateInstance(type);
            //T dataSet = new T();
        }

        private DataSet FillDataSetFromExcelFile(ExcelPackage package)
        {
            DataSet dataSet = GetDynamicDataSet();

            FieldDescription[] formFields =
                FieldDefintion.GetFieldDescriptionsDetailsForm(this._gaDataClass.ToString(),
                    this._OwnerDataRecord.ToString());

            ExcelWorksheet sheetXlSx = package.Workbook.Worksheets[1];
            DataTable dataTable = dataSet.Tables[0];

            bool atLeastOneRow = false;

            for (int rowNum = 1; rowNum < sheetXlSx.Dimension.End.Row; rowNum++)
            {
                if (AllColumnsEmpty(formFields, sheetXlSx, rowNum + 1))
                    continue;

                this.AddRowToTable(ref dataTable);
                DataRow row = dataTable.Rows[rowNum - 1];

                int cellNum = 1;

                for (int cNo = 0; cNo < formFields.Length; cNo++)
                {
                    FieldDescription fd = formFields[cNo];

                    if (this.NoImportColumn(fd))
                        continue;

                    object value = sheetXlSx.GetValue(rowNum + 1, cellNum++);

                    this.ValidateDataExcelFile(dataTable, fd, ref value, rowNum);

                    if (value == null) continue;

                    atLeastOneRow = true;
                    row[fd.FieldId] = value;
                }
            }

            if (!atLeastOneRow)
            {
                throw new ImportExcelException(Localization.GetErrorText("ImportExcelNoRows"));
            }

            return dataSet;
        }

        private FieldDescription GetFieldDescription(FieldDescription[] allFieldDescriptions, string columnName)
        {
            foreach (FieldDescription fieldDescription in allFieldDescriptions)
            {
                if (string.Equals(fieldDescription.FieldId, columnName, StringComparison.InvariantCultureIgnoreCase))
                    return fieldDescription;
            }

            return null;
        }

        private void AddRowToTable(ref DataTable dataTable)
        {
            DataRow newRow = dataTable.NewRow();

            DataSet newDS = Utils.RecordsetFactory.Make(_gaDataClass).GetNewRecord(_OwnerDataRecord);
            DataRow sourceRow = newDS.Tables[0].Rows[0];

            newRow = BusinessClass.fillDataRow(newRow, sourceRow, this._gaDataClass.ToString());

            dataTable.Rows.Add(newRow);
        }

        private bool AllColumnsEmpty(FieldDescription[] fds, ExcelWorksheet sheetXlSX, int rowNumber)
        {
            bool allEmpty = true;
            for (int i = 1; i < fds.Length; i++)
            {
                FieldDescription fd = fds[i];
                if (NoImportColumn(fd))
                    continue;

                object value = sheetXlSX.GetValue(rowNumber, i);
                if (value is string)
                {
                    if (!string.IsNullOrEmpty((string)value))
                    {
                        allEmpty = false;
                        break;
                    }
                }
                else
                {
                    if (value != null)
                    {
                        allEmpty = false;
                        break;
                    }
                }
            }

            return allEmpty;
        }

        private ExcelImportResult SaveDataSet(DataSet dataSet)
        {
            ExcelImportResult xlsx = new ExcelImportResult(true);

            BusinessClass bc = RecordsetFactory.Make(this._gaDataClass);
            DataSet ds = bc.CommitDataSet(dataSet, this._OwnerDataRecord, true);

            xlsx.Message = GASystem.AppUtils.Localization.GetGuiElementText("ImportExcelSuccessfullyImported");

            return xlsx;
        }
        private bool AvoidListContains(string item)
        {
            if ((object)item == null)
            {
                return false;
            }

            for (int index = 0; index < this.avoidCtrlTypes.Length; ++index)
            {
                if (string.Equals(avoidCtrlTypes[index], item, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        private bool NoImportColumn(FieldDescription fd)
        {
            return (fd == null || fd.HideInNew || fd.HideInDetail || fd.IsReadOnly ||
                    AvoidListContains(fd.ControlType));
        }


        /******************************************************/

        public ExcelImportResult ProcessExcelFileImport(string filePath)
        {
            ExcelImportResult result = new ExcelImportResult(true);
            try
            {
                ExcelPackage xlsPackage = this.GetExcelPackage(filePath);

                ValidateExcelFile(xlsPackage);

                DataSet dataSet = this.FillDataSetFromExcelFile(xlsPackage);

                result = this.SaveDataSet(dataSet);
            }
            catch (Exception e)
            {
                if (e is ImportExcelException)
                {
                    result.Message = e.Message;
                    result.Success = false;
                }
                else
                {
                    throw;
                }
            }

            return result;
        }
    }
}