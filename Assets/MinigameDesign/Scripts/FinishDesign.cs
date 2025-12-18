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
            string fabric = designFabric.current_fabric;
            string pattern = designPattern.current_pattern;
            print(budget + ", " + fabric + ", " + pattern);

            int colar = designChanger.colar_index;
            int sleeves = designChanger.sleeves_index;
            int hem = designChanger.hem_index;
            print(colar + ", " + sleeves + ", " + hem);

            string colour1 = designChanger.current_colour_1;
            string colour2 = designChanger.current_colour_2;
            print(colour1 + ", " + colour2);
        }
    }
}
