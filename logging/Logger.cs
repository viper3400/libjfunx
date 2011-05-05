using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace libjfunx.logging
{

      /// <summary>
      /// Ein global verfügbarer Logger.
      /// </summary>
      public sealed class Logger
      {
          #region Statische Member
              private static readonly Logger _instanz = new Logger();
              /// <summary>
                /// Loggt die übergebene Meldung.
                /// </summary>
                /// <param name="typ">Typ der Meldung.</param>
                /// <param name="meldung">Text der Meldung.</param>
                public static void Log(LogEintragTyp typ, string meldung)
                {
                        _instanz.NeueMeldung(typ, meldung);
                }
         
                /// <summary>
                /// Setzt den Logger, der zur Ausgabe der Meldungen verwendet werden soll.
                /// </summary>
               /// <param name="logger"></param>
               public static void SetLogger(ILogger logger)
               {
                       _instanz._logger = logger;
               }
               #endregion
        
               #region Instanzmember
               private ILogger _logger = null;
               private List<LogEintrag> _eintraege = new List<LogEintrag>();
        
               private void NeueMeldung(LogEintragTyp typ, string meldung)
               {
                       LogEintrag le = new LogEintrag(typ, meldung);
                       this._eintraege.Add(le);
                       if (this._logger != null)
                               this._logger.NeueMeldung(le);
               }
               #endregion
        }

    /// <summary>
    /// LoggingInterface
    /// </summary>
      public interface ILogger
      {
              /// <summary>
              /// Wird aufgerufen, sobald eine neue Meldung geloggt wird.
              /// </summary>
             /// <param name="meldung">Die neue Meldung.</param>
              void NeueMeldung(LogEintrag meldung);
              
              /// <summary>
              /// Gibt die Liste aller geloggten Meldungen zurück.
              /// </summary>
              /// <returns>Die Liste aller geloggten Meldungen.</returns>
              List<LogEintrag> GetMeldungen();

      }

       /// <summary>
       /// Mögliche Typen eines Log-Eintrags.
       /// </summary>
      public enum LogEintragTyp
      {
              /// <summary>
              /// Statusmeldung. (Level 0)
              /// </summary>
              Status,
              /// <summary>
              /// Erfolgsmeldung. (Level 2)
              /// </summary>
              Erfolg,
              /// <summary>
              /// Fehlermeldung. (Level 1)
              /// /// </summary>
              Fehler,
              /// <summary>
              /// Hinweis. (Level 3)
              /// </summary>
              Hinweis,
              /// <summary>
              /// Meldung zum Debuggen (Level 4)
              /// </summary>
              Debug
              
      }
      /// <summary>
      /// Eine Meldung für das Log.
      /// </summary>
    public struct LogEintrag

      {
              /// <summary>
              /// Der Typ der aktuellen Meldung.
              /// </summary>
              public LogEintragTyp Typ;
             
              /// <summary>
              /// Der Text der aktuellen Meldung.
              /// </summary>
              public string Text;
       
              /// <summary>
              /// Zeitpunkt zu dem die Meldung erstellt wurde.
              /// </summary>
              public DateTime Zeitpunkt;
 
           /// <summary>
              /// Konstruktor.
              /// </summary>
              /// <param name="typ">Der Typ der neuen Meldung.</param>
              /// <param name="text">Der Text der neuen Meldung.</param>
              public LogEintrag(LogEintragTyp typ, string text)
              {
                      this.Typ = typ;
                      this.Text = text;
               this.Zeitpunkt = DateTime.Now;
             }
       
              /// <summary>
              /// Gibt Typ und Text der aktuellen Meldung aus.
              /// </summary>
              /// <returns>Typ und Text der aktuellen Meldung.</returns>
              public override string ToString()
              {
                      return this.Typ.ToString() + ": " + this.Text;
              }
     }

    /// <summary>
    /// Statische Logparameter
    /// </summary>
    public static class LogDatei
    {
        /// <summary>
        /// Statischer String zum Setzen der LogDatei
        /// </summary>
        public static string Name;
    }
   
    /// <summary>
    /// Einfache Klasse zum Schreiben des Logs in eine Datei
    /// </summary>
    public class FileLogger : ILogger
    {
        /// <summary>
        /// Schreibt eine neue Meldung ins Logfile
        /// </summary>
        /// <param name="Meldung"></param>
        public void NeueMeldung(LogEintrag Meldung)
        {
            
            StreamWriter myFile = new StreamWriter(LogDatei.Name, true);
            myFile.Write(Meldung);
            myFile.Close();
        }

        /// <summary>
        /// Nicht implementiert.
        /// </summary>
        /// <returns>-</returns>
        public List<LogEintrag> GetMeldungen()
        {
                throw new Exception("The method or operation is not implemented.");
        }

    }

    /// <summary>
    /// Loggt im DABiS-Format
    /// </summary>
    public class DABiSFormatLogger : ILogger
    {

        /// <summary>
        /// Konstruktor für den DABiS Logger mit Übergabe des Loglevels
        /// </summary>
        /// <seealso cref="DABiSFormatLogger(string LogFile)"/>
        /// <remarks>Bei diesem Konstruktor kann das Loglevel übergeben werden</remarks>
        /// <param name="LogFile">Pfad zur Logdatei</param>
        /// <param name="LogLevel">LogLevel</param>
        public DABiSFormatLogger(string LogFile, LogEintragTyp LogLevel)
        {
            writer.WriteFile = LogFile;
            SetLogLevel(LogLevel);
        }

        /// <summary>
        /// Konstruktor für den DABiS Logger
        /// </summary>
        /// <seealso cref="DABiSFormatLogger(string LogFile, LogEintragTyp LogLevel)"/>
        /// <remarks>Bei diesem Konstruktor wird das Loglevel zur Abwärtskompatibilität auf 5 (Debug gesetzt!</remarks>
        /// <param name="LogFile">Pfad zur Logdatei</param>
        public DABiSFormatLogger(string LogFile)
        {
            writer.WriteFile = LogFile;
            SetLogLevel(LogEintragTyp.Debug);
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <remarks>libjfunx.Logging.LogDatei.Name muss gesetzt sein</remarks>
        /// <seealso cref="libjfunx.logging.LogDatei"/>
        [System.Obsolete("DABiSFormatLogger (string LogFile) nutzen")]
        public DABiSFormatLogger()
        {
            writer.WriteFile = LogDatei.Name;
            SetLogLevel(LogEintragTyp.Debug);
        }

        /// <summary>
        /// Setzt das Loglevel, bis zu welchem geloggt werden soll.
        /// </summary>
        /// <param name="LogLevel">LogLevel</param>
        public void SetLogLevel (LogEintragTyp LogLevel)
        {
            this.logLevel = GetDABiSLoglevel(LogLevel);
        }

        private QueueWriter writer = new QueueWriter();
        private int logLevel;

        /// <summary>
        /// Schreibt eine neue Meldung ins LogFile
        /// </summary>
        /// <param name="Meldung"></param>
        public void NeueMeldung(LogEintrag Meldung)
        {
            int iLoglevel = GetDABiSLoglevel(Meldung.Typ);
            if (iLoglevel <= this.logLevel)
            {
                try
                {
                    string machineName = CutIfBigger(System.Environment.MachineName, 13);
                    string username = CutIfBigger(System.Environment.UserName, 20);
                    Meldung.Text = System.Text.RegularExpressions.Regex.Replace(Meldung.Text, "\r\n", ", ");
                    string sEntry = Convert.ToString(iLoglevel) + " "
                                + Meldung.Zeitpunkt.ToString("dd.MM.yy HH:mm:ss")
                                + "," + Meldung.Zeitpunkt.Millisecond.ToString().Substring(0, 2) + " "
                                + CutIfBigger(Meldung.Text, 150)
                                + machineName
                                + username
                                + System.Environment.NewLine;

                    writer.EnqueueMessage(sEntry);
                }
                catch (Exception ex)
                {
                    //FS#14: Kein Exception mehr werfem, sondern Fehler ins Logfile
                    Logger.Log(LogEintragTyp.Fehler, "LogEx: " + ex.Message);
                    Logger.Log(LogEintragTyp.Fehler, "LogEx: " + ex.ToString());

                    // Hier wird nun nochmal eine Datei mit dem kompletten Inhalt der Exception geschrieben
                    try
                    {
                        StreamWriter myFile = new StreamWriter(LogDatei.Name + ".exc");
                        myFile.WriteLine(ex.ToString());
                        myFile.WriteLine(String.Format("MachineName: {0}, Username: {1}, Meldung: {2}",
                            System.Environment.MachineName, System.Environment.UserName, Meldung.Text));   
                        myFile.Close();
                    }
                    catch (Exception AltLogEx)
                    {
                        Logger.Log(LogEintragTyp.Fehler, "AltLogEx: " + AltLogEx.Message);
                    }
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

        private string CutIfBigger(string sLines, int iMax)
        {
            if (sLines.Length > iMax)
            {
                sLines = sLines.Remove(iMax);
            }

            // FS#14: sLines.Length = iMax war nich berücksichtigt
            // (sLines.Length <= iMax) ist doch aber unsinn -> wenn = dann wird doch der String 
            // ohne Änderung zurückgegeben
            // lag der Fehler im NullHandling?
            if (sLines.Length < iMax && sLines.Length > 0)
            {
                int iChars = sLines.Length;
                for (int i = iChars; i < iMax; i++)
                {
                    sLines = sLines + " ";
                }
            }

            // FS#14: eigentlich sollte ja hier die Originallänge zurückgegeben werden wenn
            // sLines.Length = iMax 
            return sLines;
        }

        /// <summary>
        /// Gibt das DABiSLoglevel zurück
        /// </summary>
        /// <param name="Typ"></param>
        /// <returns></returns>
        private int GetDABiSLoglevel(LogEintragTyp Typ)
        {
            int iLoglevel = 1;
            switch (Typ)
            {
                case LogEintragTyp.Status:
                    iLoglevel = 0;
                    break;
                case LogEintragTyp.Fehler:
                    iLoglevel = 1;
                    break;
                case LogEintragTyp.Erfolg:
                    iLoglevel = 2;
                    break;
                case LogEintragTyp.Hinweis:
                    iLoglevel = 3;
                    break;
                case LogEintragTyp.Debug:
                    iLoglevel = 4;
                    break;
                default:
                    break;
            }

            return iLoglevel;
        }
    
    }

}
