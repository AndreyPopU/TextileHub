using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestTubesManager : MonoBehaviour
{
    public static TestTubesManager instance;
    public ShirtResults shirtresults;
    

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
       
        switch (shirtresults.results[3]) {
            case 1:
                scaleDiff = 0;
                break;
            case 0:
                scaleDiff = 0;
                break;
            case 4:
                scaleDiff = 0.01f;
                break;
            case 3:
                scaleDiff = 0.01f;
                break;
            case 2:
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
        switch (shirtresults.results[3]) {
            case 1:
                ironDamage2.SetActive(true);
                break;
            case 0:
                ironDamage1.SetActive(true);
                break;
            case 4:
                ironDamage1.SetActive(true);
                break;
            case 3:
                ironDamage3.SetActive(true);
                break;
            case 2:
                ironDamage2.SetActive(true);
                break;
            default:
                yield return null;
                break;
                
                
        }
        
    }

    private IEnumerator DyeShirtCO() 
    {
        switch (shirtresults.results[3]) {
            case 1:
                stainDamage2.SetActive(true);
                break;
            case 0:
                stainDamage1.SetActive(true);
                break;
            case 4:
                stainDamage1.SetActive(true);
                break;
            case 3:
                stainDamage1.SetActive(true);
                break;
            case 2:
                stainDamage1.SetActive(true);
                break;
            default:
                yield return null;
                break;
        }
    }
    private IEnumerator DetergentShirtCO() 
    {
        switch (shirtresults.results[3]) {
            case 1:
                bleachDamagePolyester.SetActive(true);
                break;
            case 0:
                bleachDamageCotton.SetActive(true);
                break;
            case 4:
                bleachDamageWool.SetActive(true);
                break;
            case 3:
                bleachDamageLinen.SetActive(true);
                break;
            case 2:
                bleachDamageSilk.SetActive(true);
                break;
            default:
                yield return null;
                break;
        }
    }
}


