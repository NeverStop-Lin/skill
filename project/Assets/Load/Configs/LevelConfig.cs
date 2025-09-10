using System.Collections.Generic;
using UnityEngine;

// 1. 定义数据结构
[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game Config/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public List<string> CubeListName;
}
