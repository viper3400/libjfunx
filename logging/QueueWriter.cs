﻿using System;
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

        /// <summary>
        ///  Leerer Konstruktor
        /// </summary>
        public QueueWriter()
        {
        }

        /// <summary>
        /// Gibt und setzt den aktuellen Dateinmane des QueuWriters
        /// </summary>
        public string WriteFile 
        {
            get { return this.writeFile; }
            set { this.writeFile = value; } 
        }

        /// <summary>
        /// Methode zum Schreiben der Nachricht
        /// </summary>
        private void WriteMessage()
        {
            lock (lockvar)
            {
                //FS#74: Die klassenweite Variable msgQueue wird einen neuen Variable zugewiesen
                //       und dann sofort gelöscht, damit sie von aussen wieder frisch befüllt werden kann
                ArrayList msgWriteQueue = new ArrayList(msgQueue);
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
                        catch (System.ArgumentOutOfRangeException)
                        {
                            // In einigen Fällen scheint es vorzukomen, dass msgWriteQueue[i] nicht vorhanden ist,
                            // hier sollte entsprechend nichts gemacht werden
                            System.Diagnostics.EventLog.WriteEntry("libjfunx", "Unerlaubter Zugriff auf LogArray", System.Diagnostics.EventLogEntryType.Warning);
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

        /// <summary>
        ///  Schreibt einen String in eine Warteschlange und veranlasst das Schreiben in eine Datei.
        /// </summary>
        /// <param name="Message"></param>
        public void EnqueueMessage(string Message)
        {
            msgQueue.Add(Message);
            System.Threading.Thread writingThread = new System.Threading.Thread(WriteMessage);
            writingThread.Start();          
        }
    }
}
