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
// $Id: attributes.cs,v 1.7 2005/05/03 15:09:53 jmettraux Exp $
//

namespace openwfe.workitem
{
	public interface Attribute
	{
	}

	public class AtomicAttribute : Attribute
	{
		protected object val = null;

		public object GetValue () { return this.val; }

		public override string ToString ()
		{
			return ""+this.val;
		}

		public override int GetHashCode ()
		{
			return this.val.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;

			if (obj.GetType() != this.GetType()) return false;

			return this.val.Equals(((AtomicAttribute)obj).GetValue());
		}


	}

	public class StringAttribute : AtomicAttribute
	{
		public StringAttribute (string val)
		{
			this.val = val;
		}

		public string Value
		{
			get
			{
				return (string)this.val;
			}
			set
			{
				this.val = value;
			}
		}
	}

	public class Base64Attribute : StringAttribute
	{
		public Base64Attribute (string val):
			base(val)
		{
		}

		public Base64Attribute (byte[] data):
			base(System.Convert.ToBase64String(data))
		{
		}

		public byte[] getBytes ()
		{
			return System.Convert.FromBase64String(this.Value);
		}
	}

	public class RawXmlAttribute : AtomicAttribute
	{
		public RawXmlAttribute (XmlNode node)
		{
			this.val = node;
		}

		public XmlNode Value
		{
			get 
			{
				return (XmlNode)this.val;
			}
			set
			{
				this.val = value;
			}
		}
	}

	public class IntegerAttribute : AtomicAttribute
	{
		public IntegerAttribute (int val)
		{
			this.val = val;
		}

		public IntegerAttribute (string val)
		{
			this.val = int.Parse(val);
		}

		public int Value
		{
			get
			{
				return (int)this.val;
			}
			set
			{
				this.val = value;
			}
		}
	}

	public class LongAttribute : AtomicAttribute
	{
		public LongAttribute (long val)
		{
			this.val = val;
		}

		public LongAttribute (string val)
		{
			this.val = long.Parse(val);
		}

		public long Value
		{
			get
			{
				return (long)this.val;
			}
			set
			{
				this.val = value;
			}
		}
	}

	public class DoubleAttribute : AtomicAttribute
	{
		public DoubleAttribute (double val)
		{
			this.val = val;
		}

		public DoubleAttribute (string val)
		{
			this.val = double.Parse(val);
		}

		public double Value
		{
			get
			{
				return (double)this.val;
			}
			set
			{
				this.val = value;
			}
		}
	}

	public class BooleanAttribute : AtomicAttribute
	{
		public BooleanAttribute (bool val)
		{
			this.val = val;
		}

		public BooleanAttribute (string val)
		{
			this.val = bool.Parse(val);
		}

		public bool Value
		{
			get
			{
				return (bool)this.val;
			}
			set
			{
				this.val = value;
			}
		}
	}

	public interface CollectionAttribute : Attribute
	{
	}

	public class ListAttribute : CollectionAttribute
	{
		protected System.Collections.IList list = 
			new System.Collections.ArrayList();

		public ListAttribute ()
		{
		}

		public void Add (Attribute a)
		{
			this.list.Add(a);
		}

		public void Remove (Attribute a)
		{
			this.list.Remove(a);
		}

		public int Count ()
		{
			return this.list.Count;
		}

		public Attribute this [int index]
		{
			get
			{
				return (Attribute)this.list[index];
			}
			set
			{
				this.list[index] = value;
			}
		}

		public System.Collections.IEnumerator GetEnumerator ()
		{
			return this.list.GetEnumerator();
		}

		public override int GetHashCode ()
		{
			return this.list.GetHashCode();
		}
	}

	public class MapAttribute : CollectionAttribute
	{
		protected System.Collections.IDictionary map = 
			new System.Collections.Hashtable();

		public MapAttribute ()
		{
		}

		public Attribute this [Attribute key]
		{
			get
			{
				return (Attribute)this.map[key];
			}
			set
			{
				this.map[key] = value;
			}
		}

		public System.Collections.ICollection Keys ()
		{
			return this.map.Keys;
		}

		public void Remove (Attribute key)
		{
			this.map.Remove(key);
		}

		public override int GetHashCode ()
		{
			return this.map.GetHashCode();
		}

	}

	public class StringMapAttribute : MapAttribute
	{
		public Attribute this [StringAttribute key]
		{
			get
			{
				return (Attribute)this.map[key];
			}
			set
			{
				this.map[key] = value;
			}
		}
	}
}
