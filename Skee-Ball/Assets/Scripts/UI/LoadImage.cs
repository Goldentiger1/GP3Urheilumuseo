using UnityEngine;

public class LoadImage : MonoBehaviour
{
    private RectTransform rotatingIcon;
    [SerializeField]
    private float timeStep = 0.5f;
    [SerializeField]
    private float oneStepAngle = 0.5f;
    private float startTime;

    private void Awake()
    {
        rotatingIcon = GetComponent<RectTransform>();
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if(Time.time - startTime >= timeStep)
        {
            var iconAngle = rotatingIcon.localEulerAngles;
            iconAngle.z += oneStepAngle;

            rotatingIcon.localEulerAngles = iconAngle;

            startTime = Time.time;
        }
    }
}
