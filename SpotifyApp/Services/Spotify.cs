﻿using Newtonsoft.Json;
using SpotifyApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApp.Services {
    public static class Spotify {

        private static readonly HttpClient client = new HttpClient();
        private static string apiToken;
        private static string APIToken { set {
                apiToken = value;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
            }
        }

        public static async Task RequestToken() {
            HttpResponseMessage response = await client.PostAsync(
                "https://indigoassignment.appspot.com/get-token",
                new StringContent(
                    JsonConvert.SerializeObject(
                        new Dictionary<string, string> {
                            { "clientID", "88914c854e7f4c2687f971155584dabf" }
                        }
                    ),
                    Encoding.UTF8,
                    "text/json"
                )
            );

            response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);
            if (responseObject.error != null) {
                Toastr.Error("Spotify Error", (string) responseObject.error);
            } else {
                APIToken = JsonConvert.DeserializeObject<dynamic>(responseString).token;
            }
        }

        public static async Task<List<Album>> SearchAlbums(string searchString) {
            List<Album> albums = new List<Album>();
            string response = await SearchSpotify("album", searchString);
            var json = JsonConvert.DeserializeObject<dynamic>(response);
            
            foreach (var a in json.albums.items) {
                albums.Add(new Album() {
                    AlbumID = a.id,
                    Name = a.name,
                    ReleaseDate = a.release_date,
                    Image = a.images[0].url,
                    ArtistID = a.artists[0].id,
                    ArtistName = a.artists[0].name
                });
            }

            return albums;
        }

        public static async Task<List<Artist>> SearchArtists(string searchString) {
            List<Artist> artists = new List<Artist>();
            string response = await SearchSpotify("artist", searchString);
            var json = JsonConvert.DeserializeObject<dynamic>(response);

            foreach (var a in json.artists.items) {
                var image = a.images.Count > 0 ? a.images[0].url : null;

                artists.Add(new Artist() {
                    ArtistID = a.id,
                    Name = a.name,
                    Image = image,
                    Genres = a.genres.ToObject<string[]>(),
                    Followers = a.followers.total
                });
            }

            return artists;
        }

        public static async Task<string> SearchSpotify(string type, string searchString) {
            HttpResponseMessage response = await client.GetAsync(
                $"https://api.spotify.com/v1/search?q={searchString}&type={type}&limit=10"
            );

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

    }
}
