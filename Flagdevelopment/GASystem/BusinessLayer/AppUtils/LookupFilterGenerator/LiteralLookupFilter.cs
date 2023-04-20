using System;
using GASystem.BusinessLayer;
using GASystem.DataModel;

namespace GASystem.AppUtils.LookupFilterGenerator
{
	/// <summary>
	/// Ĭ�ϵİ�����ԭ�ⲻ���Ĵ���LookupFilter��ֻ��GeneralLookupFilterĬ��û�б���LookupFilter��
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
