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

    public Settings settings;

    private float queuedSave = 0;

    private async void InitManagers()
    {
        settings = new Settings();
        settings.PutSaveData(await SaveSystem.ReadFromFile(PresetFile.Settings));
    }
}
