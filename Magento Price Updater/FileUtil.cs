using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Magento_Price_Updater
{
    public static class FileUtil
    {
        /// <summary>
        /// method to print string to a file, opens a streamwriter and writes string param data to file in the current directory
        /// </summary>
        /// <param name="data">string data to be written to the file</param>
        /// <param name="fileName">OPTIONAL: name of file to be written to. default = log.txt</param>
        public static void printToFile(string data, string fileName = "log.txt")
        {
            //this intentionally isn't in a try-catch to avoid an infinite loop, if the directory doesn't exist just leave

            string filePath = Directory.GetCurrentDirectory(); //gets current directory

            if (Directory.Exists(filePath)) //checks for active directory.
            {
                try
                {
                    using (StreamWriter sr = new StreamWriter(filePath + fileName)) //open a new stream writer with path and filename provided
                    {
                        sr.WriteLine(data);
                    }
                }
                catch (Exception ex)
                {
                    writeExeptionToFile(ex.Message);
                }
            }
        }

        /// <summary>
        /// method to print string to a file, opens a streamwriter and writes string param data to file in a specified location
        /// </summary>
        /// <param name="data">string data to be written to the file</param>
        /// <param name="filePath">location of file to be written to</param>
        /// <param name="fileName">name of file to be written to</param>
        public static void printToFile(string data, string filePath, string fileName)
        {
            //this intentionally isn't in a try-catch to avoid an infinite loop, if the directory doesn't exist just leave

            if (Directory.Exists(filePath)) //checks for active directory.
            {
                try
                { 
                    using (StreamWriter sr = new StreamWriter(filePath + fileName,true)) //open a new stream writer with path and filename provided. true param means that the writer will append instead of replace the file
                    {
                        sr.WriteLine(data); 
                    }
                }
                catch (Exception ex)
                {
                    writeExeptionToFile(ex.Message);
                }
            }

        }

        /// <summary>
        /// method specifically used to write exceptions to a log in the current directory
        /// </summary>
        /// <param name="error">string of the exception to be written to the file</param>
        /// <param name="fileName">OPTIONAL: name of the file that the writer writes to. Default = errorlog.txt</param>
        public static void writeExeptionToFile(string error, string fileName = "errorlog.txt")
        {
            try
            {
                //creates line with time of error and error message
                string errorText = DateTime.Today.ToShortDateString() + " - " + DateTime.Today.ToShortTimeString() + "\tError Occurred: " + error;
                printToFile(errorText, "C:\\Users\\Danny\\source\\repos\\Magento Price Updater\\Magento Price Updater\\databases\\", "errorlog.txt");
            }
            catch (Exception ex)
            {
                writeExeptionToFile(ex.Message);
            }
        }
    }
}
