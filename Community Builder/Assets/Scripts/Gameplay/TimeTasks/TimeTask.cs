using UnityEngine;
using UnityEngine.Events;

public abstract class TimeTask : MonoBehaviour
{
    [SerializeField] private float completionTime = 5f;
    [SerializeField] private UnityEvent completionEvent;

    public float time { get; protected set; }
    protected bool paused;

    protected virtual void Awake()
    {
        GameManager.screenTapped += ScreenTapped;
    }

    private void ScreenTapped(object sender, System.EventArgs e)
    {
        int touchTimeIncrease = SaveSystemManager.Instance.gameData.GetData<int, UniversalUpgrade>(UniversalUpgrade.TouchTimeIncrease);
        IncreaseTime(touchTimeIncrease);
    }

    protected virtual void Update()
    {
        if (!paused)
            time += Time.deltaTime;


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
