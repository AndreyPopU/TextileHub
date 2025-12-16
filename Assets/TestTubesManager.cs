using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestTubesManager : MonoBehaviour
{
    public static TestTubesManager instance;

    public Image shirt;

    [Header("Animators")]
    public Animator waterAnimator;
    public Animator ironAnimator;
    public Animator dyeDropperAnimatior;
    public Animator detergetAnimator;

    [Header("Particles")]
    public ParticleSystem waterVFX;

    private void Awake() => instance = this;

    public void HotWater() => waterAnimator.SetTrigger("Play");

    public void Iron() => ironAnimator.SetTrigger("Play");

    public void Dye() => dyeDropperAnimatior.SetTrigger("Play");

    public void Detergent() => detergetAnimator.SetTrigger("Play");

    public void PlayWaterVFX() => waterVFX.Play();

    public void SoakShirt() => StartCoroutine(SoakShirtCO());

    private IEnumerator SoakShirtCO()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        float loops = 255;
        while(loops > 200)
        {
            shirt.color = new Color(loops / 255, loops / 255, loops / 255);
            loops--;
            yield return waitForFixedUpdate;
        }
    }
}
