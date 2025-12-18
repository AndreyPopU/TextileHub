using Unity.VisualScripting;
using UnityEngine;

public class FinishDesign : MonoBehaviour
{
    [SerializeField] MoneyManager moneyManager;
    [SerializeField] DesignFabric designFabric;
    [SerializeField] DesignPattern designPattern;
    [SerializeField] DesignChanger designChanger;

    public void finish_button()
    {
        float budget = moneyManager.current_budget;
        if (budget <= 0)
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
            DesignChanger.instance.selectedProperties[3] = designPattern.paternIndex;
            DesignChanger.instance.selectedProperties[4] = designFabric.fabricIndex;

            // Transfer colors
            DesignChanger.instance.primaryHex = designChanger.color1Hex;
            DesignChanger.instance.secondaryHex = designChanger.color2Hex;

            for (int i = 0; i < DesignChanger.instance.selectedProperties.Length; i++)
            {
                Debug.Log($"{DesignChanger.instance.selectedProperties[i]}");
            }

            print(DesignChanger.instance.primaryHex);
            print(DesignChanger.instance.secondaryHex);
                   
            DesignChanger.instance.FinishClothing();
        }
    }
}
