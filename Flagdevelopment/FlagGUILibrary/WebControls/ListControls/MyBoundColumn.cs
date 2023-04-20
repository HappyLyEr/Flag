using System;
using System.Web.UI.WebControls;
using GASystem.AppUtils;

namespace GASystem.WebControls.ListControls
{
	public class MyBoundColumn : BoundColumn, IComparable
	{
		private FieldDescription _fieldDefinition;


		public System.Int32 CompareTo ( System.Object obj )
		{
			if ( obj == null) return 1;
			if ( !(obj is MyBoundColumn) )
				throw new ArgumentException(); 
			
			MyBoundColumn boundColumn = (MyBoundColumn) obj;
			return this.FieldDesc.ColumnOrder.CompareTo(boundColumn.FieldDesc.ColumnOrder);
		}


		public FieldDescription FieldDesc
		{
			get
			{
				return _fieldDefinition;
			}
			set
			{
				_fieldDefinition = value;
			}
		}
	}
}