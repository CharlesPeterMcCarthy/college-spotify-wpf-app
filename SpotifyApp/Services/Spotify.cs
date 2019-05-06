using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using SpotifyApp.Interfaces;
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
        private static string APIToken {
            get {
                return apiToken;
            }
            set {
                apiToken = value;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
            }
        }

        public enum SpotifyEntity {
            Artist, Album
        }

        public static async Task RequestToken() {
            HttpResponseMessage response = null;

            try {
                response = await client.PostAsync(
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
            } catch (Exception) {
                Toastr.Error("Error", "An error occured while attempting to retrieve a Spotify Access Token");
                return;
            }

            string responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

            if (responseObject.error != null) {
                Toastr.Error("Spotify Error", (string) responseObject.error);
            } else {
                try {
                    APIToken = responseObject.token;
                } catch (Exception) {
                    Toastr.Error("Error", "No Spotify Access Token was returned from the server.");
                }
            }
        }

        public static async Task<List<ISpotifyEntity>> SearchAlbums(string searchString) {
            if (APIToken == null) {
                Toastr.Error("Error", "Unable to perform Spotify search - Please try again.");
                return null;
            }
            List<ISpotifyEntity> albums = new List<ISpotifyEntity>();
            string response = await SearchSpotify("album", searchString);
            var json = JsonConvert.DeserializeObject<dynamic>(response);

            try {
                foreach (var a in json.albums.items) {
                    albums.Add(new Album() {
                        ID = a.id,
                        Name = a.name,
                        ReleaseDate = a.release_date,
                        Image = a.images[0].url,
                        ArtistID = a.artists[0].id,
                        ArtistName = a.artists[0].name,
                        Tracks = a.total_tracks
                    });
                }
            } catch (RuntimeBinderException) {
                Toastr.Error("Error", "Could not parse Spotify albums response - Something is missing");
            } catch (Exception) {
                Toastr.Error("Error", "Unknown Spotify error when retrieving albums");
            }

            return albums;
        }

        public static async Task<List<ISpotifyEntity>> SearchArtists(string searchString) {
            if (APIToken == null) {
                Toastr.Error("Error", "Unable to perform Spotify search - Please try again");
                return null;
            }
            List<ISpotifyEntity> artists = new List<ISpotifyEntity>();
            string response = await SearchSpotify("artist", searchString);
            var json = JsonConvert.DeserializeObject<dynamic>(response);

            try {
                foreach (var a in json.artists.items) {
                    var image = a.images.Count > 0 ? a.images[0].url : null;

                    artists.Add(new Artist() {
                        ID = a.id,
                        Name = a.name,
                        Image = image,
                        Genres = a.genres.ToObject<string[]>(),
                        Followers = a.followers.total,
                        Popularity = a.popularity
                    });
                }
            } catch (RuntimeBinderException) {
                Toastr.Error("Error", "Could not parse Spotify artists response - Something is missing");
            } catch (Exception) {
                Toastr.Error("Error", "Unknown Spotify error when retrieving artists");
            }

            return artists;
        }

        private static async Task<string> SearchSpotify(string type, string searchString) {
            HttpResponseMessage response = null;

            try {
                response = await client.GetAsync(
                    $"https://api.spotify.com/v1/search?q={searchString}&type={type}&limit=10"
                );

                response.EnsureSuccessStatusCode();
            } catch (Exception) {
                Toastr.Error("Error", "An error occurred when trying to call up Spotify API");
            }

            return await response.Content.ReadAsStringAsync();
        }

    }
}
