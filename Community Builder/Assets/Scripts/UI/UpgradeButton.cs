using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button)), RequireComponent(typeof(CanvasGroup))]
public class UpgradeButton : MonoBehaviour
{
    public bool purchasable;

    [Header("Customization")]
    [SerializeField] private Color greenButtonColor = Color.green;
    [SerializeField] private Color redButtonColor = Color.red;
    [SerializeField] private TMP_ColorGradient greenTextGradient;
    [SerializeField] private TMP_ColorGradient redTextGradient;
    [Header("References")]
    [SerializeField] private Image buttonBackground;
    [SerializeField] private TextMeshProUGUI purchaseText;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        canvasGroup.interactable = purchasable;

        buttonBackground.color = purchasable ? greenButtonColor : redButtonColor;
        purchaseText.colorGradientPreset = purchasable ? greenTextGradient : redTextGradient;
    }
}
