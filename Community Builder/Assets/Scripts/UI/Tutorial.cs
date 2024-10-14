using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPage;
    [SerializeField] private Transform pagesHolder;

    private int currentIndex = 0;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("seenTutorial"))
        {
            tutorialPage.SetActive(true);
            PlayerPrefs.SetInt("seenTutorial", 0);
        }
    }

    public void ChangePage(int direction)
    {
        currentIndex += direction;
        if (currentIndex <= -1)
            currentIndex = pagesHolder.childCount - 1;
        else if (currentIndex >= pagesHolder.childCount)
            currentIndex = 0;

        foreach (Transform page in pagesHolder)
            page.gameObject.SetActive(false);

        pagesHolder.GetChild(currentIndex).gameObject.SetActive(true);
    }
}
