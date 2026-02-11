# Doomotica-Puzzle

Doomotica Puzzle – Unity Prototype
Overview

Doomotica Puzzle is a gameplay prototype set in the Doomotica universe.
The player interacts with environmental objects to manipulate hazards and influence NPC behavior through a systemic puzzle structure.

The project demonstrates:

Finite State Machine architecture for NPC behavior

Hazard interaction system (poison, soap triggers, etc.)

Modular object interaction (carriable objects, moving furniture)

Centralized GameManager & BootManager structure

Audio and camera management systems

Core Systems
NPC Finite State Machines

Each character is driven by a custom FSM system:

CharacterFSM (base logic)

StateEnum (state definitions)

Level-specific behavior implementations

NPCs react dynamically to environmental changes and hazards.

Hazard System

Objects can become hazardous (e.g. PoisonableFood).
Triggers affect NPC states and gameplay outcome.

Interaction System

Objects categorized as:

CarriableObject

MovingForniture

GenericForniture

The system allows modular expansion of interactive elements.

Tech Stack

Unity (specify version)

C#

DOTween

Custom FSM architecture

How to Run

Open project in Unity (version X.X.X)

Open main scene (specify path)

Press Play

Current Status

Work in progress:

Audio polish

Animations refinement

UX feedback improvements

Build will be available on Itch once finalized.

Author

Edoardo Cirone
