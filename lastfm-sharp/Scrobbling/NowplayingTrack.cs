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
	public class NowplayingTrack
	{
		public string Artist {get; set;}
		public string Title {get; set;}
		public string Album {get; set;}
		public TimeSpan Duration {get; set;}
		public string MBID {get; set;}
		public int Number {get; set;}
        public string AlbumArtist { get; set; }
		/// <summary>
		/// Creates an instance of Nowplaying track with specifyed attributes.
		/// </summary>
		/// <param name="artist"></param>
		/// <param name="title"></param>
		public NowplayingTrack(string artist, string title)
		{
			Artist = artist;
			Title = title;
		}
		/// <summary>
		/// Creates an instance of Nowplaying track with specifyed attributes. If attribute unknown, set to null.
		/// </summary>
		/// <param name="artist"></param>
		/// <param name="title"></param>
		/// <param name="album"></param>
		/// <param name="duration"></param>
		/// <param name="mbid"></param>
		/// <param name="tracknumber"></param>
		/// <param name="albumartist"></param>
		public NowplayingTrack(string artist, string title, string album, TimeSpan duration, string mbid, int tracknumber, string albumartist)
		{
			Artist = artist;
			Title = title;
            Album = album;
			Duration = duration;
            MBID = mbid;
            Number = tracknumber;
            AlbumArtist = albumartist;
		}
        internal RequestParameters getParameters()
        {
            RequestParameters p = new RequestParameters();
           
            p["artist"] = Artist;
            p["track"] = Title;

            if (Album != null && Album.Length != 0)
                p["album"] = Album;
            if (Duration != null && Duration.TotalSeconds != 0)
                p["duration"] = Duration.TotalSeconds.ToString();            
            if (Number  != null && Number != 0)
                p["trackNumber"] = Number.ToString();
            if (MBID != null && MBID.Length != 0)
                p["mbid"] = MBID;
            if (AlbumArtist != null && AlbumArtist.Length != 0)
                p["albumArtist"] = AlbumArtist;

            return p;
        }
	}
}
