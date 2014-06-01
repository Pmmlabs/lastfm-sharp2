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
	public class Entry
	{
		public string Artist {get; set;}
		public string Title {get; set;}
		public string Album {get; set;}
		public TimeSpan Duration {get; set;}
		public int Number {get; set;}
        public DateTime TimeStarted = DateTime.Now;
        public PlaybackSource Source = PlaybackSource.User;
        public string AlbumArtist { get; set; }

		public string MBID {get; set;}
        
        public Entry(string artist, string title)
        {
            Artist = artist;
            Title = title;
        }	

		public Entry(string artist, string title, DateTime timeStarted, PlaybackSource source, TimeSpan duration)
		{
			Artist = artist;
			Title = title;
			TimeStarted = timeStarted;
			Source = source;
			Duration = duration;
		}	
		
		public Entry(string artist, string title, DateTime timeStarted, PlaybackSource source, TimeSpan duration,
		                   string album, int trackNumber, string mbid)
		{
			Artist = artist;
			Title = title;			
			TimeStarted = timeStarted;
			Source = source;
			Duration = duration;
			Album = album;
			Number = trackNumber;
			MBID = mbid;
		}		

        internal RequestParameters getParameters()
        {
            RequestParameters p = new RequestParameters();
           
            p["artist"] = Artist;
            p["track"] = Title;
            p["timestamp"] = Utilities.DateTimeToUTCTimestamp(TimeStarted).ToString();

            if (Album != null && Album.Length != 0)
                p["album"] = Album;
            if (Duration != null && Duration.TotalSeconds != 0)
                p["duration"] = Duration.TotalSeconds.ToString();
            if (Source != PlaybackSource.User)
                p["chosenByUser"] = "0";
            if (Number != 0)
                p["trackNumber"] = Number.ToString();
            if (MBID != null && MBID.Length != 0)
                p["mbid"] = MBID;
            if (AlbumArtist != null && AlbumArtist.Length != 0)
                p["albumArtist"] = AlbumArtist;

            return p;
        }
		public override string ToString ()
		{
			return Artist + " - " + Title + " (" + TimeStarted + ")";
		}
		
		public Lastfm.Services.Track GetInfo(Session session)
		{
			return new Lastfm.Services.Track(this.Artist, this.Title, session);
		}      
    }
}
