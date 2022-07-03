# Modding Utilities
-------------------

This mod provides several utilities that were developed over time for specific mods that may be useful to the modding community at large.

#### v3.5
---------

- Added more efficient draw patch from DeckCustomization

#### v3.0
---------

- Added interface for game mode hooks on monos (Willuwontu)
- Added support for reversible effects on child gameobjects of the player

#### v2.8
---------

Added utilities for the out of bounds, agnostic of map embiggener

#### v2.7
---------

Minor `ReversibleEffect` bugfix

#### v2.6
---------

Bugfixes regarding multiple local players per client in online lobbies

#### v2.5
---------

Various bugfixes to stay compatible with RWF and UnboundLib

#### v2.4
---------

Various bugfixes regarding RWF 2v2 mode

#### v2.0
---------

Added `AIMinionHandler`.

#### v1.0
---------

BossSloth helped migrate lots of stuff from PCE for custom card effects

## More Efficient Draws
-----------------------

This mod automatically uses a _significantly_ more efficient algorithm for drawing cards from the deck. This is practically necessary since it can be possible for a significant fraction of the deck to be invalid (rarities set to 0, incompatible with players' current cards, etc.). This algorithm has been tested and verified to produce identical draws to the vanilla game's algorithm.
