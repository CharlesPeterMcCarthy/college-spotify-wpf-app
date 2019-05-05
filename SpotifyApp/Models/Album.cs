using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApp.Models {
    public class Album {

        public string AlbumID { get; set; }
        public string Name { get; set; }
        public string ArtistID { get; set; }
        public string ArtistName { get; set; }
        public string Image { get; set; }
        public DateTime ReleaseDate { get; set; }

    }
}
