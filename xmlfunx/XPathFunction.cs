using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Collections;

namespace libjfunx.xmlfunx
{
    public class XPathFunction
    {
        /// <summary>
        /// Declares an XPath Object
        /// </summary>
        /// <param name="xmlFile">the xmlFile</param>
        /// <param name="xPathExpression">the XPath Expression</param>
        public XPathFunction(string xmlFile, string xPathExpression)
        {
            this.xmlFile = xmlFile;
            this.xPathExpression = xPathExpression;
        }

        private string xmlFile;
        private string xPathExpression;

        /// <summary>
        /// Gets all xItems of the specified object (string Array)
        /// </summary>
        public string[][] xItems
        {
            get
            {
                ArrayList xItems = new ArrayList();
                xItems = GetElements();
                return (String[][])xItems.ToArray(typeof(string[]));
            }
        }
            

        /// <summary>
        /// Private method for getting elements from the selected node and File
        /// from the objects constructor
        /// </summary>
        /// <returns>System.Collections.ArrayList of Elements</returns>
        private ArrayList GetElements()
        {
            ArrayList xItems = new ArrayList();           
            XPathDocument doc = new XPathDocument(this.xmlFile);
            XPathNavigator nav = doc.CreateNavigator();


            // Compile a standard XPath expression
            XPathExpression expr;
            expr = nav.Compile(this.xPathExpression);
            XPathNodeIterator iterator = nav.Select(expr);


            // Iterate on the node set
            try
            {
                while (iterator.MoveNext())
                {
                    XPathNavigator nav2 = iterator.Current.Clone();
                    string [] tmp = {nav2.Value, this.xPathExpression};
                    //xItems.Add(nav2.Value);
                    xItems.Add(tmp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return xItems;
        }


        /// <summary>
        /// Bildet eine Schleife durch einen im Konstruktor fesgelegten XPath
        /// und löscht ein definiertes ChildNode, wenn es einen definierten Wert enthält
        /// </summary>
        /// <param name="subXPath">XPath des ChildNodes</param>
        /// <param name="conditionValue">Bedingungswert</param>
        
        public void RemoveNode(string subXPath, string conditionValue)
        {
            // Usage:
            // - neues Objekt new XPathFunction instanzieren
            // - dabei XPATH zum Hauptknoten
            // - Beispiel /D800_MODULE/TESTCASES/CASE[TITLE="XY"
            // - subXPath --> "./STEP"
            // - Bedinung --> "Testschritt 1"

            try
            {
                XmlTextReader reader = new XmlTextReader(this.xmlFile);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                reader.Close();

                //Select the node with the matching title
                XmlNode node;              
                node = doc.SelectSingleNode(this.xPathExpression);

                foreach (XmlNode child in node.SelectNodes(subXPath))
                {
                    //Hilfsvariable found, damit die Schleife nicht weitergeführt wird,
                    //wenn der Wert gefunden wurde
                    bool found = false;
                    if (found == false)
                    {
                        if (child.InnerXml == conditionValue)
                        {
                            XmlNode parent = child.ParentNode;
                            parent.RemoveChild(child);
                            doc.Save(this.xmlFile);
                            found = true;
                        }
                    }
                    

                }
            }
            catch (Exception ex)
            {
                throw ex;
                
            }
        }
	
    }
}
