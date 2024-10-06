using Newtonsoft.Json;
using System;
using UnityEngine;

public class TimeTask : MonoBehaviour, ISaveData
{
    [Header("Customization")]
    [SerializeField] private float completionTime = 5f;
    [SerializeField] protected float defaultEarnings = 1f;

    public event Action levelChanged;

    private int level_ = 1;
    public int level
    {
        get { return level_; }
        private set
        {
            levelChanged?.Invoke();
            level_ = value;
        }
    }

    public float time { get; protected set; }
    public float maxTime { get { return completionTime; } }
    protected bool paused;

    protected RoomManager taskRoom;

    protected virtual void Awake()
    {
        time = UnityEngine.Random.Range(0f, completionTime);
        GameManager.screenTapped += ScreenTapped;
    }

    protected virtual void Start()
    {
        taskRoom = GetComponentInParent<RoomManager>();
    }

    private void ScreenTapped(object sender, RoomManager currentRoom)
    {
        if (currentRoom != taskRoom) return;
        if (!SaveSystemManager.loaded) return;

        int touchTimeIncrease = SaveSystemManager.Instance.gameData.GetData<int, string>("TouchTimeIncrease");
        IncreaseTime(touchTimeIncrease);
    }

    protected virtual void Update()
    {
        if (!paused)
            time += Time.deltaTime * Mathf.Clamp(level / 1.5f, 0, int.MaxValue);

        CheckTime();
    }

    private void CheckTime()
    {
        if (time >= completionTime)
        {
            time = 0f;
            OnCompletion();
        }
    }

    public void IncreaseTime(float increase)
    {
        time += increase;
    }

    public virtual void OnCompletion()
    {
        if (!SaveSystemManager.loaded) return;

        float currentMoney = SaveSystemManager.Instance.gameData.GetData<float, string>("Money");
        SaveSystemManager.Instance.gameData.SetData("Money", currentMoney + (defaultEarnings * Mathf.Pow(level, 1.5f)));
    }

    public string GetSaveData()
    {
        string[] dataPoints = new string[2]
        {
            time.ToString(),
            level_.ToString(),
        };

        return JsonConvert.SerializeObject(dataPoints, SaveSystem.serializeSettings);
    }

    public void PutSaveData(string saveData)
    {
        string[] dataPoints = JsonConvert.DeserializeObject<string[]>(saveData, SaveSystem.serializeSettings);
        time = float.Parse(dataPoints[0]);
        level_ = int.Parse(dataPoints[1]);
    }
}
