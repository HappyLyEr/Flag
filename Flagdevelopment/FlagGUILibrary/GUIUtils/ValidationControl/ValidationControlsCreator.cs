using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using GASystem.AppUtils;

namespace GASystem.GUIUtils.ValidationControl
{
	/// <summary>
	/// Summary description for ValidationControlsCreator.
	/// </summary>
	public class ValidationControlsCreator
	{
		private Control _placeHolder;
		private FieldDescription _fd;
		private int _controlId = 0; 
		bool _enableClientSideValidation = true;

		public ValidationControlsCreator()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private string getNextControlId() 
		{
			_controlId++;
			return _fd.FieldId + "_validator" + _controlId.ToString();
		}

		private ValidationDataType parseValidationDataType(string DataFormat) 
		{
			try 
			{
				ValidationDataType validationDataTypeEnum = (ValidationDataType) Enum.Parse(typeof(ValidationDataType), DataFormat,true);
				return validationDataTypeEnum;
			}
			catch (ArgumentException ex) 
			{
				//invalid datatype in DataFormat. return string type as default
				return ValidationDataType.String;
			}
		}

		private ValidationCompareOperator parseValidationOperator(string Operator) 
		{
			try 
			{
				ValidationCompareOperator validationOperatorEnum = (ValidationCompareOperator) Enum.Parse(typeof(ValidationCompareOperator), Operator,true);
				return validationOperatorEnum;
			}
			catch (ArgumentException ex) 
			{
				//invalid datatype in Operator. return notequal as default
				return ValidationCompareOperator.NotEqual;
			}
		}

		public ValidationControlsCreator(Control ContainerControl, FieldDescription FieldDescriptionInfo )
		{
			_fd = FieldDescriptionInfo;
			_placeHolder = ContainerControl;			
			
		}

		public void AddRequiredFieldValidator() 
		{
			RequiredFieldValidator valReq = new RequiredFieldValidator();
			
			valReq.ID = getNextControlId();
			_placeHolder.Controls.Add(valReq);
			//valReq.Text = "*";
			valReq.ControlToValidate = _fd.FieldId;
			valReq.ErrorMessage = "<br/>" +  Localization.GetErrorText("FieldRequired");
			valReq.Display = ValidatorDisplay.Dynamic;
			
			valReq.EnableClientScript = _enableClientSideValidation;

            if (_fd.ControlType.ToUpper() == "DROPDOWNLIST" || _fd.ControlType.ToUpper() == "POSTBACKDROPDOWNLIST")
                valReq.InitialValue = "0";    //int 0 is used to indicate blank values for dropdowns

					
		}

		public void AddCompareToTypeValidator() 
		{ 
			CompareValidator compVal = new CompareValidator();
			compVal.ID = getNextControlId();
			_placeHolder.Controls.Add(compVal);
			compVal.ControlToValidate = _fd.FieldId;
			compVal.Operator = ValidationCompareOperator.DataTypeCheck;
			compVal.Type = parseValidationDataType(_fd.Dataformat);
			compVal.ErrorMessage ="<br/>" +  string.Format(Localization.GetErrorText("InvalidFieldDataType"), _fd.Dataformat);
			compVal.Display  = ValidatorDisplay.Dynamic;
			compVal.EnableClientScript = _enableClientSideValidation;	
		}


		public void AddCompareToFieldValidator()
		{
			CompareValidator compVal = new CompareValidator();
			compVal.ID = getNextControlId();
			_placeHolder.Controls.Add(compVal);
			compVal.ControlToValidate = _fd.FieldId;
			compVal.Operator = parseValidationOperator(_fd.CompareOperator);
			compVal.Type = parseValidationDataType(_fd.Dataformat);
			compVal.ControlToCompare =_fd.CompareToField;
			compVal.Display  = ValidatorDisplay.Dynamic;;
			compVal.EnableClientScript = _enableClientSideValidation;
			compVal.ErrorMessage = "<br/>" +  string.Format(Localization.GetErrorText("FieldCompareFailed"), 
				new Object[] {  Localization.GetCaptionText(_fd.DataType),
								Localization.GetGuiElementText(_fd.CompareOperator),
								Localization.GetCaptionText(_fd.CompareToField) });   //TODO get datatype for this field
		}


        private void AddCompareToValueValidator()
        {
            if (!(_fd.CompareToField.IndexOf("<%value=") == 0))
                return;

            CompareValidator compVal = new CompareValidator();
            compVal.ID = getNextControlId();
            _placeHolder.Controls.Add(compVal);
            compVal.ControlToValidate = _fd.FieldId;
            string compareValue = _fd.CompareToField.TrimEnd().Replace("<%value=", "").Replace("%>", "");
            compVal.ValueToCompare = compareValue;

            compVal.Operator = parseValidationOperator(_fd.CompareOperator);
            compVal.Type = parseValidationDataType(_fd.Dataformat);

            compVal.Display = ValidatorDisplay.Dynamic; ;
            compVal.EnableClientScript = _enableClientSideValidation;
            compVal.ErrorMessage = "<br/>" + string.Format(Localization.GetErrorText("FieldCompareFailed"),
                new Object[] {  Localization.GetCaptionText(_fd.DataType),
								Localization.GetGuiElementText(_fd.CompareOperator),
								compareValue });
        }


		public void AddLengthValidator() 
		{
			ValidationControl.TextBoxLengthValidator lengthVal = new TextBoxLengthValidator();
			lengthVal.ID =  getNextControlId();
			_placeHolder.Controls.Add(lengthVal);
			lengthVal.MaximumLength =  _fd.DataLength;
			lengthVal.ControlToValidate = _fd.FieldId;
			lengthVal.ErrorMessage = "<br/>" + string.Format(Localization.GetErrorText("LengthValidation"), _fd.DataLength.ToString());
			lengthVal.Display = ValidatorDisplay.Dynamic;
			lengthVal.EnableClientScript = _enableClientSideValidation;
			
		}


		public void AddAllValidatorControls() 
		{
			if (_fd.RequiredField)
				AddRequiredFieldValidator(); 
			if (_fd.Dataformat != string.Empty)
				AddCompareToTypeValidator();
            if (_fd.CompareOperator != string.Empty && _fd.CompareToField != string.Empty && !_fd.CompareToField.Contains("<%value="))
				AddCompareToFieldValidator();
            if (_fd.CompareOperator != string.Empty && _fd.CompareToField != string.Empty && _fd.CompareToField.Contains("<%value="))
                AddCompareToValueValidator();
			if (_fd.DataLength > 0 && _fd.ControlType.ToUpper() == "TEXTAREA" && _fd.DataLength != 8)   //datalenght is 8 for text and ntext (16 / 2). We are not adding a lenght validator for ntext and text
				AddLengthValidator();
		}

       

		public void AddAllValidatorControls(bool EnableClientSideValidation) 
		{
			_enableClientSideValidation = EnableClientSideValidation;
			AddAllValidatorControls();
		}


	}
}
