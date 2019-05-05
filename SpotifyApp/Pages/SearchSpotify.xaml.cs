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
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e) => SearchSpotifyAsync();

        private async void SearchSpotifyAsync() {
            string searchString = tbxSearchString.Text;
            string type = (string) cbxType.SelectedItem.ToString();

            if (searchString.Length < 1) {
                Toastr.Warning("Too Short", "Please enter a longer search query");
                return;
            }

            await Spotify.RequestToken();
            List<ISpotifyEntity> results;

            Console.WriteLine(type);

            if (type.Equals("Artist")) results = await Spotify.SearchArtists(searchString);
            else if (type.Equals("Album")) results = await Spotify.SearchAlbums(searchString);
            else {
                Toastr.Error("Error", "Invalid Search Type");
                return;
            }

            NavigationService.Navigate(new SearchResults(results));
        }

    }
}
