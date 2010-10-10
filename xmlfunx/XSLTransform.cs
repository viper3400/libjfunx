using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace libjfunx.xmlfunx
{
    public class XSLTransform
    {
        private string xmlPath;
        private string xslPath;
        private string resultFile;
    
        public XSLTransform(string XMLPath, string XSLPath, string DestiantionFile)
        {
            this.xmlPath = XMLPath;
            this.xslPath = XSLPath;
            this.resultFile = DestiantionFile;
            Transform();
        }

        public void Transform()
        {
            try
            {
                //load the Xml doc
                XPathDocument myXPathDoc = new XPathDocument(this.xmlPath);

                XslCompiledTransform myXslTrans = new XslCompiledTransform();
                //XslTransform veraltet
                //XslTransform myXslTrans = new XslTransform();

                //load the Xsl 
                myXslTrans.Load(this.xslPath);

                //create the output stream
                XmlTextWriter myWriter = new XmlTextWriter
                    (resultFile, null);

                //do the actual transform of Xml
                myXslTrans.Transform(myXPathDoc, null, myWriter);

                myWriter.Close();
            }
            catch (Exception ex) { throw ex; }

        }
    }
}
