# How to convert objects from SteamVR library to the OpenXR library
This document shows how to convert an object from the original SteamVR implementation to the new OpenXR implementation. Every object may behave differently and may require different steps, but this document includes the most important parts.

## Configuring the XR player
The XR player should have mostly everything needed work with the old sections. The only script that should be added to the root of the XR player is a Player-script. This is needed in order for the hint box to work. Why? How should I know? But it works.
![image](https://user-images.githubusercontent.com/9552313/219685147-1dccf120-796c-4fad-bc14-7b6802ecfd73.png)

## Converting a simple grabbable object to an OpenXR implementation
When converting a grabbable object, follow this procedure:

1. Add an _XR Grab Interactable_ component.
2. Create a new script. For simplicity's sake, call it something similar to the original script that is currently attached to the object, and place it in a logical place (i.e `Scripts/Objects/XR/ChangingRoom`)
3. Copy the code from the original _Grabbable_ script and modify it to work with different events. In the SteamVR implementation, `OnGrab()` is performed every frame, which the OpenXR library cannot do. Therefore modify the code to perform only on events like `OnTriggerEnter()` and `OnSelectEntered`. For an example take a look at [the original ProtectiveEquipment script](/Assets/Scripts/Objects/ProtectiveClothing.cs) and at [the converted one](https://github.com/FarmasiaVR/farmasia-vr/blob/stagingxr/Assets/Scripts/Objects/XR/ChangingRoom/XRProtectiveClothing.cs)
4. Modify the game event call to call back to an object of the same class as in the legacy code. This is because the game events might not work properly if they are not working with objects of same class as in the original script. The easiest way to circumvent this is to create a new "legacy" object in the script, give it all the required parameters from the current script and then set that legacy object as the parameter for the callback object. So instead of calling 
```C#
Events.FireEvent(EventType.SomeEvent, CallbackData.Object(this));
```
create a new object earlier in the script, give it all the same variable values as in the current script and call
```C#
Events.FireEvent(EventType.SomeEvent, CallbackData.Object(legacyObject));
```
Once again, look at an example of [original code](/Assets/Scripts/Objects/ProtectiveClothing.cs) and [converted](https://github.com/FarmasiaVR/farmasia-vr/blob/stagingxr/Assets/Scripts/Objects/XR/ChangingRoom/XRProtectiveClothing.cs) to figure out what I am talking about :DDD

## Converting a socket factory

A socket factory is a socket that spawns a new instance of the same object when an object is taken from the socket. This is used, for example, for shoe covers. Recreate a socket factory for the XR library with the following steps.

1. Convert a grabbable object to the XR library using the steps mentioned before and save it as a prefab.
2. Create a socket interactor by selecting GameObject -> XR -> Socket Interactor.
3. Under the _XR Socket Interactor_ component set the interaction manager to _None_. Otherwise if there are several interaction managers in the scene they may conflict with each other.
4. Set _Show Interactable Hover Meshes_ to _False_. Otherwise when you bring any XR object close to the socket it will display it's mesh and whether or not the object can be placed in the socket. Because in this case we don't want this, disable it.
5. Add a _XR Socket Factory_ component to the socket and place the XR grabbable object prefab as the _Socket Interactable_ parameter.

## Converting a Drag Acceptable object
This component is used, for example, on buttons in popups like hint boxes and videos. This can be converted by simply placing a _Drag Acceptable To XR Converter_ component to the object the player is supposed to be interacting with. Don't mind if the original Drag Acceptable script isn't attached to the button. It is usually attached during runtime.

## **M̴̡̳͚̅̓a̴̢̘̤͘y̶͚͇͇̑ ̴̪̈́̂g̷̫̜̔ͅō̸͚̱̦d̷͔̰̱̒ ̷̱̱̓b̷͙̱̍͐ĺ̵͖̲ͅê̷̲͂́ṣ̵͕̓̔s̸̞̋̾͑ ̸̡͊̀͋ỳ̴̏̈́͜͜o̷̗͒̋u̵͚͂͛͂ͅ ̴̰̫͍̀ọ̶̡͛̓n̵̬͋ ̸̜̙̓̈͝y̸̙̦͆̚̚o̶̟̅̓̾ŭ̶̖̑͜r̷̡̤̿ ̴̩͚̪̊̑c̶͖̭̄̚ô̶̮̙͍ṋ̷͊͆v̵̯͚̀̈́ȅ̴̝̟̐r̷̨̰̞̍ṡ̶̼̮̯i̶̜̰̇̎ȏ̵̮̣n̷̢̼͎̏͂ ̸͇̩̝̌͊̓à̵͔́d̵͎̯͍̔͂́v̵̡͠e̷̬͈̋ǹ̶̢̼͓͐̄t̵̨̬̾̇̀u̵͉͑̓́r̵͉͈̃͝e̴̼̚ṣ̷͉̌,̸̭̏̀̍ ̵̡̢̠̆͐b̴̛̖͝e̷̘̿̾̐c̶̘̃á̷̄͊ͅu̵̡̧͕͠s̵͚̣͍̿e̷͔̳͗̍̇ ̴̮̤̯̀̎͑ẖ̸͖̆̑e̴̘̒ ̶͇̬̀̿͘s̶͓̟͕̚ǘ̴͓̠̹͋͆r̶̬̚e̵̜̞̐̈́͜ ̸͈͊a̵͍̔͊s̵̨̛̰̔͒ ̴͇̄͑ḣ̸̞̈́̊ȅ̸͖̘͎l̶̳̽͊͝l̸̳̺̀̀͝ͅ ̷͉̲̳͌͝h̷̩͎͎͗a̷̗͉͐́͋s̷̡̀̈́͊ǹ̶͍̇͘'̶̡̼̓ț̵͊̓͆ ̶̤̗̀b̸̼͋l̸̨͙͖̃̕͝e̸̛̦͉͊͑s̷̞͂̌̈́š̸͖̼͉͌͗ȅ̶̡d̴͓̾͆ ̴̢̛̹̪̋̈́t̸̲͋͠h̴̛̩̱̃͘ͅi̶̢̟̊s̸̛̜̟̏̓ͅ ̷̖͔̈́̔͜ĉ̸̢͇̑̃o̴̥̘͙̒̈́͝d̴̰́̂͋ͅe̸̝͊͋**
