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
using System.Xml;
using System.Collections;

//
// $Id: Codec.cs,v 1.28 2005/05/17 10:16:32 jmettraux Exp $
//

namespace openwfe.workitem
{
	public abstract class Codec
	{
		//
		// CONSTANTS

		public const string E_SESSION = "session";
		public const string A_SESSION_ID = "id";
		public const string E_STORES = "stores";
		public const string E_STORE = "store";
		public const string A_NAME = "name";
		public const string A_WORKITEM_COUNT = "workitem-count";
		public const string A_PERMISSIONS = "permissions";
		public const string E_HEADERS = "headers";
		public const string E_HEADER = "header";
		public const string E_FLOW_EXPRESSION_ID = "flow-expression-id";
		public const string E_FLOW_EXPRESSION_IDS = "flow-expression-ids";
		public const string A_LAST_MODIFIED = "last-modified";
		public const string A_LOCKED = "locked";
		public const string A_ENGINE_ID = "engine-id";
		public const string A_INITIAL_ENGINE_ID = "initial-engine-id";
		public const string A_WORKFLOW_DEFINITION_URL = "workflow-definition-url";
		public const string A_WORKFLOW_DEFINITION_NAME = "workflow-definition-name";
		public const string A_WORKFLOW_DEFINITION_REVISION = "workflow-definition-revision";
		public const string A_WORKFLOW_INSTANCE_ID = "workflow-instance-id";
		public const string A_EXPRESSION_NAME = "expression-name";
		public const string A_EXPRESSION_ID = "expression-id";
		public const string E_OK = "ok";
		public const string A_FLOW_ID = "flow-id";
		public const string E_ATTRIBUTES = "attributes";
		public const string E_STRING = "string";
		public const string E_RAW_XML = "raw-xml";
		public const string E_BASE64 = "base64";
		public const string E_INTEGER = "integer";
		public const string E_LONG = "long";
		public const string E_DOUBLE = "double";
		public const string E_BOOLEAN = "boolean";
		public const string E_SMAP = "smap";
		public const string E_MAP = "map";
		public const string E_LIST = "list";
		public const string E_ENTRY = "entry";
		public const string E_KEY = "key";
		public const string E_VALUE = "value";
		//public const string E_FLOW_STACK = "flow-stack";
		public const string E_WORKITEM = "workitem";
		public const string E_CANCELITEM = "cancelitem";
		public const string E_LAUNCHITEM = "launchitem";
		public const string E_LAST_EXPRESSION_ID = "last-expression-id";
		public const string A_PARTICIPANT_NAME = "participant-name";
		public const string A_DISPATCH_TIME = "dispatch-time";
		public const string E_OPEN_FILTER = "open-filter";
		public const string E_CLOSED_FILTER = "closed-filter";
		public const string A_ADD_ALLOWED = "add-allowed";
		public const string A_REMOVE_ALLOWED = "remove-allowed";
		public const string E_HISTORY = "history";
		public const string E_HISTORY_ITEM = "history-item";
		public const string A_AUTHOR = "author";
		public const string A_DATE = "date";
		public const string A_HOST = "host";
		public const string E_FIELD = "field";
		public const string A_REGEX = "regex";
		public const string A_TYPE = "type";
		public const string E_LAUNCHABLES = "launchables";
		public const string E_LAUNCHABLE = "launchable";
		public const string A_URL = "url";
		public const string E_WORKFLOW_DEFINITION = "workflow-definition";
		public const string E_DESCRIPTION = "description";
		public const string A_LANG = "language";
		public const string DEFAULT_LANG = "default";
		public const string E_EXPRESSION = "expression";
		public const string E_EXPRESSIONS = "expressions";
		public const string A_APPLY_TIME = "apply-time";
		public const string A_FROZEN_TIME = "frozen-time";

		//
		// PUBLIC METHODS

		//
		// *** decoding ***

		public static object Decode (string xmlDocument)
		{
			XmlNode node = GetRootElement(xmlDocument);

			return Decode(node);
		}

		public static object Decode (XmlNode node)
		{
			if (node.Name.Equals(E_SESSION))
				return long.Parse(node.Attributes[A_SESSION_ID].Value);
			if (node.Name.Equals(E_STORES))
				return parseList(node, E_STORE);
			if (node.Name.Equals(E_EXPRESSIONS))
				return parseList(node, E_EXPRESSION);
			if (node.Name.Equals(E_STORE))
				return parseStore(node);
			if (node.Name.Equals(E_HEADERS))
				return parseList(node, E_HEADER);
			if (node.Name.Equals(E_FLOW_EXPRESSION_IDS))
				return parseList(node, E_FLOW_EXPRESSION_ID);
			if (node.Name.Equals(E_HEADER))
				return parseHeader(node);
			if (node.Name.Equals(E_ATTRIBUTES))
				return parseAttribute(node.FirstChild);
			if (node.Name.Equals(E_HISTORY_ITEM))
				return parseHistoryItem(node);
			if (node.Name.Equals(E_FIELD))
				return parseFilterEntry(node);
			if (node.Name.Equals(E_WORKITEM))
				return parseInFlowWorkitem(node);
			if (node.Name.Equals(E_LAUNCHITEM))
				return parseLaunchitem(node);
			if (node.Name.Equals(E_LAUNCHABLES))
				return parseLaunchables(node);
			if (node.Name.Equals(E_LAUNCHABLE))
				return parseLaunchable(node);
			if (node.Name.Equals(E_EXPRESSION))
				return parseExpression(node);
			if (node.Name.Equals(E_OK))
				return parseLaunchedFlowId(node);

			return null;
		}

		private static System.Collections.IList parseList (XmlNode node, string childName)
		{
			System.Collections.IList children = GetChildren(node, childName);
			System.Collections.IList result = new System.Collections.ArrayList(children.Count);

			foreach (XmlNode n in children)
			{
				result.Add(Decode(n));
			}

			return result;
		}

		private static Store parseStore (XmlNode node)
		{
			Store store = new Store();

			store.name = node.Attributes[A_NAME].Value;
			store.workitemCount = int.Parse(node.Attributes[A_WORKITEM_COUNT].Value);

			string sPermissions = node.Attributes[A_PERMISSIONS].Value;
			if (sPermissions == null) sPermissions = "";
			store.mayRead = sPermissions.IndexOf("r") > -1;
			store.mayWrite = sPermissions.IndexOf("w") > -1;
			store.mayDelegate = sPermissions.IndexOf("d") > -1;

			return store;
		}

		private static System.Collections.IList parseLaunchables (XmlNode node)
		{
			System.Collections.IList result = new System.Collections.ArrayList(50);

			System.Collections.IList children = GetChildren(node, E_LAUNCHABLE);
			foreach (XmlNode n in children)
			{
				result.Add(parseLaunchable(n));
			}

			return result;
		}

		private static Launchable parseLaunchable (XmlNode node)
		{
			string engineId = node.Attributes[A_ENGINE_ID].Value;
			string url = node.Attributes[A_URL].Value;

			return new Launchable(engineId, url);
		}

		private static string parseLaunchedFlowId (XmlNode node)
		{
			if (node.Attributes[A_FLOW_ID] == null) return null;
			return node.Attributes[A_FLOW_ID].Value;
		}

		private static FlowExpressionId parseFlowExpressionId (XmlNode node)
		{
			FlowExpressionId fei = new FlowExpressionId();

			fei.engineId = node.Attributes[A_ENGINE_ID].Value;
			fei.initialEngineId = node.Attributes[A_INITIAL_ENGINE_ID].Value;
			fei.workflowDefinitionUrl = node.Attributes[A_WORKFLOW_DEFINITION_URL].Value;
			fei.workflowDefinitionName = node.Attributes[A_WORKFLOW_DEFINITION_NAME].Value;
			fei.workflowDefinitionRevision = node.Attributes[A_WORKFLOW_DEFINITION_REVISION].Value;
			fei.workflowInstanceId = node.Attributes[A_WORKFLOW_INSTANCE_ID].Value;
			fei.expressionName = node.Attributes[A_EXPRESSION_NAME].Value;
			fei.expressionId = node.Attributes[A_EXPRESSION_ID].Value;

			return fei;
		}

		private static Header parseHeader (XmlNode node)
		{
			Header header = new Header();

			header.lastModified = node.Attributes[A_LAST_MODIFIED].Value;
			header.locked = "true".Equals(node.Attributes[A_LOCKED].Value.ToLower());
			header.flowExpressionId = parseFlowExpressionId(GetChild(node, E_FLOW_EXPRESSION_ID));
			header.attributes = (StringMapAttribute)Decode(GetChild(node, E_ATTRIBUTES));

			return header;
		}

		private static void parseWorkitem (Workitem wi, XmlNode node)
		{
			//wi.flowStack = parseList(GetChild(node, E_FLOW_STACK), E_FLOW_EXPRESSION_ID);

			wi.attributes = (StringMapAttribute)Decode(GetChild(node, E_ATTRIBUTES));
			if (wi.attributes == null) wi.attributes = new StringMapAttribute();
		}

		private static Launchitem parseLaunchitem (XmlNode node)
		{
			Launchitem li = new Launchitem();

			li.workflowDefinitionUrl = node.Attributes[A_WORKFLOW_DEFINITION_URL].Value;

			li.attributes = (StringMapAttribute)Decode(GetChild(node, E_ATTRIBUTES));
			if (li.attributes == null) li.attributes = new StringMapAttribute();

			li.descriptionMap = new System.Collections.Hashtable();

			System.Collections.IList children = GetChildren(node, E_DESCRIPTION);
			foreach (XmlNode n in children)
			{
				string lang = n.Attributes[A_LANG].Value;
				if (lang == null) lang = DEFAULT_LANG;

				li.descriptionMap[lang] = n.InnerText;
			}

			return li;
		}

		private static Cancelitem parseCancelitem (Cancelitem item, XmlNode node)
		{
			if (item == null)
			{
				if (node.Name.Equals(E_WORKITEM))
					item = new InFlowWorkitem();
				else if (node.Name.Equals(E_CANCELITEM))
					item = new Cancelitem();
			}

			parseWorkitem(item, node);

			item.participantName = 
				node.Attributes[A_PARTICIPANT_NAME].Value;
			item.lastExpressionId = 
				parseFlowExpressionId(GetChild(node, E_LAST_EXPRESSION_ID).FirstChild);

			return item;
		}

		private static InFlowWorkitem parseInFlowWorkitem (XmlNode node)
		{
			InFlowWorkitem wi = new InFlowWorkitem();

			parseCancelitem(wi, node);

			wi.lastModified = node.Attributes[A_LAST_MODIFIED].Value;
			wi.dispatchTime = node.Attributes[A_DISPATCH_TIME].Value;
			wi.filter = parseFilter(node);
			wi.history = parseList(GetChild(node, E_HISTORY), E_HISTORY_ITEM);

			return wi;
		}

		private static HistoryItem parseHistoryItem (XmlNode node)
		{
			HistoryItem item = new HistoryItem();

			item.author = node.Attributes[A_AUTHOR].Value;
			item.date = node.Attributes[A_DATE].Value;
			item.host = node.Attributes[A_HOST].Value;
			item.text = node.FirstChild.InnerText;

			item.workflowDefinitionName = 
				node.Attributes[A_WORKFLOW_DEFINITION_NAME].Value;
			item.workflowDefinitionRevision = 
				node.Attributes[A_WORKFLOW_DEFINITION_REVISION].Value;
			item.workflowInstanceId =
				node.Attributes[A_WORKFLOW_INSTANCE_ID].Value;
			item.expressionId = 
				node.Attributes[A_EXPRESSION_ID].Value;

			return item;
		}

		private static Expression parseExpression (XmlNode node)
		{
			Expression expression = new Expression();

			if (node.Attributes[A_APPLY_TIME] != null)
				expression.applyTime = node.Attributes[A_APPLY_TIME].Value;
			if (node.Attributes[A_FROZEN_TIME] != null)
				expression.frozenTime = node.Attributes[A_FROZEN_TIME].Value;
			expression.id = parseFlowExpressionId(node.ChildNodes[0]);

			return expression;
		}

		private static bool getBoolean (XmlNode node, string attributeName)
		{
			if (node.Attributes[attributeName] == null) return false;
			string val = node.Attributes[attributeName].Value.Trim().ToLower();

			return val.Equals("true");
		}

		private static Filter parseFilter (XmlNode node)
		{
			XmlNode eFilter = GetChild(node, E_OPEN_FILTER);
			if (eFilter == null) eFilter = GetChild(node, E_CLOSED_FILTER);

			if (eFilter == null) return null;

			Filter filter = new Filter();

			if (eFilter.Name.Equals(E_OPEN_FILTER))
				filter.type = Filter.TYPE_OPEN;
			else 
				filter.type = Filter.TYPE_CLOSED;

			filter.name = eFilter.Attributes[A_NAME].Value;
			filter.addAllowed = getBoolean(eFilter, A_ADD_ALLOWED);
			filter.removeAllowed = getBoolean(eFilter, A_REMOVE_ALLOWED);
			filter.entryList = parseList(eFilter, E_FIELD);

			return filter;
		}

		private static Filter.FilterEntry parseFilterEntry (XmlNode node)
		{
			Filter.FilterEntry entry = new Filter.FilterEntry();

			entry.fieldRegex = node.Attributes[A_REGEX].Value;
			entry.permissions = node.Attributes[A_PERMISSIONS].Value;

			if (node.Attributes[A_TYPE] != null)
				entry.attributeClassName = node.Attributes[A_TYPE].Value;

			return entry;
		}

		private static Attribute parseAttribute (XmlNode node)
		{
			if (node == null) return null;

			//System.Console.WriteLine("parseAttribute() node.name = "+node.Name);

			if (node.Name.Equals(E_STRING))
				return new StringAttribute(node.InnerText);
			if (node.Name.Equals(E_INTEGER))
				return new IntegerAttribute(node.InnerText);
			if (node.Name.Equals(E_LONG))
				return new LongAttribute(node.InnerText);
			if (node.Name.Equals(E_DOUBLE))
				return new DoubleAttribute(node.InnerText);
			if (node.Name.Equals(E_BOOLEAN))
				return new BooleanAttribute(node.InnerText);
			if (node.Name.Equals(E_RAW_XML))
				return new RawXmlAttribute(node);
			if (node.Name.Equals(E_BASE64))
				return new Base64Attribute(node.InnerText);

			if (node.Name.Equals(E_SMAP))
			{
				StringMapAttribute smap = new StringMapAttribute();

				fillMap(node, smap);

				return smap;
			}

			if (node.Name.Equals(E_MAP))
			{
				MapAttribute map = new MapAttribute();

				fillMap(node, map);

				return map;
			}

			if (node.Name.Equals(E_LIST))
			{
				ListAttribute result = new ListAttribute();

				foreach (XmlNode n in node.ChildNodes)
				{
					result.Add(parseAttribute(n));
				}

				return result;
			}

			return null;
		}

		private static void fillMap (XmlNode mapNode, MapAttribute map)
		{
			foreach (XmlNode nEntry in GetChildren(mapNode, E_ENTRY))
			{
				XmlNode nKey = GetChild(nEntry, E_KEY);
				XmlNode nValue = GetChild(nEntry, E_VALUE);

				Attribute aKey = parseAttribute(nKey.FirstChild);
				Attribute aValue = parseAttribute(nValue.FirstChild);

				//System.Console.WriteLine("key = "+((StringAttribute)aKey).Value);

				map[aKey] = aValue;
			}
		}

		//
		// *** encoding ***

		public static XmlDocument Encode (object data)
		{
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doEncode(doc, data));

			//System.Console.WriteLine(XmlToString(doc));

			return doc;
		}

		public static string XmlToString (XmlDocument doc, string encoding)
		{
			//StringWriter sWriter = new StringWriter(System.Text.Encoding.GetEncoding(encoding));
			StringWriter sWriter = new StringWriter();

			sWriter.WriteLine
				("<?xml version=\"1.0\" encoding=\""+encoding+"\" ?>");

			System.Diagnostics.Debug.WriteLine("  -- wrote xml header --");

			XmlTextWriter writer = new XmlTextWriter(sWriter);
			doc.WriteTo(writer);
			writer.Flush();

			string s = sWriter.ToString();

			return s;
		}

		private static XmlNode doEncode (XmlDocument doc, object data)
		{
			if (data is FlowExpressionId)
				return encode(doc, (FlowExpressionId)data);
			if (data is InFlowWorkitem)
				return encode(doc, (InFlowWorkitem)data);
			if (data is Launchitem)
				return encode(doc, (Launchitem)data);
			return null;
		}

		private static XmlNode encode (XmlDocument doc, FlowExpressionId fei)
		{
			XmlElement elt = doc.CreateElement(E_FLOW_EXPRESSION_ID);

			elt.SetAttribute
				(A_ENGINE_ID, fei.engineId);
			elt.SetAttribute
				(A_INITIAL_ENGINE_ID, fei.initialEngineId);
			elt.SetAttribute
				(A_WORKFLOW_DEFINITION_URL, fei.workflowDefinitionUrl);
			elt.SetAttribute
				(A_WORKFLOW_DEFINITION_NAME, fei.workflowDefinitionName);
			elt.SetAttribute
				(A_WORKFLOW_DEFINITION_REVISION, fei.workflowDefinitionRevision);
			elt.SetAttribute
				(A_WORKFLOW_INSTANCE_ID, fei.workflowInstanceId.ToString());
			elt.SetAttribute
				(A_EXPRESSION_NAME, fei.expressionName);
			elt.SetAttribute
				(A_EXPRESSION_ID, fei.expressionId.ToString());

			return elt;
		}

		private static XmlNode encodeList (XmlDocument doc, string eltName, IList list)
		{
			XmlElement eList = doc.CreateElement(eltName);

			foreach (object o in list)
			{
				if (o is FlowExpressionId)
					eList.AppendChild(encode(doc, (FlowExpressionId)o));
				else if (o is HistoryItem)
					eList.AppendChild(encode(doc, (HistoryItem)o));
			}

			return eList;
		}

		private static XmlNode encode (XmlDocument doc, HistoryItem item)
		{
			XmlElement eItem = doc.CreateElement(E_HISTORY_ITEM);

			eItem.SetAttribute
				(A_AUTHOR, item.author);
			eItem.SetAttribute
				(A_HOST, item.host);
			eItem.SetAttribute
				(A_DATE, item.date);
			eItem.SetAttribute
				(A_WORKFLOW_DEFINITION_NAME, item.workflowDefinitionName);
			eItem.SetAttribute
				(A_WORKFLOW_DEFINITION_REVISION, item.workflowDefinitionRevision);
			eItem.SetAttribute
				(A_WORKFLOW_INSTANCE_ID, item.workflowInstanceId.ToString());
			eItem.SetAttribute
				(A_EXPRESSION_ID, item.expressionId != null ? item.expressionId.ToString() : "null");
			
			XmlText text = doc.CreateTextNode(item.text);
			eItem.AppendChild(text);

			return eItem;
		}

		private static XmlNode encode (XmlDocument doc, Attribute a)
		{
			if (a is RawXmlAttribute)
			{
				XmlElement e = doc.CreateElement(E_RAW_XML);
				e.AppendChild(((RawXmlAttribute)a).Value);

				return e;
			}

			if (a is AtomicAttribute)
			{
				string tagName = null;

				if (a is StringAttribute) 
					tagName = E_STRING;
				else if (a is IntegerAttribute)
					tagName = E_INTEGER;
				else if (a is LongAttribute)
					tagName = E_LONG;
				else if (a is DoubleAttribute)
					tagName = E_DOUBLE;
				else if (a is BooleanAttribute)
					tagName = E_BOOLEAN;
				else if (a is Base64Attribute)
					tagName = E_BASE64;

				XmlElement eAtomicAttribute = doc.CreateElement(tagName);
				XmlText text = doc.CreateTextNode(a.ToString());
				eAtomicAttribute.AppendChild(text);

				return eAtomicAttribute;
			}

			if (a is ListAttribute)
			{
				XmlElement eList = doc.CreateElement(E_LIST);

				foreach (Attribute att in (ListAttribute)a)
				{
					eList.AppendChild(encode(doc, att));
				}

				return eList;
			}

			XmlElement eMap = null;
			if (a is StringMapAttribute)
				eMap = doc.CreateElement(E_SMAP);
			else
				eMap = doc.CreateElement(E_MAP);

			MapAttribute map = (MapAttribute)a;

			if (map == null) return eMap; // empty map

			foreach (Attribute aKey in map.Keys())
			{
				Attribute aValue = map[aKey];

				XmlElement eEntry = doc.CreateElement(E_ENTRY);
				XmlElement eKey = doc.CreateElement(E_KEY);
				XmlElement eValue = doc.CreateElement(E_VALUE);

				eKey.AppendChild(encode(doc, aKey));
				eValue.AppendChild(encode(doc, aValue));

				eEntry.AppendChild(eKey);
				eEntry.AppendChild(eValue);

				eMap.AppendChild(eEntry);
			}

			return eMap;
		}

		private static XmlNode encode (XmlDocument doc, Filter filter)
		{
			string tagName = E_CLOSED_FILTER;
			if (filter.type == Filter.TYPE_OPEN) tagName = E_OPEN_FILTER;

			XmlElement eFilter = doc.CreateElement(tagName);

			string sAddAllowed = "false";
			if (filter.addAllowed) sAddAllowed = "true";
			eFilter.SetAttribute
				(A_ADD_ALLOWED, sAddAllowed);

			string sRemoveAllowed = "false";
			if (filter.removeAllowed) sRemoveAllowed = "true";
			eFilter.SetAttribute
				(A_REMOVE_ALLOWED, sRemoveAllowed);

			foreach (Filter.FilterEntry e in filter.entryList)
			{
				eFilter.AppendChild(encode(doc, e));
			}

			return eFilter;
		}

		public static XmlNode encode (XmlDocument doc, Filter.FilterEntry entry)
		{
			XmlElement elt = doc.CreateElement(E_FIELD);

			elt.SetAttribute(A_REGEX, entry.fieldRegex);
			elt.SetAttribute(A_PERMISSIONS, entry.permissions);

			if (entry.attributeClassName != null)
				elt.SetAttribute(A_TYPE, entry.attributeClassName);

			return elt;
		}

		private static XmlNode encode (XmlDocument doc, Launchitem li)
		{
			XmlElement elt = doc.CreateElement(E_LAUNCHITEM);

			elt.SetAttribute
				(A_WORKFLOW_DEFINITION_URL, li.workflowDefinitionUrl);
			elt.SetAttribute
				(A_LAST_MODIFIED, li.lastModified);

			//elt.AppendChild(encodeList(doc, E_FLOW_STACK, li.flowStack));

			XmlElement eAttributes = doc.CreateElement(E_ATTRIBUTES);
			eAttributes.AppendChild(encode(doc, li.attributes));
			elt.AppendChild(eAttributes);

			return elt;
		}

		private static XmlNode encode (XmlDocument doc, InFlowWorkitem wi)
		{
			XmlElement elt = doc.CreateElement(E_WORKITEM);

			elt.SetAttribute
				(A_PARTICIPANT_NAME, wi.participantName);
			elt.SetAttribute
				(A_DISPATCH_TIME, wi.dispatchTime);
			elt.SetAttribute
				(A_LAST_MODIFIED, wi.lastModified);

			//elt.AppendChild(encodeList(doc, E_FLOW_STACK, wi.flowStack));
			elt.AppendChild(encodeList(doc, E_HISTORY, wi.history));

			//if (wi.attributes == null) wi.attributes = new StringMapAttribute();

			XmlElement eAttributes = doc.CreateElement(E_ATTRIBUTES);
			eAttributes.AppendChild(encode(doc, wi.attributes));
			elt.AppendChild(eAttributes);

			if (wi.filter != null)
				elt.AppendChild(encode(doc, wi.filter));

			XmlElement eLei = doc.CreateElement(E_LAST_EXPRESSION_ID);
			eLei.AppendChild(encode(doc, wi.lastExpressionId));
			elt.AppendChild(eLei);

			return elt;
		}

		//
		// METHODS

		private static XmlNode GetRootElement (string xmlDocument)
		{
			XmlTextReader reader = new XmlTextReader(new StringReader(xmlDocument));
			XmlDocument doc = new XmlDocument();
			doc.Load(reader);
			return doc.DocumentElement;
		}

		private static System.Collections.IList GetChildren 
			(XmlNode node, string childName)
		{
			if (node == null)
				return new System.Collections.ArrayList(0);

			System.Collections.IList result = 
				new System.Collections.ArrayList(node.ChildNodes.Count);

			foreach (XmlNode n in node.ChildNodes)
			{
				if (n.Name.Equals(childName)) result.Add(n);
			}

			return result;
		}

		private static XmlNode GetChild (XmlNode node, string childName)
		{
			foreach (XmlNode n in node.ChildNodes)
			{
				if (n.Name.Equals(childName)) return n;
			}

			return null;
		}
	}
}
