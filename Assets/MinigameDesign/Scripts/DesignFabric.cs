using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DesignFabric : MonoBehaviour
{
    [SerializeField] GameObject[] fabrics;

    [SerializeField] GameObject[] fabric_cotton;
    [SerializeField] GameObject[] fabric_polyester;
    [SerializeField] GameObject[] fabric_silk;
    [SerializeField] GameObject[] fabric_linen;
    [SerializeField] GameObject[] fabric_wool;
    [SerializeField] MoneyManager moneyManager;

    public string current_fabric;

    private void Start()
    {
        Fabric_Cotton(true);
    }

    void Money(float _cost)
    {
        moneyManager.Cost_Fabric(_cost);
    }

    public void Fabric_Cotton(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(50);
            current_fabric = "Cotton";

            foreach (GameObject item in fabric_cotton)
            {
                item.SetActive(true);
            }
        }
    }

    public void Fabric_Polyester(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(50);
            current_fabric = "Polyester";

            foreach (GameObject item in fabric_polyester)
            {
                item.SetActive(true);
            }
        }
    }

    public void Fabric_Silk(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(150);
            current_fabric = "Silk";

            foreach (GameObject item in fabric_silk)
            {
                item.SetActive(true);
            }
        }
    }

    public void Fabric_Linen(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(40);
            current_fabric = "Linen";

            foreach (GameObject item in fabric_linen)
            {
                item.SetActive(true);
            }
        }
    }

    public void Fabric_Wool(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(75);
            current_fabric = "Wool";

            foreach (GameObject item in fabric_wool)
            {
                item.SetActive(true);
            }
        }
    }

    void Hide()
    {
        foreach (GameObject item in fabrics)
        {
            item.SetActive(false);
        }
    }

}
