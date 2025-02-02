# Getting Started with the Project (learning Unity and Blender)

A good way to learn Unity is to make [Unity Essentials Pathway](https://learn.unity.com/pathway/unity-essentials). It familiarizes you with the Unity editor, after which you can start exploring the project in Unity. You should also learn how to create a VR interface by making [VR user Interface](https://learn.unity.com/tutorial/11-user-interface). 

You have two options to continue when you become more familiar with Unity and the project.

If you have a headset, you should make weeks 2, 3, and 4 from Aalto University’s course [Coding Virtual Worlds](https://www.sebastianjiroschlecht.com/courses/cvw2023/week2/firstscene/). Specially explore week 4 [Game Logic](https://www.sebastianjiroschlecht.com/courses/cvw2023/week4/gamelogic/) because it is used in the project.

If you do not have a headset, you should make Unity’s Pathway [Create with Code](https://learn.unity.com/course/create-with-code). If you get a headset, change to Aalto University course. It is more important.

After this, you might want to check Unity’s [Create with VR](https://learn.unity.com/course/create-with-vr?uv=2020.3) course, which has some tutorials you may find helpful. Unity offers many learning courses, but they can be quite extensive, and you may be better off learning more relevant stuff for your specific needs on YouTube. You can also ask ChatGPT, as it is surprisingly good with Unity. Make sure to specify the Unity version used in the project when asking questions, as solutions may no longer be supported. (To learn about unity and VR in general, you could make something small for fun eg. make throwable balls that play an effect on collision)

You might find the tutorial for the [XR Interaction Toolkit](https://www.youtube.com/watch?v=5ZBkEYUyBWQ) helpful.


## 3D Modeling (Blender)
For those new to modeling for games in Unity using Blender, a fantastic starting point is Blender Guru's tutorial video [Donut tutorial] (https://www.youtube.com/watch?v=B0J27sf9N1Y&list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z). This tutorial offers a solid foundation, guiding you through essential techniques.

Free models(even that they are free check the license):
[Poly Haven](https://polyhaven.com/)

It's crucial to keep performance in mind, especially when designing models for platforms like the Quest. High vertex counts might strain the Quest 2's rendering capabilities, impacting the game's performance. Therefore, optimizing models by balancing detail with efficiency becomes paramount. Blender provides tools and methods for reducing polygon counts or implementing LODs to maintain visual quality while ensuring smoother performance on hardware-constrained devices like the Quest 2.


## Unity good practices
- Make GameObjects that are repeatedly used into Prefabs https://www.youtube.com/watch?v=PSKKQKr2bnA

One key practice is using Prefabs for GameObjects, which are repeatedly utilized. Prefabs serve as templates or blueprints for GameObjects, allowing you to create and reuse identical instances throughout your project. This not only saves time but also ensures consistency across your game. Turning frequently used GameObjects into Prefabs establishes a central point of control. Any changes made to a Prefab are automatically applied to all instances of that Prefab in the scene, streamlining updates and modifications.

Moreover, Prefabs simplifies collaboration among team members. They enable seamless sharing and reimplementation of specific GameObjects or components across various scenes or projects, fostering a more efficient development workflow.

- Gameobjects with colliders shouldn’t have a negative scale because it might cause weird issues with colliders

- Use rotation instead to flip the Gameobjects

- Don’t make the scene hierarchy too complex. Having too many child Gameobjects nested makes editing the scene harder. (useful to group things in empty objects)

When your Unity scene gets crowded with a bunch of objects nested, one inside the other, it can be a bit like a maze to edit or move things around. To tidy up this maze, you can group similar objects together using what are basically invisible boxes—think of them as containers. These "containers" act like folders in a computer, helping you organize stuff tidily. So, instead of having everything scattered randomly, using these containers keeps your hierarchy clean and makes finding and working with objects a whole lot easier!
 

## Lighting

Lighting needs to be baked for scenes each time objects are added or moved around the scene. Baking lighting for a scene pre-calculates lighting and shadows and stores this data in a static lightmap. Baked lighting is generally more performance-friendly than real-time lighting, but it still has an impact on memory usage and loading times. 

Before baking lighting, ensure that all objects you wish to include in the lightmap have been selected. You can simply check all objects you want to include in the lightmap as static. Once everything is set up, you can bake the lighting via the Lighting window in Unity. This process can take some time, especially for complex scenes or high-resolution lightmaps.
