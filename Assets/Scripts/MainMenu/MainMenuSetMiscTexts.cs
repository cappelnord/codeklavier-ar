
using UnityEngine;
using TMPro;

namespace ARquatic.App {

public class MainMenuSetMiscTexts : MonoBehaviour
{
    public TextMeshProUGUI AvailableChannelsLabel;
    public TextMeshProUGUI InformationLabel;
    public TextMeshProUGUI RefreshButtonLabel;

    void Start()
    {
        AvailableChannelsLabel.text = ARAppUITexts.MainMenuAvailableChannelsLabel;
        InformationLabel.text = ARAppUITexts.MainMenuInformationLabel;
        RefreshButtonLabel.text = ARAppUITexts.ButtonRefresh;

        Destroy(gameObject);
    }
}
}
