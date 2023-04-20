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
using System.Text;
using System.Collections;
using openwfe.workitem;

//
// $Id: WorkSession.cs,v 1.19 2005/05/03 14:55:08 jmettraux Exp $
//

namespace openwfe.rest.worklist
{
	public class WorkSession: RestSession
	{
		//
		// CONSTANTS

		public const string VERSION = "openwfe-dotnet client 1.4.3 $Revision: 1.19 $";

		public const string A_GET_STORE_NAMES = "getStoreNames";
		public const string A_GET_HEADERS = "getHeaders";
		public const string A_FIND_FLOW_INSTANCE = "findFlowInstance";
		public const string A_GET_WORKITEM = "getWorkitem";
		public const string A_GET_AND_LOCK_WORKITEM = "getAndLockWorkitem";
		public const string A_RELEASE_WORKITEM = "releaseWorkitem";
		public const string A_SAVE_WORKITEM = "saveWorkitem";
		public const string A_FORWARD_WORKITEM = "forwardWorkitem";
		public const string A_LIST_LAUNCHABLES = "listLaunchables";
		public const string A_LAUNCH_FLOW = "launchFlow";
		public const string A_DELEGATE = "delegate";

		public const string P_ENGINE_ID = "engineid";
		public const string P_TARGET_STORE = "targetstore";
		public const string P_TARGET_PARTICIPANT = "targetparticipant";

		public const int DEFAULT_LIMIT = 3000;

		public const string SERVICE_NAME = "worklist";

		//
		// FIELDS

		//
		// CONSTRUCTORS

		public WorkSession (string server, int port, string username, string password, string encoding) :
			base(server, port, SERVICE_NAME, username, password, encoding)
		{
		}

		public WorkSession (string server, int port, string username, string password) :
			base(server, port, SERVICE_NAME, username, password, "ISO-8859-1")
		{
		}

		//
		// PUBLIC METHODS

		public override string GetVersion () 
		{ 
			return VERSION; 
		}

		public IList GetStoreNames ()
		{
			return (IList)openwfe.workitem.Codec
				.Decode(DoGet(A_GET_STORE_NAMES, null, null));
		}

		public IList GetHeaders (string storeName)
		{
			return GetHeaders(storeName, DEFAULT_LIMIT);
		}

		public IList GetHeaders (string storeName, int limit)
		{
			IDictionary parameters = new Hashtable();
			parameters["limit"] = limit.ToString();

			string reply = DoGet(A_GET_HEADERS, storeName, parameters);

			IList result = (IList)openwfe.workitem.Codec.Decode(reply);

			if (result == null) return new ArrayList();

			return result;
		}

		public IList FindFlowInstance (string storeName, string workflowInstanceId)
		{
			IDictionary parameters = new Hashtable();
			parameters["id"] = workflowInstanceId;

			return (IList)openwfe.workitem.Codec
				.Decode(DoGet(A_FIND_FLOW_INSTANCE, storeName, parameters));
		}

		public InFlowWorkitem GetWorkitem (string storeName, FlowExpressionId id)
		{
			return (InFlowWorkitem)DoPost(A_GET_WORKITEM, storeName, null, id);
		}

		public InFlowWorkitem GetAndLockWorkitem (string storeName, FlowExpressionId id)
		{
			return (InFlowWorkitem)DoPost(A_GET_AND_LOCK_WORKITEM, storeName, null, id);
		}

		public void ReleaseWorkitem (string storeName, InFlowWorkitem wi)
		{
			DoPost(A_RELEASE_WORKITEM, storeName, null, wi);
		}

		public void SaveWorkitem (string storeName, InFlowWorkitem wi)
		{
			DoPost(A_SAVE_WORKITEM, storeName, null, wi);
		}

		public void ForwardWorkitem (string storeName, InFlowWorkitem wi)
		{
			DoPost(A_FORWARD_WORKITEM, storeName, null, wi);
		}

		public IList ListLaunchables ()
		{
			return (IList)openwfe.workitem.Codec
				.Decode(DoGet(A_LIST_LAUNCHABLES, null, null));
		}

		public string LaunchFlow (string engineId, Launchitem li)
		{
			IDictionary parameters = new Hashtable();
			parameters[P_ENGINE_ID] = engineId;

			return (string)DoPost(A_LAUNCH_FLOW, null, parameters, li);
		}

		public void Delegate (string storeName, InFlowWorkitem wi, string targetStoreName)
		{
			IDictionary parameters = new Hashtable();
			parameters[P_TARGET_STORE] = targetStoreName;

			DoPost(A_DELEGATE, storeName, parameters, wi);
		}

		public void DelegateToParticipant (string storeName, InFlowWorkitem wi, string targetParticipant)
		{
			IDictionary parameters = new Hashtable();
			parameters[P_TARGET_PARTICIPANT] = targetParticipant;

			DoPost(A_DELEGATE, storeName, parameters, wi);
		}

		//
		// METHODS

	}
}
