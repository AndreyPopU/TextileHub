using Unity.VisualScripting;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public int score;
    public GameObject[] consequences;

    //void Start()
    //{
    //    Color givenColor = new Color(.3f, .5f, 1, 1);
    //    string primaryHex = givenColor.ToHexString();
    //    print(primaryHex);
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) DecideConsequence();
    }

    private void DecideConsequence() => consequences[score].gameObject.SetActive(true);
}
