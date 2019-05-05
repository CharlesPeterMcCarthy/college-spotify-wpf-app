using Newtonsoft.Json;
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

        public static async Task<string> SearchAlbums(string searchString) => await SearchSpotify("track", searchString);

        public static async Task<string> SearchArtists(string searchString) => await SearchSpotify("artist", searchString);

        public static async Task<string> SearchSpotify(string type, string searchString) {
            Console.WriteLine($"https://api.spotify.com/v1/search?q={searchString}&type={type}&limit=10");
            HttpResponseMessage response = await client.GetAsync(
                $"https://api.spotify.com/v1/search?q={searchString}&type=artist&limit=10"
            );

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

    }
}
