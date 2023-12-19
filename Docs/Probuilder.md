# Using ProBuilder in prefabs

Some of the structures in Laboratory prefab are made with Unity's ProBuilder package. Here's a small guide on what to do if you want to add probuilder objects into the laboratory prefab or any other prefabs in the project.

When you create a probuilder object, the mesh will be called pb_Mesh+random numbers. (see pic below with pb_Mesh916704)

<img width="700" alt="Probuilder with pb_Mesh" src="https://github.com/FarmasiaVR/farmasia-vr/assets/80990021/0c3d8b9e-b88a-4d6f-9395-2c8112ebe7fb">

The mesh is currently not properly saved. The name will be different in different scenes and Unity will not recognise it as the same mesh, which will make the laboratory prefab overrides-list look very messy. 
This is because every scene will override the mesh in the prefab with its own identical mesh.

### What you need to do 

After you've created the object, click the + symbol next to export in the probuilder menu. Choose export format as Asset and select Replace Source. Save the asset in the ProBuilder folder. 

<img width="321" alt="Export window" src="https://github.com/FarmasiaVR/farmasia-vr/assets/80990021/eb90674f-eeb3-4b40-8be0-798080ae257e">


After you have exported the mesh, the mesh will be saved and it will be the same in every scene using the laboratory prefab.

<img width="925" alt="New mesh name" src="https://github.com/FarmasiaVR/farmasia-vr/assets/80990021/3a528cb5-4e97-4934-a741-d4d79e5456f2">

ps. mind the pillar typo
