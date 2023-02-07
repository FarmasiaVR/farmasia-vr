So let's say that several people have modified the same scene in Unity. One person wanted a cube to be really **L O O O O O O N G** and someone instead made the cube very large. Now there's a merge conflict and you're worried that you're going to lose the entire scene. No worries, here's a guide on how to solve merge conflicts

![image](https://user-images.githubusercontent.com/9552313/217273127-823f5638-94e2-4579-b7f1-2f4fcd1008c8.png)

Example of a scenario that would cause a merge conflict

![image](https://user-images.githubusercontent.com/9552313/217273953-ed27cb3e-6736-4861-b121-528d618a0ccc.png)

The message you receive in GitHub Desktop when there is a merge conflict

## How to solve merge conflict using Visual Studio Code

1. Open up Visual Studio Code
2. Open the project's root folder.
3. From the leftside bar choose "Source Control"
4. From "Merge Changes" select a file with merge conflicts

![image](https://user-images.githubusercontent.com/9552313/217275297-8622170a-2e6d-4468-ba4e-690426ed3076.png)

5. From the lower right corner select "Resolve in Merge Editor"

![image](https://user-images.githubusercontent.com/9552313/217275738-8671545d-e70e-4932-b527-80a02a3aa1ce.png)

6. Now you will see three different windows. The upper-left window shows the contents of the file that currently resides in the repository. The upper-right window shows the contents of the file that is currently on your computer. The lower window shows the resulting file content. You can select which changes to keep by selecting "Accept Incoming" or "Accept Current" on every change. The rows should give a clue as to which parts of the scene are clashing. **Just make sure not to accept both incoming and current changes.** I have no idea what that would do, but I don't think it would be pretty :DD

![image](https://user-images.githubusercontent.com/9552313/217277114-8334813a-b842-4c6a-b514-275a7277fca7.png)

7. After you have gone through all the conflicts, click "Complete Merge" in the lower-right corner.
![image](https://user-images.githubusercontent.com/9552313/217278643-f08c4f27-d61f-48bb-a40a-59f9d680767d.png)

8. Make sure that the merge conflict is solved by opening the asset in Unity. If it opens and functions, then the merge conflict is solved and you can go on about your day. Remember to stay hydrated and use a seatbelt when driving! :shipit:
