using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsequenceManager : MonoBehaviour
{
    [Header("Consequences")]
    public int currentConsequence;
    public int score;
    public GameObject[] consequences;
    public TextMeshProUGUI[] descriptionTexts;
    public Sprite[] consequenceSprites;
    public string[] consequenceDescriptions;

    [Header("Final Results")]
    public GameObject[] finalResults;
    public int[] consequenceResults;
    public ShirtResults shirtResults;

    private ConsequenceResults results;

    private void Start()
    {
        // Find results
        results = FindFirstObjectByType<ConsequenceResults>();
        shirtResults = FindFirstObjectByType<ShirtResults>();
        consequenceResults = new int[3];

        // Adjust score
        results.results[0] /= 2;
        results.results[1] = results.results[1] / 2 + 1;

        // Adjust for dye
        if (shirtResults.isCO2) results.results[2] += 8;
        else if (shirtResults.isAzo) results.results[2] += 2;
        else results.results[2] += 6;

        // Adjust for fabric
        // Material order: Cotton, Polyester, Silk, Linen, Wool
        switch (shirtResults.results[4])
        {
            case 0: results.results[2] += 7; break;
            case 1: results.results[2] += 4; break;
            case 2: results.results[2] += 6; break;
            case 3: results.results[2] += 9; break;
            case 4: results.results[2] += 7; break;
        }

        // Adjust overall
        results.results[2] /= 4;

        // Convert score to consequence
        for (int i = 0; i < results.results.Length; i++)
        {
            Debug.Log($"Result {i} - {results.results[i]}");
            if (results.results[i] > 8) consequenceResults[i] = 2;
            else if (results.results[i] <= 8 && results.results[i] >= 4) consequenceResults[i] = 1;
            else if (results.results[i] < 4) consequenceResults[i] = 2;
        }
    }

    public void ShowNextConsequence()
    {
        if (currentConsequence >= 3) return;

        consequences[currentConsequence].gameObject.SetActive(true);
        consequences[currentConsequence].GetComponent<Image>().sprite = consequenceSprites[3 * currentConsequence + consequenceResults[currentConsequence]];
        descriptionTexts[currentConsequence].text = consequenceDescriptions[3 * currentConsequence + consequenceResults[currentConsequence]];
        currentConsequence++;
    }

    public void FinalResults()
    {
        for (int i = 0; i < finalResults.Length; i++)
            finalResults[i].GetComponent<Image>().sprite = consequenceSprites[3 * i + consequenceResults[i]];
    }
}
