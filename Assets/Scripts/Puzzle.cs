using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle", menuName = "Scriptable Objects/Puzzle")]
public class Puzzle : ScriptableObject
{
    public int puzzleNo;
    public PuzzleType puzzleType;

    public List<PiecePos> startingPieces;
    public List<PiecePos> solution;

    
}
