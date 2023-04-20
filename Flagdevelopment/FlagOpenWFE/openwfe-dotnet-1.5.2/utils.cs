/*
 * Copyright (c) 2005, John Mettraux, OpenWFE.org
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without 
 * modification, are permitted provided that the following conditions are met:
 *
 * . Redistributions of source code must retain the above copyright notice, this
 * list of conditions and the following disclaimer.  
 *
 * . Redistributions in binary form must reproduce the above copyright notice, 
 * this list of conditions and the following disclaimer in the documentation 
 * and/or other materials provided with the distribution.
 *
 * . Neither the name of the "OpenWFE" nor the names of its contributors may be
 * used to endorse or promote products derived from this software without
 * specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
 * POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Xml;

//
// $Id: utils.cs,v 1.7 2005/05/17 10:16:32 jmettraux Exp $
//

namespace openwfe.workitem
{
	public abstract class Utils
	{
		public static object owfe2dotnet (Attribute a)
		{
			if (a is Base64Attribute) return ((Base64Attribute)a).getBytes();

			if (a is AtomicAttribute) return ((AtomicAttribute)a).GetValue();

			if (a is ListAttribute)
			{
				ListAttribute list = (ListAttribute)a;

				System.Collections.IList result = 
					new System.Collections.ArrayList(list.Count());

				foreach (Attribute aItem in list)
				{
					result.Add(owfe2dotnet(aItem));
				}

				return result;
			}

			if (a is MapAttribute)
			{
				MapAttribute map = (MapAttribute)a;

				System.Collections.IDictionary result =
					new System.Collections.Hashtable();

				foreach (Attribute aKey in map.Keys())
				{
					object oKey = owfe2dotnet(aKey);
					object oValue = owfe2dotnet(map[aKey]);
					result[oKey] = oValue;
				}

				return result;
			}

			return null;
		}

		public static StringMapAttribute dotnet2owfeSmap (System.Collections.IDictionary dict)
		{
			StringMapAttribute smap = new StringMapAttribute();
			fillMap(smap, dict);

			return smap;
		}

		private static void fillMap (MapAttribute map, System.Collections.IDictionary dict)
		{
			foreach (object key in dict.Keys)
			{
				Attribute aKey = dotnet2owfe(key);
				Attribute aValue = dotnet2owfe(dict[key]);
				map[aKey] = aValue;
			}
		}

		public static Attribute dotnet2owfe (object o)
		{
			string s = o.ToString();

			if (o is string) return new StringAttribute(s);
			if (o is int) return new IntegerAttribute(s);
			if (o is long) return new LongAttribute(s);
			if (o is double) return new DoubleAttribute(s);
			if (o is bool) return new BooleanAttribute(s);
			if (o is XmlNode) return new RawXmlAttribute((XmlNode)o);
			if (o is byte[]) return new Base64Attribute((byte[])o);

			if (o is System.Collections.IList)
			{
				ListAttribute result = new ListAttribute();

				foreach (object oo in (System.Collections.IList)o)
				{
					result.Add(dotnet2owfe(oo));
				}

				return result;
			}

			if (o is System.Collections.IDictionary)
			{
				MapAttribute ma = new MapAttribute();
				fillMap(ma, (System.Collections.IDictionary)o);
				return ma;
			}

			return null;
		}
	}
}

namespace openwfe.time
{
	public abstract class Time
	{
		private static string format = "yyyy-MM-dd HH:mm:sszzzz";
		private static System.Globalization.DateTimeFormatInfo dtfi = null;
		
		static Time ()
		{
			dtfi = new System.Globalization.DateTimeFormatInfo();
			dtfi.FullDateTimePattern = format;
		}

		//
		// PUBLIC METHODS

		public static DateTime FromIsoDate (string isoDate)
		{
			return DateTime.Parse(addColon(isoDate), dtfi);
		}

		public static string ToIsoDate (DateTime dt)
		{
			return removeColon(dt.ToString(format));
		}

		public static string ToIsoDate ()
		{
			return ToIsoDate(DateTime.Now);
		}

		//
		// PRIVATE METHODS

		private static string removeColon (string sDate)
		{
			return 
				sDate.Substring(0, sDate.Length-3) + 
				sDate.Substring(sDate.Length-2);
		}

		private static string addColon (string sDate)
		{
			return
				sDate.Substring(0, sDate.Length-2) +
				":" +
				sDate.Substring(sDate.Length-2);
		}
	}
}
