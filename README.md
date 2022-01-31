# Pharmacy VR Game Part 2

[![Test project](https://github.com/MikkoHimanka/farmasia-vr/actions/workflows/test_runner.yml/badge.svg)](https://github.com/MikkoHimanka/farmasia-vr/actions/workflows/test_runner.yml)
[![Static code analysis](https://github.com/MikkoHimanka/farmasia-vr/actions/workflows/code_analysis.yml/badge.svg)](https://github.com/MikkoHimanka/farmasia-vr/actions/workflows/code_analysis.yml)
This fork implements part 2 of the original game
(Software engineering project, University of Helsinki 2022)
[Video](https://youtu.be/pIKCZFZo2UA)

## Description

Pharmacy VR Game is a virtual way of practicing the process of preparing clean eye medicine. Taking place in a cleanroom laboratory environment, the game works as an introduction to medicine production and cleanliness testing. The game consists of two parts – preparing the medicine and testing the microbiological cleanliness of the product. The process is divided into different steps that affect the success in the game and the cleanliness of the product. The choices made by the player will be evaluated and scored.

This project was developed as part of the course Software engineering project, organized during the autumn of 2019 at the University of Helsinki. Pharmacy VR Game was built in collaboration with the Faculty of Pharmacy (University of Helsinki) as well as the animation and visualization students of Metropolia, University of Applied Sciences. All rights are reserved to the University of Helsinki.

Customer: Faculty of Pharmacy, University of Helsinki

Implementation environment: Online Course / VR, Faculty of Pharmacy

## Development

The project was created with `Unity Version: 2019.2.3.f1` and the same version should be used for development.

- Wiki
  - [Git workflow document](https://github.com/ohtuprojekti-farmasia/farmasia-vr/wiki/Git)
  - [Code reviewing policy](https://github.com/ohtuprojekti-farmasia/farmasia-vr/wiki/Code-Review)
 
- Development documents
  - [Product and Sprint backlogs](https://docs.google.com/spreadsheets/d/1QbWEpiNEt-0H225ZIYymeJ4stKKGCl3CIwYoJT8NL5s/edit?usp=sharing)
  - [Some WIP architecture diagrams](https://app.diagrams.net/#G1zGultE_-2kT3pbttZezY0fnKzPWykIT0)

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

 
