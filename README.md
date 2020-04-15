[RUSSIAN, РУССКИЙ]

1. Название задачи - пост во все социальные сети. Выполнено задание со звездочкой (текст + картинки).

2. 
2.1 Я использовал язык C# и язык разметки XAML. Я создал приложение UWP, в котором реализовал методы API VK и Facebook. Также использовал библиотеку Newtonsoft.json и несколько вспомогательных библиотек. UWP было выбрано, потому что Facebook API требует аутентификатор магазина windows при создании десктопных приложений с использованием .NET.

2.2 Приложение десктопное, в главном окне есть кнопки аутентификации, поле для ввода сообщения, кнопка приложить фотографию, лог,
информацию об используемых аккаунтах, элементы, позволяющие выбрать группу/частную страницу публикации.

2.3 Пользователю выводится сообщение в логе событий об успешности/не успешности запросов к серверу. Это включает в себя (упрощенно): аутентификацию, загрузку изображений, отправку сообщения.

3. Ссылка на видео демонстрацию с комментариями  https://www.loom.com/share/884af6e8a35b490aa7817419deb7cc08

4. Пользователь запускает программу, ему необходимо авторизоваться, для чего для каждой социальной сети с помощью GET запроса к API происходит открытие формы авторизации, в случае успеха токен считывается и сохраняется в приложении.
Для Facebook имеет место "время жизни" токена, а так как приложение запоминает токен и не просит каждый раз авторизоваться, происходит проверка действительности токена для Facebook. Затем с помощью GET запроса получаем с API общую информацию о пользователе и список групп (страниц), для  которых пользователь является администратором. После этого ждем пока пользователь отредактирует форму отправки сообщения(напишет текст, приложит картинки, выберет группы). Когда пользователь нажимает кнопку Post происходит загрузка картинок на сервера vk и fb с помощью POST запросов. Для VK процесс такой: сначала получаем сервер для загрузки, потом загружаем картинки на этот сервер, после чего сохраняем их. Для FB: создаем или проверяем что существует альбом PostPhotos в выбранной группе, загружаем туда фотографии. Затем с помощью POST запроса происходят непосредственно публикации постов в соц. сети. На каждом этапе пользователю предоставляется информация об успешности запросов. После этого приложение готово к повторному использованию. Для этого есть специальная кнопка "reset". 

5. Запустить программу можно на операционной системе Windows 10. Для этого используем папку Installation, в ней будет Bundle приложения. Предварительно нужно сделать следующие шаги: 1. Включить режим разработчика на компьютере 2. Затем согласно действиям, описанным на сайте:
http://www.nookery.ru/decided-error-0x800b0109/ произвести манипуляции с сертификатом приложения (оно не опубликовано в Windows store и является тестовым, поэтому это необходимо). Перед тем как запускать сам Bundle приложения (PostToSocialNetworks_1.0.0.0_x86_x64_arm_Debug) нужно перейти в папку Dependecies, дальше выбрать разрядность вашей системы и установить Bundles, которые будут внутри (Microsoft.NET.CoreRuntime.1.0 и Microsoft.VCLibs.x64.Debug.14.00). После этого можно перейти к установке самого приложения. Оно появится в меню пуск, после успешной инсталляции. 
ВАЖНАЯ РЕМАРКА: Для того чтобы в приложении Facebook API мог зарегистрироваться любой пользователь оно должно пройти модерацию, это занимает в районе недели, а иногда и больше, поэтому пока что мое приложение находится в тестовом режиме и в него могут войти только аккаунты тестировщиков или админов. Для входа в facebook я предоставляю вам данный аккаунт тестировщика: login: +79255076294 password: testpokmjyt. Для ВК можете использовать свой персональный аккаунт или какой-либо другой аккаунт для тестирования - там ограничений нет.

[ENGLISH]

1.Task - Post to all social networks. Extra task accomplished (text + pictures).

2.

2.1 I used C# and XAML. The application is based on the UWP framework and Facebook Graph API + VK API were implemented in the backend.  Moreover, I used Newtonsoft.json and several additional libraries. I chose UWP because Facebook API requires authentication from Windows Store for the desktop applications based on .NET. 

2.2 The application is desktop, on the main page there are authentification buttons, message forms as well as attach button, log, active accounts information and controls, which allow to choose between posting to a public page and user's profile.

2.3 For every query to the API there is a message outputted in the log, which gives the user an indication, whether it was successful or not. Basically, that includes authentification, image uploading and sending the message 

3. Link to the narrated video demonstration (available only in Russian for now): https://www.loom.com/share/884af6e8a35b490aa7817419deb7cc08. 

4. The user launches the application. He has to authorize if he is not authorized already. Authorization is done by sending a GET query to the respective APIs. If the query was successful, the access token is stored in the program. In contrast with VK, Facebook has temporary access tokens (long-term last for 60 days). Since the program remembers the token, there is a necessity to check if the token is still valid before performing other queries. After that client acquires a list of groups administrated by the user by sending a GET query to the API. The application waits for the user to hit the "Post" button. Photos are uploaded to VK and FB servers, using the POST queries straight after that. For VK the process is the following: finding a server for uploading, uploading the pictures on the server, saving the pictures. For FB: checking whether a PostPhotos album exists and if not creating it, uploading the photos to the album. The posts are made to the social media pages. At every stage of the process, the user is notified about the success/failure of the queries. Finally, the application is ready to repeat the process. There is a special button to clean all the fields called "reset'.

5. You can run the program in Windows 10. The first step is to download the Installation folder.  The next one is to switch your PC to developer mode and approve the security certificate for the application following the steps outlined at http://www.nookery.ru/decided-error-0x800b0109/. Before running the application bundle itself, you need to make sure that Microsoft.NET.CoreRuntime.1.0 and Microsoft.VCLibs.x64.Debug.14.00 are installed. You need to navigate to the Dependencies folder choose the folder according to the type of your system and try to install the latter bundles. Now you can run the bundle itself it will appear in the windows explorer after successful installation. IMPORTANT NOTE: Facebook application needs to be approved to be used by any user. The approval process usually takes around a week, which means that the app is currently in the test mode. Due to that fact, only admins and testers can use the application. Hence I provide you with a tester account login: +79255076294 password: testpokmjyt. For VK there is no such limitation, you can use any account you want to.

