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

    private ConsequenceResults results;

    private void Start() => results = FindFirstObjectByType<ConsequenceResults>();

    public void ShowNextConsequence()
    {
        if (currentConsequence >= 3) return;

        consequences[currentConsequence].gameObject.SetActive(true);
        consequences[currentConsequence].GetComponent<Image>().sprite = consequenceSprites[3 * currentConsequence + results.results[currentConsequence]];
        descriptionTexts[currentConsequence].text = consequenceDescriptions[3 * currentConsequence + results.results[currentConsequence]];
        currentConsequence++;
    }

    public void FinalResults()
    {
        for (int i = 0; i < finalResults.Length; i++)
            finalResults[i].GetComponent<Image>().sprite = consequenceSprites[3 * i + results.results[i]];
    }

    // Convert 0-10 votes to bad, neutral, best
}
