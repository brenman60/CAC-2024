using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private List<Toggle> levelUIs;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private UpgradeButton upgradeButton;

    private TimeTask assignedTask;
    private CanvasGroup canvasGroup;
    private bool visible;

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;

        assignedTask = GetComponentInParent<TimeTask>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        visible = GameManager.Instance.currentRoom == assignedTask.taskRoom;

        progressSlider.maxValue = assignedTask.maxTime;
        progressSlider.value = assignedTask.time;

        float currentMoney = SaveSystemManager.Instance.gameData.GetData<float, string>("Money");
        upgradeCostText.text = !assignedTask.isMaxLevel ? $"<color=#ebcf34>${Mathf.RoundToInt(assignedTask.upgradeCost)}</color>" : $"<color=#ebcf34>MAX</color>";
        upgradeButton.purchasable = currentMoney >= assignedTask.upgradeCost && !assignedTask.isMaxLevel;

        ReloadLevelUI();
        UpdateCanvasGroup();
    }

    private void ReloadLevelUI()
    {
        levelText.text = assignedTask.level.ToString();

        foreach (Toggle levelUI in levelUIs)
        {
            int level = int.Parse(levelUI.name);
            levelUI.isOn = level <= assignedTask.level;
        }
    }

    private void UpdateCanvasGroup()
    {
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, visible ? 1f : 0f, Time.deltaTime * 5f);
        canvasGroup.interactable = visible;
    }

    public void UpgradeTask()
    {
        if (!assignedTask.isMaxLevel)
        {
            float currentMoney = SaveSystemManager.Instance.gameData.GetData<float, string>("Money");
            SaveSystemManager.Instance.gameData.SetData("Money", currentMoney - assignedTask.upgradeCost);

            assignedTask.level++;
        }
    }
}
