using UnityEngine;

public class ResourceManager : Singelton<ResourceManager>
{
    [Header("Model Prefabs")]
    public GameObject BallPrefab;
    public GameObject TrainingTargetPrefab;

    [Header("Effects")]
    public GameObject ObjectSpawnEffect;
    public GameObject BigObjectSpawnEffect;
}
