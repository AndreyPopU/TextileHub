using TMPro;
using UnityEngine;

public class RecycleManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public void SetScore(string score) => scoreText.text = score;

    public void ServerNextMinigame() => FindFirstObjectByType<AsyncLoad>().LoadScene(10);

    public void NextMinigame() => HostNetwork.instance.NextMinigame(0);
}
