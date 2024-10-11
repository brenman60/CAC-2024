using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class NewTask : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] private string costColor = "#ebcf34";

    [Header("References")]
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private UpgradeButton upgradeButton;

    private void Update()
    {
        float currentMoney = SaveSystemManager.Instance.gameData.GetData<float, string>("Money");
        TimeTask nextTask =  GameManager.Instance.currentRoom.HasUnboughtTask();
        upgradeButton.purchasable = nextTask != null && currentMoney >= nextTask.purchaseCost;

        UpdateCostText(nextTask);
    }

    private void UpdateCostText(TimeTask nextTask)
    {
        if (nextTask != null)
            costText.text = $"<color={costColor}>${nextTask.purchaseCost}</color>";
        else
            costText.text = $"<color={costColor}>MAX</color>";
    }

    public void PurchaseTask()
    {
        RoomManager currentRoom = GameManager.Instance.currentRoom;
        currentRoom.BuyTask();
    }
}
