using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIDesign
{
    class texcelRespondProcessor
    {
        public string[] explodeResponse(string responsePacket)
        {
            char delimiterChar = '\r';
            string[] explodedResponse = responsePacket.Split(delimiterChar);

            return explodedResponse;
        }


    }
}
