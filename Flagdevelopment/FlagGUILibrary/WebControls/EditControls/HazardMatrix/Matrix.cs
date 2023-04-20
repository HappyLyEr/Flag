using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Diagnostics;

namespace GASystem.WebControls.EditControls.HazardMatrix
{
	/// <summary>
	/// Summary description for Matrix.
	/// </summary>
	[DefaultProperty("Text"), 
	ToolboxData("<{0}:Matrix runat=server></{0}:Matrix>")]
	public class Matrix : System.Web.UI.WebControls.WebControl
	{
		public event EventHandler Collapse;
		public event EventHandler ValueChange;

		protected RiskAssessmentMatrixModel _model;
		private int _Value = 0;

		public static string HAZARD_MATRIX_VALUE_CHANGE = "HazardMatrixValueChange";

		public static int BUTTON_WIDTH = 60;
		public static int BUTTON_HEIGHT = 60;
	
		[Bindable(true)]
		public int Value
		{
			get
			{
				object val = _Value;
				return (val == null) ? (int)Risk.GREEN : (int)_Value;
			}

			set
			{
				_Value = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			DebugWrite("OnInit START");
			if (this.Visible)
			{
				CreateMatrix();
			}
			DebugWrite("OnInit END");
		}

		public Matrix()
		{
			DebugWrite("Matrix CONSTRUCTOR");
			_model = new DefaultRiskAssessmentMatrixModel();
		}

		protected virtual void OnCollapse(EventArgs e)
		{
			DebugWrite("OnCollapse START");
			if (Collapse != null)
			{
				Collapse(this, e);
			}
			DebugWrite("OnCollapse END");
		}

		private void Collapse_Click(object sender, System.EventArgs e)
		{
			DebugWrite("Collapse_Click START");
			OnCollapse(e);
			DebugWrite("Collapse_Click END");
		}

		protected virtual void OnValueChange(EventArgs e)
		{
			DebugWrite("OnValueChange START");
			if (ValueChange != null)
			{
				ValueChange(this, e);
			}
			DebugWrite("OnValueChange END");
		}

		private void ValueChange_Perform(object sender, System.EventArgs e)
		{
			DebugWrite("ValueChange_Perform START");
			OnValueChange(e);
			DebugWrite("ValueChange_Perform END");
		}

		protected void CreateMatrix()
		{
			DebugWrite("CreateMatrix START");
			if (_model == null) return;

			Literal startCollapseDiv = new Literal();
			startCollapseDiv.Text = "<div style=\"text-align: right;\">";
			this.Controls.Add(startCollapseDiv);

			LinkButton collapse = new LinkButton();
			collapse.Text = "[Collapse without making changes]";
			collapse.Click +=new EventHandler(Collapse_Click);
			this.Controls.Add(collapse);

			Literal endCollapseDiv = new Literal();
			endCollapseDiv.Text = "</div>";
			this.Controls.Add(endCollapseDiv);

			Label instructions = new Label();
			instructions.Text = "Please select the cell which corresponds to the right severity and effect levels.";
			this.Controls.Add(instructions);

			HtmlTable table = new HtmlTable();
			table.Width = "350";

			AxisInfo[] severityLabels = _model.getSeverityAxisLabels();
			// HACK We choose the first item in the list
			// TODO Implement key/value pairs
			AxisInfo severityLabel = severityLabels[0];

			AxisInfo probabilityLabels = _model.getProbabilityAxisLabels();

			for (int row = 0; row <= _model.getRowCount(); row++)
			{
				HtmlTableRow currentRow = new HtmlTableRow();

				if (row > 0)
				{
					createLabelCell(currentRow, row-1, severityLabel);
				} 
				else 
				{
					createLabelCell(currentRow, severityLabel.GetTitle(), true);
				}

				for (int column = 0; column < _model.getColumnCount(); column++)
				{
					if (row == 0)
					{
						createLabelCell(currentRow, column, probabilityLabels);
					} 
					else 
					{
						createColoredCell(currentRow, row-1, column);
					}
				}
				table.Rows.Add(currentRow);
			}

			this.Controls.Add(table);

			DebugWrite("CreateMatrix END");
		}

		protected void createLabelCell(HtmlTableRow row, String text, bool bold)
		{
			HtmlTableCell cell = new HtmlTableCell();
			cell.EnableViewState = false;
			row.Controls.Add(cell);

			Label lbl = new Label();
			lbl.EnableViewState = false;
			lbl.Text = text;

			if (bold)
				lbl.Style.Add("font-weight", "bold");
			
			cell.Controls.Add(lbl);		
		}

		protected void createLabelCell(HtmlTableRow row, int index, AxisInfo labels)
		{
			createLabelCell(row, labels.GetLabel(index), false);
		}

		protected void createColoredCell(HtmlTableRow currentRow, int row, int column)
		{
			HtmlTableCell currentCell = new HtmlTableCell();
			currentCell.EnableViewState = false;
			ImageButton imageButton = new ImageButton();

			currentCell.BgColor = getCellColor(_model.getRisk(row, column));
				
			imageButton.ImageUrl = "1px.gif";
			imageButton.Style.Add("height", "auto");
			imageButton.Style.Add("width", "auto");
			
			imageButton.CommandName = HAZARD_MATRIX_VALUE_CHANGE;
			imageButton.CommandArgument = _model.getRisk(row, column).ToString();

			imageButton.Click += new ImageClickEventHandler(imageButton_Click);
			currentCell.Controls.Add(imageButton);

			currentRow.Cells.Add(currentCell);
		}

		protected string getCellColor(int risk)
		{
			switch (risk)
			{
			case (int)Risk.RED:
				return "#FF0000";
			case (int)Risk.YELLOW:
				return "#FF9900";
			case (int)Risk.GREEN:
				return "#009900";
			default:
				return "#CCCCCC";
			}
		}

		protected void imageButton_Click(object sender, ImageClickEventArgs e)
		{
			if (sender is ImageButton)
			{
				ImageButton ib = (ImageButton)sender;
				if (ib.CommandName == HAZARD_MATRIX_VALUE_CHANGE) 
				{
					try 
					{
						this.Value = int.Parse(ib.CommandArgument);
					} 
					catch (Exception ex)
					{
						this.Value = -1;
					}
					finally 
					{
						// HACK - notify listeners of value change
						ValueChange_Perform(sender, (EventArgs)e);
					}
				}
			}
		}

		private void DebugWrite(String txt)
		{
			Debug.WriteLine("[Matrix.cs]" + txt);
		}


		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			DebugWrite("Render START");
			base.Render(output);
			DebugWrite("Render END");
		}

	}
}
