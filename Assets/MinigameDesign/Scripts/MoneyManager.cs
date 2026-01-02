using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    [SerializeField] TextMeshProUGUI text;
    public int budget = 200;
    float pattern_cost = 0;
    float fabric_cost = 0;
    float color1_cost = 0;
    float color2_cost = 0;
    public float current_budget;

    private void Awake() => instance = this;

    public void Cost_Pattern(float _cost)
    {
        pattern_cost = _cost;
        Change_Money();
    }
    public void Cost_Fabric(float _cost)
    {
        fabric_cost = _cost;
        Change_Money();
    }
    public void Cost_color(float _cost1,float _cost2)
    {
        color1_cost = _cost1;
        color2_cost = _cost2;
        Change_Money();
    }

    void Change_Money()
    {
        current_budget = (budget - pattern_cost - fabric_cost - color1_cost - color2_cost);
        text.text = (current_budget) + " Euros";
    }
}
