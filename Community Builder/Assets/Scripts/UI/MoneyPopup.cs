using TMPro;
using UnityEngine;

public class MoneyPopup : MonoBehaviour
{
    public float money;

    [Header("Customization")]
    [SerializeField] private float showingTime = 2f;
    [SerializeField] private float disappearSpeed = 2f;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI moneyText;

    float existanceTime = 0;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        moneyText.text = "$" + Mathf.RoundToInt(money);

        transform.position += new Vector3(0, 0.1f) * Time.deltaTime;

        existanceTime += Time.deltaTime;
        float alpha = 1 - ((existanceTime - showingTime) / disappearSpeed);
        canvasGroup.alpha = Mathf.Lerp(0f, 1f, Mathf.Clamp01(alpha));

        if (canvasGroup.alpha == 0f)
            Destroy(gameObject);
    }
}
