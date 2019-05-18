using UnityEngine;

public class TrainingTarget : MonoBehaviour
{
    private GameObject trainingTargetSpawnEffect;

    private void Awake()
    {
        trainingTargetSpawnEffect = GameMaster.Instance.SpawnGameObjectInstance(ResourceManager.Instance.BigObjectSpawnEffect, transform.position, Quaternion.identity, null, false);
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
