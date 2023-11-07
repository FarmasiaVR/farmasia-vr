# Updating laboratory scene occlusion culling
Occlusion culling is a way to skip processing and drawing of objects that are not visible. It is enabled in scenes that take place in the laboratory (because there are a lot of objects). When large objects (like walls) are added, moved or removed from the scene, the occlusion culling of that scene must be updated for it to work correctly. All new stationary objects should also be marked static in the editor. If objects start disappearing based on the camera position, this is also a sign that the occlusion culling must be updated.

# How to update the occlusion culling
1. Open the occlusion culling menu ![image](https://github.com/FarmasiaVR/farmasia-vr/assets/25011618/fb2529db-8ae2-49dd-8235-d0b9b7a63168)
2. Set default parameters insde the Bake tab ![image](https://github.com/FarmasiaVR/farmasia-vr/assets/25011618/7899991d-e9e7-4c5a-a3fe-5a840b125892)
3. Deselect all gameobjects or select the scene root ![image](https://github.com/FarmasiaVR/farmasia-vr/assets/25011618/dff1daad-67cf-4bc4-b7ef-e83aae3740cc)
4. Press bake in the occlusion culling menu
5. Once the bake is done save the current scene
