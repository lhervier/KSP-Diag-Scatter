# KSP Diag - Scatter

**⚠️ Work in progress.** This is an active investigation, not a finished mod. The code and the pages of this repository can still change.

**How this was made.** Written with Claude, Anthropic's AI assistant, and reviewed line by line by a
human — me. I am saying so up front, because contributions made with an AI deserve a closer
look than others, and because some people would rather stop reading here. This mod measures and fixes
nothing, so what there is to check is the reading itself: the source is public, and every reading it
takes is described in [What it measures](docs/what-it-measures.md).

A measuring instrument for KSP 1.12, for the terrain scatter drawn around your craft — the rocks, and
around the KSC the grass and the trees. At the press of a button, it writes to `KSP.log` where the
quads, the holders of their scatter and the objects themselves are drawn against the ground. It also
checks the pools the holders are taken from and handed back to, and, where a mod has given the scatter
colliders, compares where each object is drawn with where the physics engine holds its collider.

## What it measures

At each press of *Record the rocks*: the heights of the quads carrying scatter around the craft, of
their holders, and of the objects of the nearest quad against the ground. Two other readings check the
holder pools and, where a mod adds them, the scatter colliders.

**→ Full chapter: [What it measures](docs/what-it-measures.md)**

## Taking a reading

1. **Terrain scatter must be on**: *Settings → Graphics → Terrain Scatters*. Scatter only exists on the
   most detailed terrain: land a craft where there is some around it.
2. **Have KSP write its log at once**: in the debug menu (`Alt+F12 → Debugging`), tick the option that
   flushes the log instantly (`LOG_INSTANT_FLUSH` in `settings.cfg`), and follow the file as it grows,
   for instance with `tail -f KSP.log`.
3. **Press *Record the rocks*** in the mod's window (`Alt+F6` shows or hides it) once the scene has
   settled. The last line of the record tells what it holds: quads and their scatter are built over
   several frames, so if holders are not built yet, or the counts still grow from one press to the next,
   wait a few seconds and press again.

*Record the holder pools* works in every scene, and each of its records is a check of its own: it ends on
the number of rules the pools break, 0 expected. Reading 7 needs a mod that gives the scatter colliders,
and its closing line counts the colliders measured: 0 means there is nothing to read. KSP overwrites
`KSP.log` each time it starts: copy it before relaunching.

## Get it

Source: <https://github.com/lhervier/KSP-Diag-Scatter>

**Build.** It needs the .NET SDK and reads the KSP assemblies from your install. Set `KSPDIR` to your
KSP install folder and run `build.bat`. It compiles `GameData/KSPDiagScatter/KSPDiagScatter.dll`
and never touches your KSP install.

**Install.** Copy `GameData/KSPDiagScatter` into the `GameData` of KSP. It runs on a stock
install, and needs neither Harmony nor any other mod: it only reads the scene.

## License

MIT
