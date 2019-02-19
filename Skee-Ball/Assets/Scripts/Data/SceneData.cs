using System;
using UnityEngine.SceneManagement;

[Serializable]
public class SceneData
{
    public int Index { get; private set; }
    public string Name { get; private set; }

    public SceneData(int index)
    {
        Index = index;
        Name = GetSceneName(Index);
    }

    private string GetSceneName(int index)
    {
        var path = SceneUtility.GetScenePathByBuildIndex(index);
        var slash = path.LastIndexOf('/');
        var name = path.Substring(slash + 1);
        var dot = name.LastIndexOf('.');

        return name.Substring(0, dot);
    }
}
