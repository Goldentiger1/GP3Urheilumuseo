using UnityEngine;

public class Launcher : MonoBehaviour
{
    public enum SPIN_DIRECTION { LEFT, RIGHT, FRONT, BACK }

    public SPIN_DIRECTION SpinDirection;
    public float SpinSpeed;

    private GameObject ballPrefab;
    private float shootPower;
    //private readonly float minPower = 6.8f;
    //private readonly float maxPower = 7.5f;

    public Vector3 LaunchPosition;
    private readonly float minX = -0.5f;
    private readonly float maxX = 0.5f;

    private Vector3 Spin(SPIN_DIRECTION SpinDirection, float spinSpeed)
    {
        switch (SpinDirection)
        {
            case SPIN_DIRECTION.LEFT:
                return Vector3.left * spinSpeed;
            case SPIN_DIRECTION.RIGHT:
                return Vector3.left * spinSpeed;
            case SPIN_DIRECTION.FRONT:
                return Vector3.left * spinSpeed;
            case SPIN_DIRECTION.BACK:
                return Vector3.left * spinSpeed;
            default:
                return Vector3.zero;
        }     
    }

    private void Awake()
    {
        ballPrefab = Resources.Load<GameObject>("Prefabs/Ball");
    }

    private void Start()
    {
        LaunchPosition = transform.position;
    }

    private void Update()
    {
        UIManager.Instance.UpdateLaunchPositionText(LaunchPosition);

        if (Input.GetButtonDown("Jump"))
        {
            shootPower = 0;
        }

        if (Input.GetButton("Jump"))
        {
            if(shootPower >= 10)
            {
                return;
            }

            shootPower += 0.1f;
            UIManager.Instance.UpdatePowerText(shootPower);
        }

        if (Input.GetButtonUp("Jump"))
        {
            //shootPower = Random.Range(minPower, maxPower);

            //LaunchPosition = new Vector3(Random.Range(minX, maxX), transform.position.y, transform.position.z);

            var ball = Instantiate(ballPrefab, LaunchPosition, Quaternion.identity).GetComponent<BallEngine>();
            ball.AddForce(transform.forward * shootPower, ForceMode.Impulse);

            ball.AddTorque(Spin(SpinDirection, SpinSpeed), ForceMode.Impulse);
        }
    }
}
