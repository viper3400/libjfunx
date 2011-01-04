using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using libjfunx.logging;

namespace libjfunx.operating
{
    
    /// <summary>
    /// Methoden zur Verarebeitung von Dateien
    /// </summary>
    public static class FileOperation
    {

        /// <summary>
        /// Methode zum Erstellen eindeutiger Dateinamen.
        /// Setzt am Endes des Dateinamen (ohne Extension) den nächstfreien Integerwert im Verzeichnis.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filename">The filename.</param>
        /// <returns>a unique filename</returns>
        public static string GetCountedUpFilename(string filename)
        {
            if (!File.Exists(filename))
                return filename;
            else
            {
                int counter = 0;
                FileInfo fileInfo = new FileInfo(filename);
                while (File.Exists(filename))
                {
                    counter++;
                    filename = fileInfo.FullName.Replace(fileInfo.Extension, String.Empty) + counter + fileInfo.Extension;
                }
                return filename;
            }
        }

        /// <summary>
        /// Methode zum Erstellen eindeutiger Dateinamen.
        /// Trennt den letzten Buchstaben der Extension ab und setzt den nächstfreien Integerwert im Verzeichnis.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filename">The filename.</param>
        /// <returns>a unique filename</returns>
        public static string GetCountedUpExtension(string filename)
        {
            if (!File.Exists(filename))
                return filename;
            else
            {
                int counter = 0;
                FileInfo fileInfo = new FileInfo(filename);
                while (File.Exists(filename))
                {
                    counter++;
                    //filename = fileInfo.FullName.Replace(fileInfo.Extension, String.Empty) + counter + fileInfo.Extension;
                    filename = fileInfo.Extension.Substring(0, fileInfo.Extension.Length - 1) + counter;
                }
                return filename;
            }
        }

        [System.Obsolete("Use GetCountedUpFilename or GetCountedUpExtension", true)]
        public static string RenameToUniqueExtension(string filename)
        {
            //obsolete
            return null;
        }
        
        /// <summary>
        /// Methode zum Erstellen eindeutiger Dateinamen.
        /// Hängt an eine Datei einen eindeutigen Datums- und Zeitstring an 
        /// </summary>
        /// <param name="Dateiname">Name der umzubenennen Datei</param>
        /// <returns>Gibt den neuen Dateinamen zurück</returns>
        public static string RenameToUniqueDateTime (string Dateiname)
        {
            string datumzeit = DateTime.Now.ToString("yyyyMMdd_HHmmssmss");
            string neuerDateiname = String.Format("{0}.{1}", Dateiname, datumzeit);
            Logger.Log(LogEintragTyp.Debug, "Rename. Neuer Name: " + neuerDateiname);
            try
            {
                File.Copy(Dateiname, neuerDateiname);
                File.Delete(Dateiname);
                Logger.Log(LogEintragTyp.Erfolg, "Datei umbenannt: " + neuerDateiname);
            }
            catch (Exception ex) { Logger.Log(LogEintragTyp.Fehler, neuerDateiname + ":" + ex.Message); }
            
            return neuerDateiname;
        }

        /// <summary>
        /// Diese Methode gewährleistet, dass ein Pfad-String am Ende
        /// immer einen Backslash hat
        /// </summary>
        /// <param name="filePath">Der Pfadename mit oder ohne Backslash</param>
        /// <returns>Pfad mit abschliessendem Backslash</returns>
        public static string EnsureTrailingBackslash(string filePath)
        {
            // Zuerst prüfen, ob Pfad bereits einen Backslash enthält und diesen entfernen
            if (filePath.EndsWith("\\"))
            {
                filePath = filePath.Substring(0, filePath.Length - 1);
            }

            // Dem String einen Backslash hinzufügen
            filePath = filePath + "\\";
            return filePath;

        }

        /// <summary>
        /// Trennt einen Pfad an allen Backslash auf und liefert die 
        /// einzelnen Teile zurück
        /// </summary>
        /// <param name="FilePath">der aufzutrennende Pfad</param>
        /// <returns>Ein String Array mit den einzelnen Teilstrings</returns>
        public static string[] FilePathToArray(string FilePath)
        {
            char[] chSplit = { '\\' };
            string[] splittedFilePath = FilePath.Split(chSplit);
            return splittedFilePath;


        }

        /// <summary>
        /// Entfernt von einem Dateistring die Endung
        /// </summary>
        /// <param name="FullFileName"></param>
        /// <param name="Extension"></param>
        /// <returns></returns>
        public static string RemoveExtension(string FullFileName, string Extension)
        {
            string result;
            int fullFileNameLength = FullFileName.Length;
            int extensionLength = Extension.Length;
            int newFileNameLength = fullFileNameLength - extensionLength;
            result = FullFileName.Substring(0, newFileNameLength);
            return result;

        }

        /// <summary>
        /// Renames the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="newName">The new name.</param>
        public static void RenameFile(string path, string newName)
        {
            var fileInfo = new FileInfo(path);
            File.Move(path, fileInfo.Directory + newName);
        }

        
    }
}
