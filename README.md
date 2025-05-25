# 🚀 Saturn Sprinter

**Saturn Sprinter** is a VR infinite runner game developed as part of the *Virtual Reality* course at EPFL.

## 🪐 Game Concept

Set in the mesmerizing rings of Saturn, *Saturn Sprinter* challenges players to:
- Dash through an endless space track.
- Dodge fast-approaching obstacles.
- Use the items and power-ups to survive as long as possible.

Inspired by classics like **Subway Surfers** and **Temple Run**, this game brings high-speed action into a thrilling outer space VR experience.

## 💻 Scripts Details

Any script of this project has been copy-pasted or used as-is from an external source. Most of them have been entirely created by our team, even the biggest like Lanes or GameManager, iteratively. We obviously leveraged gen AI and forums at the very beginning of the project to get more comfortable and then to tackle the difficulties we encountered when implementing non-easy interactions or features but after a few weeks of practicing, we were able to add features and develop the project more efficiently by ourselves than by using AI. As an example, ArmHoverController was adapted from gen AI as it was the first feature we implemented and tracking arms movements wasn't obvious but it was not just copy-pasted, it was inspired from and then adapted to more precisely suit our expectations. The GameManager singleton idea in itself was also suggested by AI, to have a way to centralize high-level actions and communicate between independant elements but then all the code in this file is from us, as we understood how useful this object was.

- The **Hovering** custom feature mostly resides into the ArmHoverController script but it also checks the inGame property of the GameManager script and communicate its hoverCharge property to the GameUI script through its public method SetHoverBarValue.

- The **Wall Target Shooting** feature logic can be found in both the PuzzleTarget scrip and the Wall script. The PuzzleTarget script only handles the state of its game object by exposing a public method SetTargetActive (used in LaserGun when there is a collision with the laser) and the public property isActivated. And the Wall script checks this public state of all of its target children to trigger its destruction or not.

- The **last custom feature** ...

## 🎮 Platform & Technology

- **Engine**: Unity (XR Interaction Toolkit)
- **Platform**: VR (targeted at Meta Quest)
- **Language**: C#

## 👾 Team

Developed by:
- Rémi Offner
- Csaba Beleznai
- Elias Panner