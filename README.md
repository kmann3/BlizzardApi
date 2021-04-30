# BlizzardApi
C# Api for Accessing Blizzard games API

To use this app, you must first have access.
Setup your account and such here: https://develop.battle.net/access/

Once you have a client and secret, the next step is to update the App.config with the appropriate information.
If you're copying and pasting -- watch out for spaces.

There's an example in Program.cs.
Depending on what I'm doing at the time, you  may have to commentout my GetAndSaveJsonDataForApi and the next two lines, and then uncomment the example.

GetAndSaveJsonDataForApi -- purely for me / anyone willing to help. Plug in the URL (don't ever save your token / secret / client ID to everyone in the open. So if you fork, just remember that.

I'm filling the API's, basically at random. No particular reason. Just picked things that looked interesting at the time.

Currently using (https://quicktype.io/csharp) to convert JSON code to CSharp classes.

Things that could help:
* Find out if ALL api calls have ?namespace=dynamic-{region.ToDescriptionString()}&locale={locale.ToDescriptionString()}&access_token={token} in them, at a minimum. If so, I can remove that and just put them in automatically insstead of having them in each and every method.
* Help with documentation. Examplle Wow/PvpLeaderboardApi -- summary and parameter descriptions. Nearly all should be pretty obvious.
* Feedback on code format. Always looking for cleaner or smarter ways to do things, within reason. Readability is more important than hyper-effeciency.
* Help with throttling many queries. I'd like to have something where one can put in, literally, all the API's (as a test to know if any endpoints have changed somehow or another or responses have changed). Need to throttle that.
* Unit testing? But... what is there to really test? 

Code should be clean and almost trivial to use.

Way later in the future, a SQLite database to contain data following at least 3NF, pref 4NF.
