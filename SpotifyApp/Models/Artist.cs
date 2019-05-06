using SpotifyApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApp.Models {
    public class Artist : ISpotifyEntity {

        public string ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string[] Genres { get; set; }
        public int Followers { get; set; }
        public int Popularity { get; set; }
        public string GenresReadable { get { return Genres.Length > 0 ? string.Join(", ", Genres) : "N/A"; } }

    }
}
