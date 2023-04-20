using System;
using GASystem.BusinessLayer;
using GASystem.DataModel;

namespace GASystem.AppUtils.LookupFilterGenerator
{
	/// <summary>
	/// 默认的按字面原封不动的传递LookupFilter（只因GeneralLookupFilter默认没有保存LookupFilter）
	/// </summary>
	public class LiteralLookupFilter : GeneralLookupFilter
	{
        public LiteralLookupFilter(GADataClass LookupDataClass, GADataRecord LookupOwner, string LookupFilter)
            : base(LookupDataClass, LookupOwner, LookupFilter)
		{
            this.Filter = this.GetLookupfilterFilterPart(LookupFilter);
		}
	}
}
