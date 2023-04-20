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
// $Id: RestSession.cs,v 1.2 2005/05/03 14:55:08 jmettraux Exp $
//

namespace openwfe.rest
{
	public abstract class RestSession
	{
		//
		// CONSTANTS

		public const string A_END_WORK_SESSION = "endWorkSession";

		public const int RETRY = 10;
			// DoPost will retry 10 times before giving up

		//
		// FIELDS

		private string encoding = null;
		private string url = null;
		private string baseUrl = null;
		private long sessionId = -1;

		//
		// CONSTRUCTORS

		public RestSession (string server, int port, string serviceName, string username, string password, string encoding)
		{
			this.baseUrl = "http://"+server+":"+port;
			this.url = this.baseUrl+"/"+serviceName;
			this.sessionId = GetSession(username, password);
			this.encoding = encoding;
		}

		public RestSession (string server, int port, string serviceName, string username, string password) :
			this(server, port, serviceName, username, password, "ISO-8859-1")
		{
		}

		//
		// ABSTRACT METHODS

		public abstract string GetVersion ();

		//
		// PUBLIC METHODS

		public string GetEncoding ()
		{
			return this.encoding;
		}

		public Encoding ResolveEncoding ()
		{
			//if (this.encoding == null) return Encoding.UTF8;
			//return Encoding.GetEncoding(this.GetEncoding());
			return Encoding.UTF8;
		}

		public long GetSessionId ()
		{
			return this.sessionId;
		}

		public void Close ()
		{
			JustGet(A_END_WORK_SESSION, null, null);
		}
		
		//
		// METHODS

		private string ComputeUrl (string action, string storeName, IDictionary parameters)
		{
			string url = this.url;
			if (storeName != null) url += ("/"+storeName);

			url += ("?session="+this.sessionId+"&action="+action);

			if (parameters != null)
			{
				foreach (string key in parameters.Keys)
				{
					url += ("&"+key+"="+parameters[key]);
				}
			}

			return url;
		}

		protected object DoPost (string action, string storeName, IDictionary parameters, object data)
		{
			string sXml = Codec.XmlToString(((System.Xml.XmlDocument)Codec.Encode(data)), this.encoding);
			byte[] bData = Encoding.GetEncoding(this.encoding).GetBytes(sXml);
			
			WebClient wc = new WebClient();
			wc.Headers["User-agent"] = GetVersion();

			byte[] result = wc.UploadData(ComputeUrl(action, storeName, parameters), "POST", bData);

			String xmlDoc = null;
			int retry = 0;
			while (true)
			{
				try
				{
					xmlDoc = Encoding.GetEncoding(this.encoding).GetString(result);
					// not very happy with this, what happens if the doc is not in the same coding as dotnet ???

					break;
				}
				catch (Exception e)
				{
					retry++;
					if (retry > RETRY) throw e;
				}
			}

			//System.Console.WriteLine(xmlDoc);

			return Codec.Decode(xmlDoc);
		}

		protected string DoGet (string action, string storeName, IDictionary parameters)
		{
			WebRequest req = WebRequest.Create(ComputeUrl(action, storeName, parameters));
			req.Method = "GET";
			HttpWebRequest httpReq = (HttpWebRequest)req;
			httpReq.UserAgent = GetVersion();
			HttpWebResponse res = (HttpWebResponse)req.GetResponse();
			StreamReader reader = new StreamReader(res.GetResponseStream(), ResolveEncoding());
			string text = reader.ReadToEnd();
			res.Close();
			return text;
		}

		private void JustGet (string action, string storeName, IDictionary parameters)
		{
			WebRequest req = WebRequest.Create(ComputeUrl(action, storeName, parameters));
			req.Method = "GET";
			HttpWebRequest httpReq = (HttpWebRequest)req;
			httpReq.UserAgent = GetVersion();
			HttpWebResponse res = (HttpWebResponse)req.GetResponse();
			res.Close();
		}

		private long GetSession (string username, string password)
		{
			CredentialCache cache = new CredentialCache();
			cache.Add(new Uri(this.baseUrl), "Basic", new NetworkCredential(username, password));
			WebRequest req = WebRequest.Create(this.url);
			req.Method = "GET";
			req.Credentials = cache;
			HttpWebRequest httpReq = (HttpWebRequest)req;
			httpReq.UserAgent = GetVersion();
			HttpWebResponse res = (HttpWebResponse)req.GetResponse();
			StreamReader reader = new StreamReader(res.GetResponseStream(), ResolveEncoding());
			string xmlDoc = reader.ReadToEnd();
			
			res.Close();

			return (long)openwfe.workitem.Codec.Decode(xmlDoc);
		}
	}
}
