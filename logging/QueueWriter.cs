using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace libjfunx.logging
{
    /// <summary>
    /// Klasse zum Schreiben einer ArrayList in eine Datei.
    /// </summary>
    public class QueueWriter
    {
       
        private ArrayList msgQueue = new ArrayList();
        private string writeFile;
        
        //Lockvariable
        private static object lockvar = new object();

        /// <summary>
        /// Konstruktor der Klasse
        /// </summary>
        /// <param name="WriteFile">Datei in die geschrieben werden soll</param>
        public QueueWriter(string WriteFile)
        {
            this.writeFile = WriteFile;                                   
        }

        public QueueWriter()
        {
        }

        public string WriteFile { set { this.writeFile = value; } }

        /// <summary>
        /// Methode zum Schreiben der Nachricht
        /// </summary>
        private void WriteMessage()
        {
            lock (lockvar)
            {

                
                    if (msgQueue != null)
                    {
                        try
                        {
                            StreamWriter myFile = new StreamWriter(writeFile, true);
                            for (int i = 0; i < msgQueue.Count; i++)
                            {
                                myFile.Write(msgQueue[i]);
                            }
                            myFile.Close();
                            // Die Arraylist leeren
                            msgQueue.Clear();
                        }
                        catch //(Exception ex) 
                        {
                            System.Threading.Thread.Sleep(500);
                            WriteMessage();
                            //System.Windows.Forms.MessageBox.Show(ex.ToString());
                            //System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
                            //System.Windows.Forms.MessageBox.Show(p.Id.ToString());
                        }
                        finally
                        {
                            // Datei wieder beschreibbar machen für die nächsten Meldungen
                           // writingInProgess = false;
                        }
                    }
                
            }

        }

        // Schreibt einen String in eine Warteschlange und veranlasst das Schreiben in eine Datei.
        public void EnqueueMessage(string Message)
        {
            msgQueue.Add(Message);
            System.Threading.Thread writingThread = new System.Threading.Thread(WriteMessage);
            writingThread.Start();          
        }
    }
}
