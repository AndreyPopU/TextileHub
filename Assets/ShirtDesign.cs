using UnityEngine;
using UnityEngine.UI;

public class ShirtDesign : MonoBehaviour
{
    public int[] results;
    public Image image;

    private void Awake() => DontDestroyOnLoad(gameObject);

    public void SetResults(int[] results)
    {
        this.results = results;
        image.color = Color.red;
    }

    public void DestroyVisuals() // When time runs out disable everything, need this only because of results[]
    {

    }
}
