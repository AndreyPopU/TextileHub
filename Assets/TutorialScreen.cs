using UnityEngine;
using TMPro;

public class TutorialScreen : MonoBehaviour
{
    public string OverlayText;
    [SerializeField] GameObject TutorialOverlay;
    [SerializeField] TextMeshProUGUI tutorialText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorialText.text = OverlayText;
        Destroy(TutorialOverlay, 15f);
    }

}
