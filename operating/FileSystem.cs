using System;
using System.Collections.Generic;
using System.Text;



namespace libjfunx.operating
{
    public class FileSystem
    {
        private static string _operationDirNotExist = "Can not find the directory";
        private static string _operationDirNotEmpty = "Directory is not empty";
        private static string _operationSuccess = "success";
        private static string _operationFailed = "failed";

        public static void DeleteAllSubfolders(string directoryPath)
        {
            //find files in the current directory an delete them
            foreach (string fileName in System.IO.Directory.GetFiles(directoryPath))
            {
                try
                {
                    System.IO.File.Delete(fileName);
                }
                catch
                {
                    //Some files produce an exception if they cannot be deleted
                    //throw Exception ex; 
                }
            }
            //find subdirectorys in the current directory an delete them recursiv
            foreach (string directoryName in System.IO.Directory.GetDirectories(directoryPath))
            {
                DeleteAllSubfolders(directoryName);
                try
                {
                    //If no undeletable files are present the recursive search will be killed
                    System.IO.Directory.Delete(directoryName, true);
                }
                catch
                {
                    //throw Exception ex; 
                }
            }
        }

        public static string DeleteDirectory(string directoryPath, bool recursiv, bool deleteFiles)
        {
            string result = _operationFailed;
            if (System.IO.Directory.Exists(directoryPath))
            {
                if (recursiv == false && deleteFiles == false)
                {
                    System.IO.Directory.Delete(directoryPath, false);

                    if (System.IO.Directory.Exists(directoryPath))
                    {
                        result = _operationDirNotEmpty;
                    }
                    else
                    {
                        result = _operationSuccess;
                    }
                }

                if (recursiv == true && deleteFiles == false)
                {
                    System.IO.Directory.Delete(directoryPath, true);

                    if (System.IO.Directory.Exists(directoryPath))
                    {
                        result = _operationDirNotEmpty;
                    }
                    else
                    {
                        result = _operationSuccess;
                    }
                }

                if (recursiv == true && deleteFiles == true)
                {
                    System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(directoryPath);

                    DeleteAllSubfolders(directoryPath);

                    System.IO.Directory.Delete(directoryPath, true);

                    if (System.IO.Directory.Exists(directoryPath))
                    {
                        result = _operationDirNotEmpty;
                    }
                    else
                    {
                        result = _operationSuccess;
                    }
                }

                if (recursiv == false && deleteFiles == true)
                {

                    //find files in the current directory and delete them
                    foreach (string fileName in System.IO.Directory.GetFiles(directoryPath))
                    {
                        try
                        {
                            System.IO.File.Delete(fileName);
                        }
                        catch
                        {
                            //Some files produce an exception if they cannot be deleted
                            //throw Exception ex;
                        }
                    }

                    System.IO.Directory.Delete(directoryPath, false);

                    if (System.IO.Directory.Exists(directoryPath))
                    {
                        result = _operationDirNotEmpty;
                    }
                    else
                    {
                        result = _operationSuccess;
                    }
                }

            }
            else
            {
                result = _operationDirNotExist;
            }
            return result;
        }
    }
}
