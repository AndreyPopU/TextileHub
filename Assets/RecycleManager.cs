using TMPro;
using UnityEngine;

public class RecycleManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public void UpdateConsequenceScore(int amount)
    {
        FindFirstObjectByType<ConsequenceResults>().results[0] += amount;
        FindFirstObjectByType<ConsequenceResults>().results[2] += amount;
    }

    public void ServerNextMinigame() => FindFirstObjectByType<AsyncLoad>().LoadScene(10);

    public void NextMinigame() => HostNetwork.instance.NextMinigame(0);
}
