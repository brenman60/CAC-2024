using TMPro;
using UnityEngine;

public class FundingText : MonoBehaviour
{
    [SerializeField] private string moneyColor = "#ebcf34";

    private TextMeshProUGUI fundingText;

    private void Start()
    {
        fundingText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!SaveSystemManager.loaded) return;

        float currentMoney = SaveSystemManager.Instance.gameData.GetData<float, string>("Money");
        fundingText.text = $"Funding: <color={moneyColor}>${Mathf.Round(currentMoney)}</color>";
    }
}
