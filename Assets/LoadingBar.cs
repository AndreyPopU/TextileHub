using UnityEngine;

public class LoadingBar : MonoBehaviour
{
    public GameObject[] bars;
    private int index;
    private float waitingDelay = .5f;

    void Update()
    {
        if (waitingDelay > 0) waitingDelay -= Time.deltaTime;
        else
        {
            if (index >= bars.Length)
            {
                for (int i = 0; i < bars.Length; i++)
                    bars[i].SetActive(false);
                index = 0;
            }

            bars[index++].SetActive(true);
            waitingDelay = .5f;
        }
    }
}
