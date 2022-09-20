using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ARquatic.App {

public class AdjustHeightToText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
        Bounds textBounds = GetComponent<TextMeshProUGUI>().textBounds;
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, textBounds.size.y + 40f);
    }
}
}