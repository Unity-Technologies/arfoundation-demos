# This repo is intended to provide more advanced demos for AR Foundation outside of the [Samples Repo](https://github.com/Unity-Technologies/arfoundation-samples/).
For questions and issues related to AR Foundation please post on the AR Foundation Sample [issues](https://github.com/Unity-Technologies/arfoundation-samples/issues) and **NOT** in this repo. You can also post on the [AR Foundation Forums](https://forum.unity.com/forums/handheld-ar.159/)

# arfoundation-demos
AR Foundation demo projects.

Demo projects that use [*AR Foundation 3.0*](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@3.0/manual/index.html) and demonstrate more advanced functionality around certain features

This set of demos relies on five Unity packages:

* ARSubsystems ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@3.0/manual/index.html))
* ARCore XR Plugin ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arcore@3.0/manual/index.html))
* ARKit XR Plugin ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arkit@3.0/manual/index.html))
* ARKit Face Tracking ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arkit-face-tracking@3.0/manual/index.html))
* ARFoundation ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@3.0/manual/index.html))

ARSubsystems defines an interface, and the platform-specific implementations are in the ARCore and ARKit packages. ARFoundation turns the AR data provided by ARSubsystems into Unity `GameObject`s and `MonoBehavour`s.

The `master` branch is compatible with Unity 2019.3 




## Image Tracking — Also available on the asset store [here](https://assetstore.unity.com/packages/templates/ar-foundation-demos-image-tracking-164880)
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



## UX — Also available on the asset store [here](https://assetstore.unity.com/packages/templates/ar-foundation-demos-onboarding-ux-164766)

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
