using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private List<Toggle> levelUIs;

    private TimeTask assignedTask;

    private void Start()
    {
        assignedTask = GetComponentInParent<TimeTask>();

        assignedTask.levelChanged += ReloadLevelUI;
        ReloadLevelUI();
    }

    private void Update()
    {
        progressSlider.maxValue = assignedTask.maxTime;
        progressSlider.value = assignedTask.time;
    }

    private void ReloadLevelUI()
    {
        foreach (Toggle levelUI in levelUIs)
        {
            int level = int.Parse(levelUI.name);
            levelUI.isOn = level <= assignedTask.level;
        }
    }
}
