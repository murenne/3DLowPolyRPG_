# 3D Low Poly RPG

[English](./README.md) | [中文](./README_cn.md) | [日本語](./README_jp.md)

这是一个3D角色扮演类游戏项目  
以网络开源项目为基础，加上工作中参与过的项目经验，重构出来的项目

<!--ts-->
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
        * [Hedgehog](#hedgehog)
        * [Grunt](#grunt)
        * [Golem](#golem)
    * [UI Bar](#ui-bar)
        * [Player HP Bar](#player-hp-bar)
        * [Player Exp Bar](#player-exp-bar)
        * [Enemy HP Bar](#enemy-hp-bar)
    * [Statement](#statement)
<!--te-->

## Getting Started
1. 该项目基于Unity 2022.3.36f1开发，请确保你的Unity版本为2022.3.36f1及以上  
2. 从Github拷贝该项目至你指定的文件夹，用Unity打开  
3. 点击Unity上面的开始按钮，即可运行该项目  

## Control
该项目只支持键鼠控制，不支持手柄操作

| 按键      | 说明                     |
| ----------| ------------------------ |
| `WASD`       | 玩家移动                |
| `鼠标移动`       | 转换视角                |
| `鼠标左键`       | 攻击/传送/对话/拖动物品/使用物品              |
| `鼠标右键`       | 跑步                |
| `1~6`       | 使用物品                |
| `B`       | 打开/关闭背包                |
| `Esc`       | 退出到主菜单                |
| `J`       | 删除存档                |
| `K`       | 保存存档                |
| `L`       | 读取存档                |

---

## Design Pattern
本项目主要用了以下两种设计模式

### Singleton Pattern
该项目中，采用单例模式来取得需要的manager  
故创建了一个泛型单例脚本，所有的manager都继承自该泛型单例脚本  
以下为主要的manager  

### Observer Pattern
利用接口进行全局广播  
当玩家死亡时，会对所有的敌人进行广播  

## Unity Basic Features
本项目用了以下多种unity自带的功能  

### Settings
本项目为URP项目  
通过调整pipline settings和lighting settings，对项目进行了一些基础的设置  

### Terrain
该项目为LowPoly风格，所以使用了Probuilder来创建多顶点的平面作为地面  
（注：可使用ProGrids来量化移动物体的距离）  
并通过使用PolyBrush调整地面高低，并将植物/石头等场景物体刷到地面上  
从而快速建立场景

### Navigation
该项目中，地面为walkable，而树木等物体为unwalkable  
敌人均利用Nav mesh agent来进行移动  

### Events
该项目中，运用了多种event  
例如鼠标点击事件等  

### Post Processing
该项目中，利用后处理效果对场景的一些设置做了处理  

### Animation
该项目中，玩家的移动距离基于animaition的 root motion  
利用blend tree来处理不同运动阶段（走路/跑步/跳跃）的动画  
利用layer来处理移动和攻击的融合动画  

### Shader Graph
该项目中，利用了shaderGraph制作了两个shader  
occlusion shader：当玩家被遮挡时会表示玩家的轮廓  
water shader：卡通风格的水体  

### Scriptable Object
该项目中，创建了诸多scriptable object文件，用来管理各类数据  
scriptable object的脚本均以`_SO`结尾  
以下为主要的SO文件  

| 脚本名     | 解释                     |
| ----------| ------------------------ |
|CharacterData_SO        | 玩家数据               |
|AttackDataBase_SO        | 攻击数据                |
|DialogueData_SO        | 对话数据            |
|InventoryData_SO        | 仓库数据            |
|ItemData_SO        | 物品数据            |
|QuestData_SO        | 任务数据            |

---

## Character Customization System
该项目实现了一个通用的换装系统  
利用重定向骨骼的方式实现换装  
利用static 文件来保存各部位编号  
以便在不同场景中生成同一套服装  

![Customization](./README_Images/Customization.gif)

---

## Portal System
传送门分为同场景传送和异场景传送  
当玩家位于传送门附近，并用鼠标点击传送门时，即可传送  
通过指定场景/编号，就可以传送到指定地点  

![Portal](./README_Images/Portal.gif)

---

## Save System
该项目实现了一个非常简易的保存/读取系统  
利用json来保存/读取各类数据  
包括背包内容，玩家位置等  

![Save](./README_Images/Save.gif)

---

## Quest System
该项目实现了一个简易的交接任务的系统  
玩家可以在和NPC对话时接到任务，详见对话系统  
接到任务后，任务面板会有当前接到的所有的任务名  
点击任务名会看到任务细节，包括说明，要求，奖励等  
任务进度会实时更新  
当任务完成后会获取任务奖励  

![Quest](./README_Images/Quest.gif)

---

## Inventory System
该项目实现了一个简易的仓库系统，包括背包，快捷栏，装备栏  
仓库可以保存玩家获得各种物品，显示物品icon和数量  
玩家可以自由移动/拖拽/交换物品位置  
鼠标停留在物品icon上时，会显示物品的详细信息  

![Inventory](./README_Images/Inventory.gif)

### Backpack
可存储所有的物品，双击可用物品即可使用  

### Action Bar
只能存储可使用的物品，双击或按键盘上的1-6即可使用  

### Equipment Slots
只能存储可装备的物品，需要将物品从背包中拖拽放入  

---

## Dialog System
该项目有一个简易的对话系统，
可根据Scriptable Object文件的内容读取并播放对话内容，并切换对话人物的头像  
对话分为有选项和无选项，且选择特定选项中会接到任务，详见任务说明  

![Dialog](./README_Images/Dialog.gif)

### Dialog Text
文本内容位于Scriptable Object文件内，通过控制速度实现了类似打字机的效果  
可手动配置NPC的对话内容  

### Dialog Portrait
图标内容位于Scriptable Object文件内，通过控制人物的代码实现了切换头像的功能  

### Dialog Options
对话分为有选项和无选项，有选项的对话会依据选择的内容播放不同的文本内容  

---

## Camera
该项目是一个3D第三人称视角的游戏  
跟随玩家的相机为cinemachine中的freelook camera  
可以通过移动鼠标让摄像机绕着玩家旋转  
玩家的前进方向为摄像机的观察方向  

![Camera](./README_Images/Camera.gif)

---

## Character Progression 
本项目有个简易的人物处理  

### Status
人物有两种属性，随着升级，属性值会逐渐增加  

| 属性名     | 解释                     |
| ----------| ------------------------ |
| ATK       | 玩家攻击力                |
| DEF       | 玩家防御力                |


### Level
该项目有一个简易的升级模块  
击败的敌人会获得经验值，当经验值打到要求后会升级  
每一级所需的经验值也不一样  
每次升级，各属性新增的值不一样  

---

## Item
该项目分为可用道具和可装备道具  
当人物位置和物品位置重合时，会自动拾取活自动使用物品  
当鼠标位置和物品位置重合时，鼠标的icon会变成的样子，意味着该物体可以被拾取  

### Usable Items
可使用道具可存在于背包中或者action bar中  
在背包或action bar中，可双击鼠标使用  
此外，在actionbar中也可以按下键盘的1-6来使用  

![UsableItems](./README_Images/UsableItems.gif)

### Equippable Items
可装备道具可存在于背包中或者action bar中  
需要将其拖拽到玩家面板的对应格子中才能装备  
每种武器有普通攻击和暴击攻击，分别对应不同animaiton  
默认为普通攻击，当触发暴击时，自动切换到暴击的animation  

![EquippableItems](./README_Images/EquippableItems.gif)

---

## Enemy
该项目有四种敌人  
包括三种普通敌人和一个boss  
敌人分为站桩和随机巡逻两种方式  
每种敌人的攻击方式都不一样  
每当敌人的攻击发生暴击时  
攻击动画会改变  
玩家会被击退  

### Slime
当玩家进入slime的追踪范围中时，它会自动追踪玩家  
slime全身都是攻击判定点  

![Slime](./README_Images/Slime.gif)

### TurtleShell
当玩家进入刺猬的追踪范围中时，它会自动追踪玩家  
刺猬全身都是攻击判定点  
不同于slime，他的攻击距离会长一些  

![TurtleShell](./README_Images/TurtleShell.gif)

### Grunt
当玩家进入grunt的追踪范围中时，grunt会自动追踪玩家  
grunt的攻击判定点在其拿的武器上     

![Grunt](./README_Images/Grunt.gif)

### Golem
不会移动的boss  
会近战攻击，也会远程丢石头  
攻击判定点在左手和丢出来的石头上  

![Golem](./README_Images/Golem.gif)

---

## UI Bar
该项目中，设置了人物和敌人的HP Bar  

![UI](./README_Images/UI.png)

### Player HP Bar
玩家血量UI位于画面左上角，会实时更新玩家血量，血量为0时即玩家死亡  

### Player Exp Bar
玩家血量UI位于画面左上角，会实时更新玩家经验值  
当升级后，位于最上方的等级数字会变动  

### Enemy HP Bar
敌人的血条UI默认会在敌人受伤时显示相应的血量  
可以设置成总是显示  

---

## Statement
该项目整体能正常运行  
但因为时间精力有限，也许会存在一些我没发现的bug  
如果遇到bug，请提交issues  
我会在有空的时候处理  
谢谢  