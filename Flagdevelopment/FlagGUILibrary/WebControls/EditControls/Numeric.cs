using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.AppUtils;

namespace GASystem.WebControls.EditControls
{
	/// <summary>
	/// Summary description for Numeric.
	/// </summary>
	[ValidationPropertyAttribute("TextValue")]
	public class Numeric : System.Web.UI.WebControls.TextBox, INamingContainer
	{
		
		//private TextBox txtBox;
        // Tor 201611 Security 20161122 (never referenced) private CustomValidator custValidator;
        // Tor 201611 Security 20161122 (never referenced) private RequiredFieldValidator valReq;
        private int _numberBase = 1;

		protected override void OnInit(EventArgs e)
		{
		//	txtBox = new TextBox();
		//	this.Controls.Add(txtBox);
			base.OnInit (e);
		}

        public int NumberBase
        {

            set { _numberBase = value; }
            get { return _numberBase; }
        }


		public int Value 
		{
			get 
			{
				if (!IsNull)
                    return (int)(double.Parse(base.Text)*_numberBase);
					//return int.Parse(base.Text); //int.Parse(txtBox.Text);
				else 
					return 0;  //actors should check IsNull before requesting value, in case they do not, return 0 as a default value;
			}
			set {base.Text = ((double)(((double)value)/_numberBase)).ToString();}
		}


		//for db usage, if value can not be parsed as a int , IsNull = true; (e.g textbox is empty string) 
		public bool IsNull 
		{
			get 
			{
				try 
				{
					double testNumber = double.Parse(base.Text);
					return false;
				}
				catch 
				{
					return true;
				}
			}

		}

		public string TextValue 
		{
			get {return Text;}
		}

//		public override short TabIndex
//		{
//			get
//			{
//				return TabIndex;
//			}
//			set
//			{
//				TabIndex = value;
//			}
//		}


		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>

		private void custValidator_ServerValidate(object source, ServerValidateEventArgs args)
		{
			if (GASystem.AppUtils.GAUtils.IsNumeric(Text))
				args.IsValid = true;
			else
				args.IsValid = false;
		}
	}
}
