using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    InputAction startSaveAction;
    InputAction startSolutionAction;
    public enum saveType {start, solution}

    //List of all Puzzle scriptable objects
    [SerializeField] public Puzzle[] puzzles;

    //Text to be displayed when the player wins.
    [SerializeField] public GameObject victoryText;
    //Text to be displayed to give the player some help
    [SerializeField] public GameObject helpMessage;

    int levelID; //The current level

    private void Start()
    {
        startSaveAction = InputSystem.actions.FindAction("SaveStart");
        startSolutionAction = InputSystem.actions.FindAction("SaveSolution");
      
        LoadFile(); //Load the selected level from the file
    }

    public void LoadFile()
    {
        levelID = PlayerPrefs.GetInt("Level"); //Get the level

        string data = "";
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Puzzles/startPos-" + levelID + ".txt");

        data = System.IO.File.ReadAllText(filePath);
        Debug.Log(data);

        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece"); //Get every piece

        StringReader sr = new StringReader(data); //Read each line of the larger string

        foreach (var piece in pieces) //For every piece
        {
            string json = sr.ReadLine();
            PiecePos pieceToEdit = piece.GetComponent<PiecePos>();

            JsonUtility.FromJsonOverwrite(json, pieceToEdit);
            pieceToEdit.Apply();
        }

        SetLevelText();

        sr.Close();
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
        //Previously code split into two methods, SaveStartPuzzle() and SaveSolutionPuzzle()
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

    public void CheckPuzzle()
    {
        if (CheckGridPoints()) //Puzzle Solved
        {
            victoryText.SetActive(true);
        }
        else  //Unsolved
        {
            victoryText.SetActive(false);
        }
    }

    public bool CheckGridPoints()
    {
        GameObject[] gridPoints = GameObject.FindGameObjectsWithTag("GridPoint");

        foreach (var point in gridPoints) //Check every grid point
        {
            if (!point.GetComponent<GridPoint>().filled)
            {
                return false;
            }
        }
        return true;
    }

    public void SetLevelText()
    {
        Puzzle currentPuzzle = puzzles[levelID];

        TMP_Text text = GameObject.Find("Level Text").GetComponent<TMP_Text>(); 
        text.text = "Level " + levelID + " - " + currentPuzzle.puzzleType; //Get and edit text component
    }
}
