using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Building Repository", menuName = "World Generation/BuildingRepository", order = 1)]
public class BuildingRepository : ScriptableObject
{
    public GameObject[] Buildings;
    public GameObject[] Roads;
}
