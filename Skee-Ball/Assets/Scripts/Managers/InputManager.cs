﻿using UnityEngine;

public class InputManager : Singelton<InputManager>
{
    #region VARIABLES



    #endregion VARIABLES

    #region PROPERTIES



    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LevelManager.Instance.ResetBallPostions();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.Instance.ChangeNextScene();
        }
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS



    #endregion CUSTOM_FUNCTIONS
}
