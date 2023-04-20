using System;

namespace GASystem.AppUtils
{
	/// <summary>
	/// Summary description for FlagResourceFactory.
	/// </summary>
	public class FlagResourceFactory
	{
		public FlagResourceFactory()
		{
			
		}
		
		public static FlagResource Make(string LanguageKey)
		{
			return new FlagResource(LanguageKey);
		}
	}
}
