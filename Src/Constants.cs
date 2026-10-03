using UnityEngine;

namespace com.github.lhervier.ksp.diag.scatter
{
    /// <summary>
    /// The key and the window, and the fixed values the survey of the rocks relies on.
    /// </summary>
    internal static class Constants
    {
        /// <summary>Tag in front of every line this mod writes to KSP.log.</summary>
        public const string LOG_PREFIX = "[KSPDiagScatter] ";

        /// <summary>
        /// Key that shows or hides the window, pressed along with the modifier key of the game (Alt): the same
        /// for every KSP Diag. Stock only uses it in the action group editor.
        /// </summary>
        public const KeyCode WINDOW_KEY = KeyCode.F6;

        /// <summary>Id of the window, unique among the windows of the game.</summary>
        public const int WINDOW_ID = 0x47485006;

        /// <summary>Where the window opens, and how wide it is, in pixels.</summary>
        public const float WINDOW_X = 60f;
        public const float WINDOW_Y = 60f;
        public const float WINDOW_WIDTH = 420f;

        /// <summary>
        /// How far above a vertex of a rock the ray looking for the ground starts, in metres. The
        /// ground is a surface without thickness that a ray only hits from above, so the ray must start
        /// above it even when the rock is sunk into it.
        /// </summary>
        public const float RAY_START_HEIGHT = 100f;

        /// <summary>How many vertices of each rock are measured against the ground.</summary>
        public const int POINTS_PER_ROCK = 10;

        /// <summary>Layer of the terrain colliders.</summary>
        public const int TERRAIN_LAYER = 15;

        /// <summary>
        /// Vertices per rock when a kind of scatter has no mesh of its own: stock then uses two back to back
        /// squares, four vertices each.
        /// </summary>
        public const int FALLBACK_ROCK_VERTICES = 8;
    }
}
