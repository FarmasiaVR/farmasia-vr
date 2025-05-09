# How to create a popup in the new architecture

Creating a popup in the new architecture is handled by a popupManager, which is a script added to a game object.

In the Plate Coun Method this script is called [PopupManagerPCM](/tree/dev/Assets/Scripts/PlateCountMethod/PopupManagerPCM.cs), for a new scene a new script may be necessary to implement scene specific functionalities.

In PCM the popupManager is called by invoking an event and connecting the selected popup type method to the invocation in the inspector.

For example in the PCMSceneManager the popups are called only for notifications (yellow text, no points). A specific method has been defined in the sceneManager to invoke the popup notification with a [localized](/tree/dev/Docs/localization.md) message.