using Newtonsoft.Json;
using System;
using UnityEngine;

public class TimeTask : MonoBehaviour, ISaveData
{
    [Header("Customization")]
    public float purchaseCost = 100f;
    [SerializeField] private float level1Cost = 100;
    [SerializeField] private float completionTime = 5f;
    [SerializeField] protected float defaultEarnings = 1f;
    [SerializeField] private float costGrowthFactor = 1.5f;

    [Header("References")]
    [SerializeField] private GameObject moneyPopupPrefab;
    [SerializeField] private Transform moneyPopupPoint;

    public event Action levelChanged;

    private int level_ = 1;
    public int level
    {
        get { return level_; }
        set
        {
            levelChanged?.Invoke();
            level_ = Mathf.Clamp(value, 1, maxLevel);
        }
    }

    public float upgradeCost
    {
        get
        {
            // 100 is base price
            // basically interest rate formula but for cost
            return level1Cost * Mathf.Pow(costGrowthFactor, level_ - 1);
        }
    }

    public const int maxLevel = 10;
    public bool isMaxLevel
    {
        get
        {
            return level >= maxLevel;
        }
    }

    public float time { get; protected set; }
    public float maxTime { get { return completionTime; } }
    protected bool paused;

    public RoomManager taskRoom { get; protected set; }

    protected virtual void Awake()
    {
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
        float earnings = defaultEarnings * Mathf.Pow(level, 1.5f) + UnityEngine.Random.Range(-2, 3);
        SaveSystemManager.Instance.gameData.SetData("Money", earnings + currentMoney);

        MoneyPopup moneyPopup = Instantiate(moneyPopupPrefab).GetComponent<MoneyPopup>();
        moneyPopup.money = earnings;
        moneyPopup.transform.position = moneyPopupPoint.position + new Vector3(UnityEngine.Random.Range(0, 2) == 0 ? -1.5f : 1.5f, -0.25f, -0.1f);

        SoundManager.Instance.PlayAudio("MoneyCollect", true, 0.1f, moneyPopupPoint);
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
