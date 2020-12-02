using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CryptoSoft
{
    class Program
    {
        /// <summary>
        /// CryptoSoft made by Chevallier Baptiste
        /// To crypt file with XOR and an EncryptKey
        /// to use the process : CryptoSooft.exe (sourceFile) (destinationFile)
        /// </summary>
        /// <param name="args">array of argument sourceFile and destinationFile</param>
        static void Main(string[] args)
        {
            EncryptKey key = new EncryptKey();
            Encrypt(args[0], key.key, args[1]);
        }

        /// <summary>
        /// Method to encrypt the file
        /// </summary>
        /// <param name="sourceFile">The source file specified by the user </param>
        /// <param name="key">The key to encrypt the data</param>
        /// <param name="destinationFile">The destination file specified by the user </param>
        static private void Encrypt(string sourceFile, string key, string destinationFile)
        {
            try
            {   //Check if the destination file exist
                CheckDestinationFile(destinationFile);
                FileAttributes attr = File.GetAttributes(sourceFile);

                //detect whether its a directory or file
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    throw new Exception("Source path is a folder");
                }
                else if (!File.Exists(sourceFile))
                {
                    throw new Exception("Source path doesn't exist");
                }
                else
                {
                    string content = File.ReadAllText(sourceFile);
                    
                    StringBuilder szInputStringBuild = new StringBuilder(content);
                    StringBuilder szOutStringBuild = new StringBuilder(content.Length);
                    int keyLen = key.Length;
                    char Textch;
                    for (int iCount = 0; iCount < content.Length; iCount++)
                    {
                        Textch = szInputStringBuild[iCount];
                        Textch = (char)(Textch ^ key[iCount % keyLen]);
                        szOutStringBuild.Append(Textch);
                    }
                    File.WriteAllText(destinationFile, szOutStringBuild.ToString());
                    //Console.WriteLine("Encrypt Done !");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// method to check and then create the file if doesn't exist
        /// </summary>
        /// <param name="destinationFile"></param>
        private static void CheckDestinationFile(string destinationFile)
        {
            //Create Destination repo if doesn't exist
            if (File.Exists(destinationFile) == false)
            {
                File.Create(destinationFile).Close();
            }
        }
    }
}
