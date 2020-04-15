using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Windows.Storage;
using Windows.Security.Authentication.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace PostToSocialNetworks
{
    //This class implements the FB API methods for posting to a page

    class FacebookQuerySender
    {
        public static string APPLICATION_ID = "1723825537757304";
        //Should be stored on the server, and any query using app_secret should be run from the server as well.
        private static string APP_SECRET = "3b651a7993d1c80373e6317a96ae0e73";
        private const string API_URL = "https://graph.facebook.com/v6.0/";
        private static readonly HttpClient client = new HttpClient();

        private string token;

        public FacebookQuerySender(string token)
        {
            this.token = token;
        }

        public async Task<string> UpdatePost(string groupAccessToken, string postId, string message, string attach)
        {
            var values = new Dictionary<string, string>
            {
                {"attached_media",attach },
                { "message", message },
                { "access_token", groupAccessToken },
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(API_URL + postId, content);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Post(string groupId, string groupAccessToken, string message)
        {

            var values = new Dictionary<string, string>
            {
                { "message", message },   
                { "access_token", groupAccessToken },
            };


            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(API_URL + groupId + "/feed",content);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAllGroups(string userId)
        {

            var response = await client.GetAsync(API_URL + userId + "/accounts?" + "fields=name,access_token&access_token=" + token);

            return await response.Content.ReadAsStringAsync();

        }

        public async Task<string> UploadPhoto(string albumId, StorageFile file,string accessToken)
        {

            var content = new MultipartFormDataContent();
             
            var fstream = await file.OpenReadAsync();

            var streamContent = new StreamContent(fstream.AsStream());

            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                FileName = file.Name      
            };
            content.Add(new StringContent(accessToken), "access_token");
            content.Add(new StringContent("true"), "no_story");
            content.Add(streamContent);      

            var response = await client.PostAsync(API_URL + albumId+"/photos", content);

            var bytes = await response.Content.ReadAsByteArrayAsync();

            return System.Text.Encoding.GetEncoding(1251).GetString(bytes);
        }

        public async Task<string> CreateAlbum(string groupId, string groupAccessToken)
        {
            var values = new Dictionary<string, string>
            {
                { "message", "Photos from posts" },
                {"name","PostsPhotos" },           
                { "access_token", groupAccessToken },
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(API_URL + groupId + "/albums", content);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAllAlbums(string groupId, string groupAccessToken)
        {
            var response = await client.GetAsync(API_URL + groupId + "/albums?access_token=" + groupAccessToken);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> CheckTokenValid()
        {
            var response = await client.GetAsync(API_URL + "debug_token?input_token=" + token + "&access_token=" + APPLICATION_ID +"|"+APP_SECRET);

            string result = await response.Content.ReadAsStringAsync();
            JObject o = JObject.Parse(result);
            string valid = o["data"]["is_valid"].ToString();
            return valid.Equals("True");
        }

        public async Task<string> GetGeneralInfo()
        {
            var response = await client.GetAsync(API_URL + "me?fields=id,name&access_token="+token);
            
            return await response.Content.ReadAsStringAsync();
        }

    }
}
