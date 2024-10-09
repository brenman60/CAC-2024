using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{
    private static SaveSystemManager Instance_;
    public static SaveSystemManager Instance 
    {
        get
        {
            if (Instance_ == null)
                Instance_ = Instantiate(Resources.Load<SaveSystemManager>("Utils/SaveSystemManager"));

            if (!Instance_.loaded_)
                Instance_.InitManagers();

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

    private Settings settings_;
    private GameData gameData_;

    public Settings settings 
    {
        get
        {
            if (settings_ == null)
                return new Settings();

            return settings_;
        }
        private set
        {
            settings_ = value;
        }
    }
    public GameData gameData 
    {
        get
        {
            if (gameData_ == null)
                return new GameData();

            return gameData_;
        } 
        private set
        {
            gameData_ = value;
        }
    }

    private bool loaded_ = false;

    private float queuedSave = 0;

    private async void InitManagers()
    {
        if (loaded_) return;
        loaded_ = true;

        settings = new Settings();
        settings.PutSaveData(await SaveSystem.ReadFromFile(PresetFile.Settings));

        gameData = new GameData();
        gameData.PutSaveData(await SaveSystem.ReadFromFile(PresetFile.GameData));
    }

    private async void OnApplicationQuit()
    {
        await SaveSystem.WriteToFile(PresetFile.Settings, settings.GetSaveData());
        await SaveSystem.WriteToFile(PresetFile.GameData, gameData.GetSaveData());
    }
}
