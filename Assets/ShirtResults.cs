using UnityEngine;

public class ShirtResults : MonoBehaviour
{
    public int[] results;
    public string primaryHex, secondaryHex;

    private void Awake() => DontDestroyOnLoad(gameObject);
}
