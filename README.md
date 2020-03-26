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


## Image Tracking — also available on the asset store [here](https://assetstore.unity.com/packages/templates/ar-foundation-demos-image-tracking-164880)

A sample app showing off how to use Image Tracking to track multiple unique images and spawn unique prefabs for each image.

The script [`ImageTrackingObjectManager.cs`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/Scripts/ImageTrackingObjectManager.cs). handles storing prefabs and updating them based on found images. It links into the `ARTrackedImageManager.trackedImagesChanged` callback to spawn prefabs for each tracked image, update their position, show a visual on the prefab depending on it's tracked state and destroy it if removed.

The project contains two unique images
[one.png](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/TrackedImages/one.png)
[two.png](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/TrackedImages/two.png) which can be printed out or displayed on digital devices. The images are 2048x2048 pixels with a real world size of 0.2159 x 0.2159 meters.

The Prefabs for each number are prefab variants derived from [`OnePrefab.prefab`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/Art/One/OnePrefab.prefab). They use a small quad that uses the [`MobileARShadow.shader`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Common/Shaders/MobileARShadow.shader) in order to accurately show a shadow of the 3D number.

The script [`DistanceManager.cs`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/Scripts/DistanceManager.cs) checks the distances between the tracked images and displays an additional 3D model between them when they reach a [certain proximity.](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/Scripts/DistanceManager.cs#L40)

the script [`NumberManager.cs`](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/Scripts/NumberManager.cs) handles setting up a [`contraint`](https://docs.unity3d.com/Manual/Constraints.html) (in this case used to billboard the model) on the 3D number objects and provides a function to enable and disabling the rendering of the 3D model. 

### Missing Prefab in ImageTracking scene.
If you import the image tracking package or download it from the asset store **without** the Onboarding UX there will be a Missing Prefab in your scene. This prefab is a configured [ScreenSpaceUI prefab](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/UX/Prefabs/ScreenspaceUI.prefab) from the Onboarding UX. It is configured with the UI for finding an image with the goal of finding an image.

## UX — also available on the asset store [here](https://assetstore.unity.com/packages/templates/ar-foundation-demos-onboarding-ux-164766)

A UI / UX framework for providing guidance to the user for a variety of different types of mobile AR apps. 

The framework adopts the idea of having instructional UI shown with an instructional goal in mind. One common use of this is UI instructing the user to move their device around with the goal of the user to find a plane. Once the goal is reached the UI fades out. There is also a secondary instruction UI and an API that allows developers to add any number of additional UI and goals that will go into a queue and be processed one at a time.

A common two step UI / Goal is to instruct the user to find a plane. Once a plane is found you can instruct the user to tap in order to place an object. Once an object is placed fade out the UI.

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

