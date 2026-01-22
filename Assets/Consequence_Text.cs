using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class Consequence_Text : MonoBehaviour
{
    public ShirtResults shirtResults;

    [SerializeField] TMP_Text finalText;

    private string dye;
    private string fabric_text;

    private void Start()
    {
        shirtResults = FindFirstObjectByType<ShirtResults>();
    }

    public void Show_Text() 
    {
        switch (shirtResults.results[4])
        {
            case 0: fabric_text = "Cotton"; break;
            case 1: fabric_text = "Polyester"; break;
            case 2: fabric_text = "Silk"; break;
            case 3: fabric_text = "Linen"; break;
            case 4: fabric_text = "Wool"; break;
        }
        if (shirtResults.isCO2) dye = "CO2";
        else if (shirtResults.isAzo) dye = "Azo";
        else dye = "Reactive";
        int budget = 200;
        int current_budget = 0;

        finalText.text = "Fabric: " + fabric_text + "<br>Dye type: " + dye + "<br>Money spend: " + (budget - current_budget);
    }
}
