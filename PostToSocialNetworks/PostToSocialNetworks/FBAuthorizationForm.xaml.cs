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

    public sealed partial class FBAuthorizationForm : Page
    {
        public FBAuthorizationForm()
        {
            this.InitializeComponent();
            this.FBAuthorizationBrowser.Source = new Uri("https://www.facebook.com/v6.0/dialog/oauth?client_id="+FacebookQuerySender.APPLICATION_ID+
                "&display=page&response_type=token&scope=public_profile,manage_pages,publish_pages,publish_to_groups,user_photos&redirect_uri"+
                    "=https://www.facebook.com/connect/login_success.html");
        }

        private void FBAuthorizationBrowser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            string successPrefix = "https://www.facebook.com/connect/login_success.html#access_token=";
            string actualPrefix = this.FBAuthorizationBrowser.Source.ToString().Substring(0, successPrefix.Length);
            if(actualPrefix.Equals(successPrefix))
            {
                string access_token = this.FBAuthorizationBrowser.Source.ToString().Split(new char[] { '&', '=' })[1];
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["fb_access_token"] = access_token;                 
            }
        }

        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
