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
using openwfe.workitem;

namespace openwfe.test
{
	public abstract class Test
	{
		static void Main (string[] args)
		{
			DateTime start = DateTime.Now;

			System.Console.WriteLine("--starting--");

			openwfe.rest.worklist.WorkSession ws = new openwfe.rest.worklist.WorkSession("localhost", 5080, "alice", "alice");
			System.Console.WriteLine("Session id >"+ws.GetSessionId()+"<");

			System.Collections.IList stores = ws.GetStoreNames();
			foreach (openwfe.workitem.Store s in stores)
			{
				System.Console.WriteLine("store : "+s.name+"  "+s.mayRead);
			}

			openwfe.workitem.FlowExpressionId itemId = null;

			int i = 0;
			System.Collections.IList headers = ws.GetHeaders("Store.alpha");
			System.Console.WriteLine("headers.count : "+headers.Count);
			foreach (openwfe.workitem.Header h in headers)
			{
				if (itemId == null) itemId = h.flowExpressionId;

				openwfe.workitem.StringMapAttribute smap = (openwfe.workitem.StringMapAttribute)h.attributes;
				foreach (openwfe.workitem.StringAttribute aKey in smap.Keys())
				{
					openwfe.workitem.Attribute aValue = smap[aKey];
					System.Console.WriteLine("header."+i+"."+aKey+" = "+aValue);
				}
				i++;
			}

			//System.Console.WriteLine("itemId is "+itemId);

			if (itemId != null)
			{
				openwfe.workitem.InFlowWorkitem wi = ws.GetAndLockWorkitem("Store.alpha", itemId);
				if (wi != null)
				{
					System.Console.WriteLine("wi.subject = "+wi.attributes[new StringAttribute("__subject__")]);

					foreach (openwfe.workitem.StringAttribute aKey in wi.attributes.Keys())
					{
						openwfe.workitem.Attribute aValue = wi.attributes[aKey];
						System.Console.WriteLine("wi."+aKey+" = "+aValue);
					}

					wi.attributes[new StringAttribute("C#")] = new BooleanAttribute(true);
					
					ws.SaveWorkitem("Store.alpha", wi);
					wi = ws.GetWorkitem("Store.alpha", itemId);
					ws.DelegateToParticipant("Store.alpha", wi, "role-janove" );
					//ws.ForwardWorkitem("Store.alpha", wi);
				}
				else
				{
					System.Console.WriteLine("did not find the workitem");
				}
			}
			else
			{
				System.Console.WriteLine("no items to play with in Store.alpha");
			}

			//
			// list launchables...

			Launchable launchable = null;

			System.Collections.IList launchables = ws.ListLaunchables();
			foreach (Launchable l in launchables)
			{
				if (launchable == null) launchable = l;
				System.Console.WriteLine("   launchable : "+l.url);
			}

			//
			// launch a flow
//
//			Launchitem li = launchable.generateLaunchitem();
//			li.attributes = new StringMapAttribute();
//			string wfid = ws.LaunchFlow(launchable.engineId, li);
//
//			System.Console.WriteLine("launched flow "+wfid);

			//
			// test findFlowInstance()

			//System.Threading.Thread.Sleep(1000);
//
//			System.Collections.IList feis = ws.FindFlowInstance("Store.alpha", wfid);
//
//			System.Console.WriteLine("found "+feis.Count+" flowExpressionIds for flow "+wfid);

			//
			// close worksession

			ws.Close();

			System.TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - start.Ticks);
			System.Console.WriteLine("elapsed : (ms) "+ ts.Milliseconds);

			/*
			System.Console.WriteLine("date : "+openwfe.time.Time.toIsoDate(DateTime.Now));
			String iso = openwfe.time.Time.toIsoDate(DateTime.Now);
			System.Console.WriteLine("date : "+openwfe.time.Time.fromIsoDate(iso));
			*/

			//
			// a bit of control testing

			openwfe.rest.control.ControlSession cs = new openwfe.rest.control.ControlSession("localhost", 6080, "admin", "admin");

			System.Collections.IList exps = cs.ListExpressions();

			foreach (openwfe.workitem.Expression exp in exps)
			{
				System.Console.WriteLine("- "+exp.id.expressionName+"  "+exp.id.expressionId+"  "+exp.id.workflowInstanceId);
			}

			cs.Close();
		}
	}
}
