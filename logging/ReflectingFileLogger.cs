using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libjfunx.logging
{
    /// <summary>
    /// Class for logging application messages to a given filepath and name.W
    /// Works with loglevels.
    /// Derrived from the ExtendedFileLogger class.
    /// In addition this class logs the reflected names of the calling class and the method.
    /// </summary>
    public class ReflectingFileLogger : ExtendedFileLogger, ILogger
    {
        /// <summary>
        /// Constructor of the class. Takes a log filepath and name and a log level.
        /// </summary>
        /// <param name="LogFileName">Path and name of the log file.</param>
        /// <param name="LogLevel">Specfies the depth to which log entries will be writte</param>
        /// <example>
        /// using libjfunx.logging;
        ///
        /// //Create new log instance (once per app)
        /// Logger.SetLogger(new ReflectingFileLogger(@"c:\temp\testlogger.txt", LogEintragTyp.Debug));
        /// Logger.Log(LogEintragTyp.Debug, "Test");
        /// </example>
        public ReflectingFileLogger(string LogFileName, LogEintragTyp LogLevel) : base (LogFileName, LogLevel)
        {

        }

        /// <summary>
        /// Schreibt einen neue Meldung ins Logfile
        /// </summary>
        /// <param name="Message"></param>
        public override void NeueMeldung(LogEintrag Message)
        {
            if (this.validLevels.Contains(Message.Typ.ToString()))
            {
                try
                {
                    string machineName = System.Environment.MachineName;
                    string username = System.Environment.UserName;
                    Message.Text = System.Text.RegularExpressions.Regex.Replace(Message.Text, "\r\n", ", ");
                    string sEntry = Message.Typ.ToString().PadRight(8)
                                + Message.Zeitpunkt.ToString("dd.MM.yy HH:mm:ss")
                                + "," + String.Format("{0,3:D3}", Message.Zeitpunkt.Millisecond).Substring(0, 2) + " "
                                + machineName.PadRight(25)
                                + username.PadRight(25)
                                + String.Format("[{0}][{1}] {2}",
                                    new System.Diagnostics.StackTrace().GetFrame(3).GetMethod().DeclaringType.Name,
                                    new System.Diagnostics.StackTrace().GetFrame(3).GetMethod().Name,
                                    Message.Text)
                                + System.Environment.NewLine;

                    writer.EnqueueMessage(sEntry);
                }
                catch (Exception ex)
                {
                    //FS#14: Kein Exception mehr werfem, sondern Fehler ins Logfile
                    string orgLogFile = writer.WriteFile;
                    writer.WriteFile = Environment.SpecialFolder.LocalApplicationData + @"\libjfunx_exception.log";

                    Logger.Log(LogEintragTyp.Fehler, "LogEx: " + ex.Message);
                    Logger.Log(LogEintragTyp.Fehler, "LogEx: " + ex.ToString());

                    writer.WriteFile = orgLogFile;

                }
            }
            else
            {
                // Hier passiert nichts, es soll ja eben nicht geloggt werden! 
            }
        }

    }
}
