# VkLib
Portable class library to work with VK.com api. Used in [Meridian](https://github.com/Stealth2012/meridian) and some other projects.
Currently doesn't cover whole api but work in progress.

# Example
### Initializing
```
var vk = new Vkontakte("YOUR_APP_ID", "YOUR_APP_SECRET");
```
### Authentication
To invoke vk methods, you need to perform authentication at first.
To authenticate you should show a WebView (or similar) component to the user and handle redirect to https://oauth.vk.com/blank.html. More info is here: http://vk.com/dev/auth_mobile
VkLib provides 2 helper methods for this:
```
//Returns auth url for opening in WebView
vk.OAuth.GetAuthUrl();
```
```
//Extracts accessToken (or error) from url
var oauthResult = vk.OAuth.ProcessAuth(url);
```
After performing authentication you must set access token to vk object
```
vk.AccessToken = token;
```
### Calling methods
Calling methods is similar to vk api. Here are some samples:
```
//Getting info with photo and city for user with id 1
var user = await vk.Users.Get(1,"photo_50,city");

//Getting audios for user with id
var audios = await vk.Audios.Get(1);
```
