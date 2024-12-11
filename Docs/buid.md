# Making and releasing a buid to Meta Store

## Unity

`File -> Build Settings`\
`Set Platform to Android`\
`Go to Player Settings`

img1

There are a **lot** of options in the project settings that affect the quality of the build.

img2

In `Player Settings` Set the Version of your build.\
We decided to a use version naming convention where the first number is the Group (of cs students assigned to the project), second number is the Sprint and the third number is just a counter for that sprints builds.

The lightmap encoding option sets all baked lightmaps to a quality level. The quality is set to low because **there is a 1.1GB file size limitation** when uploading an .apk to the Meta Store. Thare is a way to bundle the upload to multiple files but we have not tried that.

Further down on `Player Settings -> Other Settings` submenu there is a Version number field and below that is the `Bundle Version Code`. This bundle version code **needs to be incremented** every time you publish a build or the Meta Quest Developer Hub won't accept the build.

img3

In `Player Settings -> Puhlishin Settings` you need to fill in the keystore password and the project key. These keys are stored outside of this repo, ask your project advisor for these.

img4

**Before you build make sure you have toggled off the `Device Simulator` from all the secenes** 

After all the settings are set. `Build Settings -> Build` the first build you make on a computer might take a *while*.

img5

## Meta Quest Developer Hub

### Making a release using the Meta Quest Developer Hub
1. Install [Meta Quest Developer Hub](https://developer.oculus.com/meta-quest-developer-hub)
2. Log in using the credentials that are added to the FarmasiaVR organization.
3. From the left tab select *App Distribution*.
4. Select *Aseptic Work - FarmasiaVR*
5. Select *Upload* on the channel that you want to make the release on.
6. Select or drag the APK you want to upload and click *Next*.
7. Write the release notes if you want to. This is recommended so that the users have an idea of what's changed.
8. Select *Next* and select *Upload*.
Now the APK is being uploaded and tested. After the tests have passed, the update should be immediately available.

img6

img7

After the tests are completed you can download/update the new build to the Quest 2 headset from the store/app library.\
Depending on the channel you chose to publish on you might have to change the channel in the headsets library where the app is located.
