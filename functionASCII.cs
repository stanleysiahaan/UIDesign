using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIDesign
{
    class functionASCII
    {
        public byte[] asciiBytes { get; set; }
        public char charascii { get; set; }

        public int sum { get; set; }
        string cmdFinal;
        public functionASCII()
        {
        }

        public string commandbuilder(string command)
        {
            asciiBytes = Encoding.ASCII.GetBytes(command);
            //Checksum
            sum = 0;
            foreach (byte b in asciiBytes)
            {
                sum += b;
            }
            if (sum == 127)
            {
                sum = 32;
            }
            else if (sum < 32)
            {
                sum += 33;
            }
            else if (sum > 127)
            {
                sum &= 127;
                if (sum == 127)
                {
                    sum = 32;
                }
                else if (sum < 32)
                {
                    sum += 33;
                }
            }
            //asciitochar
            charascii = Convert.ToChar(sum);
            cmdFinal = command + charascii + "\r";
            return cmdFinal;

        }
    }
}
//System.Text.Encoding.UTF8.GetString(_chartoascii);