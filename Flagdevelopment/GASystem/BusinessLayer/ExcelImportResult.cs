using System;
using GASystem.GAExceptions;

namespace GASystem.BusinessLayer
{
    public class ExcelImportResult
    {
        private bool success;
        private string message;

        public ExcelImportResult()
        {

        }
        public ExcelImportResult(bool success)
        {
            this.Success = success;
        }

        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }

    public class ImportExcelException : GAException
    {

        public ImportExcelException(string message) : base(message)
        {

        }
    }
}