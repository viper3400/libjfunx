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
                //FS#74: Die klassenweite Variable msgQueue wird einen neuen Variable zugewiesen
                //       und dann sofort gelöscht, damit sie von aussen wieder frisch befüllt werden kann
                ArrayList msgWriteQueue = new ArrayList();
                msgWriteQueue = msgQueue;
                msgQueue.Clear();

                    if (msgWriteQueue != null)
                    {
                        try
                        {
                            StreamWriter myFile = new StreamWriter(writeFile, true);
                            for (int i = 0; i < msgWriteQueue.Count; i++)
                            {
                                myFile.Write(msgWriteQueue[i]);
                            }                            
                            myFile.Close();
                            // Die Arraylist leeren
                            msgWriteQueue.Clear();
                        }
                        catch //(Exception ex) 
                        {
                            System.Threading.Thread.Sleep(500);
                            //FS#74: Wenn es beim Schreiben zu einer Exception kommt,
                            //        müssen die Daten wieder zurück in die HauptQueue
                            msgQueue.AddRange(msgWriteQueue);
                            WriteMessage();
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
