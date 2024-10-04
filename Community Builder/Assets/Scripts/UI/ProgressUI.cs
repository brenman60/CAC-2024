using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider progressSlider;

    private TimeTask assignedTask;

    private void Start()
    {
        assignedTask = GetComponentInParent<TimeTask>();
    }

    private void Update()
    {
        progressSlider.maxValue = assignedTask.maxTime;
        progressSlider.value = assignedTask.time;
    }
}
