using UnityEngine;

public class VotingManagerServer : MonoBehaviour
{
    public int currentIndex;
    public ShirtResults[] shirts;
    public ClothingManager displayShirt;

    void Start()
    {
        // Find all ShirtDesigns from last scene
        shirts = FindObjectsByType<ShirtResults>(FindObjectsSortMode.None);
    }

    public void DisplayShirt()
    {
        // If all shirts have been cycled through, move on to the next minigame
        if (currentIndex >= shirts.Length)
        {

            return;
        }

        for (int i = 0; i < shirts[currentIndex].results.Length; i++)
            displayShirt.SetProperty(shirts[currentIndex].results[i]);

        displayShirt.ResultSetPrimaryColor(shirts[currentIndex].primaryHex);
        displayShirt.ResultSetSecondaryColor(shirts[currentIndex].secondaryHex);

        currentIndex++;
    }

    public void ServerNextMinigame() => FindFirstObjectByType<AsyncLoad>().LoadScene(3);

    public void NextMinigame() => HostNetwork.instance.NextMinigame(4);
}
