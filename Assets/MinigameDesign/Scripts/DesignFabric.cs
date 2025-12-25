using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DesignFabric : MonoBehaviour
{
    #region "Fields and properties"
    [Header("Fabric Object Arrays")]
    [SerializeField] GameObject[] fabric_cotton;
    [SerializeField] GameObject[] fabric_polyester;
    [SerializeField] GameObject[] fabric_silk;
    [SerializeField] GameObject[] fabric_linen;
    [SerializeField] GameObject[] fabric_wool;

    [Header("Object References")]
    [SerializeField] MoneyManager moneyManager;
    #endregion

    private void Start()
    {
        SetFabric(0);
    }

    public void SetFabric(int index)
    {
        Hide();
        
        switch (index)
        {
            case 0: foreach (GameObject item in fabric_cotton) item.SetActive(true); Money(50); break;
            case 1: foreach (GameObject item in fabric_polyester) item.SetActive(true); Money(50); break;
            case 2: foreach (GameObject item in fabric_silk) item.SetActive(true); Money(150); break;
            case 3: foreach (GameObject item in fabric_linen) item.SetActive(true); Money(40); break;
            case 4: foreach (GameObject item in fabric_wool) item.SetActive(true); Money(75); break;
        }
    }

    void Money(float _cost)
    {
        if (moneyManager == null) return;

        moneyManager.Cost_Fabric(_cost);
    }

    void Hide()
    {
        foreach (GameObject item in fabric_cotton)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in fabric_polyester)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in fabric_silk)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in fabric_linen)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in fabric_wool)
        {
            item.SetActive(false);
        }
    }
}
