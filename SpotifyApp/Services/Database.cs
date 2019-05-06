using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using SpotifyApp.Models;

namespace SpotifyApp.Services {
    public static class Database {

        public static void Connect() {
            SQLiteConnection.CreateFile("SpotifyDB.sqlite");

            SQLiteConnection dbConn = new SQLiteConnection("Data Source=SpotifyDB.sqlite;Version=3;");
            dbConn.Open();

            string sql = "CREATE TABLE IF NOT EXISTS artists (" +
                "ID VARCHAR(200), Name VARCHAR(50), " +
                "Image VARCHAR(200), GenresReadable VARCHAR(500)," +
                "Followers INT, Popularity INT)";

            SQLiteCommand command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();

            dbConn.Close();
        }

        public static void SaveArtist(Artist artist) {
            SQLiteConnection dbConn = new SQLiteConnection("Data Source=SpotifyDB.sqlite;Version=3;");
            dbConn.Open();

            string sql = $"INSERT INTO artists (ID, Name, Image, GenresReadable, Followers, Popularity) VALUES ({artist.GetInsertStatement()})";

            SQLiteCommand command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();

            dbConn.Close();
        }

    }
}
