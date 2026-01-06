using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestTubesManager : MonoBehaviour
{
    public static TestTubesManager instance;

    public Image shirt;

    public string material;
    public string dye;

    [Header("Animators")]
    public Animator waterAnimator;
    public Animator ironAnimator;
    public Animator dyeDropperAnimatior;
    public Animator detergetAnimator;

    [Header("Particles")]
    public ParticleSystem waterVFX;

    [Header("ClothEffects")]
    [SerializeField] GameObject ironDamage1;
    [SerializeField] GameObject ironDamage2;
    [SerializeField] GameObject ironDamage3;
    [SerializeField] GameObject stainDamage1;
    [SerializeField] GameObject stainDamage2;
    [SerializeField] GameObject bleachDamageCotton;
    [SerializeField] GameObject bleachDamageLinen;
    [SerializeField] GameObject bleachDamagePolyester;
    [SerializeField] GameObject bleachDamageSilk;
    [SerializeField] GameObject bleachDamageWool;

    private void Awake() => instance = this;
        
    public void HotWater() => waterAnimator.SetTrigger("Play");

    public void Iron() => ironAnimator.SetTrigger("Play");

    public void Dye() => dyeDropperAnimatior.SetTrigger("Play");

    public void Detergent() => detergetAnimator.SetTrigger("Play");

    public void PlayWaterVFX() => waterVFX.Play();

    public void SoakShirt() => StartCoroutine(SoakShirtCO());
    
    public void IronShirt() => StartCoroutine(IronShirtCO());
    
    public void DyeShirt() => StartCoroutine(DyeShirtCO());

    public void DetergentShirt() => StartCoroutine(DetergentShirtCO());

 
    //how tf do I get the variables from the clothing piece in here?


    private IEnumerator SoakShirtCO()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        float loops = 255;
        float scaleDiff = 1;

        switch (material) {
            case "polyester":
                scaleDiff = 1;
                break;
            case "cotton":
                scaleDiff = 1;
                break;
            case "wool":
                scaleDiff = 0.01f;
                break;
            case "linnen":
                scaleDiff = 0.01f;
                break;
            case "silk":
                scaleDiff = 0.01f;
                break;
            case "nylon":
                scaleDiff = 0.01f;
                break;
            default:
                yield return null;
                break;
        }
                
        while(loops > 200)
        {
            shirt.color = new Color(loops / 255, loops / 255, loops / 255);
            shirt.transform.localScale -= new Vector3(1*scaleDiff, 1*scaleDiff, 1);
            loops--;
            yield return waitForFixedUpdate;
        }
        
    }

    private IEnumerator IronShirtCO() 
    {
        switch (material) {
            case "polyester":
                ironDamage2.SetActive(true);
                break;
            case "cotton":
                ironDamage1.SetActive(true);
                break;
            case "wool":
                ironDamage1.SetActive(true);
                break;
            case "linnen":
                ironDamage3.SetActive(true);
                break;
            case "silk":
                ironDamage2.SetActive(true);
                break;
            case "nylon":
                ironDamage2.SetActive(true);
                break;
            default:
                yield return null;
                break;
                
                
        }
        
    }

    private IEnumerator DyeShirtCO() 
    {
        switch (material) {
            case "polyester":
                stainDamage2.SetActive(true);
                break;
            case "cotton":
                stainDamage1.SetActive(true);
                break;
            case "wool":
                stainDamage1.SetActive(true);
                break;
            case "linnen":
                stainDamage1.SetActive(true);
                break;
            case "silk":
                stainDamage1.SetActive(true);
                break;
            case "nylon":
                stainDamage2.SetActive(true);
                break;
            default:
                yield return null;
                break;
        }
    }
    private IEnumerator DetergentShirtCO() 
    {
        switch (material) {
            case "polyester":
                bleachDamagePolyester.SetActive(true);
                break;
            case "cotton":
                bleachDamageCotton.SetActive(true);
                break;
            case "wool":
                bleachDamageWool.SetActive(true);
                break;
            case "linnen":
                bleachDamageLinen.SetActive(true);
                break;
            case "silk":
                bleachDamageSilk.SetActive(true);
                break;
            case "nylon":
                bleachDamagePolyester.SetActive(true);
                break;
            default:
                yield return null;
                break;
        }
    }
}


