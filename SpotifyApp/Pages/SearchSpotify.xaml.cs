using SpotifyApp.Interfaces;
using SpotifyApp.Models;
using SpotifyApp.Services;
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
    /// Interaction logic for SearchSpotify.xaml
    /// </summary>
    public partial class SearchSpotify : Page {

        public SearchSpotify() {
            InitializeComponent();

            cbxType.ItemsSource = new string[] { "Artist", "Album" };
            cbxType.SelectedValue = "Artist";
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e) => PerformSearch();
        private void KeyPressed(object sender, KeyEventArgs e) { if (e.Key == Key.Return) PerformSearch(); }
        
        private async void PerformSearch() {
            if (cbxType.SelectedIndex < 0) {
                Toastr.Warning("No Type", "Please choose a type from the drop down");
                return;
            }

            string searchString = tbxSearchString.Text;
            string type = cbxType.SelectedItem.ToString();
            SpotifyEntity entityType = type == "Artist" ? SpotifyEntity.Artist : SpotifyEntity.Album;

            if (searchString.Length < 1) {
                Toastr.Warning("Too Short", "Please enter a longer search query");
                return;
            }

            List<ISpotifyEntity> results = await GetSearchResults(searchString, entityType);

            if (results.Count > 0) NavigationService.Navigate(new SearchResults(results, entityType));
            else Toastr.Info("No Results", "There are no " + type + "s matching: '" + searchString + "'");
        }

        private async Task<List<ISpotifyEntity>> GetSearchResults(string searchString, SpotifyEntity type) {
            await Spotify.RequestToken();
            if (type == SpotifyEntity.Artist) return await Spotify.SearchArtists(searchString);
            else if (type == SpotifyEntity.Album) return await Spotify.SearchAlbums(searchString);
            else {
                Toastr.Error("Error", "Invalid Search Type");
                return null;
            }
        }

    }
}
