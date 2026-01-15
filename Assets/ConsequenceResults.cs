using UnityEngine;

public class ConsequenceResults : MonoBehaviour
{
    public int[] results = new int[3];

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
