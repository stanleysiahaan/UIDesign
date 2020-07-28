using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIDesign
{
    class SaveToCSVFile
    {
        public void CreateFile()
        {
            string fileName = String.Format("{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            string filePath = @"C:\Users\Engineer\Documents" + fileName;
            

            StreamWriter sw = new StreamWriter(filePath, false);
            sw.Write(';');

        }
    }

}


