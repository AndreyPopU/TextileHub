using UnityEngine;
using TMPro;

public class ThemeManager : MonoBehaviour
{
    [SerializeField] string[] themes;

    [SerializeField] TextMeshProUGUI Text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int randomNumber = Random.Range(0, 6);
        Text.text = themes[randomNumber];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
