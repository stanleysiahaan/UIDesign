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
        public string command;
        public byte[] asciiBytes { get; set; }
        public char charascii { get; set; }
        
        public int sum { get; set; }
        public string cmdFinal { get; set; }
        public functionASCII(string _command)
        {
            command = _command;
            chartoascii();
            checksum();
            asciitochar();
        }

        public byte[] chartoascii()
        {
            asciiBytes = Encoding.ASCII.GetBytes(command);
            return asciiBytes;
        }

        public int checksum()
        {
            sum = 0;
            foreach(byte b in asciiBytes)
            {
                sum += b;
            }            
            if (sum == 127)
            {
                sum = 32;
            }
            else if (sum < 32)
            {
                sum += 32;
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
                    sum += 32;
                }
            }
            return sum;
        }

        public char asciitochar()
        {
            charascii = Convert.ToChar(sum);
            return charascii;
        }

        public string commandbuilder()
        {
            cmdFinal = String.Join("", command, charascii, ",\r");
            return cmdFinal;
        }

    }
}
//System.Text.Encoding.UTF8.GetString(_chartoascii);