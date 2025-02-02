# How to set up a scene for teleportation

1. Select the floor object in the scene (or any other object on which you want the player to be able to teleport)
2. Add a _Teleportation Area_ component **NOTE! Not _Teleport Area_**
3. In the created component, set Interaction Manager to _None_. Otherwise if there are several interaction managers in the scene, they may conflict with each other.
4. Set the interaction layer to only _Teleport_ and disable any other layers.
![image](https://user-images.githubusercontent.com/9552313/220295431-678297c9-ba1c-499e-82cc-4844145847e5.png)

How the object inspector should look

Now you should be able to zip around your scene. Have fun!
