using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigSelect", menuName = "Game Config/ConfigSelect")]
public class ConfigSelect : ScriptableObject
{
    public ConfigSelectItem defaultConfig;
    public List<ConfigSelectItemOther> otherConfig;
}

[Serializable]
public class ConfigSelectItem
{
    public LevelConfig LevelConfig;
}
[Serializable]
public class ConfigSelectItemOther:ConfigSelectItem
{
    public string name;
}