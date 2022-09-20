using UnityEngine;
using UnityEngine.UI;

namespace ARquatic.App {

public class AdjustHeightToLayout : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float sum = 0;
        foreach(Transform t in transform)
        {
            RectTransform rect = (t.gameObject.GetComponent<RectTransform>());
            if(rect != null)
            {
                sum += rect.sizeDelta.y;
            }
        }

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, sum + 100f);
    }
}
}