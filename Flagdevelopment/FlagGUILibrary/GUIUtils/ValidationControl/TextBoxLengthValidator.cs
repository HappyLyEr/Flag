using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace GASystem.GUIUtils.ValidationControl
{
	/// <summary>
	/// Summary description for TextBoxLengthValidator.
	/// </summary>
	public class TextBoxLengthValidator : BaseCompareValidator
	{
		public TextBoxLengthValidator()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// Properties
		[Bindable(true), 
		Description("TextBoxLengthValidator_MaximumLength"), 
		Category("Behavior"), 
		DefaultValue(-1)]
		public int MaximumLength
		{
			get
			{
				object MaxLengthVS = this.ViewState["MaximumLength"];
				if (MaxLengthVS != null)
				{
					return (int) MaxLengthVS;
				}
				return -1;
			}
			set
			{
				this.ViewState["MaximumLength"] = value;
			}
		}
 
		protected override bool EvaluateIsValid()
		{
			if (this.MaximumLength < 0)
				return true;

			string ControlToValidateName = 
				base.GetControlValidationValue(base.ControlToValidate);

			return ControlToValidateName.Length <= 
				System.Convert.ToInt32(this.MaximumLength);
		}
	}
}
