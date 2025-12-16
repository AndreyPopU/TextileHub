using TMPro;
using UnityEngine;

public class RecycleManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public void SetScore(string score) => scoreText.text = score;
}
