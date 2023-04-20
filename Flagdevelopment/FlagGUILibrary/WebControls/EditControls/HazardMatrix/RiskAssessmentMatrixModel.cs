using System;

namespace GASystem.WebControls.EditControls.HazardMatrix
{
	/// <summary>
	/// Summary description for RiskAssessmentMatrixModel.
	/// </summary>
	public interface RiskAssessmentMatrixModel
	{
		int getRowCount();
		int getColumnCount();
		int getRisk(int row, int column);
		AxisInfo[] getSeverityAxisLabels();
		AxisInfo getProbabilityAxisLabels();
	}
}

