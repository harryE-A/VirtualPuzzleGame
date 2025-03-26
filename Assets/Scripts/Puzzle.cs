using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Puzzle X", menuName = "Scriptable Objects/Puzzle")]
public class Puzzle : ScriptableObject
{
    public int puzzleNo;
    public PuzzleType puzzleType;

    public string startFile;
    public string solutionFile;

    
}
