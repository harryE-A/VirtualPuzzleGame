using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
#if UNITY_EDITOR
    InputAction startSaveAction;
    InputAction startSolutionAction;
    public enum saveType {start, solution}

    private void Start()
    {
        startSaveAction = InputSystem.actions.FindAction("SaveStart");
        startSolutionAction = InputSystem.actions.FindAction("SaveSolution");
    }

    private void Update()
    {
        if (startSaveAction.WasPerformedThisFrame()) {SavePuzzle(saveType.start);}
        if (startSolutionAction.WasPerformedThisFrame()) {SavePuzzle(saveType.solution);}
    }

    public void SavePuzzle(saveType s)
    {
        string targetFile;
        //Switch helps to reuse code, only difference between saving solution or start is the file destination.
        //Previously had two methods, SaveStartPuzzle() and SaveSolutionPuzzle()
        switch (s) {
            case saveType.solution:
                targetFile = "Assets/Puzzles/solutionPos-X.txt";
                break;
            case saveType.start:
                targetFile = "Assets/Puzzles/startPos-X.txt";
                break;
            default:
                targetFile = "Assets/Puzzles/error.txt";
                break;
        }

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Piece"); //Get every piece
        //Important that the empty gameobjects that parent the physical pieces have this tag and piecePos script

        StreamWriter sw = new StreamWriter(targetFile); //Open Streamwriter to target

        string pieceInfo = new string("");

        foreach (var piece in gameObjects) //For every piece
        {
            PiecePos piecePos = piece.GetComponent<PiecePos>(); //Get the piecePos script containing pos and rotation

            sw.WriteLine(piecePos.GetId()); //Json does not like ints?
            sw.WriteLine(JsonUtility.ToJson(piecePos.GetPos()));
            sw.WriteLine(JsonUtility.ToJson(piecePos.GetRot()));
        }
        sw.Close(); //Close Streamwriter
    }
#endif

    public void LoadPuzzle()
    {

    }

}
