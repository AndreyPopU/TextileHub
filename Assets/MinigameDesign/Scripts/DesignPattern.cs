using UnityEngine;

public class DesignPattern : MonoBehaviour
{
    #region --Fields and properties--
    [Header("Pattern Object Arrays")]
    [SerializeField] GameObject[] v_pattern_stripes;
    [SerializeField] GameObject[] h_pattern_stripes;
    [SerializeField] GameObject[] d_pattern_stripes;
    [SerializeField] GameObject[] pattern_dots;
    [SerializeField] GameObject[] pattern_squares;
    [SerializeField] GameObject[] pattern_star;
    [SerializeField] GameObject[] pattern_logo;

    [Header("Saving data")]
    public int index;

    [Header("Object References")]
    [SerializeField] MoneyManager moneyManager;
    #endregion

    private void Start()
    {
        SetPattern(0);
    }

    public void SetPattern(int index)
    {
        Hide();
        switch (index)
        {
            case 0: Money(0); break;
            case 1: foreach (GameObject item in v_pattern_stripes) item.SetActive(true); Money(20); break;
            case 2: foreach (GameObject item in h_pattern_stripes) item.SetActive(true); Money(20); break;
            case 3: foreach (GameObject item in d_pattern_stripes) item.SetActive(true); Money(20); break;
            case 4: foreach (GameObject item in pattern_dots) item.SetActive(true); Money(20); break;
            case 5: foreach (GameObject item in pattern_squares) item.SetActive(true); Money(20); break;
            case 6: foreach (GameObject item in pattern_star) item.SetActive(true); Money(20); break;
            case 7: foreach (GameObject item in pattern_logo) item.SetActive(true); Money(40); break;
        }

        this.index = index;
        print((this.index));
    }

    void Hide()
    {
        foreach (GameObject item in v_pattern_stripes) { item.SetActive(false); }
        foreach (GameObject item in h_pattern_stripes) { item.SetActive(false); }
        foreach (GameObject item in d_pattern_stripes) { item.SetActive(false); }
        foreach (GameObject item in pattern_dots) { item.SetActive(false); }
        foreach (GameObject item in pattern_squares) { item.SetActive(false); }
        foreach (GameObject item in pattern_star) { item.SetActive(false); }
        foreach (GameObject item in pattern_logo) { item.SetActive(false); }

        index = 7;
    }

    void Money(float _cost)
    {
        if (moneyManager == null) return;

        moneyManager.Cost_Pattern(_cost);
    }
}
