# SimpleOscBot

SimpleOscBot is a free to use *(as long as you give credit)* template for Discord Bots with OSC integration.
Setting it up takes only a few minutes.

# Warning
This project is outdated, if you need me to update it, let me know and ill do it if demand is high enough

Join the discord here: https://discord.gg/pxwGHvfcxs

The bot comes with the following features:
- Full [Discord.NET](https://discordnet.dev/) *(Slash commands, Interactions, etc)*
- Sending any OSC data in an easy way *(Thanks to *[CoreOSC](https://github.com/PaciStardust/CoreOSC-UTF8))*
- An OSC Listener-framework that allows for easy listening on any port and custom handling
- Logging functions for easy logging

The template comes with following examples:
- A group of preconfigured slash commands to see how OSC control works
- An OscListener that splits incoming traffic to multiple ports
- An OscListener that shows the contents of incomming traffic

Known Issues:
- Seems to have trouble handling bools, this can be fixed by using 0 and 1 instead, if you know how to fix it, please lmk

## SETUP
1. Create your own discord bot at the Discord Developer Platform
2. Invite the bot into a server
3. Set up the project and install Discord.NET via NuGet
4. Copy the bot token into the config file
5. Copy the main Sever-ID into the config aswell
6. You are done! Server should be able to be started.

# Config
The config file contains various useful and needed things for your bot to run
* **BotToken**: The token of your bot (Available on the Discord Dev Portal)
* **GuildId**: The main server of your bot, this is where commands will be loaded first
* **DefaultSendIp**: The default IP for your bot to send OSC data to
* **DefaultSendPort**: The default Port your bot sends OSC data to
* **EnabledLogs**: Disables and enables different log levels in the Console window, by default everything is on, if the bot does a lot I recommend disabling "Log"
* **GlobalCommands**: Makes commands load in any server the bot is in, should only be done once the bot is set up and its actually required, takes approx 30 minutes
* **Listeners**: Information about all the listeners you want to start up at launch of the bot
	* **Name**: The identifier of the Listener in the logs
	* **Port**: The port the listener is listening on
	* **Type**: The exact type name of the listener class, this needs to be exact as it will crash otherwise (You really have to use the *full name* for it to work. *Ex: SimpleOscBot.OSCControl.ListenerDebugExample*)
	* **Data**: An array of data of any type (can be mixed) for the listener to use when starting up (*Ex: Ports to send data to, addresses to listen to*), you have to implement the parsing of those in the listener classes "AssignData" function (An example can be found in the listener examples)


# DOCUMENTATION

## using SimpleOscBot

### Program
 The programm class handles the start of the bot, it starts all other services and publishes the slash command

### InteractionHandler
This class handles everything in regard to Discords interaction system

### Config
This file reads the config file and provides access to variables that can quickly change via Config.Data

## using SimpleOscBot.Services

### Logger
This class handles all logging for both OSC and Discord

#### Logger.Log()
Logs data in multiple ways

```csharp
//Logging using only a Discord LogMessage
Logger.Log(new LogMessage(LogSeverity.Verbose, "LogSource", "LogMessage"));

//Logging using a specified severity
Logger.Log("LogMessage", "LogSource", LogSeverity.Warning);

//Leaving out the severity makes it verbose
Logger.Log("LogMessage", "LogSource")
```

#### Logger.Error()
Log errors in multiple ways

```csharp
//Using an error for logging
Logger.Error(exception, "ErrorSource");

//Without error
Logger.Error("ErrorType", "ErrorMessage", "ErrorSource", "ErrorStackTrace");

//The trace can be left out
Logger.Error("ErrorType", "ErrorMessage", "ErrorSource");

//Alternatively it can be handled like any log message
Logger.Error("ErrorMessage", "ErrorSource");
```

#### Logger "Shortcuts"
These function basically as proxies for calling Logger.Log with different severities

```csharp
//Logging Info (Dark Gray)
Logger.Info("LogInfo", "InfoSource");

//Logging Warning (Yellow)
Logger.Warning("LogWarning", "WarnSource");

//Logging Debug (Dark Blue)
Logger.Debug("LogDebug", "DebugSource");
```

### Embeds
This class is a utility class for quickly creating embeds for Discord Messages

#### Embeds.CreateField()
Creates a simple EmbedFieldBuilder to use when manually building an embed
```csharp
//Creates a basic field
var field = Embeds.CreateField("Title", "Content");

//Creates the same field but inline
var field = Embeds.CreateField("Title", "Content", true);
```

#### Embeds<span>.Info()
Creates a basic info embed to use in a reply
```csharp
//Creating the embed, color will be light gray
var info = Embeds.Info("Title", "Content");

//Using a different color
var info = Embeds.Info("Title", "Content", Color.Red);
```

#### Embeds.Error()
Creates a special type of embed to tell the user something went wrong and automatically logs it

```csharp
//Creates an error embed
var err = Embeds.Error("ErrorType", "ErrorMessage", "ErrorTrace");

//Trace is again optional
var err = Embeds.Error("ErrorType", "ErrorMessage");

//Using an error instead
var err = Embeds.Error(exception);

//Disable logging
var err = Embeds.Error("ErrorType", "ErrorMessage", "ErrorTrace", false);
var err = Embeds.Error(exception, false);
```

#### Error "Utility"
A few embeds to tell a user something went wrong that might come up more often

```csharp
//A user has given invalid input
var invalidInput = Embeds.InvalidInput();

//A command has yielded no result
var noResult = Embeds.NoResult();

//A command has been run outside a server when it shouldnt have
var invalidGuild = Embeds.InvalidGuild();

//A database error has occured, unlike the others actually marked as error
var dbError = Embeds.DbFailure();
```

## using SimpleOscBot.OSCControl

### OSC
The main class to handle OSC related things

#### OSC.InitializeListeners()
This starts all the the listeners using reflection and the config file, it can only be called once

#### OSC.SendCustom()
Send OSC-Data to any IP with any Port, it returns a boolean value if sending was successful

```csharp
//Sending a single string to /test/address on localhost:9000
var sent = OSC.SendCustom("/test/address", "127.0.0.1", 9000, "test");

//Sending both an int and a float instead
var sent = OSC.SendCustom("/test/address", "127.0.0.1", 9000, 1, 0.1f);

//To avoid confusion you can also pack it into an array
var data = new object[] { 1, 0.1f };
var sent = OSC.SendCustom("/test/address", "127.0.0.1", 9000, data);
```

#### OSC.Send()
Behaves identically to SendCustom but uses the IP and Port listed as default in the config file

```csharp
//Sending a single string to /test/address on localhost:9000
var sent = OSC.Send("/test/address", "test");

//Sending both an int and a float instead
var sent = OSC.Send("/test/address", 1, 0.1f);

//To avoid confusion you can also pack it into an array
var data = new object[] { 1, 0.1f };
var sent = OSC.Send("/test/address", data);
```

#### OSC.SplitAddress()
Splits an address into an array of strings for easier parsing

```csharp
var address = "/test/address";
//Split into "test" and "address"
var split = OSC.SplitAddress(address);
```

### OscListenerBase
Abstract class all listeners inherit from

#### Port
The port the listener listens on, can only be set when starting it

#### Name
The identifier of the listener, can only be set when starting it

#### Start()
Starts the listener with a name, port and data, can only be called once

```csharp
//Creating the instance of a listener (using an example here)
//This should never really be done manually unless neccesary
var listener = new ListenerRelayExample();

//Data (in this case ports) for the listener
var ports = new List<object>() { 9000, 9020 };

//Starting the listener
listener.Start("ListenerName", "ListenerPort", ports);
```

#### virtual AssignData()
Optional override to parse the data sent with Start

```csharp
private List<string> _strings = new();

//Overriding the function
protected override void AssignData(List<object> data)
{
	//Iterating through array
	foreach(var item in data)
	{
		//Adding all strings to list of strings
		if (item.GetType() == typeof(string))
		{
			var newString = (string)item;
			Logger.Info($"Added \"{newString}\" to list of strings in listener \"{Name}\"", Name);
			_strings.Add(newString);
		}
	}
}
```

#### abstract HandleData()
Function gets called whenever data is received by the listener

```csharp
//Setting the function
protected override Task HandleData(string address, params object[] args)
{
	//Splitting address
	var splitAddress = OSC.SplitAddress(address);

	//Return if address is somehow empty or the address is not testing
	if (splitAddress.Length == 0 || splitAddress[0] != "testing") return Task.CompletedTask;

	//Gathering a list of all types
	var typeList = new List<string>();

	foreach(var arg in args)
	{
		typeList.Add(arg.GetType().FullName);
	}

	//Logging types
	Logger.Debug($"Data sent to \"{address}\" on port {Port}: {string.Join(", ", typeList)}", Name);
	return Task.CompletedTask;
}
```

### ListenerDebugExample
An example for a listener that just logs the data sent to it

### ListenerRelayExample
An example for a listener that takes data and sends it to multiple other points
