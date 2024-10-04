using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class GameData : ISaveData
{
    public Dictionary<object, object> gameData = new Dictionary<object, object>();

    public Dictionary<object, object> defaultGameData = new Dictionary<object, object>()
    {
        ["Money"] = 0f,
        ["TouchTimeIncrease"] = 1,
    };

    public bool saveLoaded { get; private set; }

    public T GetData<T, K>(K key)
    {
        object converted = Convert.ChangeType(gameData[key], typeof(T));
        return (T)converted;
    }

    public void SetData<K, V>(K key, V value)
    {
        gameData[key] = value;
    }

    public string GetSaveData()
    {
        string[] saveData = new string[2]
        {
            JsonConvert.SerializeObject(gameData, SaveSystem.serializeSettings),
            GameManager.Instance.GetSaveData(),
        };

        return JsonConvert.SerializeObject(saveData, SaveSystem.serializeSettings);
    }

    public void PutSaveData(string saveData)
    {
        gameData = defaultGameData;
        if (saveData == null || string.IsNullOrEmpty(saveData)) { saveLoaded = true; return; }

        string[] saveData_ = JsonConvert.DeserializeObject<string[]>(saveData, SaveSystem.serializeSettings);

        Dictionary<object, object> savedGameData = JsonConvert.DeserializeObject<Dictionary<object, object>>(saveData_[0], SaveSystem.serializeSettings);
        foreach (KeyValuePair<object, object> savedData in savedGameData)
            gameData[savedData.Key] = savedData.Value;

        GameManager.Instance.PutSaveData(saveData_[1]);

        saveLoaded = true;
    }
}
