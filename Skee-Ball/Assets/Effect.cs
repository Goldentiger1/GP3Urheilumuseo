using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    private Coroutine iEffectTimeCoroutine;
    private Transform originalParent;

    private float effectDuration;

    private void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        effectDuration = particleSystem.main.duration;
        originalParent = transform.parent;

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (iEffectTimeCoroutine == null)
        {
            iEffectTimeCoroutine = StartCoroutine(IEffectLifeTime());

            AudioPlayer.Instance.PlayClipAtPoint(2, "ObjectSpawn", transform.position);
        }
    }

    private void OnDisable()
    {
        iEffectTimeCoroutine = null;
    }

    private IEnumerator IEffectLifeTime()
    {
        if(originalParent != null)
        {
            transform.position = originalParent.position;
        }

        transform.SetParent(null);

        if (particleSystem.isPlaying == false)
        {
            particleSystem.Play();
        }

        yield return new WaitForSeconds(effectDuration);

        transform.SetParent(originalParent);

        gameObject.SetActive(false);
    }
}
