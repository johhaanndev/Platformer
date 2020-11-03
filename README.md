# Platformer

Who does not know Super Mario world? In this repository, the very first level is recreated but with other assets.

**Game Explanation**

The game is based in an adventure platformer game. Player will have to go through all the map, avoiding slime (goombas like) or killing them and collecting coins to have a higher final score. Goombas are killed when the player falls right into them. But if it touches by their side, then the player dies. Another way of dying is falling off of the ground platforms.

Controls are basic: arrows for movement and jump can also be done with spacebar, just to make it more confortable for some players.

Structre game is made of 2 scenes:

Main Menu: buttons to play and quit.
Level scene: where the main gameplay is developed.

**Creation Process**

_Main Menu_

This scene is done using UI elements. Basically two buttons and text. When player hits PLAY button the game starts, if it hits EXIT, the game quits. There is on script that manages the scene travels, each button has its own method.

_Level_

The main loop takes place in this scene. Here you can find the prefabs, animations, characters and platforms. Also, in this scene all the scripts have a use:

- Gameplay Manager: attached to the GameplayManager game object. In this script all the data is taken, analized and given as informaion to multiple game Objects of the the scene. When the player dies, a "game over" menu pops up. If the player won, it pops up a "You win!" menú. This script is in control of points, it recieves if the player got more points and updates the screen so player can see how many points he has. Also, a countdown counter happens here, when time reaches 0, a "game over" menu shows up. Gameplay Manager script also has control of soundtrack for each situation.

- Player Controller: player can control the character thanks to this script. As well as its animations are set with a finite state machine. Using the OnCollisionEnter2D and OnTriggerEnter2D methods, we developed the way player kills goombas and collects coins. This script takes stores points and status, then GameplayManager gets this information.

- Camera Controller: this is cript is the responsible for the camera movement. It only needs two positions: the minimum X and maximum X available to move. This way we avoid surpass the limits. The camera follows with a little offset to the right to help player see more level, when the player surpass the limits, the camera stands.

- Slime Controller: the enemy's AI. Slimes walk between two waypoints: A & B. When it reaches x position of A point, it turns right and when it reaches B it turns left. When there are two slimes close and the hit themselves, they change directions. This script contains the animator component for its walking state. If the player kills, an exploding animation starts and then the slime game object is destroyed.

- FallDeath: it is added to a game object located under the screen with a collider set as trigger. The only method activates the death screen and deactivate player.

You can see the results in the next youtube video:

Assets from: https://o-lobster.itch.io/platformmetroidvania-pixel-art-asset-pack
