using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;

namespace libjfunx.xmlfunx
{
    public class XLinqFunction
    {
        private string xmlFileName;
    
        public XLinqFunction(string xmlFileName)
        {
            //Zuordnen des übergebenen Filenames an das private Feld
            this.xmlFileName = xmlFileName;
        }
       

        /// <summary>
        /// Iteriert durch die
        /// </summary>
        public ArrayList GetValues(string xmlElement, string xmlElementParent)
        {           
            ArrayList xItems = new ArrayList();    
            try
            {
            var xDocument = XElement.Load(this.xmlFileName);
                foreach (var a in xDocument.Elements())
                {
                    xItems.Add(a.Element(xmlElement).Value);                 
                }
              
                return xItems;
            }
           catch (Exception ex) { throw ex; }
        }

        public ArrayList GetValues(string selectNode)
        {
            ArrayList xItems = new ArrayList();
            var xDocument = XElement.Load(this.xmlFileName);
            var query = from a in xDocument.Elements()
                          select a.Element(selectNode).Value;

            foreach (var p in query)
                xItems.Add(p);
            return xItems;
        }
    }
}
