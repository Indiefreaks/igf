# Overview
The Indiefreaks Game Framework or IGF is a set of .Net libraries compiling a few years of experimenting and prototyping design patterns developing games for Microsoft Xna Framework using the SynapseGaming SunBurn graphics engine.

The goal here is to share with the community what I consider as best practices so they can avoid going through the same steps as I did. The Indiefreaks Game Framework is totally free (like in free beer) to use but it still requires you to acquire a SunBurn engine license (learn more here).

The goal here is to share with the community what I consider as best practices so they can avoid going through the same steps as I did. The Indiefreaks Game Framework is totally free (like in free beer) to use but it still requires you to acquire a SunBurn engine license ([learn more here](http://http://www.synapsegaming.com/products/sunburn/engine/)).

# Core Features
* *Target Platforms*: The Indiefreaks Game Framework works on Windows Vista, Windows 7, Xbox 360 and Windows Phone 7.1 platforms.
* *Application Framework*: One of the core features of the Indiefreaks Game Framework is to provide a full application life cycle framework to ease the creation and maintenance of games splitting logically how games are generally developped.
* *Smart Content Management*: IGF encapsulates the Xna ContentManager class to load content as WeakReferences avoiding therefore the developer the need to manage Content memory. You can also easily asynchronously preload content while rendering a custom thread safe loading screen.
* *Input Management*: InputManager handles GamePad related XBLIG requirements through a set of Connection, Disconnection events and a LogicalPlayerIndex enumeration you can use to know which PlayerIndex is in your game. Also includes a Virtual GamePad using Keyboard & Mouse or WP7 touch mapping to buttons and sticks.

# 2D & 3D Rendering
* *SunBurn Rendering*: The Indiefreaks Game Framework implements Forward and Deferred rendering using the core SynapseGaming SunBurn engine rendering capabilities. (Deferred rendering requires a SunBurn engine Pro license and isn't available on WP7)
* *Easy Hardware Instancing*: Save your game framerate using the builtin InstancingManager to render hundreds of similar meshes using 10 lines of code while keeping direct control on each instance world transforms.
* *Cameras*: The framework includes a set of 2d & 3d cameras easily extendable (static and controlled by input) that plug nicely to the SunBurn engine using a CameraManager residing inside the SunBurn SceneInterface Managers.
* *Sprite System*: The Indiefreaks Game Framework extends SunBurn 2d features with a Sprite system enabling developers to benefit from the SunBurn rendering as well as 2d world transform and collision.
* *GUI & Menu System*: Adding a Main Menu, Pause Menu, a Heads Up Display (HUD) or any other Graphical User Interface for your game is now easy, expendable and framerate friendly thanks to IGF. Thanks to IGF Smart Input System, it supports both GamePad and Mouse+Keyboard input devices switching 2 properties.
* *Particle System*: Through Mercury Particle Engine integration as SceneEntity instances, IGF lets you emit & trig particles with a few lines of code & movable within SunBurn Editor.
* *Post Processing Effects*: Add Depth Of Field, God Rays (under development), SSAO (under development), Motion Blur (under development) post processing effects to your games simply adding them to your SunBurn PostProcessorManager instance and setting a few properties that can be tweaked at runtime. (requires SunBurn Pro).

# Physics & Collisions
* *BEPUPhysics v1.1.0.0 integration*: IGF allows you to replace SunBurn's default CollisionManager with the impressive BEPUPhysics system. It supports:
	* *Dynamic & Static Box Colliders*
	* *Dynamic & Static Sphere Colliders*
	* *Dynamic & Static ConvexHull Colliders*
	* *Static Mesh Colliders*

# Game Logic
* *Abstract Network System*: IGF comes with an abstraction layer for networking concepts. It allows the developer to make single or multiplayer games without caring much how the network calls, client & server concepts are implemented.
	* *Xbox Live*: As part of the Abstract Network feature, IGF fully implements the Xbox Live features provided within the Xna framework (as well as Games For Windows Live).
	* *Lidgren Network Library*: Since Xna doesn't support Games For Windows - Live except for development purposes, IGF comes with an equivalent implementation using the [Lidgren Network Library](http://code.google.com/p/lidgren-network-gen3/) (Only for SinglePlayer & Local Area Network for now).
	* *Local Session*: If you want to make a single player game, the Local session system will be your best choice as it provides much of Xbox Live features (Player Identification, Game session management, ...) for a single player game. Moreover, it eases a lot moving your game from Single Player to Multi Player using any of the network library implementations above.
* *Logic Components & Behaviors*: Ease your game logic implementation with Player & Non Player Agents that accept your own Behavior scripts written in C# and plug them to any SceneObject or SceneEntity in your scene. Moreover, they work seemlessly with any of the above Network implementations letting you code once your game logic and play it on Local session, Xbox Live or any other network implementation.
	* *Steering Behaviors*: IGF implements Steering Behaviors for Autonomous Agents that can be applied to 2D or 3D game entities independently. Easy to setup and configure Seek, Arrive, Flee, Evade, Pursuit, Obstacle Avoidance, Cohesion, Alignment and much more…
	* *Finite State Machines*: Create your desired game entities states and add them to IGF Finite State Machine system using a simple enumeration. Simply add a StateMachineAgent component to your SceneEntity or SceneObject instances and they’ll get a brain.
	* *Goal Driven AI*: For more complex AI entities, IGF provides a Goal Driven system that can be applied to your game entities. Define their goals, implement the desirability computation and add them to the GoalBrain SunBurn Component and you’ll see your game entities make smart decisions! 

# Development Requirements
* Microsoft Xna Game Studio 4.0 refresh
* SynapseGaming SunBurn Engine Indie or Pro editions
* Windows Vista­® (x86 or x64) with Service Pack 2 - All editions except Starter Edition
* Windows 7® (x86 or x64) - All editions except Starter Edition
* Object Oriented Programming concepts
* C# programming
* Microsoft Xna Framework
* SunBurn Engine for advanced rendering
