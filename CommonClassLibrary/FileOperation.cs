using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClassLibrary
{
    /// <summary>
    /// Description of FileOperation.
    /// </summary>
    public static class FileOperation
    {
        public static bool WriteToFile(string path, string line, bool append)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, append))
                    file.WriteLine(line);
                return true;
            }
            catch (Exception) { }

            return false;
        }
    }
}