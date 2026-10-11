# What it measures

Stock builds the scatter of a terrain quad as one mesh per kind of scatter, each held by an object of its
own, a *holder* (`PQSMod_LandClassScatterQuad`).

Readings 1 to 5 are taken together, at each press of *Record the rocks*: readings 1 to 4 on every quad
carrying scatter around the craft, reading 5 on the quad nearest to it. Every position is read as a
*height*: its distance from the centre of the body, in double precision, from the exact position of that
centre that KSP keeps (`CelestialBody.position`).

Reading 6 is of another kind: taken in any scene, on a button of its own, it measures no height and checks
the pools the holders come from, during a flight or across scene switches.

Reading 7 compares, within one reading, where an object is drawn with where the physics engine holds the
collider of that same object. It only comes out with a mod that gives
the scatter colliders, since stock gives it none.

## 1. The centre of the quad

The position of the quad's transform, in Unity's terms.

## 2. The matrix of the quad

The translation of the quad's local to world matrix. Unity keeps both the centre of an object and its
matrix, computes them separately, and draws with the matrix.

Two heights alone do not tell whether two points are shifted against each other: at the same distance
from the centre of the body, they can still stand apart sideways. So the mod also measures how far the
matrix stands from the centre of the quad, in two parts: *up*, the gap in height, and *across*, the gap
sideways.

## 3. The centre of each holder

The position of the transform of each holder of the quad, one per kind of scatter.

## 4. The matrix of each holder

The translation of each holder's local to world matrix, the one its objects are drawn with, with its *up*
and *across* against the centre of the quad, as for the quad's own matrix.

## 5. The objects against the ground

For every object of scatter held by the quad nearest to the craft, at 10 of its vertices spread over the
whole object:

- **the vertex**, as drawn: the mod reads the object's shape from its mesh;
- **the ground under that vertex**: the height of the terrain collision surface, the surface a craft rests
  on, hit by a ray cast straight down from 100 m above that vertex, so that the ground is found even under
  an object sunk into it.

Every object of a kind is a copy of the same model, moved, turned and scaled. The 10 vertices are chosen
on that model, so they are the same ones in every object of that kind and at every load: the lowest vertex
of the model first, then, one after the other, the vertex furthest from all those already chosen. A model
with 10 vertices or fewer has all of them measured. Heights alone would not show an object that turns
about a point without moving it; spread over the whole object, the vertices show any part of it that
rises or sinks against the ground.

Stock sinks scatter partly into the ground on purpose: a vertex below the ground, in the gap the log
gives as well, is not an error on its own.

## 6. The pools of holders

Stock does not make a holder for each quad. Each kind of scatter of a body keeps a *pool* of holders
(`PQSLandControl.LandClassScatter`): when a quad gets scatter of that kind, a holder is taken out of the
pool, and when the quad is destroyed, the holder is handed back. Free holders hang from one container
object of the pool. A mod that changes how scatter is placed can break that cycle with nothing to see on
screen: a holder left behind on a quad that went back to the terrain's cache of quads, or destroyed along
with it.

The mod reads each pool through its private fields, and counts the holders that break a rule stock keeps:

- a holder **in use** that no longer exists, has no quad, stands on a quad that is not active (stock
  clears that flag when it hands a quad back to its cache, to reuse it elsewhere), or on a quad of another
  body;
- a **free** holder that no longer exists, does not hang from the pool's container, or still has a quad;
- a holder found both in use and free;
- a list of the pool that disagrees with the pool's own count of it;
- a holder of the scene that belongs to **no pool**.

Where a free holder stands inside its container is not one of them: stock hands a holder back without
moving it, so it keeps the offset its last quad gave it.

## 7. The collider of each object

Stock scatter has no collider: nothing rests on it and nothing hits it. Mods can add them, and the
reading then covers, for every object of the quad nearest to the craft that has one:

- **where the object is drawn**: the centre of the box its collision mesh spans, put through the matrix
  its holder is drawn with and the object's own position, turn and scale under that holder;
- **where the physics engine holds it**: the centre of the same box as the engine reports it, from the
  pose it was handed.

Both are the centre of the same box, so they can be compared even though the engine widens that box to
hold it once turned: widening a box leaves its centre where it was. The gap between the two is given as
*up* and *across*, like the others. A positive *up* means the collider stands above the object one sees.

Unity keeps the position of an object and its matrix separately and computes them by different routes;
what it hands the physics engine is its own business. This reading answers what that means on the ground:
whether the rock a craft hits is the rock it can see.

Each holder's line also gives how many of its objects carry a collider, and the closing line of the
record how many were measured over the nearest quad. Both read 0 on a stock install.

**What limits it.** An object is matched to the collider under its holder by rank, in the order the game
built them, which is how the mods that add them proceed. A mod that ordered them another way would give
gaps of metres, not of millimetres, and the mismatch would be plain.

## Precision

Unity gives centres, matrices and vertices as single precision world coordinates. The world origin stays
near the craft, so close to it they resolve a fraction of a millimetre, but the step of a `float` is
0.5 mm at 4 km from that origin and 1 mm at 8 km. Around a landed craft, the nearest quad is not affected;
the quads further away can be. Over a flight 5 km up, all of them
are.

## The log

Every record is written on lines starting with `[Diag-Scatter]`, every height in metres, from
the centre of the body, to the micrometre: subtracting two of them gives millimetres. A record reads:

```
Record … on …: scatter on. Heights are …
  quad '…': height …, matrix …, up … mm, across … mm
    holder '…': built, height …, matrix …, up … mm, across … mm
    …
  …
  rock '…' '…' #0 vertex …: ground …, vertex …, … mm above the ground
  …
  rock '…' '…' #0 collider: physics …, drawn …, up … mm, across … mm
  …
End of record …: … quads with rocks, … holders (… not built yet); nearest quad '…': … rocks, … vertices measured, … without ground under them, … colliders measured
```

- An opening line, with the body and whether scatter is on.
- **Every** quad carrying scatter around the craft, with the height of its centre, the height
  of its matrix, *up* and *across* in millimetres.
- Under each quad, each of its holders, one per kind of scatter, ordered by name: whether its objects are
  built yet, then the same heights, *up* and *across*, still against the centre of the quad.
- Then, for the nearest quad only, one line per measured vertex, object after object and holder after
  holder: the quad, the kind of scatter, the object's number, the vertex's number in the model, the
  height of the ground under the vertex and of the vertex itself, and the gap between the two in
  millimetres. A vertex without ground under it is one where the ray found no terrain: its ground and its
  gap read `--`. An object keeps its number from one load to the next, since stock places the scatter
  from a seed, and a vertex keeps its number since it is chosen on the model.
- After the vertices of an object, and only if it has a collider, one more line: the height of the centre
  of its collision box where the physics engine holds it, the height of that same centre where the object
  is drawn, and the gap between the two, *up* and *across*. A positive *up* puts the collider above the
  object one sees. Each holder's line ends on how many of its objects carry a collider, and the closing
  line on how many were measured: all read 0 on a stock install.
- A closing line, with the counts: it is the one left in sight when following the file as it grows.

A holder's matrix height minus its quad's height and its *up* are nearly the same number, by two
different routes: a difference of heights, and a projection on the vertical.

## The log of the holder pools

```
Holder record …: every pool of holders of rocks, one per body and kind of scatter
  Mun 'Rock00': in use … (counter …): … destroyed, … without a quad, … on an inactive quad, … on a quad of another body; free … (counter …): … destroyed, … outside the pool's container, … still with a quad; … both in use and free
  holder '…' in no pool, under '…'
End of holder record …: … pools, … holders in use, … free, … in no pool; … broken rules
```

- An opening line.
- One line per pool that holds any holder, by body and kind of scatter: how many holders are in use and
  free, each next to the pool's own count, then how many break each rule. A pool that holds none is left
  out.
- One line per holder that belongs to no pool, with the object it hangs from.
- A closing line, with the totals and the number of broken rules: the holders counted on the pool lines,
  one more for each count that disagrees with its list, and the holders in no pool. Expected: 0.

Holder records are numbered on their own, apart from the rock records.
