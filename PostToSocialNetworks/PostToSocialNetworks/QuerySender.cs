using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PostToSocialNetworks
{
    class QuerySender
    {
        private static readonly HttpClient client = new HttpClient();
        public const String APPLICATION_ID = "7399506";
        private const String API_URL = "https://api.vk.com/method/";

        private string token;

        public QuerySender(String token)
        {
            this.token = token;
        }

        public async Task<string> GetGeneralInfo(string userId)
        {
            var response = await client.GetAsync(API_URL + "users.get?user_id=" + userId + "&access_token=" + token + "&v=5.103");

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostToWall(string ownerId, string attached, string message)
        {
            var values = new Dictionary<string, string>
            {
                { "owner_id", ownerId },
                { "friends_only", "0" },
                {"from_group","1" },
                {"message",message },
                {"attachments",attached },
                { "access_token", token},
                {"v", "5.103" }
            };


            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(API_URL + "wall.post", content);
            return await response.Content.ReadAsStringAsync();

        }

        public async Task<string> GetUserGroups(string userId)
        {

            var response = await client.GetAsync(API_URL + "groups.get?user_id=" + userId + "&extended=1&filter=admin,editor&offset=0&count=500&access_token=" + token + "&v=5.103");

            return await response.Content.ReadAsStringAsync();

        }

        public async Task<string> GroupGetWallUploadServer(string group_id)
        {
            var response = await client.GetAsync(API_URL + "photos.getWallUploadServer?group_id=" + group_id + "&access_token=" + token + "&v=5.103");

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UserGetWallUploadServer()
        {
            var response = await client.GetAsync(API_URL + "photos.getWallUploadServer?" + "&access_token=" + token + "&v=5.103");

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UploadPhoto(string url, string path)
        {

            var content = new MultipartFormDataContent();

            var fstream = File.OpenRead(path);

            var streamContent = new StreamContent(fstream);

            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file1",
                FileName = Path.GetFileName(path)
            };

            content.Add(streamContent);

            var response = await client.PostAsync(url, content);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UserSavePhoto(string userId, string photo, string server, string hash)
        {
            var values = new Dictionary<string, string>
            {
                { "user_id", userId },
                {"photo", photo },
                {"server",server },
                { "hash", hash },
                {"caption","photo" },
                { "access_token", token},
                {"v", "5.103" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(API_URL + "photos.saveWallPhoto", content);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GroupSavePhoto(string userId, string groupId, string photo, string server, string hash)
        {
            var values = new Dictionary<string, string>
            {
                { "user_id", userId },
                {"group_id",groupId },
                {"photo", photo },
                {"server",server },
                { "hash", hash },
                {"caption","photo" },
                { "access_token", token},
                {"v", "5.103" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(API_URL + "photos.saveWallPhoto", content);

            return await response.Content.ReadAsStringAsync();
        }



    }
}
