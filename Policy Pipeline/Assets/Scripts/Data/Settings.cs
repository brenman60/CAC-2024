using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Settings : ISaveData
{
    public Dictionary<SettingType, object> settings = new Dictionary<SettingType, object>()
    {
    };

    public PlayerInput playerInput { get; private set; }
    public InputAction inputTouch { get; private set; }

    private readonly JsonSerializerSettings serializeSettings = new JsonSerializerSettings()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
    };

    public Settings()
    {
        foreach (InputDevice device in InputSystem.devices)
            InputSystem.EnableDevice(device);  

        playerInput = new PlayerInput();
        playerInput.Enable();

        inputTouch = playerInput.Mouse.Click;
        inputTouch.Enable();
    }

    ~Settings()
    {
        inputTouch.Disable();
    }

    public string GetSaveData()
    {
        return JsonConvert.SerializeObject(settings, serializeSettings);
    }

    public void PutSaveData(string saveData)
    {
        if (saveData == null) return;

        settings = JsonConvert.DeserializeObject<Dictionary<SettingType, object>>(saveData, serializeSettings);
    }
}

public enum SettingType
{

}
