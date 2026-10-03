using UnityEngine;

namespace com.github.lhervier.ksp.diag.scatter
{
    /// <summary>
    /// The survey of the rocks against the ground (the button Record the rocks): quad by quad and rock by rock,
    /// under a record number that keeps counting across scene changes.
    /// </summary>
    internal static class RockSurvey
    {
        // Static: a reload destroys the addon, and the numbers must go on from one load to the next.
        private static int recordCount;

        /// <summary>
        /// Takes a reading of the rocks around the active vessel and writes it to KSP.log under the next record
        /// number. When there is no vessel or no terrain to read, writes only a line saying so, without using
        /// up a number. Returns the last line written.
        /// </summary>
        public static string Record()
        {
            Reading reading = Reading.Take(FlightGlobals.ActiveVessel);
            if (reading == null)
            {
                const string NOTHING = "No vessel, or no terrain under it: nothing recorded.";
                Debug.Log(Constants.LOG_PREFIX + NOTHING);
                return NOTHING;
            }

            recordCount++;
            return reading.Log(recordCount);
        }
    }
}
