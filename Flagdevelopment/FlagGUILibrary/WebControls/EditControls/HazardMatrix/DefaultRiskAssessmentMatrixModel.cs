using System;
using System.Collections;

namespace GASystem.WebControls.EditControls.HazardMatrix
{
	/// <summary>
	/// Summary description for DefaultRiskAssessmentMatrixModel.
	/// </summary>
	public class DefaultRiskAssessmentMatrixModel : RiskAssessmentMatrixModel
	{
		
		protected static int[,] model = new int[,] {
											{2,2,2,2,2},
											{2,2,2,2,2},
											{2,2,2,1,1},
											{2,2,1,1,0},
											{2,1,1,0,0},
											{1,1,0,0,0}
												   };

		protected static AxisInfo[] severityAxis = new AxisInfo[] 
		{
			new AxisInfo("People", new String[] { "No health effect/injury", "Slight health effect/injury", "Minor health effect/injury", "Major health effect/injury", "PTD or 1 to 3 fatalities", "Multiple fatalities" } ), 
			new AxisInfo("Assets", new String[] { "No damage", "Slight damage", "Minor damage", "Localised damage", "Major damage", "Extensive damage" } ),
			new AxisInfo("Environment", new String[] { "No effect", "Slight effect", "Minor effect", "Localised effect", "Major effect", "Massive effect" } )
		};

		protected static AxisInfo probabilityAxis = new AxisInfo(new String[] { "Never heard of in geophysical operations", "Has occured in geophysical operations", "Incident has occured in our company", "Happens several times a year in our company", "Happens several times a year on this vessel or location" } );

		public int getRowCount()
		{
			return model.GetLength(0);
		}

		public int getColumnCount()
		{
			return model.GetLength(1);
		}

		public int getRisk(int row, int column)
		{
			return model[row, column];
		}

		public AxisInfo[] getSeverityAxisLabels()
		{
			return (AxisInfo[])severityAxis.Clone();
		}

		public AxisInfo getProbabilityAxisLabels()
		{
			return probabilityAxis;
		}

	}
	
	public enum Risk { RED, YELLOW, GREEN };
}
