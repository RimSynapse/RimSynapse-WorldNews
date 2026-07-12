# DESIGN: RimSynapse WorldNews & Storyteller Sunset

## 1. The Sunset of RimSynapse-StoryTeller
The `RimSynapse-StoryTeller` module is being officially deprecated. Its responsibilities are being strictly divided and migrated to their logically pure architectural homes:

### A. Core Hooks (Migrating to `RimSynapse-Core`)
The fundamental hooks that intercept the vanilla RimWorld Storyteller (e.g., `StorytellerComp_Synapse.cs`, `StorytellerCompProperties_Synapse.cs`, and the `TriggerPacingAdjustment` logic) act as an intelligence proxy. Because `RimSynapse-Core` is the master context tracker, these base-level intercepts belong natively in the Core module.

### B. Faction Lore (Migrating to `RimSynapse-Factions`)
The generation of dynamic faction backstories (`SynapseFactionEvaluator.cs` and `FactionStoryTracker.cs`) will be moved directly into the `RimSynapse-Factions` module. Factions handles base generation, faction cloning, and leader backstories, making it the definitive source of truth for faction lore.

---

## 2. The Vision for RimSynapse-WorldNews
With the storyteller intercepts moved to Core, this new module—**RimSynapse-WorldNews**—exists to make the RimWorld feel alive, reactive, and historically grounded. It acts as the local journalist and gossip mill for the planet.

### Core Feature 1: The Event Intercept
`RimSynapse-WorldNews` will deploy a Harmony Patch on `Verse.LetterStack.ReceiveLetter`. In RimWorld, every significant player-facing event (Raids, Manhunter Packs, Toxic Fallout, Trade Ships, Quests) generates a "Letter." By intercepting these letters, we capture the exact narrative beats the player is experiencing without having to decipher abstract game ticks. These letters are cached in an `unpublishedEvents` queue.

### Core Feature 2: The Synapse Newspaper System
Once the `unpublishedEvents` queue reaches a threshold (e.g., 3 to 5 events), the module triggers the `SynapseNewspaperGenerator`. 
- **The Prompt:** The LLM is instructed to act as a frontier journalist. It takes the raw events and synthesizes them into a cohesive narrative issue.
- **The Output:** A JSON object containing an overarching dramatic `Headline`, the `Date`, and 3 distinct `Stories` featuring rich flavor text.

### Core Feature 3: Asymmetric Layout UI
When a Newspaper Issue is published, the player receives a special notification letter. Clicking it opens a custom `Dialog_Newspaper`.
- **The Design:** An asymmetric, modern layout. To maximize visual engagement while keeping content balanced, the UI allocates equal square footage to all stories using a dynamic grid.
  - *Example Layout:* A large, prominent vertical column on the left containing the main event (33% of screen space), juxtaposed with two smaller, horizontally stacked blocks on the right (each taking ~33% of space) for minor local gossip or secondary events.

### Core Feature 4: Knowledge & Rumor Propagation (Future Expansion)
Taking the old Storyteller's `FactionRelationshipTracker`, the WorldNews module will track how information travels across the planet. When the player raids an outpost, that knowledge spreads via trader caravans. These global political shifts will occasionally feature in the "World Affairs" section of the local Newspaper.
