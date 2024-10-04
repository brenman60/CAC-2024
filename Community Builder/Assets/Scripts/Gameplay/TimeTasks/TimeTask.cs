using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class TimeTask : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] private float completionTime = 5f;
    [SerializeField] private UnityEvent completionEvent;

    public float time { get; protected set; }
    public float maxTime { get { return completionTime; } }
    protected bool paused;

    protected virtual void Awake()
    {
        GameManager.screenTapped += ScreenTapped;
    }

    private void ScreenTapped(object sender, System.EventArgs e)
    {
        int touchTimeIncrease = SaveSystemManager.Instance.gameData.GetData<int, string>("TouchTimeIncrease");
        IncreaseTime(touchTimeIncrease);
    }

    protected virtual void Update()
    {
        if (!paused)
            time += Time.deltaTime;

        CheckTime();
    }

    private void CheckTime()
    {
        if (time >= completionTime)
        {
            time = 0f;
            completionEvent?.Invoke();
        }
    }

    public void IncreaseTime(float increase)
    {
        time += increase;
    }
}
