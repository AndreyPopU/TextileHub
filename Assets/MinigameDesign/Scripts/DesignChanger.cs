using System.Drawing;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;

public class DesignChanger : MonoBehaviour
{

    [SerializeField] Sprite[] colars;
    [SerializeField] Sprite[] sleeves;
    [SerializeField] Sprite[] hems;

    [SerializeField] GameObject obj_colar;
    [SerializeField] GameObject obj_sleeves;
    [SerializeField] GameObject obj_hem;

    int colar_index;
    int sleeves_index;
    int hem_index;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //------Collar------

    public void OnButtonCollarLeft()
    {
        colar_index = colar_index - 1;
        Change_Colar();
        obj_colar.GetComponent<Image>().sprite = colars[colar_index];
        Debug.Log(colar_index);
    }

    public void OnButtonCollarRight()
    {
        colar_index = colar_index + 1;
        Change_Colar();
        obj_colar.GetComponent<Image>().sprite = colars[colar_index];
        Debug.Log(colar_index);
    }

    void Change_Colar()
    {
        if (colar_index > 4)
            colar_index = 0;
        
        if (colar_index < 0)
            colar_index = 4;
    }

    //------Sleeves------

    public void OnButtonSleevesLeft()
    {
        sleeves_index = sleeves_index - 1;
        Change_Sleeves();
        obj_sleeves.GetComponent<Image>().sprite = sleeves[sleeves_index];
        Debug.Log(sleeves_index);
    }

    public void OnButtonSleevesRight()
    {
        sleeves_index = sleeves_index + 1;
        Change_Sleeves();
        obj_sleeves.GetComponent<Image>().sprite = sleeves[sleeves_index];
        Debug.Log(sleeves_index);
    }

    void Change_Sleeves()
    {
        if (sleeves_index > 4)
            sleeves_index = 0;

        if (sleeves_index < 0)
            sleeves_index = 4;
    }

    //------Hem------

    public void OnButtonHemLeft()
    {
        hem_index = hem_index - 1;
        Change_Hem();
        obj_hem.GetComponent<Image>().sprite = hems[hem_index];
        Debug.Log(hem_index);
    }

    public void OnButtonHemRight()
    {
        hem_index = hem_index + 1;
        Change_Hem();
        obj_hem.GetComponent<Image>().sprite = hems[hem_index];
        Debug.Log(hem_index);
    }

    void Change_Hem()
    {
        if (hem_index > 3)
            hem_index = 0;

        if (hem_index < 0)
            hem_index = 3;
    }
}
