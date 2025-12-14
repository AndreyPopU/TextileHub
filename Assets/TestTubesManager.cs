using UnityEngine;

public class TestTubesManager : MonoBehaviour
{
    [Header("Animators")]
    public Animator waterAnimator;
    public Animator ironAnimator;
    public Animator dyeDropperAnimatior;
    public Animator detergetAnimator;

    [Header("Animators")]
    public ParticleSystem waterVFX;

    public void HotWater() => waterAnimator.SetTrigger("Play");

    public void Iron() => ironAnimator.SetTrigger("Play");

    public void Dye() => dyeDropperAnimatior.SetTrigger("Play");

    public void Detergent() => detergetAnimator.SetTrigger("Play");

    public void PlayWaterVFX() => waterVFX.Play();
}
