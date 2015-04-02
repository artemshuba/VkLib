# VkLib
Portable class library to work with VK.com api. Used in [Meridian](https://github.com/Stealth2012/meridian) and some other projects.
Currently doesn't cover whole api but work in progress.

# Example
```
//Initializing vk object
var vk = new Vkontakte("YOUR_APP_ID", "YOUR_APP_SECRET");

//Getting info with photo and city for user with id 1
var user = vk.Users.Get(1,"photo_50,city");

//Getting audios for user with id
var audios = vk.Audios.Get(1);
```
