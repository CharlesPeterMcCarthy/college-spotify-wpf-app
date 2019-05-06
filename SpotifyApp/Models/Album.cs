using SpotifyApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApp.Models {
    public class Album : ISpotifyEntity {

        public string ID { get; set; }
        public string Name { get; set; }
        public string ArtistID { get; set; }
        public string ArtistName { get; set; }
        public string Image { get; set; }
        public int Tracks { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ReleaseDateReadable {
            get {
                return ReleaseDate.ToString("dddd, dd MMMM yyyy");
            }
        }

    }
}
