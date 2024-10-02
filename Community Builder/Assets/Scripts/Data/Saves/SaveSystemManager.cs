using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{
    private static SaveSystemManager Instance_;
    public static SaveSystemManager Instance 
    {
        get
        {
            if (Instance_ == null)
            {
                Instance_ = Instantiate(Resources.Load<SaveSystemManager>("Utils/SaveSystemManager"));
                Instance_.InitManagers();
            }

            return Instance_;
        }
    }

    public static bool loaded
    {
        get
        {
            return Instance.loaded_;
        }
    }

    public Settings settings { get; private set; }
    public GameData gameData { get; private set; }

    private bool loaded_ = false;

    private float queuedSave = 0;

    private async void InitManagers()
    {
        settings = new Settings();
        settings.PutSaveData(await SaveSystem.ReadFromFile(PresetFile.Settings));

        gameData = new GameData();
        gameData.PutSaveData(await SaveSystem.ReadFromFile(PresetFile.GameData));

        loaded_ = true;
    }

    private async void OnApplicationQuit()
    {
        await SaveSystem.WriteToFile(PresetFile.Settings, settings.GetSaveData());
        await SaveSystem.WriteToFile(PresetFile.GameData, gameData.GetSaveData());
    }
}
