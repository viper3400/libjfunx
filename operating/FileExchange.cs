using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libjfunx.logging;

namespace libjfunx.operating
{
    /// <summary>
    /// Eine global verfügbare Klasse zum Dateiaustausch
    /// </summary>
    public sealed class FileExchange
    {
        #region Statische Member
        private static readonly FileExchange _instanz = new FileExchange();
        /// <summary>
        /// Speichert eine Datei an einem anderen Ort ab
        /// </summary>
        /// <param name="sourceFile">Quelldatei mit vollem Verzeichnispfad</param>
        /// <param name="destinationFile">Nur der Dateiname (evtl. Unterverzeichnis)</param>
        public static void Upload (string sourceFile, string destinationFile)
        {
            _instanz.UploadFile(sourceFile, destinationFile);
        }

        /// <summary>
        /// Setzt den Typ des FileExchange fest und  gibt Einstellugen mit
        /// </summary>
        /// <param name="fileExchange">Typ des FileExchanges</param>
        /// <param name="settings">Einstellungsobjekt</param>
        public static void SetFileExchange(IFileExchange fileExchange, FileExchangeSettings settings)
        {
            _instanz._fileExchange = fileExchange;
            _instanz._fileExchangeSettings = settings;
        }
        #endregion

        #region Instanzmember
        private IFileExchange _fileExchange = null;
        private FileExchangeSettings _fileExchangeSettings = null;

        private void UploadFile(string sourceFile, string destinationFile)
        {
            if (this._fileExchange != null)
                this._fileExchange.UploadFile(sourceFile, destinationFile, _instanz._fileExchangeSettings);
        }
        #endregion
    }
    /// <summary>
    /// FileExchange Interface
    /// </summary>
    public interface IFileExchange
    {
        /// <summary>
        /// Wird aufgerufen, sobald ein neues File übertragen werden soll
        /// </summary>
        /// <param name="sourceFile">Quelldatei mit vollem Dateinamen und Pfad</param>
        /// <param name="destinationFile">Zieldatei, nur Dateiname (evtl. Unterverzeichnis)</param>
        /// <param name="settings">Einstellungsobjekt</param>
        void UploadFile(string sourceFile, string destinationFile, FileExchangeSettings settings);
    }

    /// <summary>
    /// Klasse zum Schreiben ins Dateisystem
    /// </summary>
    public class FileSystemX : IFileExchange
    {
        /// <summary>
        /// Schreibt eine Daeii ins Dateisystem
        /// </summary>
        /// <param name="sourceFile">Quelldatei mit vollem Namen und Pfad</param>
        /// <param name="destinationFile">Zieldatei, nur Dateiname (evtl. Unterverzeichnis)</param>
        /// <param name="settings">Einstellungsobjekt</param>
        public void UploadFile(string sourceFile, string destinationFile, FileExchangeSettings settings)
        {
            string filePath = operating.FileOperation.EnsureTrailingBackslash(settings.RemotePath);
            filePath.Replace("/", @"\");
            System.IO.File.Copy(sourceFile, filePath + destinationFile, true);
        }
    }

    /// <summary>
    /// Klasse zum Upload auf einen FTP Server
    /// </summary>
    public class FTPServer : IFileExchange
    {
        /// <summary>
        /// Lädt eine Datei auf einen FTP Server
        /// </summary>
        /// <param name="sourceFile">Quelldatei mit vollem Namen und Pfad</param>
        /// <param name="destinationFile">Zieldatei, nur Dateiname (evtl. Unterverzeichnis)</param>
        /// <param name="settings">Einstellungsobjekt</param>
        public void UploadFile(string sourceFile, string destinationFile, FileExchangeSettings settings)
        {
            string fptServerPath = settings.RemotePath.Replace(@"\", "/");
            communication.FTPConnection ftpCon = new libjfunx.communication.FTPConnection(fptServerPath, settings.userID, settings.userPassword);
            ftpCon.Upload(sourceFile);
        }

      
    }

    /// <summary>
    /// Klasse zur Einstellung des FileExchange
    /// </summary>
    public class FileExchangeSettings
    {
        private string remotePath;
        /// <summary>
        /// Benutzerkennung
        /// </summary>
        public string userID;
        /// <summary>
        /// Benutzerpasswort
        /// </summary>
        public string userPassword;
       
        /// <summary>
        /// Entfernter Pfad (Dateipfad, FTP-Server)
        /// </summary>
        public string RemotePath
        {
            get 
            { 
                return this.remotePath; 
            }
            set
            {
                this.remotePath = value;
            }
        }
    }
}
