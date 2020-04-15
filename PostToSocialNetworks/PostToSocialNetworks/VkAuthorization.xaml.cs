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

namespace PostToSocialNetworks
{
    //Class which implements a page with a webview element for authorization and a go back button for navigating back to the mainwindow

    public sealed partial class VkAuthorization : Page
    {
        public VkAuthorization()
        {
            this.InitializeComponent();           
            this.VkAuthorizationBrowser.Source = new Uri("https://oauth.vk.com/authorize?client_id=" + VKQuerySender.APPLICATION_ID 
                + "&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=wall,photos,groups&response_type=token&v=5.103");
        }

        private void VkAuthorizationBrowser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            char[] separators = new char[] { '=', '&' };
            string[] urlParts = sender.Source.ToString().Split(separators);
           
            if (urlParts.Length >= 6)
            {
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["vk_access_token"] =  urlParts[1];
                localSettings.Values["vk_user_id"] = urlParts[5];
            }          
        }

        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
