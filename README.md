# Escape from Ironhold

## Overview
Escape from Ironhold is an isometric adventure game developed in Unity, where players assume the role of a prisoner trying to escape from a medieval dungeon. The game combines stealth mechanics and puzzle-solving elements as players explore their environment to find hidden items, evade a patrolling guard, and ultimately unlock their way to freedom.

### Setting
In this game, you control a prisoner trying to escape from a guarded medieval dungeon. The gameplay focuses on stealth and puzzle-solving. Players need to avoid the guard by hiding and timing their movements while interacting with objects in the environment to unlock the path to their freedom. The main objectives are to find a way out of the prison cell, navigate past the patrolling guard, and finally open the main gate.

### Gameplay
The game focuses on providing an engaging escape experience with multiple gameplay features:
- **Isometric Viewpoint:** The game employs an isometric camera angle, giving players a comprehensive view of their surroundings. This helps in planning movements while also keeping an eye on the guard’s patrol path.
- **Guard Patrol:** The guard follows a specific patrol route. Players must study this pattern to avoid detection, taking advantage of hiding spots and timed movements.
- **Stealth and Evasion:** If you’re spotted by the guard, you’ll be sent back to your cell and must try again. Successfully escaping the guard's sight involves using the environment and precise timing to move unseen.
- **Interactive Environment:** Players can click on various objects to interact with them. This includes examining doors, picking up keys, and using items to solve puzzles.
- **Inventory System:** Manage the items you collect as you progress, which are crucial to unlocking doors and accessing new areas.
- **Puzzle Solving:** The game requires the player to find and use objects strategically to advance, by unlocking or interacting with other items.

## Development Documentation

### Tools and Technologies
- **Game Engine:** Unity
- **Programming Language:** C#
- **Assets:** Custom scripts with selective use of Synty Studios assets (not included in this repository due to licensing).

### Architecture and Code Structure
- **Player Controller:** Handles player input, movement, and interactions. The player can navigate the environment by clicking on various points or objects, which then trigger different actions based on the context.
- **Guard AI:** The guard uses a state machine to switch between different behaviors (patrol, alert, and detection). Its FieldOfView component is constantly searching for the player, and it reacts when objects like doors are opened.
- **Camera Management:** The isometric camera dynamically follows the player, providing a consistent view of both the character and the environment. The camera's position adjusts in certain areas to maximize visibility.
- **User Interface:** The UI includes an interaction panel, an inventory display, and various in-game messages that inform the player about their actions and the consequences.
- **Interactions:** Objects are tagged with properties like Openable, Unlockable, and Interactable, which dictate how the player can use or manipulate them. Scripts control these behaviors, creating a flexible system for adding new interactable elements.

## Known Issues
- The cursor does not change forms with perfect accuracy when hovering over interactable objects.
- The cursor appears too small despite multiple attempts to enlarge it.
- The guard’s sleep routine can cause the player to wait too long for an opportunity to evade.
- Some inconsistencies in interactions, such as the guard locking the player in the cell even if the key has already been stolen.
- Some collisions are not finely tuned, allowing the player to exploit certain areas in the game.

## Future Improvements and Additions
- **Additional Items and Interactions:** Introduce new items and ways to interact with the environment, such as using stones to distract the guard or extinguishing torches to reduce visibility.
- **Additional Levels:** Expand the game beyond the dungeon, allowing the player to escape from the castle of Ironhold itself.
- **Sound Effects:** Currently, the game has no sound or music. Adding these elements will significantly improve the atmosphere and player immersion.
- **Tips and Tutorials:** As the game grows more complex, adding in-game tutorials and hints will help players understand the mechanics faster.
- **Story Development:** Developing a more detailed story and narrative background will add depth to the player’s experience.

## Acquired Knowledge
Throughout the development of Escape from Ironhold, I gained valuable insights into game development and Unity’s tools:
- Learned how to handle key Unity components like the camera, canvas, UI elements, colliders, event system, input system, particle effects, and AI navigation.
- Studied mathematical and geometric concepts (e.g., vectors, quaternions, linear interpolation) and their applications in computer graphics.
- Gained a deeper understanding of how different components in Unity communicate and the events order in each frame.
- Realized that in 3D game development, no detail is too small, especially when it comes to environment interactions.
- Discovered the complexities involved in implementing even seemingly simple mechanics, like doors, which can lead to unpredictable gameplay outcomes.