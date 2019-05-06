using SpotifyApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SpotifyApp.Services.Spotify;

namespace SpotifyApp.Pages {
    /// <summary>
    /// Interaction logic for SearchResults.xaml
    /// </summary>
    public partial class SearchResults : Page {

        public List<ISpotifyEntity> Results { get; set; }

        public SearchResults() {
            InitializeComponent();
        }

        public SearchResults(List<ISpotifyEntity> results, SpotifyEntity entityType) : this() {
            if (entityType == SpotifyEntity.Artist) lbxArtists.ItemsSource = results;
            if (entityType == SpotifyEntity.Album) lbxAlbums.ItemsSource = results;
        }

    }
}
