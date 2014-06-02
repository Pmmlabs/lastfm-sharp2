//  
//  Copyright (C) 2009 Amr Hassan
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;
using System.Threading;

namespace Lastfm.Scrobbling
{
	internal class Request
	{
		RequestParameters Parameters;
		Uri URI {get; set;}
		
		internal Request(Uri uri, RequestParameters parameters)
		{
			URI = uri;
			Parameters = parameters;
		}

        internal StreamReader execute()
		{			
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
			
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(Parameters.ToString());
			request.ContentLength = data.Length;
			request.UserAgent = Utilities.UserAgent;
			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";
            request.Headers["Accept-Charset"] = "utf-8";
			
			if (Lib.Proxy != null)
				request.Proxy = Lib.Proxy;
			
			Stream writeStream = request.GetRequestStream();
			writeStream.Write(data, 0, data.Length);
			writeStream.Close();
			
			HttpWebResponse webresponse;
            try
            {
				webresponse = (HttpWebResponse)request.GetResponse();
            }catch (WebException e){                
               webresponse = (HttpWebResponse)e.Response;
            }
			
			StreamReader output = new StreamReader(webresponse.GetResponseStream());

            using (XmlReader reader = XmlReader.Create(output))
            {
                reader.ReadToFollowing("lfm");
                reader.MoveToFirstAttribute();
                if (reader.Value != "ok")
                {
                    reader.ReadToFollowing("error");
                    throw new ScrobblingException(reader.ReadElementContentAsString());
                }
            }
			
			return output;
		}
        /// <summary>
        /// Executing request in separate thread
        /// </summary>
        internal void executeThreaded()
        {
            Thread submittingThread = new Thread(new ThreadStart(this.executeThreadedHelper));
            submittingThread.Start();
        }
        private void executeThreadedHelper()
        {
            execute();
        }
    }
}
