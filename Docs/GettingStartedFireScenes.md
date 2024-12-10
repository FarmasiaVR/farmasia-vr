# Scenario Overview for Fire Safety

## Emergency Exit Scenario 1 and 2

These scenes are meant for practicing escaping from a fire that is too large to extinguish. In these scenes the player must find a route to an emergency exit that isn’t blocked by fires. Then the player has to exit through that emergency exit door by grabbing the handle.

Scripts used:
- SimpleFire.cs
- This script is used for controlling a fire attached to the player
- Turns on the fire vfx and sounds
- PlayerInteractions.cs
	- This script is used for igniting the fire attached to the player, when going too close to a fire
	- This script also triggers a game over when the player has been on fire for too long



## Emergency Eye Shower Scenario

### Description: 

This scene trains the player to use the laboratory’s eyeshowers. The Scene starts with a text box informing the player that it’s the end of the day and an instruction to put a bottle away into the fume closet. When the player teleports near the closet with the bottle in hand, liquid bursts from the bottle and a pop up appears, informing the player that someone bumped into them and chemicals bursted into their eyes. The player is instructed to use the lab’s eyeshowers to clean their eyes. When they do, the scene ends with a final text box menu.

Scripts used:

-ColliderChecker.cs
	- Invokes the objects collide event when the bottle enters BurstArea
-EyeShowerScenarioManager.cs
	- Takes care of what happens when bottle bursts and eyes are washed.



## Emergency Shower Scenario

### Description:

This scene simulates a chemical spill emergency to train players in using the laboratory’s emergency showers. It starts at a workstation where the player is directed to pick up a bottle containing NaOH (sodium hydroxide). As they do, the bottle is designed to tip over, simulating a clumsy spill of the alkali on the player. Immediately, a red alert text prompts them to locate and use the nearest emergency shower. Successfully standing under the shower for more than 5 seconds triggers an ending text, marking the completion of the level.

### Scripts used:

- TipOverObject.cs
- Tips over bottle when grabbed
- Triggers visual & audio effect
- Sets warning text to active
- Enables AcidExtinguisherShower script
- AcidExtinguisherShower.cs
- Checks if the player has showered for 5 seconds
- Triggers end text




## Fire Extinguisher Scenario
### Description:
The idea of this scene is to simulate an accident when working with the bunsenburner. The player spawns at his workstation, reads the instruction text, goes to the lab entrance which has a proximity trigger that prompts text and enables another proximity trigger located at the workstation. When the player now returns to his workspace the newly enabled trigger activates and tips over the bunsen burner causing some papers to ignite. The player now has to go get an extinguisher to extinguish the burning papers. After the player has extinguished the flames and deactivated/ turned off the bunsenburner, a text box appears reminding the player to contact the supervisor for further instructions.
### Scripts used:
SpreadableFire.cs (handles the fire propagation)
BunsenburnerFire.cs (can ignite the spreadable firegrid turned on )
RotateObject.cs (used to animate the burner tipping over)
ActiveSpreadableFires.cs (checks how many active fires in the scene, and calls an event when no fires are burning, it only starts tracking fires after one has been lit)
PlayerProximityTrigger.cs (trigger event when player steps into the collision box)



## Fire Blanket Scenario

### Description
This scene is meant for training the player to use fire blankets. In the current state of this scene the player must grab the fire blanket and open it. After that a fire lights up on the floor close to the player. The player must then extinguish that fire by throwing the fire blanket on top of it. This scene is work in progress. The fire blanket should have at least some physics. Also the fire should have some reason for starting. For example there could be some device in the laboratory that lights on fire.

### Scripts used
- FireBlanketSceneManager.cs
	- Used for tracking the tasks the player has done (grabbing the blanket, opening it, extinguishing the fire)

