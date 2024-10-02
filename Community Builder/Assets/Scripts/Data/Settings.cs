using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Settings : ISaveData
{
    public Dictionary<SettingType, object> settings = new Dictionary<SettingType, object>();

    private Dictionary<SettingType, object> defaultSettings { get; } = new Dictionary<SettingType, object>()
    {

    };

    public bool saveLoaded { get; private set; }

    public PlayerInput playerInput { get; private set; }
    public InputAction inputClick { get; private set; }

    public Settings()
    {
        foreach (InputDevice device in InputSystem.devices)
            InputSystem.EnableDevice(device);  

        playerInput = new PlayerInput();
        playerInput.Enable();

        inputClick = playerInput.Mouse.Click;
        inputClick.Enable();
    }

    ~Settings()
    {
        playerInput.Disable();
        inputClick.Disable();
    }

    public string GetSaveData()
    {
        return JsonConvert.SerializeObject(settings, SaveSystem.serializeSettings);
    }

    public void PutSaveData(string saveData)
    {
        settings = defaultSettings;
        if (saveData == null || string.IsNullOrEmpty(saveData)) { saveLoaded = true; return; }

        Dictionary<SettingType, object> savedSettings = JsonConvert.DeserializeObject<Dictionary<SettingType, object>>(saveData, SaveSystem.serializeSettings);
        foreach (KeyValuePair<SettingType, object> savedSetting in savedSettings)
            if (settings.ContainsKey(savedSetting.Key))
                settings[savedSetting.Key] = savedSetting.Value;

        saveLoaded = true;
    }
}

public enum SettingType
{

}
