using UnityEngine;

public class LevelManager : Singelton<LevelManager>
{
    public int Points { get; private set; }

    private void Awake()
    {

    }
}
