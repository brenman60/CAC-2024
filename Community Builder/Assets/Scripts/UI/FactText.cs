using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;

public class FactText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI factText;

    private static readonly ReadOnlyCollection<string> facts = new ReadOnlyCollection<string>(new List<string>()
    {
        @"""Individuals who regularly volunteer have a 27% higher chance of finding employment"" - doublethedonation.com",
        @"""Volunteers are worth an average $28.54 an hour according to an Independent Sector Study"" - volunteerhub.com",
        @"""Those who volunteer regularly have a 27% better chance of gaining employment"" - volunteerhub.com",
        @"""According to an AmeriCorps report, people who volunteer over 100 hours a year are some of the healthiest people in the U.S"" - volunteerhub.com",
        @"""Volunteers experience 38% fewer nights in the hospital"" - doublethedonation.com",
        @"""80% of companies offer volunteer grants worth between $8-$15 per hour volunteered"" - doublethedonation.com",
        @"""Volunteering has been linked to reduced mortality in older adults"" - rosterfy.com",
        @"""78% of volunteers who were surveyed by the United Health Group said volunteering reduced their stress levels"" - rosterfy.com",
        @"""70% of corporate volunteers believe volunteering boosts morale in the workplace better than company events do"" - rosterfy.com",
        @"""Unemployed volunteers are more likely to find work than non-volunteers"" - teamstage.io",
        @"""The average volunteer in the US donates $1,456 worth of time each year"" - socialjusticeresourcecenter.org",
    });

    private RectTransform factTextRect;
    private float factCooldown;

    private void Start()
    {
        factTextRect = factText.GetComponent<RectTransform>();
        factCooldown = Random.Range(5f, 10f);
    }

    private void Update()
    {
        factTextRect.anchoredPosition -= new Vector2(25f, 0f) * Time.deltaTime;

        factCooldown -= Time.deltaTime;
        if (factCooldown <= 0)
        {
            factCooldown = 10f;

            factText.text = facts[Random.Range(0, facts.Count)];
            factTextRect.anchoredPosition = new Vector2(factTextRect.sizeDelta.x / 2f, 0f);

            factCooldown += factText.text.Length;
        }
    }
}
