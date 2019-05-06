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
    /// Interaction logic for SearchResults.xaml
    /// </summary>
    public partial class SearchResults : Page {

        public ObservableCollection<ISpotifyEntity> Results { get; set; }
        public SpotifyEntity Type { get; set; }

        public SearchResults() {
            InitializeComponent();
        }

        public SearchResults(List<ISpotifyEntity> results, SpotifyEntity entityType) : this() {
            Results = new ObservableCollection<ISpotifyEntity>(results);
            Type = entityType;

            if (Type == SpotifyEntity.Artist) {
                lbxArtists.ItemsSource = Results;
                cbxSort.ItemsSource = new string[] { "Name", "Popularity", "Followers" };
            }
            if (Type == SpotifyEntity.Album) {
                lbxAlbums.ItemsSource = Results;
                cbxSort.ItemsSource = new string[] { "Name", "Artist", "Tracks", "Release Date" };
            }

            tbxResultsCount.Text = Results.Count.ToString();
        }

        private void CbxSort_SelectionChanged(object sender, SelectionChangedEventArgs e) => SortResults();

        private void ChbxSortDescending_Clicked(object sender, RoutedEventArgs e) => SortResults();

        private void SortResults() {
            string sortBy = cbxSort.SelectedValue.ToString();
            bool sortDescending = (bool)chbxSortDescending.IsChecked;

            if (Type == SpotifyEntity.Artist) SortArtists(sortBy, sortDescending);
            if (Type == SpotifyEntity.Album) SortAlbums(sortBy, sortDescending);
        }

        private void SortArtists(string sortBy, bool sortDescending) {
            switch (sortBy) {
                case "Name":
                    DisplaySortedList(sortDescending ? 
                        Results.OrderByDescending(r => ((Artist)r).Name) : 
                        Results.OrderBy(r => ((Artist)r).Name));
                    break;
                case "Popularity":
                    DisplaySortedList(sortDescending ?
                        Results.OrderByDescending(r => ((Artist)r).Popularity) :
                        Results.OrderBy(r => ((Artist)r).Popularity));
                    break;
                case "Followers":
                    DisplaySortedList(sortDescending ?
                        Results.OrderByDescending(r => ((Artist)r).Followers) :
                        Results.OrderBy(r => ((Artist)r).Followers));
                    break;
                default:
                    Toastr.Error("Error", "Invaid sort type");
                    break;
            }
        }

        private void SortAlbums(string sortBy, bool sortDescending) {
            switch (sortBy) {
                case "Name":
                    DisplaySortedList(sortDescending ?
                        Results.OrderByDescending(r => ((Album)r).Name) :
                        Results.OrderBy(r => ((Album)r).Name));
                    break;
                case "Artist":
                    DisplaySortedList(sortDescending ?
                        Results.OrderByDescending(r => ((Album)r).Name) :
                        Results.OrderBy(r => ((Album)r).ArtistName));
                    break;
                case "Tracks":
                    DisplaySortedList(sortDescending ?
                        Results.OrderByDescending(r => ((Album)r).Name) :
                        Results.OrderBy(r => ((Album)r).Tracks));
                    break;
                case "Release Date":
                    DisplaySortedList(sortDescending ?
                        Results.OrderByDescending(r => ((Album)r).Name) :
                        Results.OrderBy(r => ((Album)r).ReleaseDate));
                    break;
                default:
                    Toastr.Error("Error", "Invaid sort type");
                    break;
            }
        }

        private void DisplaySortedList(IOrderedEnumerable<ISpotifyEntity> sorted) {
            Results = new ObservableCollection<ISpotifyEntity>(sorted);

            if (Type == SpotifyEntity.Artist) lbxArtists.ItemsSource = Results;
            if (Type == SpotifyEntity.Album) lbxAlbums.ItemsSource = Results;
        }

    }
}
