using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DesignFabric : MonoBehaviour
{
    [SerializeField] GameObject[] fabrics;

    [SerializeField] GameObject[] fabric_cotton;
    [SerializeField] GameObject[] fabric_denim;

    private void Start()
    {
        Fabric_Cotton(true);
    }

    public void Fabric_Cotton(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();

            foreach (GameObject item in fabric_cotton)
            {
                item.SetActive(true);
            }
        }

    }

    public void Fabric_Denim(bool toggle_value)
    {
        if (toggle_value)
        {
                Hide();

            foreach (GameObject item in fabric_denim)
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
