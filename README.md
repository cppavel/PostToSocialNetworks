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
 
