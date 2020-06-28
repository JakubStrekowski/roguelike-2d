# Dester's soul

Dester's soul is a 2d roguelike game made with Unity engine. Its core game mechanics were ported from my another project: The Kat'zu Dungeon, which was made as a console ASCII game.
Link to The Kat'zu Dungeon text-game repository: https://github.com/JakubStrekowski/The-Kat-zu-Dunegon-Text

The goal of this game is to gain as high score as possible. Score is measured by collected gold and killed enemies. Each level has a staircase that leads to another level.

Controls:
* arrows - movement
* 1-6 - using collected item
* 'space' - pass a turn


 The game uses A* pathfinding algorithm to generate corridors between rooms and for enemies to follow player. Enemies intelligence is organized in state machine pattern, so their behaviour is easy to expand.

