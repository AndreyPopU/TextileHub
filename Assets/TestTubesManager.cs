using UnityEngine;

public class TestTubesManager : MonoBehaviour
{
    public Animator waterAnimator;
    public Animator ironAnimator;
    public Animator dyeDropperAnimatior;
    public Animator detergetAnimator;

    public void HotWater() => waterAnimator.SetTrigger("Play");

    public void Iron() => ironAnimator.SetTrigger("Play");

    public void Dye() => dyeDropperAnimatior.SetTrigger("Play");

    public void Detergent() => detergetAnimator.SetTrigger("Play");
}
