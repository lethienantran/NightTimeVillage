# Night Time Village - 2D Online Multiplayer Game
# Authors/Developers: Le Duy An Tran, Le Thien An Tran
![gameplay1](https://user-images.githubusercontent.com/114903308/203625170-577e3f4e-4a6e-469f-a2a4-ed314e5535b7.png)
# About:
A 2D Online Multiplayer Game where players will be playing as a normal villager who has to caught the kidnappers, thieves when the night comes. The kidnappers and the thieves in the day can either act as a normal villager - improving the village gates and finding lost people, assets, and figured out who are the kidnappers and thieves. Players can discuss in chats, casting skills while at night to defend the house from kidnappers and thieves. There will be days counted and time for specific actions of specific player's role.
# Softwares:
Aseprite, Unity Game Engine, Visual Studio 2022, PlayFab, Photon
# Game Features Development Process:
***1. Account Authentication (Login/Register):*** For managing player accounts, after looking and doing research for the best solution, PlayFab powered by Microsoft Azure was the only one less expensive and suitable for the scale of the game as well as its easy APIs with lots of documentations and a helpful community.
![register](https://user-images.githubusercontent.com/114903308/203625218-687705d5-7a47-4903-8ebf-5f78f656b36e.png)
***2. Room Creation:*** After developing the draft, for the build production we decided to go for having a start area that contains the assets (houses, decorations, background, etc) of the player and also the profile information (avatars, names, etc). In our opinion, this will increase the interaction and keep the player engaged.
![createroom](https://user-images.githubusercontent.com/114903308/203519581-09353ad8-9a5c-46f7-862a-2fc8096f48cc.png)
<p align="center"><em>The Appearance of the First Room Creation Draft</em></p>

![createroom](https://user-images.githubusercontent.com/114903308/203625151-77ef5f6f-ca98-47a0-8e0a-797510eff191.png)
<p align="center"><em>The Appearance of the Build Room Creation</em></p>

***3. Join/In Party:*** This is the section where we developed and changed a lot from the arts as well as the room functions. We strived to get it closer to our ideas and improved it further. We recognized that as a cooperative and social game, it should be given as much communication as possible to the player. We implemented this chat box using Photon Remote Procedure Call to deliver message, update it to every client as this is the best way we can do so far, and it is working efficiently for the purpose. These messages won't be stored amd it is just displaying and updating through Unity Text UI. Also, during this developing process, we also worked on real-time character customization and successfully got it working! It was some sort of our big achievements, and we learned a lot throught the challenge!
![inroom](https://user-images.githubusercontent.com/114903308/203519604-c9b54305-d65e-490a-a1d4-c6fde9a77398.png)
<p align="center"><em>The Party of the Draft Version</em></p>

![roomchat2](https://user-images.githubusercontent.com/114903308/203625238-d0539d88-739a-4d51-a7e0-6f363ba94a94.png)
<p align="center"><em>The Party of the Build Version</em></p>

![roomchat3](https://user-images.githubusercontent.com/114903308/203625262-48980ddb-615c-4129-b3f7-b169a7cef72a.png)
<p align="center"><em>The Customs of the Player can be changed and updated</em></p>

***4. Character Selection:*** We spent some time designing and building the scene for starting character selection when there is a new player. We first checked if the player has ever had data stored in their account on PlayFab yet, and based on that will decide which scene to direct the players, either new player session, or current/returning player session.
![1](https://user-images.githubusercontent.com/114903308/203625103-0f74af05-d5e8-4bf6-9b2e-5d90aaf7fb98.png)
<p align="center"><em>One of the Starting Characters</em></p>

![2](https://user-images.githubusercontent.com/114903308/203625111-5ea8c809-395d-4689-8c58-7e6ae5d2fb6d.png)
<p align="center"><em>One of our Favorite Starting Characters</em></p>

***5. Multiplayer Gameplay:*** This involved many of our desired functions and we had made some working with our best. This is the hardest part yet, it is a worthy challenge and had given us so much knowledge on networking and APIs. We also successed in separating the chat box during the party, and in the gameplay. The first challenge for us that we had overcome was while building the draft, the main goal is to make this player no longer visible, and uninteractable while they are in the house, and can only retrieved those back by entering and being in the same house.
![gameplay2](https://user-images.githubusercontent.com/114903308/203519630-3a60bcd6-9c24-42b3-b39d-01b32bd58bdc.png)
<p align="center"><em>The Appearance of how the Gameplay looks in Draft Version</em></p>

![gameplay3](https://user-images.githubusercontent.com/114903308/203519635-11c31e94-cbf4-4efd-9672-fca3f94465e9.png)
<p align="center"><em>The player is entering their house, and the view will be changed to be inside that house for that player</em></p>

![gameplay4](https://user-images.githubusercontent.com/114903308/203519638-2b764373-2d79-4d80-9ab6-7e2d71c539c1.png)
<p align="center"><em>If the player is in the house, within the Orthographic View - they should not be visible and interactable</em></p>

![chatgame1](https://user-images.githubusercontent.com/114903308/203625139-7bc0c9b0-0260-4fa1-ac12-95c3ba61ac52.png)
![chatgamee2](https://user-images.githubusercontent.com/114903308/203625141-099852c9-253c-4863-a46d-969fe35b9b32.png)
![gamechat4](https://user-images.githubusercontent.com/114903308/203625161-21411efe-4424-45da-ad05-4bbce49e79b4.png)

![gameplay2](https://user-images.githubusercontent.com/114903308/203625172-d71aa05b-fef0-4691-84e7-6eaf0ac71c1b.png)

![nightgame1](https://user-images.githubusercontent.com/114903308/203625189-086286d1-654b-466e-b110-b309237e3cff.png)








