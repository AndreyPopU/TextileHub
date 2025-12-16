using UnityEngine;

public class PlayEffect : MonoBehaviour
{
    public ParticleSystem effect;

    public void Play() => effect.Play();

    public void Stop() => effect.Stop();

    public void SoakShirt() => TestTubesManager.instance.SoakShirt();
}
