using SpotifyApp.Models;
using SpotifyApp.Pages;
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

namespace SpotifyApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            Setup();
            NavigateToSearchSpotify();
        }

        public void Setup() {
            Toastr.TurnOnNotifications();
            Database.Setup();
        }

        private void SearchSpotify(object sender, RoutedEventArgs e) => NavigateToSearchSpotify();

        private void MyArtists(object sender, RoutedEventArgs e) => NavigateToMyArtists();

        private void MyAlbums(object sender, RoutedEventArgs e) => NavigateToMyAlbums();

        private void SetheadingText(string heading) => tblkHeading.Text = heading;

        private void NavigateToSearchSpotify() {
            SearchSpotify ss = new SearchSpotify();

            SetheadingText("Search Spotify");
            mainFrame.NavigationService.Navigate(ss);
        }

        private void NavigateToMyArtists() {
            MySaved ms = new MySaved(SpotifyEntity.Artist);

            SetheadingText("My Artists");
            mainFrame.NavigationService.Navigate(ms);
        }

        private void NavigateToMyAlbums() {
            MySaved ms = new MySaved(SpotifyEntity.Album);

            SetheadingText("My Artists");
            mainFrame.NavigationService.Navigate(ms);
        }
    }
}
