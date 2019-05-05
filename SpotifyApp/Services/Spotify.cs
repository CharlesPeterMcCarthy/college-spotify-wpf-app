using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;


namespace SpotifyApp.Services {
    public static class Spotify {

        private static readonly HttpClient client = new HttpClient();
        private static string APIToken { get; set; }

        public static async void RequestToken() {
            HttpResponseMessage response = await client.PostAsync(
                "https://indigoassignment.appspot.com/get-token",
                new StringContent(
                    JsonConvert.SerializeObject(
                        new Dictionary<string, string> {
                            { "cliejntID", "88914c854e7f4c2687f971155584dabf" }
                        }
                    ),
                    Encoding.UTF8,
                    "text/json"
                )
            );

            string responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);
            if (responseObject.error != null) {
                Toastr.Error("Spotify Error", (string) responseObject.error);
            } else {
                APIToken = JsonConvert.DeserializeObject<dynamic>(responseString).token;
                Toastr.Success("Token", "New Token: " + APIToken);
            }
        }

    }
}
