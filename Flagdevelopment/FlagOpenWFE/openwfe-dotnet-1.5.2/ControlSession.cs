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
// $Id: ControlSession.cs,v 1.3 2005/05/17 10:16:32 jmettraux Exp $
//

namespace openwfe.rest.control
{
	public class ControlSession: RestSession
	{
		//
		// CONSTANTS

		public const int DEFAULT_LIMIT = 100;

		public const string SERVICE_NAME = "engine";

		public const string A_LIST_EXPRESSIONS = "listexpressions";
		public const string A_CANCEL_EXPRESSION = "cancelexpression";
		//public const string A_CANCEL_FLOW = "cancelflow";
		public const string A_FREEZE_EXPRESSION = "freezeexpression";
		public const string A_UNFREEZE_EXPRESSION = "unfreezeexpression";

		//
		// FIELDS

		//
		// CONSTRUCTORS

		public ControlSession (string server, int port, string username, string password, string encoding) :
			base(server, port, SERVICE_NAME, username, password, encoding)
		{
		}

		public ControlSession (string server, int port, string username, string password) :
			base(server, port, SERVICE_NAME, username, password, "ISO-8859-1")
		{
		}

		//
		// PUBLIC METHODS

		public override string GetVersion () 
		{ 
			return openwfe.rest.worklist.WorkSession.VERSION; 
		}

		public IList ListExpressions ()
		{
			return (IList)openwfe.workitem.Codec
				.Decode(DoGet(A_LIST_EXPRESSIONS, null, null));
		}

		public void CancelExpression (FlowExpressionId id)
		{
			DoPost(A_CANCEL_EXPRESSION, null, null, id);
		}

		/*
		public void CancelFlow (FlowExpressionId id)
		{
			DoPost(A_CANCEL_FLOW, null, null, id);
		}
		*/

		public void FreezeExpression (FlowExpressionId id)
		{
			DoPost(A_FREEZE_EXPRESSION, null, null, id);
		}

		public void UnfreezeExpression (FlowExpressionId id)
		{
			DoPost(A_UNFREEZE_EXPRESSION, null, null, id);
		}

		//
		// METHODS

	}
}
