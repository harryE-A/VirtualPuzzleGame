using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    InputAction startSaveAction;
    InputAction startSolutionAction;
    public enum saveType {start, solution}

    [SerializeField] public Puzzle[] puzzles;

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
            PiecePos p = piece.GetComponent<PiecePos>();
            sw.WriteLine(JsonUtility.ToJson(p)); Debug.Log(JsonUtility.ToJson(p));

            //PiecePos piecePos = piece.GetComponent<PiecePos>(); //Get the piecePos script containing pos and rotation

            //sw.WriteLine(piecePos.GetId()); //Json does not like ints?
            //sw.WriteLine(JsonUtility.ToJson(piecePos.GetPos()));
            //sw.WriteLine(JsonUtility.ToJson(piecePos.GetRot()));
        }
        sw.Close(); //Close Streamwriter
    }

    private void Awake() //When the Game scene is loaded, also setup the puzzle picked in level select screen.
    {
        int level = UIManager.level;
        //LoadPuzzle(level);
    }

    public void LoadPuzzle(int levelID)
    {
        Debug.Log(levelID);

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Piece"); //Get every piece

        string targetFile = "Assets/Puzzles/startPos-" + levelID + ".txt"; //HERE
        StreamReader sr = new StreamReader(targetFile);

        //GameObject pieceToEdit;

        foreach (var piece in gameObjects) //For every piece
        {
            string pieceID = sr.ReadLine(); //Get id
            //foreach (var p in gameObjects) //Find gameobject with same id
            //{
            //    if (p.GetComponent<PiecePos>().GetId().ToString() == pieceID)
            //    {
            //        pieceToEdit = p;
            //        break;
            //    }
            //}

            string xyz = sr.ReadLine();
            string q = sr.ReadLine();

            JsonUtility.FromJsonOverwrite(xyz, piece.GetComponent<PiecePos>());
            //JsonUtility.FromJsonOverwrite(q, piece);
        }
    }

}
