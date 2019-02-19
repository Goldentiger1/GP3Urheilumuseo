using Valve.VR.InteractionSystem;

public class PlayerEngine : Player
{
    public bool Instance
    {
        get
        {
            return instance;
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
