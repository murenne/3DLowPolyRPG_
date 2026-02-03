# 3D Low Poly RPG

[English](./README.md) | [中文](./README_cn.md) | [日本語](./README_jp.md)

3Dロールプレイングゲーム（RPG）プロジェクトです。
ウェブ上のオープンソースプロジェクトをベースに、実務での開発経験を活かしてリファクタリングおよび機能拡張を行ったものです。

## Tech Stack

| カテゴリ | テクノロジー | バージョン | 説明 |
| :--- | :--- | :--- | :--- |
| **ゲームエンジン** | Unity | 2022.3.36f1 | コアゲーム開発エンジン |
| **レンダリング** | Universal Render Pipeline (URP) | 14.0.11 | 高性能レンダリングパイプライン |
| **グラフィックス** | Shader Graph | 内蔵 | ビジュアルシェーダー作成ツール |
| **グラフィックス** | Post Processing | 内蔵 | 視覚効果および後処理 |
| **入力** | Unity Input System | 1.7.0 | モダンな入力管理システム |
| **カメラ** | Cinemachine | 2.10.0 | 動的なカメラシステム |
| **アニメーション** | DOTween | - | トゥイーンアニメーションライブラリ |
| **アニメーション** | Timeline | 1.7.6 | カットシーンおよびシークエンス作成 |
| **AI/ナビゲーション** | NavMesh & AI Navigation | 1.1.5 | 敵のパスファインディングとナビゲーション |
| **レベルデザイン** | ProBuilder | 5.2.2 | 3Dモデリングおよびレベルデザイン |
| **レベルデザイン** | PolyBrush | 1.1.6 | 地形ペイントおよびオブジェクト配置 |
| **レベルデザイン** | ProGrids | 3.0.3-preview.6 | グリッドベースのオブジェクトスナップ |
| **データ** | Scriptable Objects | 内蔵 | データ駆動型ゲームデザイン |
| **データ** | JSON | 内蔵 | セーブ/ロードシステムのシリアライズ |

---

* [3D Low Poly RPG](#3d-low-poly-rpg)
    * [Tech Stack](#tech-stack)
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
1. 本プロジェクトは **Unity 2022.3.36f1** をベースに開発されています。Unityのバージョンが **2022.3.36f1** 以上であることを確認してください。
2. GitHubからプロジェクトを指定のフォルダにクローンし、Unityで開きます。
3. Unityエディタ上部の **Play** ボタンをクリックすると、プロジェクトが実行されます。

## Control
本プロジェクトは**キーボードとマウス**のみサポートしており、ゲームパッド（コントローラー）には対応していません。

| キー / 操作 | 機能説明 |
| :--- | :--- |
| `WASD` | プレイヤー移動 |
| `マウス移動` | カメラ操作 / 視点回転 |
| `左クリック` | 攻撃 / テレポート / 会話 / アイテムドラッグ / アイテム使用 |
| `右クリック` | 走る（スプリント） |
| `1~6` | アイテム使用（アクションバー） |
| `B` | バックパックの開閉 |
| `Esc` | メインメニューへ戻る |
| `J` | セーブデータを削除 |
| `K` | セーブ |
| `L` | ロード |

---

## Design Pattern
本プロジェクトでは、主に以下の2つのデザインパターンを使用しています。

### Singleton Pattern
必要なManager（管理クラス）にアクセスするために、ジェネリックなシングルトンパターンを採用しています。
ジェネリックシングルトンスクリプトを作成し、すべてのManagerスクリプトがこのクラスを継承しています。

### Observer Pattern
グローバルなイベント通知（ブロードキャスト）を行うためにインターフェースを利用しています。
例：プレイヤーが死亡した際、すべての敵に対して死亡イベントを通知します。

## Unity Basic Features
本プロジェクトでは、Unityの標準機能を多数活用しています。

### Settings
本プロジェクトは **URP (Universal Render Pipeline)** プロジェクトです。
Pipeline Settings や Lighting Settings を調整し、基礎的な設定を行っています。

### Terrain
Low Poly（ローポリ）スタイルのため、**ProBuilder** を使用して多頂点の平面を作成し、地面としています。
（注：ProGridsを使用することで、オブジェクト移動の数値化・スナップが可能です）。
また、**PolyBrush** を使用して地面の起伏を調整したり、植物や岩などのシーンオブジェクトをブラシで配置し、素早いシーン構築を行っています。

### Navigation
地面は **Walkable**（歩行可能）、木などの障害物は **Unwalkable**（歩行不可）に設定されています。
敵キャラクターは **NavMesh Agent** を利用して移動します。

### Events
マウスクリックイベントなど、Unityの各種イベント機能を使用しています。

### Post Processing
ポストプロセス（後処理）エフェクトを使用して、シーンのビジュアルを調整しています。

### Animation
プレイヤーの移動距離はアニメーションの **Root Motion** に基づいています。
**Blend Tree** を使用して、移動の各段階（歩き/走り/ジャンプ）のアニメーションを処理しています。
**Animation Layer**（Avatar Mask）を使用して、移動と攻撃の融合アニメーションを実現しています。

### Shader Graph
**Shader Graph** を使用して、以下の2つのシェーダーを作成しました。
* **Occlusion Shader:** プレイヤーが物体に遮られた際に、シルエットを表示します。
* **Water Shader:** カートゥーンスタイルの水面表現です。

### Scriptable Object
各種データを管理するために、多数の **Scriptable Object (SO)** ファイルを作成しています。
SOスクリプトはすべて末尾が `_SO` となっています。
主なSOファイルは以下の通りです。

| スクリプト名 | 説明 |
| :--- | :--- |
| **CharacterData_SO** | プレイヤーデータ |
| **AttackDataBase_SO** | 攻撃データ |
| **DialogueData_SO** | 会話データ |
| **InventoryData_SO** | 倉庫/インベントリデータ |
| **ItemData_SO** | アイテムデータ |
| **QuestData_SO** | クエストデータ |

---

## Character Customization System
汎用的な着せ替えシステムを実装しています。
ボーンのリターゲット（再割り当て）方式を利用して装備の変更を実現しています。
静的（Static）ファイルを利用して各部位のIDを保存し、シーンが変わっても同じ服装が生成されるように管理しています。

![Customization](./README_Images/Customization.gif)

---

## Portal System
ポータル（転送装置）は、同一シーン内の移動と、別シーンへの移動の両方をサポートしています。
プレイヤーがポータルの近くでクリックするとテレポートします。
シーン名やIDを指定することで、任意の場所へ移動可能です。

![Portal](./README_Images/Portal.gif)

---

## Save System
簡易的なセーブ/ロードシステムを実装しています。
**JSON** 形式を利用して、バックパックの中身やプレイヤーの位置などの各種データを保存・読み込みします。

![Save](./README_Images/Save.gif)

---

## Quest System
簡易的なクエスト受注・報告システムです。
NPCと会話することでクエストを受注できます（会話システム参照）。
受注後、クエストパネルにクエスト名が表示されます。
クエスト名をクリックすると、説明、達成条件、報酬などの詳細が表示されます。
クエストの進行状況はリアルタイムで更新され、達成すると報酬を獲得できます。

![Quest](./README_Images/Quest.gif)

---

## Inventory System
バックパック、アクションバー、装備スロットを含む倉庫システムです。
獲得したアイテムを保存し、アイコンと数量を表示します。
アイテムの位置は自由に移動・ドラッグ・交換が可能です。
アイコンにマウスを合わせると、詳細情報（ツールチップ）が表示されます。

![Inventory](./README_Images/Inventory.gif)

### Backpack
すべてのアイテムを収納可能。使用可能アイテムはダブルクリックで使用します。

### Action Bar
**使用可能アイテム（Usable Items）** のみ収納可能。ダブルクリック、またはキーボードの `1-6` キーで使用します。

### Equipment Slots
**装備可能アイテム（Equippable Items）** のみ収納可能。バックパックから対応するスロットへドラッグして装備します。

---

## Dialog System
Scriptable Objectファイルの内容を読み込んで再生し、キャラクターの顔アイコンを切り替える簡易会話システムです。
会話には選択肢がある場合とない場合があり、特定の選択肢を選ぶことでクエストを受注できます。

![Dialog](./README_Images/Dialog.gif)

### Dialog Text
テキスト内容はSOファイル内にあります。表示速度を制御することで**タイプライター風のエフェクト**を実現しています。
NPCの会話内容は手動で設定可能です。

### Dialog Portrait
アイコン画像はSOファイル内にあります。コード制御により、発言者に合わせて顔アイコンが切り替わります。

### Dialog Options
選択肢のある会話をサポートしており、選んだ選択肢に応じて異なるテキストが再生されます。

---

## Camera
本プロジェクトは **3Dサードパーソン（第三者視点）** ゲームです。
プレイヤーを追従するカメラには **Cinemachine FreeLook Camera** を使用しています。
マウス移動でカメラをプレイヤーの周囲で回転させることができ、カメラの向いている方向がプレイヤーの前進方向となります。

![Camera](./README_Images/Camera.gif)

---

## Character Progression
簡易的なキャラクター成長要素があります。

### Status
レベルアップに伴い、以下のステータスが徐々に増加します。

| 属性名 | 説明 |
| :--- | :--- |
| **ATK** | プレイヤー攻撃力 |
| **DEF** | プレイヤー防御力 |

### Level
簡易的なレベルアップモジュールです。
敵を倒すと経験値（EXP）を獲得し、必要経験値に達するとレベルアップします。
各レベルに必要な経験値は異なり、レベルアップ時のステータス上昇値も異なります。

---

## Item
アイテムは「使用可能（Usable）」と「装備可能（Equippable）」に分類されます。
キャラクターとアイテムの位置が重なると自動的に拾います。
マウスカーソルがアイテムに重なると、アイコンが変化し、拾えることを示します。

### Usable Items
バックパックまたはアクションバーに配置可能です。
ダブルクリック、またはアクションバーで `1-6` キーを押して使用します。

![UsableItems](./README_Images/UsableItems.gif)

### Equippable Items
バックパックに配置可能です。
プレイヤーパネルの対応する **装備スロット** にドラッグして装備する必要があります。
各武器には「通常攻撃」と「クリティカル攻撃」があり、それぞれ異なるアニメーションが設定されています。
デフォルトは通常攻撃ですが、クリティカル発生時には自動的にクリティカル攻撃アニメーションに切り替わります。

![EquippableItems](./README_Images/EquippableItems.gif)

---

## Enemy
4種類の敵（3種類の通常敵と1体のボス）が存在します。
敵の行動パターンには「定点待機」と「ランダム巡回」の2種類があります。
攻撃方法は敵ごとに異なります。
敵の攻撃でクリティカルが発生した場合：
* 攻撃アニメーションが変化します。
* プレイヤーは **ノックバック**（後退）します。

### Slime
追跡範囲に入ると自動的にプレイヤーを追いかけます。
全身が攻撃判定ポイントです。

![Slime](./README_Images/Slime.gif)

### Turtle Shell (Hedgehog)
追跡範囲に入ると自動的にプレイヤーを追いかけます。
全身が攻撃判定ポイントです。
スライムよりも攻撃射程が長いです。

![TurtleShell](./README_Images/TurtleShell.gif)

### Grunt
追跡範囲に入ると自動的にプレイヤーを追いかけます。
手に持っている武器に攻撃判定があります。

![Grunt](./README_Images/Grunt.gif)

### Golem
移動しないボスキャラクターです。
近接攻撃に加え、岩を投げる遠距離攻撃も行います。
左手と、投げられた岩に攻撃判定があります。

![Golem](./README_Images/Golem.gif)

---

## UI Bar
プレイヤーと敵のHPバーを表示します。

![UI](./README_Images/UI.png)

### Player HP Bar
画面左上にあり、リアルタイムで更新されます。HPが0になるとプレイヤーは死亡します。

### Player Exp Bar
画面左上にあり、リアルタイムで更新されます。
レベルアップすると、上部のレベル数値が変動します。

### Enemy HP Bar
デフォルトでは、敵がダメージを受けた時のみ表示されます。
「常に表示」するように設定することも可能です。

---

## Statement
このプロジェクトは全体として正常に動作します。
しかし、時間と労力が限られていたため、私が発見していないバグが存在する可能性があります。
もしバグを見つけた場合は、Issues（課題）に投稿してください。
時間があるときに対応させていただきます。
ありがとうございます。