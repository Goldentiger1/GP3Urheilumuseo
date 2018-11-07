using UnityEngine;

public class Launcher : MonoBehaviour
{
    private GameObject ballPrefab;
    private float shootPower;
    private readonly float minPower = 5f;
    private readonly float maxPower = 10f;

    private void Awake()
    {
        ballPrefab = Resources.Load<GameObject>("Prefabs/Ball");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            shootPower = Random.Range(minPower, maxPower);

            var ball = Instantiate(ballPrefab, transform.position, Quaternion.identity).GetComponent<BallEngine>();
            ball.AddForce(transform.forward * shootPower, ForceMode.Impulse);
        }
    }
}
