# VRCSharp
A C# asynchronous wrapper for VRChat's API

# CURRENT FEATURES
Login to accounts via username and password <br />
(Un)Friend users <br />
VoteKick users <br />
Fetch Users <br />
Fetch Worlds <br />
Mute Users <br />
Block Users <br />
Hide Users <br />
Send Notifications to Users <br />
Annoy (Send out an invite a user cant get rid of <br />
Proxy Support <br />

# ROAD MAP (Planned Features)
Events such as for onMuteGiven, etc <br />
Message Users <br />
And more coming soon! Give me some suggestions! Thank you - Yaekith<br />

# Example Usuage
```csharp
VRCSharpSession session = new VRCSharpSession("Username", "Password");
await session.Login();

 if (session.Authenticated)
{
    var user = await session.GetAPIUserByID("usr_e28db278-1ccd-4c23-89b9-9933e619000e");

    if (await session.Moderate(user, VRCSharp.API.Moderation.ModerationType.Mute))
    {
      Console.WriteLine("Moderated! Yay!");
    }
                    
}
```
