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

    private void Start()
    {
        Pattern_None(true);
    }

    public void Pattern_None(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();
        }

    }

    public void V_Pattern_Stripes(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();

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
