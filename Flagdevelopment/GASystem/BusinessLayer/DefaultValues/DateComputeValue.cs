using System;
using System.Collections.Generic;
using System.Text;

namespace GASystem.BusinessLayer.DefaultValues
{
    // Tor 20140604 : Included in project
    class DateComputeValue : IDefaultValue
    {
        GASystem.AppUtils.FieldDescription _fd;
        
        public DateComputeValue (GASystem.AppUtils.FieldDescription fd)
        {
            _fd = fd;
        }

        #region IDefaultValue Members

        public object GetValue()
        {
            // Tor 20140604 compute new date based on local or UTC time
            // example: <%datecompute;UTC;add;day;10%> (parameters: base (UTC/Local), operator (add/subtract), unit (minute,hour,day,week,month,year),number of units))
            string avalue = _fd.CopyFromFieldId.ToLower();
            avalue = avalue.TrimEnd().Replace("<%datecompute;", "").Replace("%>", "");
            string[] values = avalue.Split(';');
            if (values.Length != 4)
            {
                throw new GASystem.GAExceptions.GAValidationException("Error in datecomputation definition", new Exception());
            }
            string basedate = values[0];
            string operand = values[1];
            string unit = values[2];
            string quantity = values[3];

            DateTime now = (basedate == "utc") ? System.DateTime.UtcNow : System.DateTime.Now ;

            int numberOfUnits;
            bool isNumeric = int.TryParse(quantity, out numberOfUnits);
            if (!isNumeric) return now;

            if (operand == "subtract") numberOfUnits = numberOfUnits * -1;

            if (unit == "minute") return now.Date.AddMinutes(numberOfUnits);
            if (unit == "hour") return now.Date.AddHours(numberOfUnits);
            if (unit == "day") return now.Date.AddDays(numberOfUnits);
            if (unit == "week") return now.Date.AddDays(numberOfUnits * 7);
            if (unit == "month") return now.Date.AddMonths(numberOfUnits);
            if (unit == "year") return now.Date.AddYears(numberOfUnits);
            // default
            return now;
        }
/*
            try
        {
            numVal = Convert.ToInt32(quantity);
        }
        catch (FormatException e)
        {
            Console.WriteLine("Input string is not a sequence of digits.");
        }
 
            int units=ToInt32(quantity);


            throw new Exception("The method or operation is not implemented.");
        }
            */
        #endregion
        /*
        private System.DateTime getBaseDate(string basedate )
        {
            IDefaultValue myDate = null;
            if (basedate.Trim() == "datetimenow")
            {
                myDate = new DateTimeNowValue();
            }
            if (myDate == null) throw new GASystem.GAExceptions.GAValidationException("Error in datecomputation definition", new Exception());

            object myGetValue = myDate.GetValue();
            try
            {
                return (System.DateTime)myGetValue;
            }
            catch (System.InvalidCastException ex)
            {
                throw new GASystem.GAExceptions.GAValidationException("Date null or not date field", new Exception());
                
            }
               
        }
        private int getQuantity(string quantity)
        { 
        // if quantity not numeric throw
            // else return value
        }
         */

    }
}
