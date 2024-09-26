using System.IO;
using System.Threading.Tasks;
using System;
using UnityEngine;
using AESEncryption;

public static class SaveSystem
{
    public static string mainPath
    {
        get
        {
            return Application.persistentDataPath;
        }
    }

    public static string settingsPath
    {
        get
        {
            return Path.Combine(mainPath, settingFileName + defaultExtension);
        }
    }

    public static string gameDataPath
    {
        get
        {
            return Path.Combine(mainPath, gameDataFileName + defaultExtension);
        }
    }

    private const string defaultExtension = ".polipip";
    private const string settingFileName = "settings";
    private const string gameDataFileName = "gData";

    public static async Task<bool> WriteToFile(string filePath, string data)
    {
        try
        {
            using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string encrypedData = AesOperation.EncryptString(data);
                    await writer.WriteAsync(encrypedData);

                    return true;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error writing to file with path '" + filePath + "': " + e.GetBaseException());
            return false;
        }
    }

    public static async Task<string> ReadFromFile(string filePath)
    {
        try
        {
            using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string rawData = await reader.ReadToEndAsync();
                    string unencrypted = await AesOperation.DecryptString(rawData);

                    return unencrypted;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading from file '" + filePath + "': " + e.GetBaseException());
            return null;
        }
    }
}
