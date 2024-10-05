using Newtonsoft.Json;
using UnityEngine;

public class TimeTask : MonoBehaviour, ISaveData
{
    [Header("Customization")]
    [SerializeField] private float completionTime = 5f;
    [SerializeField] protected float defaultEarnings = 1f;

    public int speedLevel { get; private set; } = 1;
    public int earningsLevel { get; private set; } = 1;

    public float time { get; protected set; }
    public float maxTime { get { return completionTime; } }
    protected bool paused;

    protected RoomManager taskRoom;

    protected virtual void Awake()
    {
        time = Random.Range(0f, completionTime);
        GameManager.screenTapped += ScreenTapped;
    }

    protected virtual void Start()
    {
        taskRoom = GetComponentInParent<RoomManager>();
    }

    private void ScreenTapped(object sender, RoomManager currentRoom)
    {
        if (currentRoom != taskRoom) return;

        int touchTimeIncrease = SaveSystemManager.Instance.gameData.GetData<int, string>("TouchTimeIncrease");
        IncreaseTime(touchTimeIncrease);
    }

    protected virtual void Update()
    {
        if (!paused)
            time += Time.deltaTime * speedLevel;

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
        float currentMoney = SaveSystemManager.Instance.gameData.GetData<float, string>("Money");
        SaveSystemManager.Instance.gameData.SetData("Money", currentMoney + (defaultEarnings * earningsLevel));
    }

    public string GetSaveData()
    {
        string[] dataPoints = new string[3]
        {
            time.ToString(),
            speedLevel.ToString(),
            earningsLevel.ToString(),
        };

        return JsonConvert.SerializeObject(dataPoints, SaveSystem.serializeSettings);
    }

    public void PutSaveData(string saveData)
    {
        string[] dataPoints = JsonConvert.DeserializeObject<string[]>(saveData, SaveSystem.serializeSettings);
        time = float.Parse(dataPoints[0]);
        speedLevel = int.Parse(dataPoints[1]);
        earningsLevel = int.Parse(dataPoints[2]);
    }
}
