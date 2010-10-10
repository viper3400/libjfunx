using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace libjfunx.timefunx
{
    public class FileAge
    {
        public FileAge(FileInfo Datei)
        {
            this.datei = Datei;
        }

        private FileInfo datei;
        public int FileAgeInDays ()
        {
            return CalculatedFileAge().Days;
        }

        private TimeSpan CalculatedFileAge ()
        {
            return DateTime.Now - this.datei.LastWriteTime;
        }

        /// <summary>
        /// Gibt an, ob die Datei im Objekt älter als die übergebenen Tage ist.
        /// </summary>
        /// <param name="Days">Anzahl Tage, mit denen verglichen werden soll</param>
        /// <returns>true, wenn Datei älter als x Tage</returns>
        public bool isOlder(int Days)
        {
            if (CalculatedFileAge().Days > Days) return true;
            else return false;
        }

        /// <summary>
        /// Löscht die Datei, wenn Sie älter ist, als die übergebenen Tage
        /// </summary>
        /// <param name="Days"></param>
        /// <returns>true bei erfolgter Löschung, false bei neuerer Datei</returns>
        public bool DeleteFileIfOlder(int Days)
        {
            if (isOlder(Days) == true)
            {
                try
                {
                    this.datei.Delete();
                    return true;
                }
                catch (Exception ex) { throw ex; }
            }
            else return false;
            
        }


    }
}
