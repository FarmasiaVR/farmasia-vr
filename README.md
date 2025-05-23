# Pharmacy VR Game for Faculty of Pharmacy, University of Helsinki

Software engineering project, University of Helsinki 2019-2025

[Youtube playlist of Demo videos](https://www.youtube.com/playlist?list=PL_YKqaBch35I016hDedNtHaKyVa_G7NhX)

Group 1 [Mid Demo fall 2019](https://youtu.be/pIKCZFZo2UA)\
Group 1 [End Demo fall 2019](https://youtu.be/3NkUEzHm15Q?si=QZiTjBtOvQ7L5s3M)\
Group 2 [End Demo spring 2022](https://drive.google.com/file/d/1a1dkk5ZBnloUZdbxUlrPs3CoHDZ6xzBD/view?pli=1)\
Group 3 [End Demo summer 2022](https://youtu.be/L8vCH3BmtcM?si=GYhPiph-9YZCylY9)\
Group 4 [Mid Demo spring 2023](https://drive.google.com/file/d/1fFVl9ZidW_DfH8o3wDsv8h7nQMT_z1kf/view?usp=sharing)\
Group 4 [End Demo spring 2023](https://drive.google.com/file/d/1CMKsz_xbZPIIZJpUDFGbTqreYAgsLnUi/view?usp=share_link)\
Group 5 [Mid Demo fall 2023](https://youtu.be/ASA7PMW_mGI?si=EdcNciua4nFXLOs2)\
Group 5 [End Demo fall 2023](https://youtu.be/uL822Pbvwr0?si=HbdrWTF9DVj5KPED)\
Group 6 [Mid Demo fall 2024](https://youtu.be/iXBMdBXMA6c?si=W2hqBsOusFJbewu)\
Group 6 [End Demo fall 2024](https://www.youtube.com/watch?v=lc7FpVJEIuk)\
Group 7 [Mid Demo spring 2025](https://www.youtube.com/watch?v=zKT39n3busw)\
Group 7 [End Demo spring 2025](https://youtu.be/fC8Luha38eU)

## Description

Pharmacy VR Game is a virtual way of practicing laboratory work in Viikki Pharmaceutical laboratory.

The following scenarios are implemented in the game at this point in time:
  - Controls tutorial
  - Fire safety (Including multiple scenarios)
  - Changing room (Washing hands and gearing up for aseptic work)
  - Membrane Filtration (Preparation room and aseptic work)

There is also a scene for plate count method in the aseptic room that is started and under construction. Also there is an abandoned scenario for medicine preparation.

This project was started in 2019 in a University of Helsinki software engineering project. The first team laid the groundwork for VR gameplay and progression, and created the medicine preparation scene. In spring 2022, the second team expanded the game to include the membrane filtration procedure and made multiple upgrades and improvements to the game's systems. In summer 2022, the third team created a new changing room scenario, improved the overall look of the game and fixed various game systems. In spring 2023 the fourth team focused on porting the game to use the Unity's XR Interaction toolkit, porting the game to Oculus Quest, creating a new fire safety scenario and improving the user experience. In fall 2023, the fifth team focused on the fire safety part of the game, creating new scenarios and improving the former one, and making the laboratory more realistic by decorating the interior as it is in real life. The sixth group worked on the project in the fall of 2024, focusing on bug fixes and quality improvements in old scenarios and implementing English and Swedish translations to the whole game. The sixth group also started work on the new plate count method scenario. In spring 2025, the seventh group completed the plate count method scenario that had been started by the previous group. They also fixed several bugs in the existing scenarios and implemented a certificate system, allowing players to receive certificates after completing scenarios. Additionally, they made performance improvements to the game by updating the occlusion culling system. 

Pharmacy VR Game was built in collaboration with the Faculty of Pharmacy (University of Helsinki) as well as the animation and visualization students of Metropolia, University of Applied Sciences. The project is released under an open-source MIT License, see `LICENSE`.

Customer: Faculty of Pharmacy, University of Helsinki

Implementation environment: Online Course / VR, Faculty of Pharmacy

## Getting started with development

[Learning Tools](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/GettingStartedLearningTools.md)\
[Tips, tricks & links from group 6](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/tips_tricks_links_group6.md)\
[Guide to merge conflicts](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/merge_conflict.md)


### Cloning

First install Unity using Unity's official guide. Make sure to install the version of Unity that the project was developed on. This can be done by first cloning the project to your local machine, opening the cloned directory by selecting `Open` in the Projects tab in Unity Hub. This will prompt you to install the correct version of Unity. The project will not open in any older version of Unity and opening the project in a newer version of Unity will upgrade the project, which may break things. Updating the engine is recommended, but make sure that everyone in the development team is on the same page about which version of Unity should be used.

Additionally, make sure to install Blender. Otherwise some models may be missing when the project is opened.

### Development tools

The original project was created with `Unity Version: 2019.2.3.f1` and for development version has been updated to `Unity Version: 2022.3.56f1` .

Additional tools used for development:
- Blender
- Unity XR Interaction Toolkit
- [Probuilder](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/Probuilder.md)
- Github Desktop (highly recommended)
- Meta Quest Link
- Meta Quest Developer Hub
- Sidequest


## Project Architecture and Systems overview

You can read more about the architecture used in this project in the [architecture documentation](/Docs/Architecture/architecture.md) & [architecture picture](https://github.com/FarmasiaVR/farmasia-vr/tree/dev/Docs/Architecture). Note that this architecture is only used in the newer scenes.

[Poke interactor](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/Architecture/pokeinteractor.md)\
[Task List](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/Architecture/tasklist.md)\
[Task Manager](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/Architecture/taskmanager.md)\
[Mistake](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/Architecture/Classes/mistake.md)\
[Task](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/Architecture/Classes/task.md)

### Old architecture

[Progress system](/Docs/progress_system.md) \
**NOTE! This is used only in the legacy portions of the game (Changing Room, Membrane Filtration and Medicine Preparation). Please, please, PLEASE avoid using this implementation at all costs. Only refer to this document if you want to fix problems with the legacy scenarios.**

Changing Room, Membrane Filtration and Medicine Preparation scenes use the older `GObject` architecture. Unfortunately there wasn't any documentation for the old architecture besides the progress system document that was mentioned above.

### Systems overview

[Conversion from SteamVR](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/conversion.md) (This has already been implemented.)

[Scenario Overview for Fire Safety](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/GettingStartedFireScenes.md)\
[Fire Systems Overview](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/FireSystemsOverview.md)

[Connecting a scene to Main menu](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/connecting_scene_to_mainmenu.md)\
[Setting up teleportation](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/teleportation.md)

## Localization

The older scenes including Membrane Filtration use an [outdated localization system](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/Translator.Translate.md) but they should be already translated to fin/eng/swe. Unity has a built-in [localization](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/localization.md) system that is used on newer scenes.

## Performance

The game runs smoothly while the VR headset is connected to a PC and the PC is casting Unity's game window to the headset. But there are significant issues with performance on the Quest 2 running a build of the game on its own. We have tried to fix the performance issues the best we could but there is still a lot of room for improvement. Here are some methods that can improve and measure performance.

### Occlusion culling

`Window -> Rendering -> Occlusion Culling` This improves performance by rendering only the objects in the scene that are visible to the player camera.

[Occlusion culling doc (works on all scenes)](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/updating-laboratory-scene.md)

### Lighting

`Window -> Rendering -> Lighting` & `Window -> Rendering -> Light Explorer`

Real time lights use a lot of resources. In almost all the scenes (Membrane filtration excluded) the lights are baked and the real time lights are cut to a minimum.

There are a lot of different lighting systems in use in this project.

At least group 6 did not get a full understanding on the different light sources, reflection probes, light probes, rendering pipelines and emissive materials. So good luck :)

### Performance metrics

Unity has a built in performance analyzer. `Window -> Analysis -> Profiler`\
You can analyse the performance of the game when it is running on your Unity game window. This also works when the VR headset is connected with Meta Quest Link.

There should also be a way to connect the VR headset to the Unity profiler via ip or a cable and analyze the performance when the App is running just on the headset. But group 6 could not get this working. :(

There is also a OVRmetrics app on the Meta Store that should able real time analysis of a running app on the Quest 2 headset but the app is somehow f'd up and at least group 6 could not install it to the headset.

## Build & deploy

If you want to make a build of the game, open Unity and select `File -> Build Setting`. Select the platform you want to build for, select `Switch Platform` if necessary and select `Build`. 

There may be some prerequisites when building for a specific platform.

### Android
When building for the **Oculus Quest**, make sure to install the following modules using Unity Hub:

    - Android Build Support
    - OpenJDK
    - Android SDK & NDK Tools

Additionally, if you are using the IL2CPP scripting backend, then make sure to also install the Desktop development with C++ module using Visual Studio Installer.

### Windows
 The prerequisites depend on which scripting backend is used in the project. This can be changed by clicking `Player Settings` in the build window and selecting `Other Settings -> Scripting backend`. Mono does not require any extra installations. IL2CPP performs a bit better, but requires the following extra modules.
    
    - Windows Build Support (IL2CPP) (Installed using Unity Hub)
    - Desktop development with C++ (Installed using Visual Studio Installer)

[AppLab release to Meta store](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/applab_release.md) (old documentation)\
[Making and publishing a build](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/buid.md)

## Playtesting

Grab objects by pressing the Grab button. The position of this button depends on the controller, but usually this is about where your middle finger rests when you are holding a controller. This is a toggle, which means that to release the object from your hand you need to press the grab button again.

To grab onto objects from a distance, hold the joystick forward or hold your finger on the top part of the touchpad, point it at an object and release the finger.

The move around the scene, hold the joystick towards yourself or press the lower part of the touchpad, point at the part of the level where you want to move and release your finger to move there.

To use extra functionality of an object, like spraying with a cleaning bottle, press the trigger button while holding the object.

To transfer liquids between objects, move the joystick right or press the right side of the touchpad to transfer liquid into the object and move the joystick left or press the left side of the touchpad to move liquid from the object.

[Screencasting the game from Quest to browser](https://github.com/FarmasiaVR/farmasia-vr/blob/dev/Docs/quest_casting.md)

### Test controls

To play the game without a VR headset, select the XR player in the Unity scene hierarchy and enable `XR Device Simulator` in the inspector. The controls should be visible on the screen

## Contributors

<!-- readme: contributors -start -->
<table>
	<tbody>
		<tr>
            <td align="center">
                <a href="https://github.com/MatiasSinisalo">
                    <img src="https://avatars.githubusercontent.com/u/50097749?v=4" width="100;" alt="MatiasSinisalo"/>
                    <br />
                    <sub><b>Matias Sinisalo</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/farmasia-vr">
                    <img src="https://avatars.githubusercontent.com/u/55394182?v=4" width="100;" alt="farmasia-vr"/>
                    <br />
                    <sub><b>Farmasia-vr</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/Uxusino">
                    <img src="https://avatars.githubusercontent.com/u/89293277?v=4" width="100;" alt="Uxusino"/>
                    <br />
                    <sub><b>Uxusino Uxusone</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/Cloudperry">
                    <img src="https://avatars.githubusercontent.com/u/25011618?v=4" width="100;" alt="Cloudperry"/>
                    <br />
                    <sub><b>Roni Hokkanen</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/porrasm">
                    <img src="https://avatars.githubusercontent.com/u/31691452?v=4" width="100;" alt="porrasm"/>
                    <br />
                    <sub><b>Eetu Ikonen</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/Mirex97">
                    <img src="https://avatars.githubusercontent.com/u/32763253?v=4" width="100;" alt="Mirex97"/>
                    <br />
                    <sub><b>Kukkis</b></sub>
                </a>
            </td>
		</tr>
		<tr>
            <td align="center">
                <a href="https://github.com/Veikkosuhonen">
                    <img src="https://avatars.githubusercontent.com/u/54055199?v=4" width="100;" alt="Veikkosuhonen"/>
                    <br />
                    <sub><b>Veikmaster</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/NuiS4ncE">
                    <img src="https://avatars.githubusercontent.com/u/55931311?v=4" width="100;" alt="NuiS4ncE"/>
                    <br />
                    <sub><b>Teo Uosukainen</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/SirVeggie">
                    <img src="https://avatars.githubusercontent.com/u/32365239?v=4" width="100;" alt="SirVeggie"/>
                    <br />
                    <sub><b>Veikka</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/vrfarmasia">
                    <img src="https://avatars.githubusercontent.com/u/98387910?v=4" width="100;" alt="vrfarmasia"/>
                    <br />
                    <sub><b>Vrfarmasia</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/doc97">
                    <img src="https://avatars.githubusercontent.com/u/4580546?v=4" width="100;" alt="doc97"/>
                    <br />
                    <sub><b>Daniel Riissanen</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/jobatabs">
                    <img src="https://avatars.githubusercontent.com/u/2370315?v=4" width="100;" alt="jobatabs"/>
                    <br />
                    <sub><b>Jonatan Noponen</b></sub>
                </a>
            </td>
		</tr>
		<tr>
            <td align="center">
                <a href="https://github.com/sashausoskin">
                    <img src="https://avatars.githubusercontent.com/u/9552313?v=4" width="100;" alt="sashausoskin"/>
                    <br />
                    <sub><b>Sasha Usoskin</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/UncSald">
                    <img src="https://avatars.githubusercontent.com/u/133899575?v=4" width="100;" alt="UncSald"/>
                    <br />
                    <sub><b>Alingué Marc</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/aihyytiainen">
                    <img src="https://avatars.githubusercontent.com/u/12662050?v=4" width="100;" alt="aihyytiainen"/>
                    <br />
                    <sub><b>Antti Hyytiäinen</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/opturtio">
                    <img src="https://avatars.githubusercontent.com/u/86497253?v=4" width="100;" alt="opturtio"/>
                    <br />
                    <sub><b>opturtio</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/VilhoHeikkinen">
                    <img src="https://avatars.githubusercontent.com/u/91131515?v=4" width="100;" alt="VilhoHeikkinen"/>
                    <br />
                    <sub><b>VilhoHeikkinen</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/jarkmaen">
                    <img src="https://avatars.githubusercontent.com/u/73038801?v=4" width="100;" alt="jarkmaen"/>
                    <br />
                    <sub><b>jarkmaen</b></sub>
                </a>
            </td>
		</tr>
		<tr>
            <td align="center">
                <a href="https://github.com/heidihas">
                    <img src="https://avatars.githubusercontent.com/u/32390965?v=4" width="100;" alt="heidihas"/>
                    <br />
                    <sub><b>Heidi Hassinen</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/arolaeemil">
                    <img src="https://avatars.githubusercontent.com/u/101641321?v=4" width="100;" alt="arolaeemil"/>
                    <br />
                    <sub><b>arolaeemil</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/juhkure">
                    <img src="https://avatars.githubusercontent.com/u/16820390?v=4" width="100;" alt="juhkure"/>
                    <br />
                    <sub><b>juhkure</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/JuhoSiitonen">
                    <img src="https://avatars.githubusercontent.com/u/104775829?v=4" width="100;" alt="JuhoSiitonen"/>
                    <br />
                    <sub><b>Juho Siitonen</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/MikkoHimanka">
                    <img src="https://avatars.githubusercontent.com/u/28507056?v=4" width="100;" alt="MikkoHimanka"/>
                    <br />
                    <sub><b>Mikko Himanka</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/Saukka">
                    <img src="https://avatars.githubusercontent.com/u/80990021?v=4" width="100;" alt="Saukka"/>
                    <br />
                    <sub><b>Saukka</b></sub>
                </a>
            </td>
		</tr>
		<tr>
            <td align="center">
                <a href="https://github.com/Stenbras">
                    <img src="https://avatars.githubusercontent.com/u/117363774?v=4" width="100;" alt="Stenbras"/>
                    <br />
                    <sub><b>Stenbras</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/Reksa97">
                    <img src="https://avatars.githubusercontent.com/u/36817054?v=4" width="100;" alt="Reksa97"/>
                    <br />
                    <sub><b>Reko Kälkäjä</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/levomaaa">
                    <img src="https://avatars.githubusercontent.com/u/148433955?v=4" width="100;" alt="levomaaa"/>
                    <br />
                    <sub><b>Aapo Levomäki</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/SamiP7">
                    <img src="https://avatars.githubusercontent.com/u/57110244?v=4" width="100;" alt="SamiP7"/>
                    <br />
                    <sub><b>SamiP7</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/Colabres">
                    <img src="https://avatars.githubusercontent.com/u/146994007?v=4" width="100;" alt="Colabres"/>
                    <br />
                    <sub><b>Colabres</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/McIlola">
                    <img src="https://avatars.githubusercontent.com/u/128368080?v=4" width="100;" alt="McIlola"/>
                    <br />
                    <sub><b>McIlola</b></sub>
                </a>
            </td>
		</tr>
		<tr>
            <td align="center">
                <a href="https://github.com/kivik-beep">
                    <img src="https://avatars.githubusercontent.com/u/72075784?v=4" width="100;" alt="kivik-beep"/>
                    <br />
                    <sub><b>Kivik-beep</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/smausquared">
                    <img src="https://avatars.githubusercontent.com/u/120834179?v=4" width="100;" alt="smausquared"/>
                    <br />
                    <sub><b>Samu Äyhynmäki</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/nicolaskivimaki">
                    <img src="https://avatars.githubusercontent.com/u/86207135?v=4" width="100;" alt="nicolaskivimaki"/>
                    <br />
                    <sub><b>nicolaskivimaki</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/hepitk">
                    <img src="https://avatars.githubusercontent.com/u/31772375?v=4" width="100;" alt="hepitk"/>
                    <br />
                    <sub><b>Hepitk</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/Mahlamaki">
                    <img src="https://avatars.githubusercontent.com/u/117199066?v=4" width="100;" alt="Mahlamaki"/>
                    <br />
                    <sub><b>Susanna Mahlamäki</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/tjvalkonen">
                    <img src="https://avatars.githubusercontent.com/u/33684997?v=4" width="100;" alt="tjvalkonen"/>
                    <br />
                    <sub><b>tjvalkonen</b></sub>
                </a>
            </td>
		</tr>
		<tr>
            <td align="center">
                <a href="https://github.com/TrollRoll2">
                    <img src="https://avatars.githubusercontent.com/u/101802614?v=4" width="100;" alt="TrollRoll2"/>
                    <br />
                    <sub><b>TrollRoll2</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/BananaMayo">
                    <img src="https://avatars.githubusercontent.com/u/101586122?v=4" width="100;" alt="BananaMayo"/>
                    <br />
                    <sub><b>Niklas Nygård</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/tuovinenemma">
                    <img src="https://avatars.githubusercontent.com/u/102189885?v=4" width="100;" alt="tuovinenemma"/>
                    <br />
                    <sub><b>tuovinenemma</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/selsama">
                    <img src="https://avatars.githubusercontent.com/u/56793802?v=4" width="100;" alt="selsama"/>
                    <br />
                    <sub><b>Sonja Salmi</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/SPitkanen">
                    <img src="https://avatars.githubusercontent.com/u/77848087?v=4" width="100;" alt="SPitkanen"/>
                    <br />
                    <sub><b>SPitkanen</b></sub>
                </a>
            </td>
		</tr>
	<tbody>
</table>
<!-- readme: contributors -end -->
