using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApp.Models {
    public class Artist {

        public string ArtistID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string[] Genres { get; set; }
        public int Followers { get; set; }

    }
}
