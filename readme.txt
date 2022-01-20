Stealth Game


Teammitglieder:
- Lucas Engelbrecht (202956)
- Niklas Zimmer (202397)


Besonderheiten des Projekts:
* Nach dem Auschecken des Projekts, um Paid Assets zu laden:
   * git submodule init
   * git submodule update
* Assets/Scenes/MainScene öffnen für Demo-Level
* Assets/Scenes/TestScene öffnen für kleine Testumgebung


Herausforderungen:
* Third Person Controller (mit Cinemachine)
   * am Anfang sind wir auf einige Probleme gestoßen, da wir uns vorher noch nicht mit Cinemachine beschäftigt hatten
* Interaktion mit Objekten
   * Problem: bei einfachen Raycast von der Kamera aus, um Interaktionsobjekt zu erfassen, hat man zu wenig Spielraum
   * Lösung: OverlapSphere am Ende des Raycast; mit Objekten innerhalb dieser Sphere kann man interagieren
* Verhalten der Gegner
   * viele verschiedene Aktionen (Aufheben von Münze, Entfernen von Scherben)
   * Rücksetzen von Aktionen, falls Spieler in Sichtfeld (Stoppen von Umdrehen der Wache, Stoppen vom Folgen der Münze)
   * In diesem Bereich wurden Erfahrungen mit Behavior Trees gesammelt.
* Animationen
   * eine Herausforderung hierbei war das Timing der Animationen mit Animation Events
* Zusammenspiel verschiedener Systeme
   * In den Szenen gibt es verschiedene Systeme, wie Sound Objekte (geben ein Geräusch für eine bestimmte Zeit ab), Notifier Objekte (benachrichtigen den Spieler, wenn sie in seinem Blickfeld sind) und den Alarm durch Kameras oder den Hund.
   * Mit all diesen Systemen kann der Wächter interagieren und die Systeme können sich überlappen (neuer Sound, während der Wächter einem Sound folgt). Das muss alles im Verhalten des Wächters beachtet werden.
* Blender:
   * Hier war besonders der Export von Blender in Unity ein Problem, da Objekte teilweise falsch rotiert waren.
   * Rigging von Charakteren


Verwendete Assets:
* Verhalten der Kamera: https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html 
* Standard Assets: https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-for-unity-2018-4-32351 
* Sound für Topf: https://freesound.org/people/Clearwavsound/sounds/540339/
* Sound für Münze: https://www.storyblocks.com/audio/stock/coin-quarter-drop-bounce-rocks-bxlrthnlwbk0wxtpxs.html
* Sound für Hundebellen: https://freesound.org/people/ivolipa/sounds/328730/
* Sound für Kamera-Alarm: https://freesound.org/people/JSilverSound/sounds/381957/
* Gramophon-Musik: https://www.youtube.com/watch?v=UkXhvmc6PVU
* Hund: https://assetstore.unity.com/packages/3d/characters/animals/mammals/oscar-the-dog-quirky-series-119972
* Cinemachine: https://unity.com/de/unity/features/editor/art-and-design/cinemachine 
* Textur Holz:https://www.textures.com/download/3DScans0540/134744
* Textur Boden MainScene:https://3dtextures.me/2021/10/11/tiles-046/
* Animationen: https://www.mixamo.com/#/ 


Quellen:
* Blender zu Unity Export:https://www.youtube.com/watch?v=vrN9duEoA6g
* Sichtkegel der Wachen: https://www.youtube.com/watch?v=73Dc5JTCmKI
* Animation der Wachen: https://www.youtube.com/watch?v=blPglabGueM
* Third Person Controller mit Cinemachine: https://www.youtube.com/watch?v=4HpC--2iowE 
* Springen und Fallen des Spielers: https://www.youtube.com/watch?v=qITXjT9s9do 
* Umrandung (Shader): https://www.youtube.com/watch?v=SlTkBe4YNbo 
* Behavior Trees: https://www.youtube.com/watch?v=nKpM98I7PeM, https://www.youtube.com/watch?v=jhB_GFgS6S0 
* Kamera RenderTexture: https://www.youtube.com/watch?v=Goqe5IorfN0 
* Wurflinie der Münze: https://www.youtube.com/watch?v=6mJMmF5sLxk 
* Animations-Events für den Schritt-Sound: https://www.youtube.com/watch?v=Bnm8mzxnwP8
* Zerstörung der Töpfe: https://www.youtube.com/watch?v=p3mYcyxyBlw




Inspiration:
* https://www.reddit.com/r/gameideas/comments/4qp7mb/30_random_game_ideas_3/ (Idee #6)
* Hitman (Gameplay)
* Untitled Goose Game (Design)
* The Legend of Zelda: Phantom Hourglass (Gameplay)
* Splatoon (Gameplay)


Video-Link:
https://www.youtube.com/watch?v=SkaxRZ2lOTc


Video-Links der letzten Präsentation:
https://www.youtube.com/watch?v=fK61ishDOmI (Teil 1)
https://www.youtube.com/watch?v=rBZiP2j9dNE (Teil 2)


Projekt-Link:
https://github.com/zacklucas99/labor-games