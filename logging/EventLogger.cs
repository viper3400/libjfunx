using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace libjfunx.logging
{
    /// <summary>
    /// Stellt einen EventLoger für das ILogger Interface zur Verfügung
    /// </summary>
    public class EventLogger : ILogger
    {

        /// <summary>
        /// Konstrukutor
        /// </summary>
        /// <param name="source">Name des Anwendung</param>
        /// <param name="log">Name des EventLogs</param>
        public EventLogger(string source, string log)
        {
            this.source = source;
            this.log = log;
        }

        private string source;
        private string log;

        /// <summary>
        /// Schreibt einen neue Meldung ins EventLog
        /// </summary>
        /// <param name="Eintrag"></param>
        public void NeueMeldung(LogEintrag Eintrag)
        {
            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, log);
            EventLog.WriteEntry(source, Eintrag.Text, GetEventLogEntryType(Eintrag.Typ));
        }

        /// <summary>
        /// Wandelt eine LogEintragTyp in einen EventLogEntryType u,
        /// </summary>
        /// <param name="Typ"></param>
        /// <returns></returns>
        private EventLogEntryType GetEventLogEntryType(LogEintragTyp Typ)
        {
            EventLogEntryType LogType = EventLogEntryType.Error;
            switch (Typ)
            {
                case LogEintragTyp.Status:
                    LogType = EventLogEntryType.Information;
                    break;
                case LogEintragTyp.Hinweis:
                    LogType = EventLogEntryType.Information;
                    break;
                case LogEintragTyp.Fehler:
                    LogType = EventLogEntryType.Error;
                    break;
                case LogEintragTyp.Erfolg:
                    LogType = EventLogEntryType.Information;
                    break;
                case LogEintragTyp.Debug:
                    LogType = EventLogEntryType.Information;
                    break;
                default:
                    break;
            }

            return LogType;
        }
        
        /// <summary>
        /// Nicht implementiert.
        /// </summary>
        /// <returns>-</returns>
        public List<LogEintrag> GetMeldungen()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Löscht das EventLog komplett
        /// </summary>
        public void DeleteLog()
        {
            EventLog.Delete(this.log);
        }
    }

}
