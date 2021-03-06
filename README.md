# GP20-2021-0419-Realtime-Gameserver
Make sure, that .NET Core 5 SDK is installed from https://www.microsoft.com/net/download

## My final results of Part 1 and 2:

![TCPTimeServer](https://user-images.githubusercontent.com/69099899/116010093-4792d680-a61d-11eb-9edc-d0edc65f92c3.gif)


## Part 1 - Time Server:

First, let’s do a few demo projects:\
Create a folder named TimeServer\
Open the Terminal in that Folder

If you do not know, how to do that, google.\
One way would be to open the Terminal (cmd on windows) and type in cd PATH, for example: cd C:/Users/…. Make sure to put quotation marks around the path or to escape it, if you have white spaces in it.

Now, validate, that you are in the correct folder, by using `pwd`.
Now, use the command `dotnet new console`
If it says `dotnet` not found, you have probably not installed .NET Core 5 SDK, yet.

This command should have created a new C# Project for you. You can go ahead and open the `.csproj`-File in Rider.

Okay, for our small demo project, let’s build a small time-server using TCP.
The idea is, that anybody can connect to this server and our server will respond with the current time.

You will need: The `TcpListener`-class found in `System.Net.Sockets`.
`Start` will start the listener.
The `AcceptTcpClient`-Method handles the acknowledgement of new connections for you.
`GetStream` on the `Client` gets you the current stream used for the client.
`Encoding.ASCII.GetBytes` Can convert a String to ASCII-Bytes for you.
The `Write`-Method on the Stream allows you to send Bytes over the socket.
You need to call `Close` on the `Stream` as well as the `Client` when you are done with the client.
`Stop` will stop the listener. Only do this on Server Shutdown.

Your task is to, whenever a client got accepted (so when `AcceptTcpClient` stopped blocking), to send a message sending the current DateTime (DateTime.Now) back to that client and then close the Stream and Client again.
This means, that whenever someone connects via TCP, our Server will send the Time and close the connection.
Neat little TimeServer.
You can Run the Code within Rider using the Play Button.
Not much will happen, yet, though.
We need a Client to Connect in order to see, whether everything works.

=> Test, what happens, if you start listening to the same port on multiple clients. Or listen to a port that is already listened on.
=> Google "How to see listening UDP ports on Windows/MacOS"


## Part 2 - TCP Client:

Okay, now let’s build a client: Create a Unity 2D Project and name it `Agario`
We will reuse this project for all of our game server test scripts.

In Unity, we want to try to connect to our TimeServer.
In Unity, we will need the `TcpClient`-class together with the correct port number (used in the `TcpListener`)
Now, you got a `Client`, just as from `AcceptTcpClient` on `TcpListener`.
So, you again can call `GetStream`
And on that function, you can call `Read` to read information.
It will return bytes, which you need to convert to a string again.
If you think about how you converted a string to bytes, you might come up with a solution to this problem.
Are you able to Add this Logic to a GetTime-Script and call it from a Button and print the output to a `UnityEngine.UI.Text`? :)

## My final results of Part 3 and 4:

![UDPTwoClients](https://user-images.githubusercontent.com/69099899/116010101-537e9880-a61d-11eb-975b-d304d33d3e98.gif)


## Part 3 - OpenWord-MMO-Server

Alright, now, let’s continue with UDP.
Let’s create a small Mini-Game Server.
The idea is, that the server accepts any segments sent via UDP.
It only allows a single word to be sent at a time.
How can you validate, that only one word was sent?
Also, it only allows words to have up to 20 characters per word.
How can you validate, that the word is not too long?
What the server does, is, it remembers the text that was sent and adds the next text after a whitespace behind it. And so on.
And it every time sends the whole text back to the client.
So, if someone sends “Hi”, it will respond “Hi” If then, someone sends “Welcome”, it will respond “Hi Welcome”
Someone sends “World”, it will respond “Hi Welcome World” etc.

You need to create a new Project in a Folder named `OpenWord-MMO`
Then use `dotnet new console` in that directory to create another console project.
You need to create a new `System.Net.Sockets.UdpClient` and pass it a port number that you want to use.
For example `11000`.
Now, the way, this API works, is:
```cs
var remoteEP = new IPEndPoint(IPAddress.Any, 11000); 
var data = udpClient.Receive(ref remoteEP);
```

The IP filters, what IP addresses we want to filter on.
It is passed as a ref parameter, so that the Receive Function can change its value.
When we receive data, the remoteEP will actually contain the IP Address and Port Number of whoever just sent us bytes.
Therefore, we can use it to send back information.

To respond, you can use `udpClient.Send(bytes, bytesLength, remoteEP);` to send a response.

Do this all endlessly in a loop and your Server should run until the end of our solar system :)

Well, our UDP Server is standing.
You can Run it using the Play Button in Rider.

Again, it won't du much, until someone sends us info.
You can use `nc -u 127.0.0.1 11000` in the Terminal to connect to your server and send text.


## Part 4 - UDP Client:

Now, in Unity, you’ll have to do it the other way round.
Now, you should send a word to your OpenWord-MMO-Port.
To do that, you need an `Input`-`TextField` And a Send-`Button` to send the Input from the Text-Field.\
When it is pressed:\
You need to create another `UDPClient` on a port of your choice.\
You need to first `Send` Bytes.\
And then `Read` for a response.\
And print the response to your Output.

Remember, that the GameServer only accepts Single Words with less than 20 characters?\
Test, what happens, if you try to enter two words, or a word of 30 characters size?\
What happens, if you send an empty text?\
What do you want to happen?

Bonus: In the Unity Client, show a Popup-Message whenever an Error was received.\
So, whenever the server decided, that the input was not okay.


## Part 5 - Agar.io

Barely Passed: 0 Points done. (But everything before)\
Passed: 3 Points done.

For the last part, we are going to implement what we got for implementing our own version of Agar.io

Now, this will be a bit exciting.
We need a lot of features.

The idea of the game is:
The Game has a certain Field Size, e.g. 100x100. `FIELD_SIZE`
All Players need to move within that Field Size. `CLAMP_POSITION`

Now, Players need to be able to Connect. `CONNECT_GAME`
And Spawn in a random location. `PLAYER_SPAWN`, `RANDOM_POSITION`
And Disconnect. `DISCONNECT_GAME`

While they are Connected:
They have the camera attached to themselves. `FOLLOW_CAMERA`
They see themselves as a Circle. `PLAYER_VISUAL`
They are able to Move Around (and pretty much constantly moving). `PLAYER_INPUT`
The players always move towards the Mouse. `MOUSE_POSITION`, `VECTOR_DIRECTION`
But if you are unsure on how to implement that in Unity, you might as well start with WASD controls for now. `WASD_INPUT`

Also, the Game Server spawns small Orbs randomly over the Map (within the Map’s bounds) every x seconds. `SPAWN_ORBS`, `RANDOM_POSITION`, `UPDATE_LOOP`

The players, when touching those orbs, collect them. This will increase their score by one. And the score also increases their appearance (their size). `COLLECT_ORB`, `UPDATE_VISUALS`, `INCREASE_SCORE`

Now, the most interesting part:
Players can eat each other by fully overlapping each other. `DISTANCE_CHECK`
The smaller player gets eaten and starts with a score of zero at a random location again. `PLAYER_RESPAWN`
The larger player gets the other player’s score added to his own. `INCREASE_SCORE`

A few difficult challenges:
How to update a player’s position and score to other players? `OTHER_PLAYERS_BROADCAST` `CURRENTLY_CONNECTED_PLAYERS`
Can you display a small leaderboard? `PLAYER_LEADERBOARD`
Can you show the players’ names on their players? `PLAYER_NAMES`
Can you add Cheat Protection? `CHEAT_PROTECTION`
How do you handle Lags? Do players get stuck and then teleport? Can you somehow `INTERPOLATE`?
