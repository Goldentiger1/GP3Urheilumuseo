using UnityEngine;

public class InputManager : Singelton<InputManager>
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LevelManager.Instance.ResetBallPostions();
        }
    }
}
