using SpotifyApp.Interfaces;
using SpotifyApp.Models;
using SpotifyApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MySaved.xaml
    /// </summary>
    public partial class MySaved : Page {

        public ObservableCollection<ISpotifyEntity> Results { get; set; }
        public SpotifyEntity Type { get; set; }

        public MySaved() {
            InitializeComponent();
        }

        public MySaved(SpotifyEntity type) : this() {
            Type = type;
            if (Type == SpotifyEntity.Artist) GetSavedArtists();
            if (Type == SpotifyEntity.Album) GetSavedAlbums();
        }

        private void GetSavedArtists() {
            Results = Database.RetrieveArtists();
            lbxArtists.ItemsSource = Results;
        }

        private void GetSavedAlbums() {
            Results = Database.RetrieveAlbums();
            lbxAlbums.ItemsSource = Results;
        }

    }
}
