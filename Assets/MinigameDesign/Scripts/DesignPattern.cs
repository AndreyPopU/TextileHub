using UnityEngine;

public class DesignPattern : MonoBehaviour
{
    [SerializeField] GameObject[] patterns;

    [SerializeField] GameObject[] v_pattern_stripes;
    [SerializeField] GameObject[] h_pattern_stripes;
    [SerializeField] GameObject[] d_pattern_stripes;
    [SerializeField] GameObject[] pattern_dots;
    [SerializeField] GameObject[] pattern_squares;
    [SerializeField] GameObject[] pattern_star;
    [SerializeField] GameObject[] pattern_logo;
    [SerializeField] MoneyManager moneyManager;

    public string current_pattern;

    private void Start()
    {
        Pattern_None(true);
    }

    void Money(float _cost)
    {
        moneyManager.Cost_Pattern(_cost);
    }

    public void Pattern_None(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(0);
            current_pattern = "None";
        }
    }

    public void V_Pattern_Stripes(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(20);
            current_pattern = "V Stripes";

            foreach (GameObject item in v_pattern_stripes)
            {
                item.SetActive(true);
            }
        }
    }


    public void H_Pattern_Stripes(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(20);
            current_pattern = "H Stripes";

            foreach (GameObject item in h_pattern_stripes)
            {
                item.SetActive(true);
            }
        }
     }

    public void D_Pattern_Stripes(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(20);
            current_pattern = "D Stripes";

            foreach (GameObject item in d_pattern_stripes)
            {
                item.SetActive(true);
            }
        }
    }

    public void Pattern_Dots(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(20);
            current_pattern = "Dots";

            foreach (GameObject item in pattern_dots)
            {
                item.SetActive(true);
            }
        }
    }

    public void Pattern_Squares(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(20);
            current_pattern = "Squares";

            foreach (GameObject item in pattern_squares)
            {
                item.SetActive(true);
            }
        }
    }

    public void Pattern_Star(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(20);
            current_pattern = "Star";

            foreach (GameObject item in pattern_star)
            {
                item.SetActive(true);
            }
        }
    }

    public void Pattern_Logo(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
            Money(40);
            current_pattern = "Logo";

            foreach (GameObject item in pattern_logo)
            {
                item.SetActive(true);
            }
        }
    }

    void Hide()
    {
        foreach (GameObject item in patterns)
        {
            item.SetActive(false);
        }
    }
}
