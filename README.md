# Weth the Discovery

A treasure hunter who just so happens to arm himself with a heavy machine gun.

## Changalog

### Release 1.1.0

* **(1.1.0)**: ASplitshot compatibility fix (Shouldn't be a gameplay difference.), Relic redo, artifact buffs, Goodies (starter) slight nerf (and correct call to artifact method)
  * **dev.1**: New relic system!
  * **dev.2*: Artifact buffs + New Artifact
  * **dev.3*:  Goodies changes
  * **dev.4*: Splitshot code stuff
  * **dev.5*: Bonus hidden relics

### Release 1.0.0

* **(1.0.1)**: DialogueMachine 0.18: Fixed cross-mod dialogue implementation
* **(1.0.0)**: Release! ID obtained

### BETA 3: Balancing and adjustments + Final changes

* **(0.3.47)**: Scatter Trash redo, base shoots three asteroids (10101), A upgrades the sides to giants, B shoots four asteroids (11011), fixed small font leakage from unused library, and applied correct tints to pierce cards
* **(0.3.46)**: Drive collector GONE! Goodies offering no longer duplicate options
* **(0.3.45)**: Fixed Feral not doing the feral animation bit and adjusted feral animation
* **(0.3.44)**: Splitshot not actually setting the correct values fixed, added a little bit of flavour to Weth Illeana duo interaction, fixed animations (intense, sly, angry eyes, some past stuff)
* **(0.3.43)**: Dialogue polish (+ new dialogue), Space Relic description clarifications, Splitshot pierce icon updated, splitshot midrow mechanic patches (midrow hit animation + fixed issue where some artifacts that count shots fired didn't count splitshots that hit midrow), Disabler A costs 1 and doesn't exhaust
* **(0.3.42)**: Weth gets actual artifacts?!
  * Disabler:
    * Base: Cost back to 1
    * A: Now copies Base but costs 0
    * B: Now copies Base but retains
* **(0.3.41)**: Bloom, Unstoppable Force, Disabler balance changes, mirage blast animation slight change
  * Bloom: Added minidrive across the board
  * Unstoppable B: Now does two 0 damage pierce attacks instead of a single 1 damage pierce and is now coloured RED
  * Disabler:
    * Base: Cost lowered to 0, now exhausts
    * A: original B
    * B: Brittle instead of weaken, now also fleeting
    * A & B: Sends new Disabler to Exhaust pile instead of Discard pile
  * Mirage Blast: Teleports only on the second movement instead of all three
* **(0.3.40)**: Relic code cleanup
* **(0.3.39)**: Fixed issue with pulsedrive not being handled in Focus relic and fixed Focus offering the same relic twice if you own that relic
* **(0.3.38)**: On second thought, having the midi cards isn't a great idea... also addressed one of the sprites not being registered properly
* **(0.3.37)**: Glow adjustment (less showy more subtle) & reduced file bloat (by using ogg instead o wavs)
* **(0.3.36)**: Glow cards! (and basically overhaul of the Weth rarity thing (three general rarity classes all the cards inherit from, with extra render taking care of rarity border and shine.))
* **(0.3.35)**: Removed Mine goodie and replaced with Bubble goodie, Buffed HiddenGem, glow borders
* **(0.3.34)**: New SpaceRelic 2: Split it into four things! Also fixed Terminus: Milestones doing weird math (but arriving to the correct conclusion)
* **(0.3.33)**: Changed all turquoise crystal sprites to time crystals to match themeing and changed Weth's frame such that each zone advances the crystal on the frame, and organised sprite folder and glowy crystal overlay
* **(0.3.32)**: Added description for an upcoming duo and added a reply dialogue towards Illeana. Also added bang sfx to memory 1
* **(0.3.31)**: Splitshot aligns better with Attacks with pierce vs bubble behaviour
* **(0.3.30)**: Memories completed yes & Extreme Violence B is all pierce and splitshots and reversed
* **(0.3.29)**: Completed Memories and 1st bg
* **(0.3.28)**: Sprites?! + mini sprite fix (lacked earring) + CrisisCall sending cards in the wrong direction + silenced logs + Redundancy in artifact removal due to some events not letting artifact get removed
* **(0.3.27)**: Oops! Memory! + polished SpaceRelic... oh god the sprites I have to make...
* **(0.3.26)**: DM0.17: Uses preprocessor debug instaed && release version, Feral now does 2 damage.
* **(0.3.25)**: Missing period in a description, more artifact dialogue, Feral drive potential reduced but base damage kept, DialogueMachine 0.16: Made the harmless warnings shut up and moved dialogue validators after mod existence checks
* **(0.3.24)**: Cancelled DbD and Sodapop, instead have Feral and Mirage Blast
* **(0.3.23)**: DialogueMachine fix (0.15) (wasn't editing switchsays correctly) + dialogue lines "who" part is also checked for invalid characters + Warnings of failed edits + Duo artifacts are now excluded from dialogues if Duo is not enabled
* **(0.3.22)**: Fuck bayblast. Gone. Removed. Dead. Replaced by Death-By-Destruction and Soda Pop cards
* **(0.3.21)**: Fixed giant asteroid and mega asteroid icons
* **(0.3.20)**: Fixed rendering not applied correctly
* **(0.3.19)**: Fixed splitshot not applying flux + sfx attack change
* **(0.3.18)**: Weth Card Rework 1 + Goodie giver RNG rework
  * Splitshot: Buffed damage +1 across all upgrades
  * Cargo Blaster: Uses BayblastV2 and B is a dual
  * Disabler: Rarity reduced to Common
  * Compensate: Reduced overall damage, and only last hit deals damage.
  * Double Tap: Deals damage only on the last hit
  * Spreadshot: Renamed to Strewshot and buffed to match Splitshot's power level.
  * Discovery: Instead of giving 1 random card, offers a choice.
  * NEW Feral Blast: Replaces Double Blast, now an X energy card that is a midrow destroyer.
  * Extreme Violence: Cost +1, A is now B, A is reduced cost Base.
  * Double Blast: Removed.
  * All rock dispensing card's art tint is brighter
  * Most of the cardart has been brightened
* **(0.3.17)**: Bayblast Rework & Weth goodie giver now allows it to be offered as a card choice
* **(0.3.16)**: Dialogue bugfixes and Piercing Splitshot now forks into three instead of just going through
* **(0.3.15)**: Fixed HiddenGem not giving permanent exhausts and PowerCrystals not powering.
* **(0.3.14)**: Forgot that ResidualShot should also proc for Peri.
* **(0.3.13)**: Fixed DialogueMachine's bugs (0.14), updated required api version stuff in nickel.json
* **(0.3.12)**: New version of DialogueMachine (0.13) that allows you to override fields of existing dialogue
* **(0.3.11)**: Finished vanilla duos AND sprites, redid some animation sprites
* **(0.3.10)**: More duos!
* **(0.3.9)**: Readded gone artifacts as duos
* **(0.3.8)**: Removed a few artifacts (duo time) and changed Artifact Excursion and Relic Jaunt to have Terminus type, reworked Terminus: Excursion once again. New sprites for the artifacts
* **(0.3.7)**: Uncommon goodies are infinitely available if either Treasure Hunter or Seeker is not present
* **(0.3.6)**: Fixed Weth error due to order of definition
* **(0.3.5)**: Forgot Weth is missing thing dialogue and adjusted plead animation
* **(0.3.4)**: Fixed DM's edge case
* **(0.3.3)**: Dialogue machine modded support
* **(0.3.2)**: Relic Jaunt added to boss artifacts as an alternative to Artifact Excursion and Artifact Excursion slight rework and EXE now exhaust as it should, fixed builtin pulsedrive giving kokoro pulsedrive a boost unintentionally
* **(0.3.1)**: EXE not offered as starter issue, solo start deck, Pulsedrive(card) B now no longer exhausts. Alt starter Triple Tap replaced by Pulsedrive(card)
* **(0.3.0)**: Switched Pulsedrive to Kokoro edition

### BETA 2: Chit Chat Time

* **(0.2.11)**: Sprites!
* **(0.2.10)**: Combat and Event chatter!
* **(0.2.9)**: Story chatter!
* **(0.2.8)**: Artifact chatter Weth edition
* **(0.2.7)**: Artifact chatter continuation + character and artifact double checker + artifact by type story variables
* **(0.2.6)**: Chatter time! Artifact edition
* **(0.2.5)**: Added more sprites
* **(0.2.4)**: Added support for dynamically finding and adding to a SaySwitch
* **(0.2.3)**: Support for other languages
* **(0.2.2)**: Gave goodies shine, and developed new dialogue machine
* **(0.2.1)**: Fixed goodies giver not having an amount, and you can only have one uncommon goodie at all times. And swapped Alt and Main starters.
* **(0.2.0)**: Weth's starter artifact is now limited to once per combat. Treasure Seeker is not limited but now requires 10. All special cards are temporary. Rare type special cards are not offered randomly in artifacts anymore. Area bosses drop Max Shield and Max Hull (crystal boss, pirate boss respectively). Also ScatterTrash art tint corrected.

### ALPHA 1: It's Art Time

* **(0.1.5)**: Fixed Bayblast not having any sfx
* **(0.1.4)**: Common card arts! And fixed Splitshot's tooltips
* **(0.1.3)**: Uncommon card arts!
* **(0.1.2)**: Rare card arts! And changed Pearl Dispenser a little bit, and Pow Pow
* **(0.1.1)**: Sprited main sprites + removed energy patch
* **(0.1.0)**: No change except version

### Pre-release

* **(0.0.15)**: Bigger asteroids!
* **(0.0.14)**: Bayblast!
* **(0.0.13)**: Triple tap, Double tap, Compensator, Extreme Violence shoot faster now
* **(0.0.12)**: More splitshot cards! (Finished them all), Brightened Splitshot icon, rebalanced Crisis Call, Pow Pow, Pulsedrive (card).
  * Crisis Call: Base and A now give 2 pulsedrive and 2 goodies, B gives 3 pulsedrive and 2 exhaust Pulsedrive(card)s
  * Pow Pow: B now gives 2 Powershots,
  * Pulsedrive: A and B now exhaust, A gives 2 pulsedrive and gives 1 base pulsedrive(card), B gives 2 pulsedrive and 2 temp B pulsedrive(card)
* **(0.0.11)**: Splitshot vfx!
* **(0.0.10)**: Splitshot!
* **(0.0.9)**: Artifact art!
* **(0.0.8)**: Fixed issue where ArtifactExcursion would not give artifact at the end of an elite battle.
* **(0.0.7)**: Fix relic artifact tooltip description not showing up and fixed number not numbering
* **(0.0.6)**: Infinitely scaling artifacts!
* **(0.0.5)**: Artifacts! As well as artifact hiders.
* **(0.0.4)**: Goodies card giver.
* **(0.0.3)**: Pulsedrive!
* **(0.0.2)**: Adding a bunch of easy to add cards
* **(0.0.1)**: BAM! (Began Assured Mutilation)!
