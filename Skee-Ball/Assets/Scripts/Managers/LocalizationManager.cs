using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class LocalizationData
{
    public LocalizationItem[] Items;
}

[SerializeField]
public class LocalizationItem
{
    public string Key;
    public string Value;
}

public class LocalizationManager : MonoBehaviour
{
   
}
