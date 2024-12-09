# Pharmacy VR Game for Faculty of Pharmacy, University of Helsinki

Software engineering project, University of Helsinki 20XX-2024

[End Demo ???](https://drive.google.com/file/d/1a1dkk5ZBnloUZdbxUlrPs3CoHDZ6xzBD/view?pli=1)\
[End Demo ???](https://drive.google.com/file/d/1CMKsz_xbZPIIZJpUDFGbTqreYAgsLnUi/view?usp=share_link)\
[End Demo fall 2019](https://youtu.be/pIKCZFZo2UA)\
[Mid Demo fall 2023](https://youtu.be/ASA7PMW_mGI?si=EdcNciua4nFXLOs2)\
[End Demo fall 2023](https://youtu.be/uL822Pbvwr0?si=HbdrWTF9DVj5KPED)\
[Mid Demo fall 2024](https://youtu.be/iXBMdBXMA6c?si=W2hqBsOusFJbewu)

## Description

Pharmacy VR Game is a virtual way of practicing the process of preparing clean eye medicine. Taking place in a cleanroom laboratory environment, the game works as an introduction to medicine production and cleanliness testing. The game consists of three parts – the use of protective clothing & washing hands, preparing the medicine and testing the microbiological cleanliness of the product. The process is divided into different steps that affect the success in the game and the cleanliness of the product. The choices made by the player will be evaluated and scored.

This project was started in 2019 in a University of Helsinki software engineering project. The first team laid the groundwork for VR gameplay and progression, and created the medicine preparation scene. In spring 2022, the second team expanded the game to include the membrane filtration procedure and made multiple upgrades and improvements to the game's systems. In summer 2022, the third team created a new changing room scenario, improved the overall look of the game and fixed various game systems. In spring 2023 the fourth team has focused on porting the game to use the Unity's XR Interaction toolkit, porting the game to Oculus Quest, creating a new fire safety scenario and improving the user experience. In fall 2023, the fifth team has focused on the fire safety part of the game, creating new scenarios and improving the former one, and making the laboratory more realistic by decorating the interior as it is in real life.

Pharmacy VR Game was built in collaboration with the Faculty of Pharmacy (University of Helsinki) as well as the animation and visualization students of Metropolia, University of Applied Sciences. All rights are reserved to the University of Helsinki.

Customer: Faculty of Pharmacy, University of Helsinki

Implementation environment: Online Course / VR, Faculty of Pharmacy

## Development

The original project was created with `Unity Version: 2019.2.3.f1` and for development version has been updated to `Unity Version: 2022.3.10f1` .

Additional tools used for development:
- Blender 3.4
- Unity XR Interaction Toolkit 2.3.0

- Development documents
  - [Product and Sprint backlogs](https://github.com/orgs/FarmasiaVR/projects/1)


- Legacy documentation
  - [Progress system](/Docs/progress_system.md) **NOTE! This is used only in the legacy portions of the game (Changing Room, Membrane Filtration and Medicine Preparation). Please, please, PLEASE avoid using this implementation at all costs. Only refer to this document if you want to fix problems with the legacy scenarios.**

## Cloning

First install Unity using Unity's official guide. Make sure to install the version of Unity that the project was developed on. This can be done by first cloning the project to your local machine, opening the cloned directory by selecting `Open` in the Projects tab in Unity Hub. This will prompt you to install the correct version of Unity. The project will not open in any older version of Unity and opening the project in a newer version of Unity will upgrade the project, which may break things. Updating the engine is recommended, but make sure that everyone in the development team is on the same page about which version of Unity should be used.

Additionally, make sure to install Blender. Otherwise some models may be missing when the project is opened.

## Architecture

You can read more about the architecture used in this project in the [architecture documentation](/Docs/Architecture/architecture.md). Note that this architecture is only used in the newer scenes.

Changing Room, Membrane Filtration and Medicine Preparation scenes use the older "GObject" architecture. Unfortunately there wasn't any documentation for the old architecture besides the progress system document that was mentioned above. 

## Build & Deployment

If you want to make a build of the game, open Unity and select `File -> Build Setting`. Select the platform you want to build for, select `Switch Platform` if necessary and select `Build`. 

There may be some prerequisites when building for a specific platform.

### Windows
 The prerequisites depend on which scripting backend is used in the project. This can be changed by clicking `Player Settings` in the build window and selecting `Other Settings -> Scripting backend`. Mono does not require any extra installations. IL2CPP performs a bit better, but requires the following extra modules.
    
    - Windows Build Support (IL2CPP) (Installed using Unity Hub)
    - Desktop development with C++ (Installed using Visual Studio Installer)

### Android
When building for the Oculus Quest, make sure to install the following modules using Unity Hub:

    - Android Build Support
    - OpenJDK
    - Android SDK & NDK Tools

Additionally, if you are using the IL2CPP scripting backend, then make sure to also install the Desktop development with C++ module using Visual Studio Installer.

## Testing

There are some unit tests written for some scripts in the new architecture. To run these unit tests, launch Unity, select `Window -> General -> Test Runner`, select PlayMode and click `Run all`.

## Controls

Grab objects by pressing the Grab button. The position of this button depends on the controller, but usually this is about where your middle finger rests when you are holding a controller. This is a toggle, which means that to release the object from your hand you need to press the grab button again.

To grab onto objects from a distance, hold the joystick forward or hold your finger on the top part of the touchpad, point it at an object and release the finger.

The move around the scene, hold the joystick towards yourself or press the lower part of the touchpad, point at the part of the level where you want to move and release your finger to move there.

To use extra functionality of an object, like spraying with a cleaning bottle, press the trigger button while holding the object.

To transfer liquids between objects, move the joystick right or press the right side of the touchpad to transfer liquid into the object and move the joystick left or press the left side of the touchpad to move liquid from the object.

### Test controls

To play the game without a VR headset, select the XR player in the Unity scene hierarchy and enable `XR Device Simulator` in the inspector. The controls should be visible on the screen

## Contributors

<!-- readme: contributors -start -->
<table>
<tr>
    <td align="center">
        <a href="https://github.com/farmasia-vr">
            <img src="https://github.com/farmasia-vr.png" width="100;" alt="farmasia-vr"/>
            <br />
            <sub><b>Farmasia-vr</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/porrasm">
            <img src="https://github.com/porrasm.png" width="100;" alt="porrasm"/>
            <br />
            <sub><b>Eetu Ikonen</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/Mirex97">
            <img src="https://github.com/Mirex97.png" width="100;" alt="Mirex97"/>
            <br />
            <sub><b>Kukkis</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/SirVeggie">
            <img src="https://github.com/SirVeggie.png" width="100;" alt="SirVeggie"/>
            <br />
            <sub><b>Veikka</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/doc97">
            <img src="https://github.com/doc97.png" width="100;" alt="doc97"/>
            <br />
            <sub><b>Daniel Riissanen</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/Veikkosuhonen">
            <img src="https://github.com/Veikkosuhonen.png" width="100;" alt="Veikkosuhonen"/>
            <br />
            <sub><b>Veikmaster</b></sub>
        </a>
    </td></tr>
<tr>
    <td align="center">
        <a href="https://github.com/vrfarmasia">
            <img src="https://github.com/vrfarmasia.png" width="100;" alt="vrfarmasia"/>
            <br />
            <sub><b>Vrfarmasia</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/heidihas">
            <img src="https://github.com/heidihas.png" width="100;" alt="heidihas"/>
            <br />
            <sub><b>Heidi Hassinen</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/Reksa97">
            <img src="https://github.com/Reksa97.png" width="100;" alt="Reksa97"/>
            <br />
            <sub><b>Reko Kälkäjä</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/MikkoHimanka">
            <img src="https://github.com/MikkoHimanka.png" width="100;" alt="MikkoHimanka"/>
            <br />
            <sub><b>Mikko Himanka</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/hepitk">
            <img src="https://github.com/hepitk.png" width="100;" alt="hepitk"/>
            <br />
            <sub><b>Hepitk</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/kivik-beep">
            <img src="https://github.com/kivik-beep.png" width="100;" alt="kivik-beep"/>
            <br />
            <sub><b>Kivik-beep</b></sub>
        </a>
    </td></tr>
<tr>
    <td align="center">
        <a href="https://github.com/SPitkanen">
            <img src="https://github.com/SPitkanen.png" width="100;" alt="SPitkanen"/>
            <br />
            <sub><b>SPitkanen</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/tjvalkonen">
            <img src="https://github.com/tjvalkonen.png" width="100;" alt="tjvalkonen"/>
            <br />
            <sub><b>tjvalkonen</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/jarkmaen">
            <img src="https://github.com/jarkmaen.png" width="100;" alt="jarkmaen"/>
            <br />
            <sub><b>jarkmaen</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/Siihi">
            <img src="https://github.com/Siihi.png" width="100;" alt="Siihi"/>
            <br />
            <sub><b>Siihi</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/sonicsasha">
            <img src="https://github.com/sonicsasha.png" width="100;" alt="sonicsasha"/>
            <br />
            <sub><b>Sasha Usoskin</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/juhkure">
            <img src="https://github.com/juhkure.png" width="100;" alt="juhkure"/>
            <br />
            <sub><b>juhkure</b></sub>
        </a>
    </td>
</tr>
<tr>
    <td align="center">
        <a href="https://github.com/MatiasSinisalo">
            <img src="https://github.com/MatiasSinisalo.png" width="100;" alt="MatiasSinisalo"/>
            <br />
            <sub><b>Matias Sinisalo</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/NuiS4ncE">
            <img src="https://github.com/NuiS4ncE.png" width="100;" alt="NuiS4ncE"/>
            <br />
            <sub><b>Teo Uosukainen</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/opturtio">
            <img src="https://github.com/opturtio.png" width="100;" alt="opturtio"/>
            <br />
            <sub><b>opturtio</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/SamiP7">
            <img src="https://github.com/SamiP7.png" width="100;" alt="SampiP7"/>
            <br />
            <sub><b>SamiP7</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/selsama">
            <img src="https://github.com/selsama.png" width="100;" alt="selsama"/>
            <br />
            <sub><b>Sonja Salmi</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/Cloudperry">
            <img src="https://github.com/Cloudperry.png" width="100;" alt="Cloudperry"/>
            <br />
            <sub><b>Roni Hokkanen</b></sub>
        </a>
    </td>
    </tr>
    <tr>
      <td align="center">
        <a href="https://github.com/levomaaa">
            <img src="https://github.com/levomaaa.png" width="100;" alt="levomaaa"/>
            <br />
            <sub><b>Aapo Levomäki</b></sub>
        </a>
    </td>
    <td align="center">
      <a href="https://github.com/Mahlamaki">
          <img src="https://github.com/Mahlamaki.png" width="100;" alt="Mahlamaki"/>
          <br />
          <sub><b>Susanna Mahlamäki</b></sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/smausquared">
          <img src="https://github.com/smausquared.png" width="100;" alt="smausquared"/>
          <br />
          <sub><b>Samu Ähynmäki</b></sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/JuhoSiitonen">
          <img src="https://github.com/JuhoSiitonen.png" width="100;" alt="JuhoSiitonen"/>
          <br />
          <sub><b>Juho Siitonen</b></sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/aihyytiainen">
          <img src="https://github.com/aihyytiainen.png" width="100;" alt="aihyytiainen"/>
          <br />
          <sub><b>Antti Hyytiäinen</b></sub>
      </a>
  </tr>
</table>
<!-- readme: contributors -end -->
