using UnityEngine;

public class ShirtResults : MonoBehaviour
{
    public int[] results;
    public string primaryHex, secondaryHex;
    public bool isAzo, isCO2;
    public string playerId;

    private void Awake() => DontDestroyOnLoad(gameObject);
}
