using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libjfunx.logging
{
    /// <summary>
    /// Klasse zum Loggen von Meldungen, abgrenzbar durch ein Logelevel.
    /// </summary>
    public class ExtendedFileLogger : ILogger
    {
        protected QueueWriter writer = new QueueWriter();
        protected string[] validLevels;
        
        /// <summary>
        /// Konstruktor der Klasse
        /// </summary>
        /// <param name="LogFileName">Name der Logdatei</param>
        /// <param name="LogLevel">Gibt die Tiefe an, bis zu der geloggt werden soll.</param>
        public ExtendedFileLogger(string LogFileName, LogEintragTyp LogLevel)
        {
            this.writer.WriteFile = LogFileName;
            this.validLevels = ValidLoglevels(LogLevel);
        }

        /// <summary>
        /// Schreibt einen neue Meldung ins Logfile
        /// </summary>
        /// <param name="Message"></param>
        public virtual void NeueMeldung(LogEintrag Message)
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
                                + Message.Text
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

        /// <summary>
        /// Nicht implementiert.
        /// </summary>
        /// <returns>-</returns>
        public List<LogEintrag> GetMeldungen()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private string[] ValidLoglevels(LogEintragTyp MaxLogLevel)
        {
            
            switch (MaxLogLevel)
            {
                case LogEintragTyp.Debug:
                    return new string[] { LogEintragTyp.Debug.ToString(), LogEintragTyp.Erfolg.ToString(), 
                        LogEintragTyp.Fehler.ToString(), LogEintragTyp.Hinweis.ToString(), LogEintragTyp.Status.ToString()};
                case LogEintragTyp.Hinweis:
                    return new string[] { LogEintragTyp.Erfolg.ToString(), 
                        LogEintragTyp.Fehler.ToString(), LogEintragTyp.Hinweis.ToString(), LogEintragTyp.Status.ToString()};
                case LogEintragTyp.Erfolg:
                    return new string[] { LogEintragTyp.Erfolg.ToString(), 
                        LogEintragTyp.Fehler.ToString(), LogEintragTyp.Status.ToString()};
                case LogEintragTyp.Fehler:
                    return new string[] { LogEintragTyp.Fehler.ToString(), LogEintragTyp.Status.ToString()};
                case LogEintragTyp.Status:
                    return new string[]{ LogEintragTyp.Status.ToString()};
                default:
                       return new string[] { LogEintragTyp.Debug.ToString(), LogEintragTyp.Erfolg.ToString(), 
                        LogEintragTyp.Fehler.ToString(), LogEintragTyp.Hinweis.ToString(), LogEintragTyp.Status.ToString()};
            }
        }

    }
}
