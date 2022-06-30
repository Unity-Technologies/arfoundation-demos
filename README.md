# This repository is no longer actively maintained. It has been archived as a resource.

# This repo is intended to provide more advanced demos for AR Foundation outside of the [Samples Repo](https://github.com/Unity-Technologies/arfoundation-samples/).
For questions and issues related to AR Foundation please post on the AR Foundation Sample [issues](https://github.com/Unity-Technologies/arfoundation-samples/issues) and **NOT** in this repo. You can also post on the [AR Foundation Forums](https://forum.unity.com/forums/ar.161/)

  


# arfoundation-demos
AR Foundation demo projects.

Demo projects that use [*AR Foundation 4.1.7*](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/manual/index.html) and demonstrate more advanced functionality around certain features

This set of demos relies on five Unity packages:

* ARSubsystems ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@4.1/manual/index.html))
* ARCore XR Plugin ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arcore@4.1/manual/index.html))
* ARKit XR Plugin ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arkit@4.1/manual/index.html))
* ARFoundation ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/manual/index.html))

ARSubsystems defines an interface, and the platform-specific implementations are in the ARCore and ARKit packages. ARFoundation turns the AR data provided by ARSubsystems into Unity `GameObject`s and `MonoBehavour`s.

The `master` branch is compatible with Unity 2020.3.13f1+

### Building for Unity 2020.2
When building for *Android in Unity 2020.2* you need to modify the following settings under Project Settings / Player / Publishing Settings
* Uncheck Custom Main Gradle Template and 
* Uncheck Custom Launcher Gradle Template  

These are been removed during the upgrade to Unity 2020.3 LTS

  
[Image Tracking](#image-tracking--also-available-on-the-asset-store-here) | [Onboarding UX](#ux--also-available-on-the-asset-store-here) | [Mesh Placement](#mesh-placement) | [Shaders](#shaders)
------------ | ------------- | ------------- | ----------------

  

## Image Tracking 
![img](https://user-images.githubusercontent.com/2120584/86505962-759de600-bd7f-11ea-80c5-b494cdd96427.png)
  

A sample app showing off how to use Image Tracking to track multiple unique images and spawn unique prefabs for each image.

The script [`ImageTrackingObjectManager.cs`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/Scripts/ImageTrackingObjectManager.cs). handles storing prefabs and updating them based on found images. It links into the `ARTrackedImageManager.trackedImagesChanged` callback to spawn prefabs for each tracked image, update their position, show a visual on the prefab depending on it's tracked state and destroy it if removed.

The project contains two unique images
[one.png](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/TrackedImages/one.png)
[two.png](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/TrackedImages/two.png) which can be printed out or displayed on digital devices. The images are 2048x2048 pixels with a real world size of 0.2159 x 0.2159 meters.

The Prefabs for each number are prefab variants derived from [`OnePrefab.prefab`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/Art/One/OnePrefab.prefab). They use a small quad that uses the [`MobileARShadow.shader`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Common/Shaders/MobileARShadow.shader) in order to accurately show a shadow of the 3D number.

The script [`DistanceManager.cs`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/Scripts/DistanceManager.cs) checks the distances between the tracked images and displays an additional 3D model between them when they reach a [certain proximity.](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/Scripts/DistanceManager.cs#L40)

the script [`NumberManager.cs`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/Scripts/NumberManager.cs) handles setting up a [`contraint`](https://docs.unity3d.com/Manual/Constraints.html) (in this case used to billboard the model) on the 3D number objects and provides a function to enable and disabling the rendering of the 3D model. 


  


![img](https://user-images.githubusercontent.com/2120584/86506046-52276b00-bd80-11ea-83de-77ceb634ac8c.gif)
### Missing Prefab in ImageTracking scene.
If you import the image tracking package or download it from the asset store **without** the Onboarding UX there will be a Missing Prefab in your scene. This prefab is a configured [ScreenSpaceUI prefab](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Prefabs/ScreenspaceUI.prefab) from the Onboarding UX. It is configured with the UI for finding an image with the goal of finding an image.


  
## UX 

![img](https://user-images.githubusercontent.com/2120584/87749152-b8fb4a00-c7ac-11ea-807c-0e04325f69da.png)

A UI / UX framework for providing guidance to the user for a variety of different types of mobile AR apps. 

The framework adopts the idea of having instructional UI shown with an instructional goal in mind. One common use of this is UI instructing the user to move their device around with the goal of the user to find a plane. Once the goal is reached the UI fades out. There is also a secondary instruction UI and an API that allows developers to add any number of additional UI and goals that will go into a queue and be processed one at a time.

A common two step UI / Goal is to instruct the user to find a plane. Once a plane is found you can instruct the user to tap in order to place an object. Once an object is placed fade out the UI.

  

![img](https://user-images.githubusercontent.com/2120584/87749208-e2b47100-c7ac-11ea-93ef-5955e2a541b1.png)

The [instructional UI](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/UIManager.cs#L25-L35) consist of the following animations / videos
- Cross Platform Find a plane
- Find a Face
- Find a Body
- Find an Image
- Find an Object
- [ARKit Coaching Overlay](https://developer.apple.com/documentation/arkit/arcoachingoverlayview?language=objc)
- Tap to Place
- None

  


All of the instructional UI (except the ARKit coaching overlay) is an included .webm video encoded with VP8 codec in order to support transparency. 

With the following [goals](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/UIManager.cs#L42-L52) to fade out UI
- Found a plane
- Found Multiple Planes
- Found a Face
- Found a Body
- Found an Image
- Found an Object
- Placed an Object
- None

  


The goals are checking the associated `ARTrackableManager` number of trackables count. One thing to note is this is just looking for a trackable to be added, it does not check the tracking state of said trackable.

The script [`UIManager.cs`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/UIManager.cs) is used to configure the Instructional Goals, secondary instructional goals and holds references to the different trackable managers.

UIManager manages a [queue](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/UIManager.cs#L120) of [`UXHandle`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/UIManager.cs#L6-L16) which allows any instructional UI with any goal to be dynamically added at runtime. To do this you can store a reference to the UIManager and call [`AddToQueue()`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/UIManager.cs#L374-L377) passing in a UXHandle object. For testing purposes to visualize every UI video I use the following setup.

```
m_UIManager = GetComponent<UIManager>();      
m_UIManager.AddToQueue(new UXHandle(UIManager.InstructionUI.CrossPlatformFindAPlane, UIManager.InstructionGoals.PlacedAnObject));
m_UIManager.AddToQueue(new UXHandle(UIManager.InstructionUI.FindABody, UIManager.InstructionGoals.PlacedAnObject));
m_UIManager.AddToQueue(new UXHandle(UIManager.InstructionUI.FindAFace, UIManager.InstructionGoals.PlacedAnObject));
m_UIManager.AddToQueue(new UXHandle(UIManager.InstructionUI.FindAnImage, UIManager.InstructionGoals.PlacedAnObject));
m_UIManager.AddToQueue(new UXHandle(UIManager.InstructionUI.FindAnObject, UIManager.InstructionGoals.PlacedAnObject));
m_UIManager.AddToQueue(new UXHandle(UIManager.InstructionUI.ARKitCoachingOverlay, UIManager.InstructionGoals.PlacedAnObject));
```



There's a [`m_CoachingOverlayFallback`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/UIManager.cs#L76) used in order to enable the ARKit coaching overlay on supported devices but fall back to Cross Platform Find a Plane when it is not. 

The script [`ARUXAnimationManager.cs`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/ARUXAnimationManager.cs) holds references to all the videos, controls all the logic for fading the UI in and out, managing the video swapping and swapping the associated text with each video / UI.

The script [`DisableTrackedVisuals`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/DisableTrackedVisuals.cs) holds a reference to the ARPlaneManger and ARPointCloudManager to allow for disabling both the spawned objects from the managers and the managers themselves, preventing further plane tracking or feature point (point clouds) tracking.

  


### Tracking Reasons
![img](https://user-images.githubusercontent.com/2120584/87749234-fb248b80-c7ac-11ea-9cba-99032ee3bc0d.png)

When the [session](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@4.0/manual/session-subsystem.html) (device) is not tracking or has lost tracking there are a variety of different [reasons](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@4.0/api/UnityEngine.XR.ARSubsystems.NotTrackingReason.html) why. It can be helpful to show these reasons to users so they better understand the experience or what may be hindering it.

Both [ARKit](https://developer.apple.com/documentation/arkit/arcamera/trackingstate/reason) and [ARCore](https://developers.google.com/ar/reference/java/arcore/reference/com/google/ar/core/TrackingFailureReason) have slightly different reasons but in AR Foundation these are surfaced through the same shared API.

The [ARUXReasonsManager.cs](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/ARUXReasonsManager.cs) handles the visualization of the states and subscribes to the state change on the ARSession. The reasons are set and the display text and icon are changed in the [SetReaons()](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/ARUXReasonsManager.cs#L175-L247) method. Here I treat both Initializing and Relocalizing the same and for english display `Initializing augmented reality.` 


  


## Localization

### If you want to use localization make sure to read the required addressables building documentation at the end of this section.

![img](https://user-images.githubusercontent.com/2120584/87749265-17282d00-c7ad-11ea-952a-e1e055091eab.png)

The Instructional UI and the Reasons have localization support through the [Unity localization package](https://docs.unity3d.com/Packages/com.unity.localization@0.7/manual/). It's enabled for the the instructional UI in AR UX Animation Manager with the [m_LocalizeText](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/ARUXAnimationManager.cs#L183) bool and with reasons in the AR UX Reasons Manager with the [m_LocalizeText](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/ARUXReasonsManager.cs#L113) bool.

Localization currently supports the following languages
- English
- French
- German
- Italian
- Spanish
- Portuguese
- Russian
- Simplified Chinese
- Korean
- Japanese
- Hindi
- Dutch

> Tamil and Telugu translations are available but due to font rendering complexities are not enabled currently

The localizations are supported through a [CSV](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Common/Localization/AR%20Foundation%20Demos%20-%20Localization%20-%20Sheet.csv) that is imported into the project and parsed into the proper localization table via [StringImporter.cs](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Common/Scripts/Editor/StringImporter.cs).


  


### If you would like to help out, have a suggestion for a better translation or want to add additional languges please reach out and comment on this publicly available [Sheet](https://docs.google.com/spreadsheets/d/1xxHfDvdQI2SE6JFhy24Z4JZMs4bn8uIaMBbRfW6OlOU/edit?usp=sharing) 

In the scene Localization is driven by the script [LocalizationManager.cs](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/LocalizationManager.cs) which has a [SupportedLanguages enum](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/LocalizationManager.cs#L13-L26) for each supported language. The current implementation only supports selecting and setting a language at **compile time** and **NOT** at runtime. This is because the selected language from the enum is [set](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/LocalizationManager.cs#L122) in the Start() method of [LocalizationManager.cs](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/LocalizationManager.cs).  

After the language is set the localized fields are retrieved from the tables based on specific keys for each value and then referenced in the AR UX Animation Manager and AR UX Reasons Manager.

Many languages require unique fonts in order to properly render the characters for these languages the font's are swapped at runtime along with language specific settings in [SwapFonts()](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Scripts/LocalizationManager.cs#L150-L179)


  


### Packing Asset bundles for building localization support
The Localization package uses [Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@1.12/manual/index.html) to organize and pack the translated strings. There are some additional steps required to properly build these for your application. If you're localizing the text for the instructions or the reasons you will need to do these steps.

1. Open the Addressables Groups window (Window / Asset Management / Addressables / Groups)

![img](https://user-images.githubusercontent.com/2120584/87748240-35405e00-c7aa-11ea-970b-8b0e653ae3b5.png)

2. In the Addressables Groups Window click on the Build Tab / New Build / Default Build Script

![img](https://user-images.githubusercontent.com/2120584/87748387-9bc57c00-c7aa-11ea-99b5-d83c52e29369.png)

3. You will need to do this for every platform you are building for. (Once for Android and once for iOS).
  
  
# Mesh Placement
![img](https://user-images.githubusercontent.com/2120584/87866691-77e47080-c939-11ea-9fe9-25a68ddd8a4b.JPG)
An example scene for using [ARKit meshing](https://docs.unity3d.com/Packages/com.unity.xr.arkit@4.0/manual/arkit-meshing.html) feature with the available surface [classifications](https://developer.apple.com/documentation/arkit/armeshclassification) to place unique objects on surfaces. This demo adds some additional functionality for use cases helpful outside of this demo such as a placement reticle and the [DOTween tweening library](http://dotween.demigiant.com/). 

## Disclaimer
Meshing is only supported through ARKit on LiDAR Enabled devices (iPad Pro, iPhone 12 Pro, iPhone 13 Pro)

## Mesh Classificatons
Classifying the surfaces is managed by the [MeshClassificationManager.cs](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Meshing/Scripts/MeshClassificationManager.cs) which maintains a [Dictionary](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Meshing/Scripts/MeshClassificationManager.cs#L49) of TrackableID's and a native array of ARMeshClassifications. By subscribing to the meshesChanged event on the AR Mesh Manager we maintain the dictionary of added, updated and removed meshes based on trackable ID's of the meshes generated. 

> there is currently an issue with the trackable ID's on the meshes found so we use the string name is order to properly extract and store the correct trackable ID of the meshes

Once we have an up to date dictionary we can query it based on a trackable ID as the key and a triangle index as the index in our native array. This returns an ARMeshClassification enum. 

To update the label at the top of the demo we use a physics raycast to raycast against the megamesh generated by the ARMeshManager to get the correct triangle index and parse the current classification for a more readable string label. 
> to generate a mesh collider for physics raycast our megamesh must contain a mesh collider component on it

## Mesh Placement
The [Mesh Placement Manager](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Meshing/Scripts/ClassificationPlacementManager.cs) script handles showing the UI for each unique surface and spawning the objects at the placement reticle position. In the Update method I am checking against [specific classifications](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Meshing/Scripts/ClassificationPlacementManager.cs#L101-L104), in this case Table, Floor and Wall to enable or disable specific UI buttons. The UI buttons are configured in the scene to pass an index and instantiate the assigned prefab in the object list for each surface.

There's also some additional logic for placing floor and table objects to rotate them towards the user (Camera transform).

## Placement Reticle
A way to place content on surfaces based on the center screen position of the users device. This reticle shows a visual that can snap to mesh (generated ARKit mesh) or planes. It uses an AR raycast to find the surfaces and snaps to AR Raycast Hit pose position and rotation. 

There is also additional logic to scale up the reticle's local scale based on the distance away from the user (AR Camera transform). 

For determining between snapping to a mesh and a plane we use a Raycast Mask. 

Mesh:
```m_RaycastMask = TrackableType.PlaneEstimated;```
Plane:
```m_RaycastMask = TrackableType.PlaneWithinPolygon;```


## Mesh Key
To visualize and understand the different classified surfaces we are using a modified version of the [MeshFracking](https://github.com/Unity-Technologies/arfoundation-samples/blob/main/Assets/Scenes/Meshing/Scripts/MeshClassificationFracking.cs) script available in AR Foundaiton Samples. We've added an additional helper method to modify the alpha color of the generated meshes [ToggleVisability().](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Meshing/Scripts/MeshClassificationFracking.cs#L350-L366) This is all driven by a Toggle UI button in the scene and changes the shared material color on each material on the generated prefabs. By default they are configured to be completely transparent.


## DOTween is available on the Unity Asset store [here](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
For this demo it is used to scaling up the placed objects as they appear. 

It was developed by Daniele Giardini - Demigiant and is Copyright (c) 2014. Full License for DOTween available [here](http://dotween.demigiant.com/license.php)


# Shaders

A collection of Shaders built for AR and AR use cases

## ARKit Fog
![img](https://user-images.githubusercontent.com/2120584/89489924-66d09780-d760-11ea-8ce8-2b0f04af59db.jpg)
This effect uses the latest Depth API and is **only** available on LiDAR enabled iOS devices like the iPad Pro. Currently this is only supported in the built-in render pipeline.

The Fog scene uses an ARKit background shader that incorporates Unity scene fog into the shader used for rendering the ARKit device camera feed to the screen. It is heavily based on the ARKitBackground shader shipped with the ARKit package. 
>In order to properly use this shader the Custom Material checkbox on the ARCameraBackground component must be checked and a material with this shader assigned.

The Fog scene includes AR plane finding and placement scritps to place a virtual object in AR that uses the standard shader. The scene has fog enabled in the [Lighting Settings window](https://docs.unity3d.com/Manual/lighting-window.html). It's set to an end distance of 35 and a UI slider that changes the scene fog between 1-35 is configured in the scene. Manipulating this slider will change the appearance of the density of the fog in the scene.

![img](https://user-images.githubusercontent.com/2120584/89490315-6edd0700-d761-11ea-9965-af86a9bbc296.gif)

# Universal Render Pipeline Shader Graph Shaders

## Disclaimer
These shaders are configured with [Universal Render Pipeline](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest) and [Shader Graph](https://docs.unity3d.com/Packages/com.unity.shadergraph@latest). They are expected to change or potentially break if either package is updated. Some of the implementations of these shaders are based on current API's and package structures that are not guaranteed to be consistent in future packages.

## Setup
To enable these shaders you must assign the [Universal Render Pipeline Asset](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Shaders/URP/UniversalRenderPipelineAsset.asset) in Project Graphics settings. This Pipeline asset has been pre-configured to work well with the shadow shaders and the ARFoundationForwardRendererData asset has been configured to properly render with AR Foundation by adding the AR Background Render Feature. 

## Wireframe shader
![MeshViz](https://user-images.githubusercontent.com/2120584/90083549-b7e40c80-dcc7-11ea-8dde-0b41eda98c23.png)

This shader enables visualizing the edges of any mesh in Unity. Here we are using it to visualize the runtime mesh generated by ARKit from the AR Mesh Manager. This shader works by combining barycentric data (vertex colors) with a custom Shader graph for determining edges and painting pixels an assigned color.

The [MeshVisualization scene](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Shaders/Wireframe/Scenes/MeshVisualization.unity) is configured to apply the barycentric data at runtime by subscribing to the MeshChanged events in the AR Mesh Manager component. The AR Mesh Manager uses the Mesh Visualization prefab that has a Mesh Filter and Mesh Renderer with an assigned material using the WireFrame shader graph. When meshes are added or updated the barycentric data also gets updated on the mesh enabling this effect.

![MeshViz](https://user-images.githubusercontent.com/2120584/90083497-89fec800-dcc7-11ea-9142-5e93f662695b.gif)


> This sample enables and disables plane tracking by Toggling the AR Plane Manager component. You can see the optimizations with the mesh along flat surfaces when plane tracking is enabled.

## Shadows

Shadows are important for grounding objects in AR. It helps the user better understand the depth and position of augmented content in the real world. For enabling shadows in the Universal Render Pipeline custom .hlsl files have been written and used in shader graphs as custom nodes. 

Both of these shaders are built to be applied on a single flat surface such as a plane or quad. For heirarchy setup you can see that the prefabs using these shaders have an empty root game object, the plane at the base of the object and the content positioned above that both as children of the root object.

### Blurred Shadows
![BlurredShadows](https://user-images.githubusercontent.com/2120584/90286951-b03d7880-de2b-11ea-94da-cb81387191e2.gif)
The blurred shadows have two custom node inputs. One for contact shadows created with objects close to the plane using this shader. Another for implementing stocastic blurring to create much softer shadows giving it a blurred edge. It is recommended to use these shadows for dynamic content.

* It is highly recommended to have `No Cascades` in the Shadow settings of the Universal Render Pipeline Asset, Shadow Settings to greatly increase performance.

To further increase performance you can lower the [NUM_STEPS](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Shaders/Shadows/ShaderGraphs/BlurredShadowPlaneLighting.hlsl#L6) from 10 to 5 and the [TOTAL_STEPS](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Shaders/Shadows/ShaderGraphs/PlaneContactShadows.hlsl#L7) from 10 to 5. There will be a noticable visual difference but should give higher performance especially on lower end platforms. 


### Hard Shadows
![ShadowsHardGif](https://user-images.githubusercontent.com/2120584/90287687-33ab9980-de2d-11ea-86ba-50c8d95dc3df.gif)
This uses a custom node to consume the lighting data in the scene and apply it to a transparent surface. The shadows of this shader are driven by the graphics and quality settings in the project. These are the level of shadows you can expect out of the box from Unity. It is recommended to use these shadows for static content.

## Camera Grain - Only compatible in Unity 2020.3+ and ARKit
![CameraGrainGif](https://user-images.githubusercontent.com/2120584/90287142-2215c200-de2c-11ea-8663-e180019c1e1d.gif)
![CameraGrain](https://user-images.githubusercontent.com/2120584/90286950-af0c4b80-de2b-11ea-8fdb-fb15bf8bd253.png)
Camera grain is a unique feature to ARKit which produces a tileable metal texture to match the visual characteristics of the current video stream. In Unity this is surfaced as a 3D grain texture through the [ARCameraFrameEventArgs](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/api/UnityEngine.XR.ARFoundation.ARCameraFrameEventArgs.html). For  the shader sample this grain texture is then applied to a custom shader graph that also creates visual noise on the object. This effect in general is very subtle and more visible in darker areas where the grain on the camera feed is also more apparent. 

> Compatibility for 2020.2+ version of Unity is due to how 3D textures are handled and managed

This sample also uses a script [ProbePlacement](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Common/Scripts/ProbePlacement.cs) for manually placing environmental probes to further enhance the effect.

