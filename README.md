# FalloutLockPicking
 Recreating the lockpicking feature from Fallout 4 in Unity.

## Description
 Move the mouse to rotate the bobby pin into place. Press and hold W on the keyboard to attempt to pick the lock. The closer the lock goes to being fully horizontal, the closer it is to being unlocked.
 
 Hold W too long and the bobby pin will break. Unlocking will generate a new lock and an additional attempt can be made.
 
 I used the Unity game engine, as I continue to learn more about game design. Because of this, all the scripting is done in C#.

 ### Scenes
  Standalone Demo - ["LockPicking_Standalone_Demo.unity"](/Assets/Scenes/)
  
  **-=Not Yet Complete=-**  ~~Fist-Person Demo - ["LockPicking_FP_Demo.unity"](/Assets/Scenes/)~~

 ### Challenges and Updates
  Turns out, trigonometry is very handy to know when you want to do some math on rotations and angles! It was a challenge to understand how exactly all the rotations needed to be handled to calculate a range from which the lock could be unlocked.
  
  See the credits section below for the links, but essentially the script from Zeppelin Games defines a range in degrees from the center of the lockpick's local position and allows the lockpick to be rotated along that axis. A rotation in degrees from the center of that range is generated for each new lock and is tested against the rotation angle of the lockpick when attempting to unlock.
  
  In addition to this core logic, I implemented some animations, sound effects, and managers to handle the possibility of different lock difficulties and the other gameplay designs that come along with that. 
  
  The current demo scene that is complete is just an overlay of the lockpicking itself, but I am working to finish up a first-person scene with doors of varying difficulties. The player would interact not with static UI, but with the world-space objects directly. Expect that update after version 2.0a.

## Install and Run
 You should be able to run the project if you download and unzip the repository and [Add project from disk](https://docs.unity3d.com/hub/manual/AddProject.html) in the Unity Hub Projects tab.
 
 Please let me know if you come across any errors in the way I've prepared things.
 
 I used Unity 2022.3.13f1 LTS and the URP.

 You can also download the unity package for just the standalone demo on my [itch.io page](https://evankurtzart.itch.io/lockpicking-project).

## How to Use

 ### Standalone Demo
  I do not doubt that there are better ways of achieving this, but as this was mainly a project about further learning, I'll leave any refactoring up to you.
  
  The [lockpicking script](/Assets/Scripts/LockPicking_Standalone_Demo.cs) is attached to the bobby pin gameobject, which is a child of the inner lock object. There is an additional bobbypin object that is used to animate, since the animator has control of the transform until Update() is completed. You could do this in LateUpdate(), but having a separate object to animate was how I chose to go about it. 
  
  Anyway, the script needs references to the lockpicking camera, the inner lock gameobject, the lockfollow empty gameobject (the pivot point of the bobbypin), the Animator attached to the lock gameobject, two Audio Sources, and some audio clips. The rest are simple variables, but I recommend leaving them where they are.
  
  ![Screenshot of the standalone scene, focusing on the lockpicking script in the inspector.](https://img.itch.zone/aW1hZ2UvMjUxMjg3MS8xNTEzNjczMS5wbmc=/original/yH6NfO.png)
  
  The camera is needed to find where the mouse is relative to the screenspace and the lockpick.
  
  The inner lock is needed to be able to rotate the lock (and its children).
  
  The lock follow empty lets us set local position on the lockpick and keep track of where that point in space is when the lock is turned, since it is itself a child of the inner lock.
  
  We have two audio sources so that one can be set to loop, and 7 audio clips, one for unlocking, one for the looping wiggle sound, one for breaking the pick, and 4 for when the bobby pin is being moved.
  
  This script makes calls to the managers mainly for UI purposes, but also for handling lock difficulty.

  ### First-Person Demo
   I'll update the README when the FP Demo is released.

## Credits
 - The logic that calculates position and rotation angles for the lockpick and inner lock is from a [Zeppelin Games tutorial on YouTube](https://www.youtube.com/watch?v=68iYL-rktQ4&list=PLEj1kOxzPTLWX_q_XvjFF9h_3cS4C1jyu&index=12).
 - I found the code that enqueues console logs for printing to a UI element [from derHugo on this StackOverflow question](https://stackoverflow.com/questions/60228993/putting-debug-log-as-a-gui-element-in-unity).
 - The unlock and movement sound effects were spliced from an [audio clip by Breviceps on freesound.org](https://freesound.org/people/Breviceps/sounds/458405/), licensed under CC 0.
 - The sound effect that plays when a pick is broken is from an [audio clip by duncanlewismackinnon on freesound.org](https://freesound.org/people/duncanlewismackinnon/sounds/159331/), licensed under CC BY 3.0.
 - The managers are based on the work from [Joseph Hocking's Unity In Action Third Edition](https://www.manning.com/books/unity-in-action-third-edition)​.

## Licenses
 The assets in the project, namely the lock, pick, and screwdriver models and textures, are licensed under [CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/)
 The code is licensed under [GNU GPLv3.0](https://www.gnu.org/licenses/gpl-3.0-standalone.html)
 
