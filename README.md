# SheepSleepScramble

[ゲームはこちら](https://unityroom.com/games/sheepsleepscramble)

初めて友人と作成したTPSのシステムをベースに少しゲーム制作知識の理解の整理も兼ねて作った短編ゲームです。エリアの防衛や射撃と近接攻撃で敵を倒す基本要素だけでなく、敵が落とすアイテムのリソース配分、コンボを維持することによって高まるスコアやプレイヤーの強化など、敵を倒す事や攻撃を当てることにプラスの要素を多く設けています。敵の狙いとなる拠点はランダムではなく状況によって切り替わり、プレイヤーが一か所に留まらず走り回って敵を攻撃したりエリアを防衛できるように工夫しました。

**プレイヤーの操作、ステータス、攻撃判定など**  
PivotControll.cs：カメラの位置  
Action3dCam.cs：カメラの位置  
上二つのスクリプトはボーンデジタル様から出版されている「Unityゲーム プログラミング・バイブル」より、自由に使用できるソースコードをお借りしたものをベースにしています。  
PlayerController.cs：プレイヤーの操作に関連するスクリプト  
PlayerStatus.cs：プレイヤーのステータス  
MeleeAssist.cs：近接攻撃の方向補正  
MeleeWeapon.cs：近接攻撃の辺り判定など  
MagicTrail.cs：射撃の軌跡エフェクト  
Recoil.cs：射撃攻撃の反動　[こちらの動画を参考にしています](https://youtu.be/geieixA4Mqc)


**マップ上のアイテム**  
PickItem.cs：アイテムの取得に関するスクリプト  
ImageLookAtCamera.cs：3D空間に配置する2D画像のビルボード  


**敵の挙動やステータス、生成など**  
EnemyMovement.cs：敵の移動  
EnemyStatus.cs：敵ステータスの元となるスクリプタブルオブジェクト  
MakaiHitsujiStatus.cs：各種類の敵のステータス  
EnemyCommander.cs：敵の全体の動きを制御するスクリプト  
EnemyGenerator.cs：敵を生成するスクリプト  


**マップ上のオブジェクトなど**  
AreaScript.cs：A,B,C各エリア操作や処理に関するスクリプト  
GrillController.cs：グリルの操作や処理に関するスクリプト  
EffectGenerator.cs：エフェクトを表示するスクリプト  
NPCController.cs：演出用人間キャラの処理に関するスクリプト  


**ゲーム全体のルールやスコア管理など**  
ResultManager.cs：クリア時、ゲームオーバー時のスコア表示用スクリプト  
ScoreDirector.cs：攻撃ヒット数やキル数の計測と表示UI、それによるボーナス効果処理などのスクリプト  
TitleManager.cs：タイトルシーンのボタン操作全般  
WaveController.cs：ウェーブや敵を生成する数の進行管理  
ShakeObjectsByDOTween.cs：UIやカメラを揺らすスクリプト  

SoundManagerは[こちらの記事]( https://i-school.memo.wiki/d/SoundManager%A4%C7%A5%B2%A1%BC%A5%E0%C6%E2%A4%CE%B2%BB%B8%BB%A4%F2%B4%C9%CD%FD%A4%B9%A4%EB
)からコードをお借りしました。
