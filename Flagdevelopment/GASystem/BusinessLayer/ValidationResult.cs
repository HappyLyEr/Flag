using System;
using System.Collections;


namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for ValidationResult.
	/// </summary>
	public class ValidationResult
	{
		private bool _isValid;
		private ArrayList _validationErrorMessages;

		public ValidationResult()
		{
			_isValid = true; //the inital validationresult is valid because there are no _validationErrorMessages
			_validationErrorMessages = new ArrayList();
		}

		public void AddValidationErrorMessage(String message)
		{
			_isValid = false; //if a validation message is added, the validation is not valid!
			if (null==_validationErrorMessages)
				_validationErrorMessages = new ArrayList();
	
			_validationErrorMessages.Add(message);

		}

		public bool IsValid
		{
			get
			{
				return _isValid;
			}
		}

		public ArrayList ValidationErrorMessages
		{
			get
			{
				return _validationErrorMessages;
			}
		}
		
	}
}
