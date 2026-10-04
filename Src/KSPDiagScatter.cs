using UnityEngine;

namespace com.github.lhervier.ksp.diag.scatter
{
    /// <summary>
    /// Terrain scatter recorder: a small window whose buttons each take a reading and write it to KSP.log in
    /// full. Record the rocks reads the rocks around the active vessel against the ground
    /// (<see cref="RockSurvey"/>), in flight only; Record the holder pools reads every holder of rocks against
    /// the pool it belongs to (<see cref="HolderSurvey"/>), in any scene, so that the pools can be read after
    /// leaving a body as well. Mod+F6 shows or hides the window.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
    public class KSPDiagScatter : MonoBehaviour
    {
        private static readonly KeyBinding WINDOW = new KeyBinding(Constants.WINDOW_KEY);

        // Static: the window keeps its place and its visibility from one scene to the next.
        private static bool visible = true;

        /// <summary>Whether the window shows, as Mod+F6 toggles it; the measures go on either way.</summary>
        internal static bool WindowVisible
        {
            get { return visible; }
            set { visible = value; }
        }

        private static Rect windowRect = new Rect(Constants.WINDOW_X, Constants.WINDOW_Y, Constants.WINDOW_WIDTH, 0f);

        // The closing line of the last record, shown under the buttons.
        private static string last = "";

        private void Update()
        {
            if (GameSettings.MODIFIER_KEY.GetKey() && WINDOW.GetKeyDown())
            {
                visible = !visible;
            }
        }

        private void OnGUI()
        {
            if (!visible)
            {
                return;
            }
            GUI.skin = HighLogic.Skin;
            windowRect = GUILayout.Window(Constants.WINDOW_ID, windowRect, DrawWindow, "KSP Diag - Scatter");
        }

        private void DrawWindow(int id)
        {
            GUILayout.BeginVertical();
            GUI.enabled = HighLogic.LoadedSceneIsFlight;
            if (GUILayout.Button("Record the rocks"))
            {
                RecordRocks();
            }
            GUI.enabled = true;
            if (GUILayout.Button("Record the holder pools"))
            {
                RecordHolders();
            }
            GUILayout.Label(last);
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        /// <summary>Records the rocks, as the button does; returns the closing line of the record.</summary>
        internal string RecordRocks()
        {
            last = RockSurvey.Record();
            return last;
        }

        /// <summary>Records the holder pools, as the button does; returns the closing line of the record.</summary>
        internal string RecordHolders()
        {
            last = HolderSurvey.Record();
            return last;
        }

        /// <summary>Where the window is on the screen, and how big.</summary>
        internal Rect WindowRect
        {
            get { return windowRect; }
            set { windowRect = value; }
        }

    }
}
