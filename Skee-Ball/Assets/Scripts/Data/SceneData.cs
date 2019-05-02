using System;
using UnityEngine.SceneManagement;

[Serializable]
public class SceneData
{
    public int Index { get; private set; }
    public int NarrationIndex { get; private set; }
    public string Name { get; private set; }

    public SceneData(int index)
    {
        Index = index;
       
        Name = GetSceneName(Index);

        NarrationIndex = CorrectSceneNarrationIndex(Name);
    }

    private string GetSceneName(int index)
    {
        var path = SceneUtility.GetScenePathByBuildIndex(index);
        var slash = path.LastIndexOf('/');
        var name = path.Substring(slash + 1);
        var dot = name.LastIndexOf('.');

        return name.Substring(0, dot);
    }

    private int CorrectSceneNarrationIndex(string sceneName)
    {
        switch (sceneName)
        {
            case "1879":

            return 1;

            case "1970":

            return 5;

            case "2000":

            return 7;

            case "2010":

            return 8;

            default:

            return 0;
        }
    }
}
