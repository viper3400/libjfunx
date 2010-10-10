using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using libjfunx.logging;

namespace libjfunx.communication
{
    /// <summary>
    /// Stellte Funktionen für FTP Verbindungen bereit
    /// </summary>
    public class FTPConnection
    {
        /// <summary>
        /// Konstruktor der Klasse
        /// </summary>
        /// <param name="FtpServerIP"></param>
        /// <param name="FtpUserID"></param>
        /// <param name="FtpPassword"></param>
        public FTPConnection(string FtpServerIP, string FtpUserID, string FtpPassword)
        {
            initFtp(FtpServerIP, FtpUserID, FtpPassword);           
        }
   
        private void initFtp(string FtpServerIP, string FtpUserID, string FtpPassword)
        {
            this.ftpServerIP = FtpServerIP;
            this.ftpUserID = FtpUserID;
            this.ftpPassword = FtpPassword;
            Logger.Log(LogEintragTyp.Hinweis, String.Format("FTP-Objekt mit Server {0}", this.ftpServerIP));  
        }

        private string ftpServerIP;
        private string ftpUserID;
        private string ftpPassword;

        /// <summary>
        /// Lädt eine Datei per FTP auf einen Server
        /// </summary>
        /// <param name="filename"></param>
        public void Upload(string filename)
        {
            Logger.Log(LogEintragTyp.Hinweis, "Start der FTP-Uploadsequenz");
            FileInfo fileInf = new FileInfo(filename);
            string uri = "ftp://" +
         ftpServerIP + "/" + fileInf.Name;
            FtpWebRequest reqFTP;

            // Create FtpWebRequest object from the Uri provided
            reqFTP =
         (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP +
         "/" + fileInf.Name));

            // Provide the WebPermission Credintials
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

            // By default KeepAlive is true, where the control connection is not closed
            // after a command is executed.
            reqFTP.KeepAlive = false;

            // Specify the command to be executed.
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            // Specify the data transfer type.
            reqFTP.UseBinary = true;

            // Notify the server about the size of the uploaded file
            reqFTP.ContentLength = fileInf.Length;

            // The buffer size is set to 2kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
            FileStream fs = fileInf.OpenRead();

            try
            {
                // Stream to which the file to be upload is written
                Stream strm = reqFTP.GetRequestStream();

                // Read from the file stream 2kb at a time
                contentLen = fs.Read(buff, 0, buffLength);

                // Till Stream content ends
                while (contentLen != 0)
                {
                    // Write Content from the file stream to the FTP Upload Stream
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                // Close the file stream and the Request Stream
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                Logger.Log(LogEintragTyp.Fehler, ex.Message);
                throw;
            }
            Logger.Log(LogEintragTyp.Hinweis, "Ende der FTP-Uploadsequenz");
        }

        /// <summary>
        /// Lädt eine Datei von einem FTP Server herunter
        /// </summary>
        /// <param name="filePath">The local filepath</param>
        /// <param name="fileName">Name of the remote and the local file to be created</param>
        public void Download(string filePath, string fileName)
        {
            Logger.Log(LogEintragTyp.Hinweis, "Start der FTP-Downloadsequenz");
            FtpWebRequest reqFTP;
            try
            {
                //filePath = <<The full path where the file is to be created. the>>, 
                //fileName = <<Name of the file to be createdNeed not name on FTP server. name name()>>
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);
                
              

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                
               
            }           
            catch (Exception ex)
            {
                Logger.Log(LogEintragTyp.Fehler, ex.Message);
                throw;
            }
            Logger.Log(LogEintragTyp.Hinweis, "Ende der FTP-Downloadsequenz");
            
        }

        /// <summary>
        /// Löscht eine Daeti auf dem FTP Server
        /// </summary>
        /// <param name="fileName">Name der Remotedatei</param>
        public void DeleteFileOnServer(string fileName)
        {
                                               
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileName));
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials= new NetworkCredential(ftpUserID, ftpPassword);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            //Console.WriteLine("Delete status: {0}", response.StatusDescription);
            response.Close();       
        }

        /// <summary>
        /// Prüft, ob eine Datei auf einem FTP Server existiert
        /// </summary>
        /// <param name="fileName">Name der Remotedatei</param>
        /// <returns>Gibt true zurück, wenn Datei vohanden, sonst false</returns>
        public bool FileExistsOnFtp(string fileName)
        {
            // zunächst mal ganz normal die Verbindung vorbereiten
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileName));
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            FtpWebResponse response = null;

            try
            {
                // Wenn es die Datei nicht gibt, dann kommt es hier zu einer Exception
                response = (FtpWebResponse)reqFTP.GetResponse();
                return true;
            }
            catch (WebException ex)
            {           
                // Prüfen, ob es die erwartete Exception ist - ansonsten wird jede andere Ausnahme weiter gegeben
                // Hier kommen je nach Sprache und OS Installation unterschiedliche Meldugnen zurück
                // EN: "The remote server returned an error: (550) File unavailable (e.g., file not found, no access)."
                // DE: "Der Remoteserver hat einen Fehler zurückgegeben: (550) Datei nicht verfügbar (z.B. nicht gefunden oder kein Zugriff)."
                // Somit wird nur auf die Existens des Teilstring mit dem Fehlercode 550 geprüft
                int check = ex.Message.IndexOf(": (550)");
                // Wenn nicht -1 zurückkommt, dann ist der Wert enthalten
                if (check != -1)
                { 
                    // In dem Fall liefert die Funktion false zurück
                    return false;
                }
                else throw;
            }
            catch (Exception) { throw; }
        }
    }
}
