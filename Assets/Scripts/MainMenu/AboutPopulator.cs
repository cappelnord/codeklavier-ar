using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARquatic.App {

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
        ARAppUITexts.PopulateAbout(this);
    }

    public void Headline(string text)
    {
        GameObject obj = Instantiate(HeadlinePrefab, Content);
        obj.GetComponent<TextMeshProUGUI>().text = text;

        Instantiate(HeadlineDividerPrefab, Content);
    }

    public void Paragraph(string text)
    {
        GameObject obj = Instantiate(ParagraphPrefab, Content);
        obj.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void LinkButton(string text, string url)
    {
        GameObject obj = Instantiate(MoreInformationButtonPrefab, Content);
        Button button = obj.GetComponent<Button>();

        obj.transform.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true)[0].text = text;

        button.onClick.AddListener(delegate () {
            Application.OpenURL(url);
        });
    }

    public void ParagraphDivider()
    {
        Instantiate(ParagraphDividerPrefab, Content);
    }
}
}