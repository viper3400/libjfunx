using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libjfunx.logging;

namespace libjfunx.operating
{

    /// <summary>
    /// Stellt Methoden zum Start einer externen Anwendung bereit
    /// </summary>
    public class ExternalProcess
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="callProcess">Die Kommandozeile zum Aufruf des Programms</param>
        /// <example> 
        /// <code>
        /// private ExternalProcess p = new ExternalProcess("notepad.exe");
        /// private void buttonStartProcess_Click(object sender, EventArgs e)
        /// {
        ///     p.RunProcessInThread();  
        /// }
        ///
        /// </code>
        /// </example>
        public ExternalProcess(string callProcess)
        {
            this._process = callProcess;
        }

        /// <summary>
        /// leere Konstruktor
        /// <remarks>Prozess muss noch gesetzt werden</remarks>
        /// <see cref="CallProcess"/>
        /// </summary>
        public ExternalProcess()
        {
        }

        /// <summary>
        /// Setzt oder gibt den aufzurufenden Prozess
        /// </summary>
        public string CallProcess 
        {
            get { return this._process; } 
            set { this._process = value; }
        }

        /// <summary>
        /// Setzt die Argumenten für einen aufzurufenden Prozesse
        /// </summary>
        public string ProcessArguments { set { this._processArguments = value; } }
        
        private System.Diagnostics.Process p;
        private string _process = "";
        private string _processArguments;
        private bool _isRunning = false;

        /// <summary>
        /// Tritt ein, wenn der übegebene Prozess beendet wird
        /// </summary>
        public event EventHandler Exited;

        /// <summary>
        /// Tritt ein, wenn versucht wird, dem Prozess erneut zu starten
        /// </summary>
        public event EventHandler IsAlreadyRunning;
        
        /// <summary>
        /// Ruft den Status des Prozesses ab
        /// </summary>
        public bool IsRunning { get { return this._isRunning; } }

        /// <summary>
        /// Startet den übergebenen Prozess
        /// </summary>
        /// <param name="stateInfo"></param>
        private void RunProcess(Object stateInfo)
        {
            Logger.Log(LogEintragTyp.Erfolg, CallProcess + " wird versucht zu starten.");
            // Verhidndert, dass versucht wird, ohne Prozess zu starten
            if (this._process != "")
            {
                // Verhindert, dass der Prozess doppelt gestartet wird
                if (!this._isRunning)
                {
                    /// 1.0.3.1
                    try
                    {
                        p = new System.Diagnostics.Process();
                        // Handle the Exited event that the Process class fires.
                        this.p.Exited += new EventHandler(p_Exited);
                        p.EnableRaisingEvents = true;
                        //p.SynchronizingObject = this;
                        Logger.Log(LogEintragTyp.Debug,"ExProcess:  " + this._process);
                        Logger.Log(LogEintragTyp.Debug, "ExArgument: " + this._processArguments);
                        p.StartInfo.FileName = this._process;
                        p.StartInfo.Arguments = this._processArguments;
                        p.Start();
                        this._isRunning = true;
                    }
                    catch (Exception ex) { Logger.Log(LogEintragTyp.Fehler, "ExProcess: " + ex.Message); }
                }
                else if (this._isRunning)
                {
                    Logger.Log(LogEintragTyp.Fehler, CallProcess + " läuft bereits");
                    if (IsAlreadyRunning != null) IsAlreadyRunning(this, new EventArgs());
                }
            }
            else throw new ArgumentNullException("CallProcess", "Bitte einen Prozess übergeben, der gestartet werden soll");
           
            
        }

        /// <summary>
        /// Startet den übergebenen Prozess in einem neuen Thread
        /// </summary>
        public void RunProcessInThread()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(RunProcess));
        }

        /// <summary>
        /// Löst den externen Exited Event aus, wenn der Prozess beendet wird und 
        /// setzt den bool _isRunning zurück 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void p_Exited(object sender, EventArgs e)
        {
            Logger.Log(LogEintragTyp.Erfolg, CallProcess + " wurde beendet");
            this._isRunning = false;
            if (Exited != null) Exited(this, e);
        }
    }
}
