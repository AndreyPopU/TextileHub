using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Text.RegularExpressions;

public class FinishDesign : MonoBehaviour
{
    #region --Fields and properties--
    [SerializeField] MoneyManager moneyManager;
    [SerializeField] DesignFabric designFabric;
    [SerializeField] DesignPattern designPattern;
    [SerializeField] DesignChanger designChanger;
    [SerializeField] TMP_Text finalText; 
    #endregion

    public void SendDesign()
    {
        float budget = moneyManager.current_budget;
        if (budget < 0)
        {
            print("You're out of budget, you cannot send this item out");
        }
        else
        {
            int colar = designChanger.colar_index;
            int sleeves = designChanger.sleeves_index;
            int hem = designChanger.hem_index;


            // Transfer all properties to the manager

            // Collar, Sleeves, Bottom, Overlay, Material

            DesignChanger.instance.selectedProperties[0] = colar;
            DesignChanger.instance.selectedProperties[1] = sleeves;
            DesignChanger.instance.selectedProperties[2] = hem;
            DesignChanger.instance.selectedProperties[3] = designPattern.index;
            DesignChanger.instance.selectedProperties[4] = designFabric.index;

            // Transfer colors
            DesignChanger.instance.primaryHex = designChanger.color1Hex;
            DesignChanger.instance.secondaryHex = designChanger.color2Hex;

            for (int i = 0; i < DesignChanger.instance.selectedProperties.Length; i++)
            {
                Debug.Log($"{DesignChanger.instance.selectedProperties[i]}");
            }
                   
            DesignChanger.instance.FinishClothing();
        }
    }

    public void ShowText()
    {
        var pattern = @"\d";
        var message = designChanger.current_colour_1;
        var dye_1 = Regex.Match(message, pattern);
        message = designChanger.current_colour_1;
        var dye_2 = Regex.Match(message, pattern);

        finalText.text = "Fabric: " + designFabric.fabric_text + "<br>Dye type: " + dye_1 + "<br>Pattern: " + designPattern.pattern_text + "<br>Dye type: " + dye_2 + "<br>Money spend: " + (moneyManager.budget-moneyManager.current_budget);
    }
}
