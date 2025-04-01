# NoMoreMomoGameJam
 
# Retro Game Jam GDD Template

## Game Title
**No Mo Momo**

## Team Name
**Team Lebron**

## Team Members
- Imoudu Ibrahim (77825776)
- Gavin Ashworth (35669746)
- Jimi Ademola (92857770)

---

## Core Concept
Our game is inspired by **Frogger** but is themed around **Avatar: The Last Airbender**. Instead of playing as a frog, you play as **Momo**. Your journey takes you through the Air, Water, Earth, and Fire kingdoms, culminating in a face-off against **Azula** to save the Spirit World. Along the way, you'll learn the four elemental bending skills that enhance your survivability, aiming to become a mini-avatar and stop Azula.

---

## Core Gameplay

### Game Loop
- The player moves through levels by dodging obstacles, using **W, A, S, D** for movement.
- The goal is to reach the five homes at the end of each level.
- After completing each level, the player gains the bending skill associated with that level (Air, Water, Earth, Fire).
- Elemental power-ups act as skills on cooldowns.

#### Elemental Skills
- **Air**: Move twice as fast temporarily.
- **Water**: Replenish one life (Max 3 Lives).
- **Earth**: Provide a shield that absorbs hits (invincibility frames).
- **Fire**: Destroy obstacles in a 3x3 range (useful during the Azula boss fight).

**Feedback** is provided through sound effects, UI pop-ups, and visual changes in the sprite and environment.

---

## Player Controls

| Action         | Key    |
|-----------------|--------|
| Move Up         | W      |
| Move Down       | S      |
| Move Left       | A      |
| Move Right      | D      |
| Air Skill       | 7      |
| Water Skill     | 8      |
| Earth Skill     | 9      |
| Fire Skill      | 0      |

---

## Level & Progression

### Game Structure
- **Level 1: Air Temple (Easiest)**  
  - Platforms: Floating Rocks
  - Projectiles: None
  - Falling takes one life and resets to the start
- **Level 2: Water Tribe (Easy)**
  - Platforms: Floating Ice
  - Projectiles: Icicles
  - Falling takes one life and resets to the start
- **Level 3: Earth Kingdom/Ba Sing Se (Medium)**
  - Platforms: Rocks over pits
  - Projectiles: Flying Rocks/Boulders
  - Falling takes one life and resets to the start
- **Level 4: Fire Nation Capital (Hard)**
  - Platforms: Moving Platforms over a pit
  - Projectiles: Fireballs
  - Falling takes one life and resets to the start
- **Level 5: Azula Boss Fight (Hardest)**
  - Fire and Lightning attacks that escalate in difficulty as you destroy the crystals in the boss fight
  - Destroy Azula’s 4 healing crystals using Firebending
  - Falling takes one life and resets to the start

### Progression System
- As levels progress, projectiles increase in frequency and speed.
- Platform movement becomes faster.
- Bending skills are earned after each completed level.

---

## Scoring & Win/Loss Conditions

### Winning
- Complete all levels without losing all lives.
- Players can restart the final boss fight if they lose all lives (because the boss fight is fun).

### Score System
- Points are awarded for:
  - Vertical progress
  - Reaching homes
  - Defeating Azula
  - Bonus Peach on each map for 1000 points
  - Completing the game without losing lives
  - Finishing quickly


---

## Assets

### Models & Art
- Custom-made pixel art by **Gavin Ashworth**

### Sound & Music
- Sourced from:
  - [Zapsplat](https://www.zapsplat.com/) (Sound FX)

### Playthrough
- Regular Level
https://github.com/user-attachments/assets/37ec5209-e15d-441c-b89e-2c4b13f86671

- Boss Level



https://github.com/user-attachments/assets/a637c5ce-00eb-4a6a-9f4a-dbe12dfb6437



### UI
- Font: [Press Start 2P](https://www.fontspace.com/press-start-2p-font-f11591)

## Feedback / Playtesting
Feedback results can be found ![here](https://github.com/GavinAshworth/NoMoMomoGameJam/blob/main/FEEDBACK.md)
