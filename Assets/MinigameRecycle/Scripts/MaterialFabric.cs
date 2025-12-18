using System.Collections;
using UnityEngine;

public class MaterialFabric : MonoBehaviour
{
    public bool found;
    public bool visible;
    public float noticeTime = 3;
    public GameObject visibleCounterpart;

    private ParticleSystem foundEffect;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (found) return;

        visible = spriteRenderer.isVisible;

        if (visible)
        {
            if (noticeTime > 0) noticeTime -= Time.deltaTime;
            else
            {
                visibleCounterpart.SetActive(true);
                StartCoroutine(ShakeCo());
                RecyclingManager.instance.imperfectionsFound++;
                found = true;
            }
        }
    }

    private IEnumerator ShakeCo()
    {
        Vector3 shakePos;
        Vector3 originalPos = transform.position;
        float duration = .2f;
        float force = .1f;

        while(duration > 0)
        {
            float randomX = Random.Range(originalPos.x - 1 * force, originalPos.x + 1 * force);
            float randomY = Random.Range(originalPos.y - 1 * force, originalPos.y + 1 * force);
            shakePos = new Vector3(randomX, randomY, transform.position.z);
            transform.position = shakePos;

            duration -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        transform.position = originalPos;
    }
}
