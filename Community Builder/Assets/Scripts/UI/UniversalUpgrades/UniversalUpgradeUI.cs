using TMPro;
using UnityEngine;

public class UniversalUpgradeUI : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] private UniversalUpgrade upgrade;
    [SerializeField] private string currentLevelColor = "#34c6eb";
    [SerializeField] private string nextLevelColor = "#34ebc0";
    [SerializeField] private string costColor = "#ebcf34";
    [SerializeField] private float firstCost = 50f;
    [SerializeField] private float costScaling = 0.5f;
    [Header("References")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private CanvasGroup purchaseButtonCanvasGroup;

    private float cost;

    private void Update()
    {
        if (SaveSystemManager.loaded)
            UpdateLevelText();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float currentMoney = SaveSystemManager.Instance.gameData.GetData<float, string>("Money");
            SaveSystemManager.Instance.gameData.SetData("Money", currentMoney + 10f);
        }
    }

    private void UpdateLevelText()
    {
        int currentLevel = SaveSystemManager.Instance.gameData.GetData<int, UniversalUpgrade>(upgrade);
        print(currentLevel);
        cost = Mathf.Round(firstCost * Mathf.Pow(1 + costScaling, currentLevel));
        levelText.text = $"<color={currentLevelColor}>{currentLevel}</color> -> <color={nextLevelColor}>{currentLevel + 1}</color> | <color={costColor}>${cost}</color>";

        float currentMoney = SaveSystemManager.Instance.gameData.GetData<float, string>("Money");
        purchaseButtonCanvasGroup.interactable = currentMoney >= cost;
    }

    public void PurchaseUpgrade()
    {
        int currentLevel = SaveSystemManager.Instance.gameData.GetData<int, UniversalUpgrade>(upgrade);
        SaveSystemManager.Instance.gameData.SetData(upgrade, currentLevel + 1);

        float currentMoney = SaveSystemManager.Instance.gameData.GetData<float, string>("Money");
        SaveSystemManager.Instance.gameData.SetData("Money", currentMoney - cost);
    }
}
