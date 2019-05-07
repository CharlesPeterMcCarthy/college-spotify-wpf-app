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
        public ObservableCollection<ISpotifyEntity> FilteredResults { get; set; }
        public SpotifyEntity Type { get; set; }

        public MySaved() {
            InitializeComponent();
        }

        public MySaved(SpotifyEntity type) : this() {
            Type = type;

            if (Type == SpotifyEntity.Artist) GetSavedArtists();
            if (Type == SpotifyEntity.Album) GetSavedAlbums();

            UpdateResultsCount();
        }

        private void GetSavedArtists() {
            Results = Database.RetrieveArtists();
            FilteredResults = Results;
            lbxArtists.ItemsSource = FilteredResults;
        }

        private void GetSavedAlbums() {
            Results = Database.RetrieveAlbums();
            FilteredResults = Results;
            lbxAlbums.ItemsSource = FilteredResults;
        }

        private void UpdateResultsCount() {
            string typeString = Type == SpotifyEntity.Artist ? "artist" : "album";
            typeString += Results.Count != 1 ? "s" : "";
            string searchQuery = tbxSearchQuery.Text;

            tbxSavedCount.Text = searchQuery.Length > 0 ? $"{FilteredResults.Count} matching {typeString}" : $"You have {Results.Count} saved {typeString}";
        }

        private void DeleteEntity(object sender, RoutedEventArgs e) {
            if (Type == SpotifyEntity.Artist) {
                bool deleted = Database.DeleteArtist(((ListBoxItem)lbxArtists.ContainerFromElement((Button)sender)).Content as Artist);
                GetSavedArtists();
            }
            if (Type == SpotifyEntity.Album) {
                bool deleted = Database.DeleteAlbum(((ListBoxItem)lbxAlbums.ContainerFromElement((Button)sender)).Content as Album);
                GetSavedAlbums();
            }

            UpdateResultsCount();
        }

        private void TbxSearchQuery_TextChanged(object sender, TextChangedEventArgs e) {
            string searchQuery = ((TextBox)sender).Text;

            if (Type == SpotifyEntity.Artist) FilterArtists(searchQuery);
            if (Type == SpotifyEntity.Album) FilterAlbums(searchQuery);
        }

        private void FilterArtists(string searchQuery) => DisplayFilteredList(Results.Where(r => ((Artist)r).Name.ToLower().Contains(searchQuery.ToLower())));

        private void FilterAlbums(string searchQuery) => DisplayFilteredList(Results.Where(r => ((Album)r).Name.ToLower().Contains(searchQuery.ToLower())));

        private void DisplayFilteredList(IEnumerable<ISpotifyEntity> filtered) {
            FilteredResults = new ObservableCollection<ISpotifyEntity>(filtered);

            if (Type == SpotifyEntity.Artist) lbxArtists.ItemsSource = FilteredResults;
            if (Type == SpotifyEntity.Album) lbxAlbums.ItemsSource = FilteredResults;

            UpdateResultsCount();
        }
    }
}
