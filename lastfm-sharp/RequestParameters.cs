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
using System.Collections.Generic;
using System.Web;
using System.Text;

namespace Lastfm
{
	internal class RequestParameters : SortedDictionary<string, string>
	{
        internal RequestParameters(string serialization)
            : base()
        {
            string[] values = serialization.Split('\t');

            for (int i = 0; i < values.Length - 1; i++)
            {
                if ((i % 2) == 0)
                    this[values[i]] = values[i + 1];
            }
        }

        internal RequestParameters(RequestParameters parameters)
            : base(parameters)
        {
        }

        public RequestParameters()
            : base()
        {
        }

		public override string ToString()
		{
			string values = "";
			foreach(string key in this.Keys)
				values += key + "=" + this[key] + "&";
			values = values.Substring(0, values.Length - 1);
			
			return values;
		}
        /// <summary>
        /// Prepares string to md5-hashing and submitting as signature
        /// </summary>
        /// <param name="secret">your api_secret</param>
        /// <returns></returns>
        public string ToStringForSig(string secret)
        {
            string values = "";
            foreach (string key in this.Keys)
                values += key + this[key];

            return values + secret;
        }
        /// <summary>
        /// Adds all given parameters to existing collection
        /// </summary>
        /// <param name="parameters">object to copy from</param>
        internal void Append(RequestParameters parameters)
        {
            foreach (KeyValuePair<string, string> kvp in parameters)
            {
                Add(kvp.Key, kvp.Value);
            }
        }
		internal byte[] ToBytes()
		{	
			return Encoding.ASCII.GetBytes(ToString());
		}
		
		internal string serialize()
		{
			string line = "";
			
			foreach (string key in Keys)
				line += key + "\t" + this[key] + "\t";
			
			return line;
		}				
	}
}
