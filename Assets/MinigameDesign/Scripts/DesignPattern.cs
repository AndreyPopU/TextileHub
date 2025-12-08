using UnityEngine;

public class DesignPattern : MonoBehaviour
{
    [SerializeField] GameObject[] patterns;

    [SerializeField] GameObject[] pattern_stripes;

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

    public void Pattern_Stripes(bool toggle_value)
    {
        if (toggle_value)
        {
            Hide();

            foreach (GameObject item in pattern_stripes)
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
