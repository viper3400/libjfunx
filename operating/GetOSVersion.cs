using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libjfunx.logging;

namespace libjfunx.operating
{
    /// <summary>
    /// Aufzählung von OSVersionen
    /// </summary>
    public enum OSVersion { Windows7, BeforeWindows7 };
    
    /// <summary>
    /// Klasse zum Feststellen der Betriebssystemversion
    /// </summary>
    public static class GetOSVersion
     {            

        /// <summary>
        /// Gibt als OSVersion Windows7 oder Beforewindows7 zurück
        /// </summary>
        public static OSVersion Version
        {
            get
                {
                    OSVersion result;

                    OperatingSystem os = System.Environment.OSVersion;
                    if ((os.Version.Minor == 1)
                    && (os.Version.Major == 6)
                    && (os.Version.Build >= 7100))
                    {
                        result = OSVersion.Windows7;
                    }
                    else result = OSVersion.BeforeWindows7;
                    Logger.Log(LogEintragTyp.Hinweis, "OSVersion: " + result);
                    return result;
                }
            }
        }
    
}
