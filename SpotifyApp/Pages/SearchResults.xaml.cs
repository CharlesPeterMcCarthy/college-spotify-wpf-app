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

namespace SpotifyApp.Pages {
    /// <summary>
    /// Interaction logic for SearchResults.xaml
    /// </summary>
    public partial class SearchResults : Page {

        public SearchResults() {
            InitializeComponent();
        }

        public SearchResults(List<ISpotifyEntity> results) : this() {
            Console.WriteLine(results[0].Name);

            lbxResults.ItemsSource = results;
        }

    }
}
