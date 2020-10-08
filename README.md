### POST TO SOCIAL NETWORKS

I used C# and XAML. The application is based on the UWP framework and Facebook Graph API + VK API. Moreover, I used Newtonsoft.json and several additional libraries. I chose UWP because Facebook API requires authentication from Windows Store for the desktop applications based on .NET. 

The application is desktop, on the main page there are authentification buttons, message forms as well as attach button, log, active accounts information and controls, which allow to choose between posting to a public page and user's profile.

For every query to the API there is a message outputted in the log, which gives the user an indication, whether it was successful or not. Basically, that includes authentification, image uploading and sending the message 

The user launches the application. He has to authorize if he is not authorized already. Authorization is done by sending a GET query to the respective APIs. If the query was successful, the access token is stored in the program. In contrast with VK, Facebook has temporary access tokens (long-term last for 60 days). Since the program remembers the token, there is a necessity to check if the token is still valid before performing other queries. After that client acquires a list of groups administrated by the user by sending a GET query to the API. The application waits for the user to hit the "Post" button. Photos are uploaded to VK and FB servers, using the POST queries straight after that. For VK the process is the following: finding a server for uploading, uploading the pictures on the server, saving the pictures. For FB: checking whether a PostPhotos album exists and if not creating it, uploading the photos to the album. The posts are made to the social media pages. At every stage of the process, the user is notified about the success/failure of the queries. Finally, the application is ready to repeat the process. There is a special button to clean all the fields called "reset".
. 
