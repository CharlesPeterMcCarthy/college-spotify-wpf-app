using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using SpotifyApp.Models;
using System.IO;
using System.Collections.ObjectModel;
using SpotifyApp.Interfaces;

namespace SpotifyApp.Services {
    public static class Database {

        private static SQLiteConnection dbConn { get; set; }

        private static void OpenDB() => dbConn.Open();

        private static void CloseDB() => dbConn.Close();

        public static void Setup() {
                // Create DB if it doesn't exists - This will only run once
            if (!File.Exists("SpotifyDB.sqlite")) SQLiteConnection.CreateFile("SpotifyDB.sqlite");

            dbConn = new SQLiteConnection("Data Source=SpotifyDB.sqlite;Version=3;datetimeformat=CurrentCulture");
            OpenDB();

            string sql = "CREATE TABLE IF NOT EXISTS artists (" +
                "ID VARCHAR(200) PRIMARY KEY, Name VARCHAR(50), " +
                "Image VARCHAR(200), GenresReadable VARCHAR(500)," +
                "Followers INT, Popularity INT)";

            SQLiteCommand command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();

            sql = "CREATE TABLE IF NOT EXISTS albums (" +
                "ID VARCHAR(200) PRIMARY KEY, Name VARCHAR(50), " +
                "ArtistID VARCHAR(200), ArtistName VARCHAR(200)," +
                "Image VARCHAR(200), Tracks INT, ReleaseDate DATETIME)";

            command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();

            CloseDB();
        }

        public static void SaveArtist(Artist artist) {
            OpenDB();

            string sql = $"INSERT INTO artists (ID, Name, Image, GenresReadable, Followers, Popularity) VALUES ({artist.GetInsertStatement()})";
            SQLiteCommand command = new SQLiteCommand(sql, dbConn);

            try {
                command.ExecuteNonQuery();
            } catch (SQLiteException) {
                Toastr.Error("Error", $"You have already saved the artist '{artist.Name}'.");
                CloseDB();
                return;
            }

            Toastr.Success("Artist Saved", $"The artist '{artist.Name}' has been saved.");

            CloseDB();
        }

        public static void SaveAlbum(Album album) {
            OpenDB();

            string sql = $"INSERT INTO albums (ID, Name, ArtistID, ArtistName, Image, Tracks, ReleaseDate) VALUES ({album.GetInsertStatement()})";
            SQLiteCommand command = new SQLiteCommand(sql, dbConn);

            try {
                command.ExecuteNonQuery();
            } catch (SQLiteException) {
                Toastr.Error("Error", $"You have already saved the album '{album.Name}'.");
                CloseDB();
                return;
            }

            Toastr.Success("Album Saved", $"The album '{album.Name}' has been saved.");

            CloseDB();
        }

        public static ObservableCollection<ISpotifyEntity> RetrieveArtists() {
            OpenDB();

            ObservableCollection<ISpotifyEntity> artists = new ObservableCollection<ISpotifyEntity>();

            string sql = "SELECT * FROM artists";
            SQLiteCommand command = new SQLiteCommand(sql, dbConn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                artists.Add(new Artist() {
                    ID = (string)reader["ID"],
                    Name = (string)reader["Name"],
                    Image = (string)reader["Image"],
                    GenresReadable = (string)reader["GenresReadable"],
                    Followers = (int)reader["Followers"],
                    Popularity = (int)reader["Popularity"]
                });
            }

            CloseDB();

            return artists;
        }

        public static ObservableCollection<ISpotifyEntity> RetrieveAlbums() {
            OpenDB();

            ObservableCollection<ISpotifyEntity> albums = new ObservableCollection<ISpotifyEntity>();

            string sql = "SELECT * FROM albums";
            SQLiteCommand command = new SQLiteCommand(sql, dbConn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                albums.Add(new Album() {
                    ID = (string)reader["ID"],
                    Name = (string)reader["Name"],
                    ArtistID = (string)reader["ArtistID"],
                    ArtistName = (string)reader["ArtistName"],
                    Image = (string)reader["Image"],
                    Tracks = (int)reader["Tracks"],
                    ReleaseDate = (DateTime)reader["ReleaseDate"]
                });
            }

            CloseDB();

            return albums;
        }

        public static bool DeleteArtist(Artist artist) {
            OpenDB();

            string sql = $"DELETE FROM artists WHERE ID = '{artist.ID}'";
            SQLiteCommand command = new SQLiteCommand(sql, dbConn);

            try {
                command.ExecuteNonQuery();
            } catch (SQLiteException) {
                Toastr.Error("Error", $"Unable to delete artist '{artist.Name}'.");
                CloseDB();
                return false;
            }

            Toastr.Success("Arist Deleted", $"'{artist.Name}' has been deleted.");

            CloseDB();

            return true;
        }

        public static bool DeleteAlbum(Album album) {
            OpenDB();

            string sql = $"DELETE FROM albums WHERE ID = '{album.ID}'";
            SQLiteCommand command = new SQLiteCommand(sql, dbConn);

            try {
                command.ExecuteNonQuery();
            } catch (SQLiteException) {
                Toastr.Error("Error", $"Unable to delete album '{album.Name}'.");
                CloseDB();
                return false;
            }

            Toastr.Success("Album Deleted", $"'{album.Name}' has been deleted.");

            CloseDB();

            return true;
        }

    }
}
