using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CryptoSoft
{
    class EncryptKey
    {
        public string key { get; set; }
        private readonly string _appDataPath;
        private readonly string _keyFilePath;
        private readonly string _configPath;
        /// <summary>
        /// Object to get the key store in the config file
        /// </summary>
        public EncryptKey()
        {
            Environment.SetEnvironmentVariable("appDataPath",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CryptoSoft");
            _appDataPath = Environment.GetEnvironmentVariable("appDataPath");
            _keyFilePath = Environment.GetEnvironmentVariable("appDataPath") + @"\config\key.json";
            _configPath = Environment.GetEnvironmentVariable("appDataPath") + @"\config";
            //Console.WriteLine(_keyFilePath);
            CheckAndPrepareEnvironment();
            LoadConfig();
        }
        /// <summary>
        /// Method to load the key from the config file
        /// </summary>
        private void LoadConfig()
        {
            CheckAndPrepareConfigKeyFile();
            key = File.ReadAllText(_keyFilePath);
        }
        /// <summary>
        /// Method to check if the key config file exist and is usable
        /// </summary>
        public void CheckAndPrepareConfigKeyFile()
        {
            // create file if doesn't exist
            if (!File.Exists(_keyFilePath))
            {
                File.Create(_keyFilePath).Close();
                //Console.WriteLine("file created");
            }
            // check file isn't empty
            if (File.ReadAllText(_keyFilePath) == "")
            {
                File.WriteAllText(_keyFilePath, JsonSerializer.Serialize(key));
                //Console.WriteLine("file write in");
            }
            else
            {
                // check if file is usable
                try
                {
                    File.ReadAllText(_keyFilePath);
                    //Console.WriteLine("file is usable");
                }
                catch (Exception)
                {
                    //Console.WriteLine(e);
                    int i = 0;
                    while (File.Exists(_configPath + @"\notReadableSavesFile_" + i + ".json"))
                    {
                        i++;
                    }
                    File.Move(_keyFilePath, _configPath + @"\notReadableSavesFile_" + i + ".json");
                    //Console.WriteLine("new file + stock old file");
                    CheckAndPrepareConfigKeyFile();
                }
            }
        }
        /// <summary>
        /// Method to prepare the AppData environment
        /// </summary>
        public void CheckAndPrepareEnvironment()
        {
            if (Directory.Exists(_appDataPath))
            {
                CheckForOneDataType("key","config");
            }
            else
            {
                Directory.CreateDirectory(_appDataPath);
                Directory.CreateDirectory(_appDataPath + @"\config");
                File.Create(_appDataPath + @"\config\key.json").Close();
            }
        }
        /// <summary>
        /// Method to specify more config file to check and create if doesn't exist
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="directoryName"></param>
        private void CheckForOneDataType(string fileName, string directoryName)
        {
            if (File.Exists(_appDataPath + @"\" + directoryName + @"\" + fileName + ".json")) return;
            if (!Directory.Exists(_appDataPath + @"\" + fileName))
            {
                Directory.CreateDirectory(_appDataPath + @"\" + fileName);
            }
            File.Create(_appDataPath + @"\" + directoryName + @"\" + fileName + ".json").Close();
        }
    }
}
