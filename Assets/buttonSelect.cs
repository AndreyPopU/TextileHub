using UnityEngine;

public class buttonSelect : MonoBehaviour
{
    public GameObject tab1;
    public GameObject tab2;
    public GameObject tab3;
    public GameObject tab4;
    public GameObject tab5;

    public void Tab_1 (bool toggle_value)
    {
        if (toggle_value)
        {
            tab1.SetActive(true);
            tab2.SetActive(false);
            tab3.SetActive(false);
            tab4.SetActive(false);
            tab5.SetActive(false);

        }
    }
    
    public void Tab_2 (bool toggle_value)
    {
        if (toggle_value)
        {
            tab1.SetActive(false);
            tab2.SetActive(true);
            tab3.SetActive(false);
            tab4.SetActive(false);
            tab5.SetActive(false);
        }
    }
    public void Tab_3 (bool toggle_value)
    {
        if (toggle_value)
        {
            tab1.SetActive(false);
            tab2.SetActive(false);
            tab3.SetActive(true);
            tab4.SetActive(false);
            tab5.SetActive(false);       
        }
    }
    public void Tab_4 (bool toggle_value)
    {
        if (toggle_value)
        {
            tab1.SetActive(false);
            tab2.SetActive(false);
            tab3.SetActive(false);
            tab4.SetActive(true);
            tab5.SetActive(false);
        }
    }
    public void Tab_5 (bool toggle_value)
    {
        if (toggle_value)
        {
            tab1.SetActive(false);
            tab2.SetActive(false);
            tab3.SetActive(false);
            tab4.SetActive(false);
            tab5.SetActive(true);
        }
    }
}
