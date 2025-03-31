using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
#if UNITY_EDITOR
    InputAction startSaveAction;
    InputAction startSolutionAction;

    private void Start()
    {
        startSaveAction = InputSystem.actions.FindAction("SaveStart");
        startSolutionAction = InputSystem.actions.FindAction("SaveSolution");
    }

    private void Update()
    {
        if (startSaveAction.WasPerformedThisFrame()) {SaveStartPuzzle();}
        if (startSolutionAction.WasPerformedThisFrame()) {SaveSolutionPuzzle();}
    }

    public void SaveStartPuzzle()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Piece");
        string test = new string("");

        foreach (var piece in gameObjects)
        {
            PiecePos piecePos = piece.GetComponent<PiecePos>();

            string temp = JsonUtility.ToJson(piecePos);
            test += temp + "\n";
        }
            Debug.Log(test);
    }
    public void SaveSolutionPuzzle()
    {

    }
#endif

    public void LoadPuzzle()
    {

    }

}
