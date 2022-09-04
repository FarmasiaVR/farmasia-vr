# Pharmacy VR Game Part 3

[![Test project](https://github.com/MikkoHimanka/farmasia-vr/actions/workflows/test_runner.yml/badge.svg)](https://github.com/MikkoHimanka/farmasia-vr/actions/workflows/test_runner.yml)
[![Static code analysis](https://github.com/MikkoHimanka/farmasia-vr/actions/workflows/code_analysis.yml/badge.svg)](https://github.com/MikkoHimanka/farmasia-vr/actions/workflows/code_analysis.yml)

This fork implements part 3 of the original game
(Software engineering project, University of Helsinki 2022)
[Video](https://youtu.be/pIKCZFZo2UA)

## Description

Pharmacy VR Game is a virtual way of practicing the process of preparing clean eye medicine. Taking place in a cleanroom laboratory environment, the game works as an introduction to medicine production and cleanliness testing. The game consists of three parts – the use of protective clothing & washing hands, preparing the medicine and testing the microbiological cleanliness of the product. The process is divided into different steps that affect the success in the game and the cleanliness of the product. The choices made by the player will be evaluated and scored.

This project was started in 2019 in a University of Helsinki software engineering project. The first team laid the groundwork for VR gameplay and progression, and created the medicine preparation scene. In spring 2022, the second team expanded the game to include the membrane filtration procedure and made multiple upgrades and improvements to the game's systems. In summer 2022, the third team created a new changing room scenario, improved the overall look of the game and fixed various game systems.

Pharmacy VR Game was built in collaboration with the Faculty of Pharmacy (University of Helsinki) as well as the animation and visualization students of Metropolia, University of Applied Sciences. All rights are reserved to the University of Helsinki.

Customer: Faculty of Pharmacy, University of Helsinki

Implementation environment: Online Course / VR, Faculty of Pharmacy

## Development

<img src=/Docs/architecture.jpg width="600" />

The original project was created with `Unity Version: 2019.2.3.f1` and for development version has been updated to `Unity Version: 2019.4.35f1` .

- Documentation
  - [Progress system](/Docs/progress_system.md)
 
- Development documents
  - [Product and Sprint backlogs](https://docs.google.com/spreadsheets/d/1ja0GFDzCd-8x3NgSYdBKIdUyQT5DWfr7M5xOzrtkwyA)

## Cloning

First step for you is to setup the platform definition files. Create file  projectRoot/Assets/csc.rsp
If your system supports VR, set the file contents to this:
```
-define:UNITY_VRCOMPUTER
```
If your system does not support VR, set the file contents to this:
```
-define:UNITY_NONVRCOMPUTER
```

The definitions are system specific so the file csc.rsp file is ignored in gitignore.

## Build & Deployment

The project can be run locally using Unity. If you want to build on the command-line, please refer to the [deployment documentation](https://github.com/ohtuprojekti-farmasia/farmasia-vr/wiki/Deployment).

A dedicated test machine is used to run the project with VR.

## Controls

### Test controls

In Unity play mode press space to activate the test camera and hand controls.

Press K to move the camera, L to control the right hand and J to control the left hand.

Hands are controlled with WASD, Q and E. Mouse left click is used for grabbing and ungrabbing. Right click is used for interacting. 

## Contributors

<!-- readme: contributors -start -->
<table>
<tr>
    <td align="center">
        <a href="https://github.com/farmasia-vr">
            <img src="https://avatars.githubusercontent.com/u/55394182?v=4" width="100;" alt="farmasia-vr"/>
            <br />
            <sub><b>Farmasia-vr</b></sub>
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
    <td align="center">
        <a href="https://github.com/SirVeggie">
            <img src="https://avatars.githubusercontent.com/u/32365239?v=4" width="100;" alt="SirVeggie"/>
            <br />
            <sub><b>Veikka</b></sub>
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
        <a href="https://github.com/Veikkosuhonen">
            <img src="https://avatars.githubusercontent.com/u/54055199?v=4" width="100;" alt="Veikkosuhonen"/>
            <br />
            <sub><b>Veikmaster</b></sub>
        </a>
    </td></tr>
<tr>
    <td align="center">
        <a href="https://github.com/vrfarmasia">
            <img src="https://avatars.githubusercontent.com/u/98387910?v=4" width="100;" alt="vrfarmasia"/>
            <br />
            <sub><b>Vrfarmasia</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/heidihas">
            <img src="https://avatars.githubusercontent.com/u/32390965?v=4" width="100;" alt="heidihas"/>
            <br />
            <sub><b>Heidi Hassinen</b></sub>
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
        <a href="https://github.com/MikkoHimanka">
            <img src="https://avatars.githubusercontent.com/u/28507056?v=4" width="100;" alt="MikkoHimanka"/>
            <br />
            <sub><b>Mikko Himanka</b></sub>
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
        <a href="https://github.com/kivik-beep">
            <img src="https://avatars.githubusercontent.com/u/72075784?v=4" width="100;" alt="kivik-beep"/>
            <br />
            <sub><b>Kivik-beep</b></sub>
        </a>
    </td></tr>
<tr>
    <td align="center">
        <a href="https://github.com/SPitkanen">
            <img src="https://avatars.githubusercontent.com/u/77848087?v=4" width="100;" alt="SPitkanen"/>
            <br />
            <sub><b>SPitkanen</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/tjvalkonen">
            <img src="https://avatars.githubusercontent.com/u/33684997?v=4" width="100;" alt="tjvalkonen"/>
            <br />
            <sub><b>tjvalkonen</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/jarkmaen">
            <img src="https://avatars.githubusercontent.com/u/73038801?v=4" width="100;" alt="jarkmaen"/>
            <br />
            <sub><b>jarkmaen</b></sub>
        </a>
    </td>
    <td align="center">
        <a href="https://github.com/Siihi">
            <img src="https://avatars.githubusercontent.com/u/70064875?v=4" width="100;" alt="Siihi"/>
            <br />
            <sub><b>Siihi</b></sub>
        </a>
    </td></tr>
</table>
<!-- readme: contributors -end -->
