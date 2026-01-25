# 3D Low Poly RPG

[English](./README.md) | [中文](./README_cn.md) | [日本語](./README_jp.md)

This is a 3D Role-Playing Game project.
It is based on open-source web projects and has been refactored and enhanced using professional experience gained from actual game development.

* [3D Low Poly RPG](#3d-low-poly-rpg)
    * [Getting Started](#getting-started)
    * [Control](#control)
    * [Design Pattern](#design-pattern)
        * [Singleton Pattern](#singleton-pattern)
        * [Observer Pattern](#observer-pattern)
    * [Unity Basic Features](#unity-basic-features)
        * [Settings](#settings)
        * [Terrain](#terrain)
        * [Navigation](#navigation)
        * [Events](#events)
        * [Post Processing](#post-processing)
        * [Animation](#animation)
        * [Shader Graph](#shader-graph)
        * [Scriptable Object](#scriptable-object)
    * [Character Customization System](#character-customization-system)
    * [Portal System](#portal-system)
    * [Save System](#save-system)
    * [Quest System](#quest-system)
    * [Inventory System](#inventory-system)
        * [Backpack](#backpack)
        * [Action Bar](#action-bar)
        * [Equipment Slots](#equipment-slots)
    * [Dialog System](#dialog-system)
        * [Dialog Text](#dialog-text)
        * [Dialog Portrait](#dialog-portrait)
        * [Dialog Options](#dialog-options)
    * [Camera](#camera)
    * [Character Progression](#character-progression)
        * [Status](#status)
        * [Level](#level)
    * [Item](#item)
        * [Usable Items](#usable-items)
        * [Equippable Items](#equippable-items)
    * [Enemy](#enemy)
        * [Slime](#slime)
        * [Turtle Shell](#turtle-shell)
        * [Grunt](#grunt)
        * [Golem](#golem)
    * [UI Bar](#ui-bar)
        * [Player HP Bar](#player-hp-bar)
        * [Player Exp Bar](#player-exp-bar)
        * [Enemy HP Bar](#enemy-hp-bar)
    * [Statement](#statement)
## Getting Started
1. This project is developed based on **Unity 2022.3.36f1**. Please ensure your Unity version is **2022.3.36f1** or higher.
2. Clone the project from GitHub to your specified folder and open it with Unity.
3. Click the **Play** button at the top of Unity to run the project.

## Control
This project only supports **Keyboard & Mouse** control. Gamepads are not supported.

| Key / Input | Action Function |
| :--- | :--- |
| `WASD` | Player Movement |
| `Mouse Move` | Camera Control / Rotate View |
| `Left Click` | Attack / Teleport / Interact / Drag Item / Use Item |
| `Right Click` | Sprint / Run |
| `1~6` | Use Item (Action Bar) |
| `B` | Open/Close Backpack |
| `Esc` | Return to Main Menu |
| `J` | Delete Save File |
| `K` | Save Game |
| `L` | Load Game |

---

## Design Pattern
This project primarily uses the following two design patterns:

### Singleton Pattern
A generic Singleton pattern is used to access required Managers.
A generic singleton script was created, and all Manager scripts inherit from this generic class.

### Observer Pattern
Interfaces are used for global broadcasting.
For example, when the player dies, an event is broadcast to all enemies.

## Unity Basic Features
This project utilizes various built-in Unity features:

### Settings
This is a **URP (Universal Render Pipeline)** project.
Basic configurations were made by adjusting Pipeline Settings and Lighting Settings.

### Terrain
Due to the Low Poly style, **ProBuilder** was used to create multi-vertex planes for the ground.
(Note: ProGrids can be used to quantize object movement).
**PolyBrush** was used to adjust terrain height and paint scene objects (plants/rocks) onto the ground for rapid scene construction.

### Navigation
The ground is marked as **Walkable**, while objects like trees are **Unwalkable**.
Enemies utilize the **NavMesh Agent** for movement.

### Events
Various Unity Events are used, such as mouse click events.

### Post Processing
Post-processing effects are applied to enhance the scene's visuals.

### Animation
Player movement distance is based on Animation **Root Motion**.
**Blend Trees** are used to handle different movement stages (Walk/Run/Jump).
**Animation Layers** (Avatar Masks) are used to blend movement and attack animations.

### Shader Graph
Two custom shaders were created using **Shader Graph**:
* **Occlusion Shader:** Displays the player's silhouette when occluded by objects.
* **Water Shader:** A cartoon-style water effect.

### Scriptable Object
Numerous **Scriptable Object (SO)** files are created to manage various data types.
All SO scripts end with `_SO`.
Main SO files include:

| Script Name | Explanation |
| :--- | :--- |
| **CharacterData_SO** | Player Data |
| **AttackDataBase_SO** | Attack Data |
| **DialogueData_SO** | Dialogue Data |
| **InventoryData_SO** | Warehouse/Inventory Data |
| **ItemData_SO** | Item Data |
| **QuestData_SO** | Quest Data |

---

## Character Customization System
A general-purpose clothing/equipment system is implemented.
It uses **Bone Retargeting** to swap equipment meshes.
Static files are used to save the IDs of body parts to ensure the same outfit is generated across different scenes.

![Customization](./README_Images/Customization.gif)

---

## Portal System
Portals support both same-scene and cross-scene teleportation.
When the player is near a portal and clicks on it, they are teleported.
By specifying the scene/ID, the player can be transported to a specific location.

![Portal](./README_Images/Portal.gif)

---

## Save System
A simple Save/Load system is implemented.
It uses **JSON** to save/read various data, including inventory contents and player position.

![Save](./README_Images/Save.gif)

---

## Quest System
A simple quest acceptance/submission system.
Players can accept quests when talking to NPCs (see Dialog System).
Once accepted, the quest name appears in the quest panel.
Clicking the quest name reveals details, including description, requirements, and rewards.
Quest progress updates in real-time.
Rewards are granted upon completion.

![Quest](./README_Images/Quest.gif)

---

## Inventory System
Includes a Backpack, Action Bar, and Equipment Slots.
Stores items obtained by the player, displaying icons and quantities.
Players can freely move, drag, and swap item positions.
Hovering over an item icon displays detailed information (Tooltips).

![Inventory](./README_Images/Inventory.gif)

### Backpack
Stores all items. Double-click usable items to use them.

### Action Bar
Can only store **Usable Items**. Use by double-clicking or pressing `1-6` on the keyboard.

### Equipment Slots
Can only store **Equippable Items**. Items must be dragged from the backpack into the slot.

---

## Dialog System
A simple dialogue system that reads and plays content from Scriptable Object files and switches character portraits.
Dialogues can be linear or have branching options. Selecting specific options can trigger quests.

![Dialog](./README_Images/Dialog.gif)

### Dialog Text
Text content is stored in SO files. A **Typewriter effect** is implemented by controlling text speed.
NPC dialogue content is manually configurable.

### Dialog Portrait
Icon content is stored in SO files. Character portraits switch dynamically based on code control.

### Dialog Options
Dialogues support branching options. Different text flows play based on the selected option.

---

## Camera
This is a **3D Third-Person** game.
The camera following the player is a **Cinemachine FreeLook Camera**.
Moving the mouse rotates the camera around the player.
The player moves in the direction the camera is facing.

![Camera](./README_Images/Camera.gif)

---

## Character Progression
Simple character stat processing.

### Status
Attributes increase gradually with levels.

| Attribute | Explanation |
| :--- | :--- |
| **ATK** | Player Attack Power |
| **DEF** | Player Defense Power |

### Level
A simple leveling module.
Defeating enemies grants Experience Points (EXP). The player levels up upon reaching the required EXP.
The EXP required varies for each level.
Stat increases vary per level up.

---

## Item
Items are categorized into **Usable** and **Equippable**.
Items are automatically picked up when the character overlaps with them.
When the mouse hovers over an item, the cursor changes, indicating interaction is possible.

### Usable Items
Can exist in the Backpack or Action Bar.
Used by double-clicking or pressing `1-6` (Action Bar only).

![UsableItems](./README_Images/UsableItems.gif)

### Equippable Items
Can exist in the Backpack.
Must be dragged to the corresponding **Equipment Slot** on the player panel to equip.
Each weapon has a Normal Attack and a Critical Attack, each with different animations.
It defaults to Normal Attack; triggering a critical hit automatically switches to the Critical Animation.

![EquippableItems](./README_Images/EquippableItems.gif)

---

## Enemy
There are four types of enemies: three normal enemies and one boss.
Enemies have two states: **Stationary** (Idle) or **Random Patrol**.
Each enemy has a unique attack pattern.
When an enemy triggers a critical hit:
* The attack animation changes.
* The player suffers **Knockback**.

### Slime
Automatically chases the player when entering its tracking range.
The entire body acts as an attack hitbox.

![Slime](./README_Images/Slime.gif)

### Turtle Shell (Hedgehog)
Automatically chases the player when entering its tracking range.
The entire body acts as an attack hitbox.
Has a longer attack range than the Slime.

![TurtleShell](./README_Images/TurtleShell.gif)

### Grunt
Automatically chases the player when entering its tracking range.
The attack hitbox is located on its weapon.

![Grunt](./README_Images/Grunt.gif)

### Golem
A stationary Boss.
Performs melee attacks and throws rocks for ranged attacks.
Hitboxes are located on its left hand and the thrown rocks.

![Golem](./README_Images/Golem.gif)

---

## UI Bar
Displays HP Bars for both the player and enemies.

![UI](./README_Images/UI.png)

### Player HP Bar
Located at the top-left. Updates real-time. Player dies when HP reaches 0.

### Player Exp Bar
Located at the top-left. Updates real-time.
The level number at the top updates upon leveling up.

### Enemy HP Bar
Default behavior: Displays only when the enemy takes damage.
Can be configured to "Always Show".

---

## Statement
The project runs normally as a whole.
However, due to limited time and energy, there may be bugs that I haven't discovered yet.
If you encounter any bugs, please submit an issue.
I will address them when I have time.
Thank you!