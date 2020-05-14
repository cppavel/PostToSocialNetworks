[ENGLISH]

1.Task - Post to all social networks. Extra task accomplished (text + pictures).

2.

2.1 I used C# and XAML. The application is based on the UWP framework and Facebook Graph API + VK API were implemented in the backend.  Moreover, I used Newtonsoft.json and several additional libraries. I chose UWP because Facebook API requires authentication from Windows Store for the desktop applications based on .NET. 

2.2 The application is desktop, on the main page there are authentification buttons, message forms as well as attach button, log, active accounts information and controls, which allow to choose between posting to a public page and user's profile.

2.3 For every query to the API there is a message outputted in the log, which gives the user an indication, whether it was successful or not. Basically, that includes authentification, image uploading and sending the message 

3. Link to the narrated video demonstration (available only in Russian for now): https://www.loom.com/share/884af6e8a35b490aa7817419deb7cc08. 

4. The user launches the application. He has to authorize if he is not authorized already. Authorization is done by sending a GET query to the respective APIs. If the query was successful, the access token is stored in the program. In contrast with VK, Facebook has temporary access tokens (long-term last for 60 days). Since the program remembers the token, there is a necessity to check if the token is still valid before performing other queries. After that client acquires a list of groups administrated by the user by sending a GET query to the API. The application waits for the user to hit the "Post" button. Photos are uploaded to VK and FB servers, using the POST queries straight after that. For VK the process is the following: finding a server for uploading, uploading the pictures on the server, saving the pictures. For FB: checking whether a PostPhotos album exists and if not creating it, uploading the photos to the album. The posts are made to the social media pages. At every stage of the process, the user is notified about the success/failure of the queries. Finally, the application is ready to repeat the process. There is a special button to clean all the fields called "reset'.

5. You can run the program in Windows 10. The first step is to download the Installation folder.  The next one is to switch your PC to developer mode and approve the security certificate for the application following the steps outlined at http://www.nookery.ru/decided-error-0x800b0109/. Before running the application bundle itself, you need to make sure that Microsoft.NET.CoreRuntime.1.0 and Microsoft.VCLibs.x64.Debug.14.00 are installed. You need to navigate to the Dependencies folder choose the folder according to the type of your system and try to install the latter bundles. Now you can run the bundle itself it will appear in the windows explorer after successful installation. 
