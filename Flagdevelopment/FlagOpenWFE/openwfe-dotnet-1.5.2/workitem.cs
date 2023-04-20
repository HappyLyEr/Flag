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
using System.IO;
using System.Net;
using System.Xml;

//
// $Id: workitem.cs,v 1.15 2005/05/17 10:16:32 jmettraux Exp $
//

namespace openwfe.workitem
{

	//
	// WORKITEMS

	public abstract class Workitem
	{
		public string lastModified = null;
		//public System.Collections.IList flowStack = new System.Collections.ArrayList(0);
		public StringMapAttribute attributes = null;
	}

	public class Launchitem : Workitem
	{
		public string workflowDefinitionUrl = null;
		public System.Collections.IDictionary descriptionMap = null;
	}

	public class Cancelitem : Workitem
	{
		public FlowExpressionId lastExpressionId = null;
		public string participantName = null;
	}

	public class InFlowWorkitem : Cancelitem
	{
		public string dispatchTime = null;
		public Filter filter = null;
		public System.Collections.IList history = null;

		public void AddHistoryItem (string author, string text)
		{
			HistoryItem hi = new HistoryItem();

			hi.author = author;
			hi.text = text;
			hi.date = openwfe.time.Time.ToIsoDate();
			hi.host = System.Net.Dns.GetHostName();

			hi.workflowDefinitionName = this.lastExpressionId.workflowDefinitionName;
			hi.workflowDefinitionRevision = this.lastExpressionId.workflowDefinitionRevision;
			hi.workflowInstanceId = this.lastExpressionId.workflowInstanceId;

			if (this.history == null) 
				this.history = new System.Collections.ArrayList();

			this.history.Add(hi);
		}
	}

	//
	// HEADER

	public class Header
	{
		public FlowExpressionId flowExpressionId = null;
		public StringMapAttribute attributes = null;
		public string lastModified;
		public bool locked = false;
	}

	//
	// FLOW EXPRESSION ID

	public class FlowExpressionId
	{
		public string engineId = null;
		public string initialEngineId = null;
		public string workflowDefinitionUrl = null;
		public string workflowDefinitionName = null;
		public string workflowDefinitionRevision = null;
		public string workflowInstanceId = null;
		public string expressionName = null;
		public string expressionId = null;

		public override int GetHashCode()
		{
			return
				(engineId+
				"."+
				initialEngineId+
				"."+
				workflowDefinitionUrl+
				"."+
				workflowDefinitionName+
				"."+
				workflowDefinitionRevision+
				"."+
				workflowInstanceId+
				"."+
				expressionName+
				"."+
				expressionId).GetHashCode();
		}
	}

	//
	// EXPRESSION (used by 'control')

	public class Expression
	{
		public FlowExpressionId id = null;
		public string applyTime = null;
		public string frozenTime = null;
	}

	//
	// FILTER

	public class Filter
	{
		public const int TYPE_OPEN = 0;
		public const int TYPE_CLOSED = 1;

		public string name = null;
		public int type = -1;
		public bool addAllowed = true;
		public bool removeAllowed = true;
		public System.Collections.IList entryList = null;

		//
		// inner class : FilterEntry

		public class FilterEntry
		{
			public string fieldRegex = null;
			public System.Text.RegularExpressions.Regex compiledFieldRegex = null;
			public string permissions = null;
			public string attributeClassName = null;
		}
	}

	//
	// STORE

	public class Store
	{
		public string name = null;
		public int workitemCount = 0;

		public bool mayRead = false;
		public bool mayWrite = false;
		public bool mayDelegate = false;
	}

	//
	// HISTORY ITEM

	public class HistoryItem
	{
		public string author = null;
		public string date = null;
		public string host = null;
		public string text = null;
		public string workflowDefinitionName = null;
		public string workflowDefinitionRevision = null;
		public string workflowInstanceId = null;
		public string expressionId = null;
	}

	//
	// LAUNCHABLE

	public class Launchable
	{
		public string engineId = null;
		public string url = null;

		public Launchable (string engineId, string url)
		{
			this.engineId = engineId;
			this.url = url;
		}

		public Launchitem generateLaunchitem ()
		{
//			XmlTextReader reader = new XmlTextReader(this.url);
//
//			reader.Read();
//
			Launchitem li;
//
//			while ( ! reader.Name.Equals(Codec.E_WORKFLOW_DEFINITION) && 
//				    ! reader.Name.Equals(Codec.E_LAUNCHITEM))
//			{
//				reader.Read();
//			}
//
//			if (reader.Name.Equals(Codec.E_WORKFLOW_DEFINITION))
//			{
				li = new Launchitem();
				li.workflowDefinitionUrl = this.url;

				System.Diagnostics.Debug.WriteLine(" **** launchitem.wfdUrl : "+li.workflowDefinitionUrl);

//				reader.Close();

				return li;
//			}
//
//			reader.Close();
//
//			XmlDocument doc = new XmlDocument();
//			doc.Load(new XmlTextReader(this.url));
//
//			li = (Launchitem)Codec.Decode(doc.DocumentElement);
//
//			System.Diagnostics.Debug.WriteLine(" **** launchitem.wfdUrl : "+li.workflowDefinitionUrl);
//
//			return li;
		}
	}
}
