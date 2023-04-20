using System;
using System.Web.UI.WebControls;

namespace GASystem.GUIUtils
{
	/// <summary>
	/// Helper class for creating HTML tags in form of Literal classes
	/// </summary>
	public class HTMLLiteralTags
	{
		public HTMLLiteralTags()
		{
			//
			// TODO: Add constructor logic here
			//
		}



		private static Literal createSimpleTag(string TagName) 
		{
			Literal tag = new Literal();
			tag.Text = TagName;
			return tag;

			
		}

		/// <summary>
		/// Create the start tag. Uses System.Web.UI.HtmlTextWriterTag as a referece for valid tags to create.
		/// Note. A simple implementation. Does not take into account attributes that migth be required for the tag.
		/// </summary>
		/// <param name="TagName">System.Web.UI.HtmlTextWriterTag enumerator</param>
		/// <returns>Literal class containg the tag</returns>
		public static Literal CreateStartTag(System.Web.UI.HtmlTextWriterTag TagName) 
		{
			return createSimpleTag("<" + TagName.ToString() + ">");
		}

		/// <summary>
		/// Create the start tag with class attribute. Uses System.Web.UI.HtmlTextWriterTag as a referece for valid tags to create.
		/// Note. A simple implementation. Does not take into account attributes that migth be required for the tag.
		/// </summary>
		/// <param name="TagName">System.Web.UI.HtmlTextWriterTag enumerator</param>
		/// <returns>Literal class containg the tag</returns>
		public static Literal CreateStartTag(System.Web.UI.HtmlTextWriterTag TagName, string ClassName) 
		{
			return createSimpleTag("<" + TagName.ToString() + " class=\"" + ClassName + "\">");
		}



		/// <summary>
		/// Create the end tag. Uses System.Web.UI.HtmlTextWriterTag as a referece for valid tags to create.
		/// Note. A simple implementation. Use together with create start tag
		/// </summary>
		/// <param name="TagName">System.Web.UI.HtmlTextWriterTag enumerator</param>
		/// <returns>Literal class containg the tag</returns>
		public static Literal CreateEndTag(System.Web.UI.HtmlTextWriterTag TagName) 
		{
			return createSimpleTag("</" + TagName.ToString() + ">");
		}

		/// <summary>
		/// Create a single <br/> tag
		/// </summary>
		/// <param name="TagName">System.Web.UI.HtmlTextWriterTag enumerator</param>
		/// <returns>Literal class containg the tag</returns>
		public static Literal CreateBRTag()
		{
			return createSimpleTag("<br/>");
		}


		public static Literal CreateTextElement(string Text) 
		{
			Literal tag = new Literal();
			tag.Text = Text;
			return tag;
		}
	}
}
