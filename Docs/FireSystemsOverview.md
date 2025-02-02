
# Fires

There are 4 different fire game objects in the project:
- FireArea
- SimpleFire
- SpreadableFire

## FireArea:
FireArea is a game essentially a box with a fire effect that has a collider to prevent teleportation through it, and another box that ignites the player (with PlayerInteractions.cs), when the player enters its area. 
It is used in the emergency exit scenes called(at the time of writing this document) EmergencyExit and EmergencyExit 1

## SimpleFire:
Simple fires are used in the demo hallway in the firesafety menu, as well as on the player(for VFX purposes when ignited). SimpleFire like the FireArea can have a collider that ignites the player with a triggerbox (using PlayerInteractions.cs). The SimpleFire.cs mostly handles the flame start- and extinguish animations but it can call functions when ignited and extinguished through unity events.
SimpleFires can be extinguished using the FireExtinguisherCollision.cs

## SpreadableFire:
SpreadableFire is used in the extinguishing scenario. It can dynamically spread and be extinguished. 
 It has a “grid” based propagation system where any ignitable vertex in the grid can be ignited and then propagates to adjacent ignitable vertices. It then generates a mesh based on the burning parts in the grid and updates a fire effect to emit from it. Once part of the grid is extinguished that part won’t ignite and burn again. The generated mesh can collide and ignite the player (using PlayerInteractions.cs). The SpreadableFire can be ignited via collision of a burning object or by passing a world point into the IgniteClosestVertexNonCollision function.
There are two events fireIgnited which is called when the grid is not burning and is ignited, and fireExtinguished which is called when extinguished.
The fire can be extinguished using the ExtingushSpreadableFire.cs (just noticed that the script has a typo in the name hehe). This script interacts via the ExtinguishVerticesInRaidus function.

## You burn?:
If the player is ignited there is a timer counting down until you burn to death. You can extinguish yourself by using an Emergency shower this is using the ShowerExtinguisher.cs



# Extinguishing

## Abstract view of the fire extinguisher:
The extinguisher in use is called ExtinguisherWithHoseAndPin. It has scripts to extinguish both the SimpleFire and the SpreadableFire. Activate/enable the extinguish functionality happens using events in the XR Socket Interactor, located on the prefab at Body->upper handle->PinAttach. The XR Grab Interactable has events for the actual extinguishing.

## FireHose:
The prefab is called firehosessystem_hosed. (There are prefabs for older and simpler systems as just firehosesystem and other one firehosesystem_simple, but they haven’t been updated after the completion of the firehosesystem_hosed).
The scripts involved with the firehosesystem_hosed are FireHoseSystemBehaviour.cs (located at the Turnable_part -> hose(1/2/3) -> firehosehead(1/2/3)) and FireHoseSpawner.cs (located at Turnable_part -> reels). First one mainly controls working of the firehoseheads and the latter mainly controls the reel and hose lengths and their activation. They are both commented quite extensively as they involve many functions. There are also two fire extinguishing related scripts located at each firehosehead.
The extinguishing of the firehose works pretty much the same way as the on the ExtinguisherWithHoseAndPin. Though to enable extinguishing functionalities the script FireHoseSystemBehaviour.cs is called from XR Grab Interactable located at switch_and_mainline->firehoseswitch.  And the extinguishing is handled from Turnable_part->hose1->firehosehead1 (or hose2 or hose3 depending on how much of the hose you’ve rolled out) using the events on their respective XR Grab Interactable components.
The different hose lengths can be extended simply by scaling the hose(1/2/3) and adjusting position. There are enough armatures in the hoses for this not to look too rough. This way though the scaling of the hosehead itself and collision box for extinguishing should be reverted afterwards. The behaviour of a hose becomes very erratic if pulled too far for its length. The hoses include some collision boxes and parts with enabled gravity.
In the hosesystem many parts interact with each other so if changes are made the system should be tested with care.


## Fire blanket (which needs work):
The fire blanket right now is in a work in progress state. And will have to be reworked.
The root object of the prefabs(FireBlanket) XR Grab Interactable has an event that calls the SpawnBlanket function in the script FireBlanketPackageScript.cs (this script is kind of redundant since the gameobject active toggle could be done directly in the event. But who am I to judge?). This enables the white square used to extinguish fires (only simple fires) using the FireBlanketScript.cs which is essentially a carbon copy of the FireExtinguisherCollision.cs  script.



# Relevant Scripts:
- PlayerInteractions.cs
- SimpleFire.cs
- SpreadableFire.cs
- FireExtinguisherCollision.cs
- ExtinguishSpreadableFire.cs
- ShowerExtinguisher.cs
- FireBlanketScript.cs
- FireBlanketPackageScript.cs
- FireHoseSystemBehaviour.cs
- FireHoseSpawner.cs
