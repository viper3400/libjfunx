using System;
using System.IO;
using System.Xml.Serialization;

namespace libjfunx.xmlfunx
{
    public static class XmlFileOperations
    {
        public static void SaveElement(object element, string Filename)
        {
            Type serType = element.GetType();
            XmlSerializer ser = new XmlSerializer(serType);
            FileStream str = new FileStream(Filename, FileMode.Create);
            ser.Serialize(str, element);
            str.Close();
        }

        public static object LoadElement(string Filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(object));
            StreamReader sr = new StreamReader(Filename);
            object Element = (object)ser.Deserialize(sr);
            sr.Close();
            return Element;
        }

        public static object LoadElement(object element, string Filename)
        {
            object Element = 0;
            if (File.Exists(Filename))
            {
                Type serType = element.GetType();
                XmlSerializer ser = new XmlSerializer(serType);
                StreamReader sr = new StreamReader(Filename);
                Element = (object)ser.Deserialize(sr);
                sr.Close();
            }
            return Element;
        }

        public static void DeleteElement(string FileName)
        {
            File.Delete(FileName);
        }

        public static string FileStamp()
        {
            return DateTime.Now.Ticks.ToString();
        }
    }
}
