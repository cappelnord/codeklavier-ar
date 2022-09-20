using UnityEngine;

namespace ARquatic.App {

[DefaultExecutionOrder(-1000)]
public class SetLanguage : MonoBehaviour
{
    void Awake()
    {
        ARAppUITexts.SwitchLanguage();
        Destroy(gameObject);
    }
}
}