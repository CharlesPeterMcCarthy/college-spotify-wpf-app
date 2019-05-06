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

        private void BtnSearch_Click(object sender, RoutedEventArgs e) => SearchSpotifyAsync();

        private async void SearchSpotifyAsync() {
            if (cbxType.SelectedIndex < 0) {
                Toastr.Warning("No Type", "Please choose a type from the drop down");
                return;
            }

            string searchString = tbxSearchString.Text;
            string type = cbxType.SelectedItem.ToString();

            if (searchString.Length < 1) {
                Toastr.Warning("Too Short", "Please enter a longer search query");
                return;
            }

            List<ISpotifyEntity> results = await GetSearchResults(searchString, type);

            if (results.Count > 0) NavigationService.Navigate(new SearchResults(results));
            else Toastr.Info("No Results", "There are no " + type + "s matching: '" + searchString + "'");
        }

        private async Task<List<ISpotifyEntity>> GetSearchResults(string searchString, string type) {
            await Spotify.RequestToken();
            if (type.Equals("Artist")) return await Spotify.SearchArtists(searchString);
            else if (type.Equals("Album")) return await Spotify.SearchAlbums(searchString);
            else {
                Toastr.Error("Error", "Invalid Search Type");
                return null;
            }
        }

    }
}
