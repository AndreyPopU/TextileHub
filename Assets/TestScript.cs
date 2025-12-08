using Unity.VisualScripting;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Start()
    {
        Color givenColor = new Color(.3f, .5f, 1, 1);
        string primaryHex = givenColor.ToHexString();
        print(primaryHex);
    }
}
