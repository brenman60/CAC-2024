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
        return JsonConvert.SerializeObject(gameData, SaveSystem.serializeSettings);
    }

    public void PutSaveData(string saveData)
    {
        gameData = defaultGameData;
        if (saveData == null || string.IsNullOrEmpty(saveData)) { saveLoaded = true; return; }

        Dictionary<object, object> savedGameData = JsonConvert.DeserializeObject<Dictionary<object, object>>(saveData, SaveSystem.serializeSettings);
        foreach (KeyValuePair<object, object> savedData in savedGameData)
            gameData[savedData.Key] = savedData.Value;

        saveLoaded = true;
    }
}
