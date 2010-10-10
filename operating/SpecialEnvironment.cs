using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace libjfunx.operating
{
    /// <summary>
    /// Erweiterte Umgebungsvariablen
    /// </summary>
    public class SpecialEnvironment
    {
        /// <summary>
        /// Aufzählung, was für AllUsers verfügbar ist
        /// </summary>
        public enum AllUsers : int
        {
            Desktop = 0,
            Startmenue
        }

        /// <summary>
        /// Gibt den Pfad des AllUsers-Verzeichnisses nach OS Version zurück
        /// </summary>
        /// <param name="location">Desktop od. Startmenü</param>
        /// <returns>AllUsers-Pfad</returns>
        public static string GetSpecialEnvironmentPath(AllUsers location)
        {
            string result = "";

            switch (location)
            {
                case AllUsers.Desktop:
                    if (Environment.OSVersion.Version.Major > 4 && Environment.OSVersion.Version.Minor > 1)
                        result = Path.Combine(Environment.GetEnvironmentVariable("PUBLIC"), "Desktop");
                    else
                        result = Path.Combine(Environment.GetEnvironmentVariable("ALLUSERSPROFILE"), "Desktop");
                    break;
                case AllUsers.Startmenue:
                    if (Environment.OSVersion.Version.Major > 4 && Environment.OSVersion.Version.Minor > 1)
                        result = Path.Combine(Environment.GetEnvironmentVariable("PROGRAMDATA"), @"Microsoft\Windows\Start Menu");
                    else
                        result = Path.Combine(Environment.GetEnvironmentVariable("ALLUSERSPROFILE"), "Startmenü");
                    break;
                default:
                    result = "";
                    break;
            }

            return result;

        }

    }

}
