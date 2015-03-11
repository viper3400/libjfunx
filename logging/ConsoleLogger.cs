using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace libjfunx.logging
{
    public class ConsoleLogger : ILogger
    {

        public void NeueMeldung(LogEintrag meldung)
        {


            string entry = (meldung.Zeitpunkt.ToString("dd.MM.yy HH:mm:ss")
                                + "," + String.Format("{0,3:D3}", meldung.Zeitpunkt.Millisecond).Substring(0, 2) + " "
                                + String.Format("  [{0}][{1}] {2}",
                                    new System.Diagnostics.StackTrace().GetFrame(3).GetMethod().DeclaringType.Name,
                                    new System.Diagnostics.StackTrace().GetFrame(3).GetMethod().Name,
                                    meldung.Text));

            Console.WriteLine(entry);
        
        }

        public List<LogEintrag> GetMeldungen()
        {
            throw new NotImplementedException();
        }
    }
}
