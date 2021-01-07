using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AboutPopulator : MonoBehaviour
{
    public Transform Content;

    public GameObject HeadlinePrefab;
    public GameObject HeadlineDividerPrefab;
    public GameObject ParagraphPrefab;
    public GameObject MoreInformationButtonPrefab;
    public GameObject ParagraphDividerPrefab;

    void Start()
    {
        Clean();
        Populate();
    }

    public void Clean()
    {
        foreach (Transform t in Content)
        {
            Destroy(t.gameObject);
        }
    }

    private void Populate()
    {
        Headline("About");
        Paragraph("This can be a short text about the app, the project and everything else ... Let's keep it short!");
        LinkButton("CodeKlavier Homepage", "https://codeklavier.space/");
        
        ParagraphDivider();
        
        Headline("Credits");
        Paragraph("Best Piano Lady\nSweet Prince\nWhiskey the Cat\nGerman Moustache Dude");

        ParagraphDivider();

        Headline("Juan Appreciation");
        Paragraph("Juan is a nice dude. He should have initially been part of the project but could not unfortunately because he is now Corporate Juan.");
    }

    private void Headline(string text)
    {
        GameObject obj = Instantiate(HeadlinePrefab, Content);
        obj.GetComponent<TextMeshProUGUI>().text = text;

        Instantiate(HeadlineDividerPrefab, Content);
    }

    private void Paragraph(string text)
    {
        GameObject obj = Instantiate(ParagraphPrefab, Content);
        obj.GetComponent<TextMeshProUGUI>().text = text;
    }

    private void LinkButton(string text, string url)
    {
        GameObject obj = Instantiate(MoreInformationButtonPrefab, Content);
        Button button = obj.GetComponent<Button>();

        obj.transform.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true)[0].text = text;

        button.onClick.AddListener(delegate () {
            Application.OpenURL(url);
        });
    }

    private void ParagraphDivider()
    {
        Instantiate(ParagraphDividerPrefab, Content);
    }
}
