using System.Collections.Generic;
using UnityEngine;
using com.github.lhervier.ksp.mcpserver;

namespace com.github.lhervier.ksp.diag.scatter
{
    /// <summary>
    /// What KSP-MCPServer, when it is installed, offers of this mod as tools: its buttons and the moving of its
    /// window. Each method works on the window loaded in the scene. Nothing here is needed to play by hand, and
    /// this mod runs the same without KSP-MCPServer: only that server reads the attribute.
    /// </summary>
    internal static class McpTools
    {
        [McpTool("scatter_record_rocks",
            "Presses Record the rocks in the window of KSP Diag - Scatter: writes a record of the rocks around " +
                "the active vessel to KSP.log, in flight only, and returns its closing line.")]
        internal static object RecordRocks()
        {
            return Window().RecordRocks();
        }

        [McpTool("scatter_record_holders",
            "Presses Record the holder pools in the window of KSP Diag - Scatter: writes a record of every pool " +
                "of holders of rocks to KSP.log, in any scene, and returns its closing line.")]
        internal static object RecordHolders()
        {
            return Window().RecordHolders();
        }

        [McpTool("scatter_move_window",
            "Moves the window of KSP Diag - Scatter, as dragging it does: x and y in pixels from the top left " +
            "corner of the screen. Returns its position and size (x, y, width, height).")]
        internal static object MoveWindow(double x, double y)
        {
            KSPDiagScatter window = Window();
            Rect rect = window.WindowRect;
            rect.x = (float)x;
            rect.y = (float)y;
            window.WindowRect = rect;
            return new Dictionary<string, object>
            {
                { "x", (double)rect.x },
                { "y", (double)rect.y },
                { "width", (double)rect.width },
                { "height", (double)rect.height }
            };
        }

        private static KSPDiagScatter Window()
        {
            KSPDiagScatter window = Object.FindObjectOfType<KSPDiagScatter>();
            if (window == null)
            {
                throw new System.InvalidOperationException("KSP Diag - Scatter is not loaded in this scene");
            }
            return window;
        }
    }
}
