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

        int level = UIManager.level;
        if(level != 0)
        {
          LoadPuzzle(level);
        }
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
        
        //Important that the empty gameobjects that parent the physical pieces have this tag and piecePos script attached
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Piece"); //Get every piece

        StreamWriter sw = new StreamWriter(targetFile); //Open Streamwriter to target

        foreach (var piece in gameObjects) //For every piece
        {
            PiecePos p = piece.GetComponent<PiecePos>();
            sw.WriteLine(JsonUtility.ToJson(p)); //Write to file in JSON
        }
        sw.Close(); //Close Streamwriter
    }

    public void LoadPuzzle(int levelID)
    {
        Debug.Log(levelID);

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Piece"); //Get every piece

        string targetFile = "Assets/Puzzles/startPos-" + levelID + ".txt";
        StreamReader sr = new StreamReader(targetFile);

        foreach (var piece in gameObjects) //For every piece
        {
            string json = sr.ReadLine();
            PiecePos pieceToEdit = piece.GetComponent<PiecePos>();

            JsonUtility.FromJsonOverwrite(json, pieceToEdit);
            pieceToEdit.Apply();

        }

        sr.Close();
    }

}
