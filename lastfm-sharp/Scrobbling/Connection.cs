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

namespace Lastfm.Scrobbling
{
	/// <summary>
	/// A connection to the Last.fm scrobbling service. Can be used individually for scrobbling
	/// or through a <see cref="ScrobbleManager"/> object.
	/// </summary>
	public class Connection
	{
        private string ApiSecret { get; set; }
        private string ApiURL = "http://ws.audioscrobbler.com/2.0/";
        private RequestParameters parameters {get; set;}
		
		public Connection(Session authenticatedSession)
		{
			parameters = new RequestParameters();
            parameters["api_key"] = authenticatedSession.APIKey;
            parameters["sk"]=authenticatedSession.SessionKey;
            ApiSecret = authenticatedSession.APISecret;
		}
		
		/// <summary>
		/// Send the now playing notification.
		/// </summary>
		/// <param name="track">
		/// A <see cref="NowplayingTrack"/>
		/// </param>
		public void ReportNowplaying(NowplayingTrack track)
		{
            RequestParameters p = new RequestParameters(parameters);
            p.Append(track.getParameters());
            p["method"] = "track.updateNowPlaying";
            p["api_sig"] = Utilities.MD5(p.ToStringForSig(ApiSecret));

			Request request = new Request(new Uri(this.ApiURL), p);

			// A BadSessionException occurs when another client has made a handshake
			// with this user's credentials, should redo a handshake and pass this 
			// exception quietly.
            try
            {
				request.executeThreaded();
            }
            catch (BadSessionException)
            {
                this.ReportNowplaying(track);
            }
		}
		
		/// <summary>
		/// Public scrobble function. Scrobbles a PlayedTrack object.
		/// </summary>
		/// <param name="track">
		/// A <see cref="PlayedTrack"/>
		/// </param>
		public void Scrobble(Entry track)
		{
            RequestParameters p = new RequestParameters(parameters);
            // Add parameters of track to base parameters
            p.Append(track.getParameters());
			// This scrobbles the collection of parameters no matter what they belong to.
			this.Scrobble(p);
		}
		
		/// <summary>
		/// The internal scrobble function, scrobbles pure request parameters.
		/// Could be for more than one track, as specified by Last.fm, but they recommend that
		/// only one track should be submitted at a time.
		/// </summary>
		/// <param name="parameters">
		/// A <see cref="RequestParameters"/>
		/// </param>
		internal void Scrobble(RequestParameters parameters)
		{
            parameters["method"] = "track.scrobble";
            parameters["api_sig"] = Utilities.MD5(parameters.ToStringForSig(ApiSecret));
			Request request = new Request(new Uri(this.ApiURL), parameters);

			// A BadSessionException occurs when another client has made a handshake
			// with this user's credentials, should redo a handshake and pass this 
			// exception quietly.
			try
			{
				request.executeThreaded();
			} catch (BadSessionException) {
				this.Scrobble(parameters);
			}
		}			
	}
}
