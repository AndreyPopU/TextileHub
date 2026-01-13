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

    private ConsequenceResults results;

    private void Start()
    {
        results = FindFirstObjectByType<ConsequenceResults>();
        consequenceResults = new int[3];

        //// Convert score to consequence
        //for (int i = 0; i < results.results.Length; i++)
        //{
        //    if (results.results[i] > 8) consequenceResults[i] = 2;
        //    else if (results.results[i] <= 8 && results.results[i] >= 4) consequenceResults[i] = 1;
        //    else if (results.results[i] < 4) consequenceResults[i] = 2;
        //}

        // Fail-safe
        for (int i = 0; i < consequenceResults.Length; i++)
        {
            consequenceResults[i] = Random.Range(0, 3);
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
