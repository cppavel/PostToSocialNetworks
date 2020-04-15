using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Security.Authentication.Web;

//SHORT DESCRIPTION OF THE CLASS

//Main page of the application, which provides user an opportunity to authorize to FB and VK and create a post, which will be sent to both social
//networks accounts. There are controls for entering a message, attaching pictures and choosing whether to post to a group or to a private page
//(applicable for VK only, since FB deprecated the opportunity to post to user's page without using their dialogs in 2018)
//There are several textblocks, which provide information about the accounts currently logged in as well as a full log of messages, which indicate 
//whether the query to API was successful.

//Calls to API are implemented in VKQuerySender.cs and FacebookQuerySender.cs. This class uses the latter to perform requests.
//Json parsing is mostly done in the MainPage class


namespace PostToSocialNetworks
{
    public sealed partial class MainPage : Page
    {
        //fields for VK API
        VKQuerySender vkQuerySender;
        private string vkToken;
        private string vkUserId;
        private JToken[] vkGroupsList;
        private JToken[] vkPhotos;
        int vkSelectedGroup = -1;

        //fields for FB API
        FacebookQuerySender fbQuerySender;
        private string fbToken;
        private string fbUserId;
        List<JToken> facebookGroups = new List<JToken>();
        int fbSelectedGroup = -1;
     
        //common fields
        private bool sendToGroupPage = false;
        private List<StorageFile> pathesToPhoto = new List<StorageFile>();
        private string messagePage ="default_message";

        public MainPage()
        {
            this.InitializeComponent();
        }

        #region eventsPurelyForUI

        private void sendToGroup_Checked(object sender, RoutedEventArgs e)
        {
            this.sendToGroupPage = true;
        }

        private void sendToGroup_Unchecked(object sender, RoutedEventArgs e)
        {
            this.sendToGroupPage = false;
        }

        private void sendToGroup_Indeterminate(object sender, RoutedEventArgs e)
        {
            this.sendToGroupPage = false;
        }

        private void message_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.messagePage = message.Text;
        }

        private void vkGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.vkSelectedGroup = vkGroups.SelectedIndex;
        }

        private void fbGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.fbSelectedGroup = this.fbGroups.SelectedIndex;
        }

        private async void vkAuthorize_Click(object sender, RoutedEventArgs e)
        {
            await WebView.ClearTemporaryWebDataAsync(); //clearing cookies
            Frame.Navigate(typeof(VkAuthorization));
        }

        private async void fbAuthorize_Click(object sender, RoutedEventArgs e)
        {
            await WebView.ClearTemporaryWebDataAsync();
            Frame.Navigate(typeof(FBAuthorizationForm));
        }

        #endregion eventsPurelyForUI

        //these are the events which are used for requesting the APIS through QuerySender classes 
        //and uploading the required files from the system.

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            vkPhotos = null;
            vkSelectedGroup = -1;
            this.vkGroups.SelectedIndex = -1;

            fbSelectedGroup = -1;

            sendToGroupPage = false;
            messagePage = "";
            pathesToPhoto.Clear();
            this.attachedImages.Text = "Currently attached:";      
            this.sendToGroup.IsChecked = false;
            this.message.Text = "Enter message";
            this.log.Text += "[APPLICATION]Reset DONE: " + DateTime.Now + "\n";
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                //object for getting the ids and tokens
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

                //vkontakte
                vkToken = (string)localSettings.Values["vk_access_token"];
                vkUserId = (string)localSettings.Values["vk_user_id"];
                if (vkToken != null)
                {
                    vkQuerySender = new VKQuerySender(vkToken);
                    string generalInfo = await vkQuerySender.GetGeneralInfo(vkUserId);
                    JObject o = JObject.Parse(generalInfo);

                    string jname = o["response"][0]["first_name"].ToString();
                    string jsurname = o["response"][0]["last_name"].ToString();
                    string text = "VK Account Info:\nName:" + jname + "\nSurname: " + jsurname + "\n";
                    this.userInfo.Text = text;
                    string groups = await vkQuerySender.GetUserGroups(vkUserId);
                    o = JObject.Parse(groups);
                    string jcount = o["response"]["count"].ToString();
                    int n = int.Parse(jcount);
                    vkGroupsList = new JToken[n];

                    string[] str = new string[n];
                    for (int i = 0; i < n; i++)
                    {
                        vkGroupsList[i] = o["response"]["items"][i];
                        str[i] = vkGroupsList[i]["name"].ToString();
                    }

                    this.vkGroups.Items.Clear();

                    for (int i = 0; i < n; i++)
                    {
                        this.vkGroups.Items.Add(str[i]);
                    }


                    this.log.Text += "[VK]Token succesfully added, general info parsed, groups (admin, editor) parsed: " + DateTime.Now + "\n";

                }

                //facebook

                fbToken = (string)localSettings.Values["fb_access_token"];

                if (fbToken != null)
                {

                    fbQuerySender = new FacebookQuerySender(fbToken);

                    bool valid = await fbQuerySender.CheckTokenValid();
                    if (valid)
                    {
                        string generalInfo = await fbQuerySender.GetGeneralInfo();

                        JObject o = JObject.Parse(generalInfo);

                        string name = o["name"].ToString();
                        fbUserId = o["id"].ToString();

                        this.userInfo.Text += "FB Account Info:\nName:" + name;

                        string groups = await fbQuerySender.GetAllGroups(fbUserId);
                        o = JObject.Parse(groups);

                        if (o["data"].HasValues)
                        {
                            JToken current = o["data"].First;
                            JToken end = o["data"].Last;

                            do
                            {
                                facebookGroups.Add(current);
                                current = current.Next;
                            } while (current!=null&&current != end);
                        }

                        this.fbGroups.Items.Clear();

                        for(int i = 0; i<facebookGroups.Count;i++)
                        {
                            this.fbGroups.Items.Add(facebookGroups[i]["name"].ToString());
                        }

                        this.log.Text += "[FB]Token succesfully added, general info parsed, groups (admin, editor) parsed: " + DateTime.Now + "\n";

                    }
                    else
                    {
                        throw new Exception("Facebook token has expired");
                    }
                }

            }
            catch (Exception error)
            {
                this.log.Text += "[APPLICATION]" + error.Message + " " + DateTime.Now + "\n[APPLICATION]Authorize again\n";
            }
        }
     
        private async void attach_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
                openPicker.CommitButtonText = "Open";
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".gif");
                StorageFile file = await openPicker.PickSingleFileAsync();

                string filePath = file.Path;

                if (filePath != null)
                {
                    //vk allows no more than 10 attachments, facebook has no such limitation
                    if (pathesToPhoto.Count == 10)
                    {
                        pathesToPhoto.RemoveAt(0);
                    }
                    pathesToPhoto.Add(file);
                    string res = "Currently attached: \n";
                    for (int i = 0; i < pathesToPhoto.Count; i++)
                    {
                        res += pathesToPhoto[i].Path + "\n";
                    }
                    this.attachedImages.Text = res;
                }
            }
            catch (Exception error)
            {
                this.log.Text += "[APPLICATION]" + error.Message + " " + DateTime.Now + "\n[APPLICATION]No photo loaded to the client\n";
            }
        
        }

        private async void post_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //vk group post
                if (this.sendToGroupPage && vkSelectedGroup != -1)
                {
                    string full_atttach = "";
                    string group_id = vkGroupsList[vkSelectedGroup]["id"].ToString();

                    //vk api often requires the group_id to be negative to distinguish it from user_id, which is always positive
                    int group_id_minus = -int.Parse(group_id);

                    vkPhotos = new JToken[pathesToPhoto.Count];

                    string server = await vkQuerySender.GroupGetWallUploadServer(group_id);
                    JObject o = JObject.Parse(server);

                    string url = o["response"]["upload_url"].ToString();

                    log.Text += "[VK]Server to Upload successfuly found: " + DateTime.Now + "\n";

                    for (int i = 0; i < pathesToPhoto.Count; i++)
                    {

                        string success = await vkQuerySender.UploadPhoto(url, pathesToPhoto[i]);
                        vkPhotos[i] = JObject.Parse(success);

                    }

                    for (int i = 0; i < pathesToPhoto.Count; i++)
                    {
                        string jphoto = vkPhotos[i]["photo"].ToString();
                        string jserver = vkPhotos[i]["server"].ToString();
                        string jhash = vkPhotos[i]["hash"].ToString();

                        string saveResult = await vkQuerySender.GroupSavePhoto(vkUserId, group_id, jphoto, jserver, jhash);
                        o = JObject.Parse(saveResult);

                        string jownerid = o["response"][0]["owner_id"].ToString();
                        string jmediaid = o["response"][0]["id"].ToString();

                        log.Text += "[VK]Photo " + (i + 1) + " uploaded: " + DateTime.Now + "\n";

                        string attached = "photo" + jownerid + "_" + jmediaid;
                        full_atttach += attached + ",";
                    }

                    if (full_atttach.Length > 0)
                        full_atttach = full_atttach.Substring(0, full_atttach.Length - 1);

                    string post = await vkQuerySender.PostToWall(Convert.ToString(group_id_minus), full_atttach, messagePage);

                    o = JObject.Parse(post);

                    string postId = o["response"]["post_id"].ToString();

                    log.Text += "[VK]Post DONE, ID: " + postId+": " + DateTime.Now + "\n";
                }
                //vk private page post
                else if(!sendToGroupPage)
                {
                    vkPhotos = new JToken[pathesToPhoto.Count];

                    string server = await vkQuerySender.UserGetWallUploadServer();
                    JObject o = JObject.Parse(server);
                   
                    string url = o["response"]["upload_url"].ToString();

                    log.Text += "[VK]Server to Upload successfuly found: " + DateTime.Now + "\n";

                    for (int i = 0; i < pathesToPhoto.Count; i++)
                    {

                        string success = await vkQuerySender.UploadPhoto(url, pathesToPhoto[i]);
                        vkPhotos[i] = JObject.Parse(success);
                                  
                    }
                    string full_atttach = "";
                    for (int i = 0; i < pathesToPhoto.Count; i++)
                    {
                        string jphoto = vkPhotos[i]["photo"].ToString();
                        string jserver = vkPhotos[i]["server"].ToString();
                        string jhash = vkPhotos[i]["hash"].ToString();

                        string saveResult = await vkQuerySender.UserSavePhoto(vkUserId, jphoto, jserver, jhash);
                        o = JObject.Parse(saveResult);                    

                        string jownerid = o["response"][0]["owner_id"].ToString();
                        string jmediaid = o["response"][0]["id"].ToString();

                        log.Text += "[VK]Photo " + (i + 1) + " uploaded: " + DateTime.Now + "\n";

                        string attached = "photo" + jownerid + "_" + jmediaid;
                        full_atttach += attached + ",";
                    }

                    if (full_atttach.Length > 0)
                        full_atttach = full_atttach.Substring(0, full_atttach.Length - 1);

                    string post = await vkQuerySender.PostToWall(vkUserId, full_atttach, messagePage);

                    o = JObject.Parse(post);

                    string postId = o["response"]["post_id"].ToString();

                    log.Text += "[VK]Post DONE, ID: " + postId + ": " + DateTime.Now + "\n";
                }
                //facebook group(page) post
                if (sendToGroupPage && this.fbSelectedGroup != -1)
                {
                    JToken currentGroup = facebookGroups[fbSelectedGroup];
                    string groupId = currentGroup["id"].ToString();
                    string groupAccessToken = currentGroup["access_token"].ToString();

                    string allAlbums = await fbQuerySender.GetAllAlbums(groupId, groupAccessToken);

                    JObject o = JObject.Parse(allAlbums);

                    JToken data = o["data"];

                    JToken current = data.First;
                    JToken end = data.Last;
                    string albumId = "";
                    do
                    {
                        string name = current["name"].ToString();
                        if (name.Equals("PostsPhotos"))
                        {
                            albumId = current["id"].ToString();
                            break;
                        }
                        current = current.Next;
                    } while (current != null && current != end);

                    //if there is no such album we create it
                    if (albumId.Equals(""))
                    {
                        string albumCreated = await fbQuerySender.CreateAlbum(groupId, groupAccessToken);
                        o = JObject.Parse(albumCreated);
                        albumId = o["id"].ToString();
                    }

                    log.Text += "[FB]Facebook album PostsPhotos created/found, ID: " + albumId + ": " + DateTime.Now + "\n";

                    //parsing the photos into a json object
                    JArray attached = new JArray();

                    for (int i = 0; i < pathesToPhoto.Count; i++)
                    {
                        string photoUploaded = await fbQuerySender.UploadPhoto(albumId, pathesToPhoto[i], groupAccessToken);
                        o = JObject.Parse(photoUploaded);
                        string currentPhotoId = o["id"].ToString();
                        JObject temp = new JObject(new JProperty("media_fbid", currentPhotoId), new JProperty("message", ""));
                        attached.Add(temp);

                        log.Text += "[FB]Photo " + (i + 1) + " uploaded: " + DateTime.Now + "\n";
                    }

                    //I noticed that there is no method which allows to post with more than one attachment.
                    //However, I found a method which modifies a post and can add the attachments to it
                    //Hence the procedure is: 1. Create a post to get an id 2. Modify a post by adding attachments

                    //1.creating post
                    string result = await fbQuerySender.Post(groupId, groupAccessToken, messagePage);

                    o = JObject.Parse(result);

                    string postId = o["id"].ToString();

                    string attachments = attached.ToString();
                    //2.modifying post
                    string postingResult = await fbQuerySender.UpdatePost(groupAccessToken, postId, messagePage, attachments);

                    o = JObject.Parse(postingResult);

                    string finalResult = o["success"].ToString();

                    if (finalResult.Equals("True"))
                    {
                        log.Text += "[FB]Post DONE, ID: " + postId + ": " + DateTime.Now + "\n";
                    }
                    else
                    {
                        throw new Exception("Post to FB was not succesfull");
                    }

                }
            }
            catch (Exception error)
            {
                this.log.Text += "[APPLICATION]" + error.Message + " " + DateTime.Now + "\n[APPLICATION]Authorize again\n";
            }
        }   
    }
}
