using UnityEngine;

public class ConsequenceResults : MonoBehaviour
{
    public int[] results;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
