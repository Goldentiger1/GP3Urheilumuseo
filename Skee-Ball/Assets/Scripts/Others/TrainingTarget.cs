using UnityEngine;

public class TrainingTarget : MonoBehaviour
{
    private GameObject trainingTargetSpawnEffect;

    private void Awake()
    {
        var trainingTargetEffectPrefab = ResourceManager.Instance.BigObjectSpawnEffect;

        trainingTargetSpawnEffect = Instantiate(
            trainingTargetEffectPrefab,
            transform.position,
            Quaternion.identity);

        trainingTargetSpawnEffect.name = trainingTargetEffectPrefab.name;

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (trainingTargetSpawnEffect == null)
        {
            Debug.LogError("OnEnable: effect is null");
            return;
        }

        trainingTargetSpawnEffect.SetActive(true);
    }

    private void OnDisable()
    {
        if(trainingTargetSpawnEffect == null)
        {
            Debug.LogError("OnDisable: effect is null");
            return;
        }

        trainingTargetSpawnEffect.SetActive(false);
    }
}
