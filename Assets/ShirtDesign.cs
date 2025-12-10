using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShirtDesign : MonoBehaviour
{
    public string playerName;
    public Image image;
    public TextMeshProUGUI playerNameText;
    public GameObject checkMark;

    private void Start() => playerNameText.text = playerName;

    public void SetResults(int[] results, string primaryHex, string secondaryHex)
    {
        // Create an empty object that stores the results
        GameObject shirtResults = new GameObject("ShirtResults");
        shirtResults.AddComponent<ShirtResults>();
        ShirtResults shirtScript = shirtResults.GetComponent<ShirtResults>();
        shirtScript.results = results;
        shirtScript.primaryHex = primaryHex;
        shirtScript.secondaryHex = secondaryHex;
        image.color = new Color(0.92f, 0.36f, .16f, 1);
        checkMark.SetActive(true);
    }
}
