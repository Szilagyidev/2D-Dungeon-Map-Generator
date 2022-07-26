using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParamaters_", menuName = "PCG/SimpleRandomWalkData")]

public class SimpleRandomWalkData : ScriptableObject
{
    public int iterations = 10, walkLenght = 10;
    public bool startRandomlyEachIteration = true;
}
