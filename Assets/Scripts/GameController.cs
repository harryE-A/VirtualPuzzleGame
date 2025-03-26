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
        startSaveAction = InputSystem.actions.FindAction("Save Start");
        startSolutionAction = InputSystem.actions.FindAction("Save Solution");
    }

    private void Update()
    {
        if (startSaveAction.WasPerformedThisFrame())
        {
            SaveStartPuzzle();
        }
        if (startSolutionAction.WasPerformedThisFrame())
        {
            SaveSolutionPuzzle();
        }
    }

    public void SaveStartPuzzle()
    {

    }
    public void SaveSolutionPuzzle()
    {

    }
#endif

    public void LoadPuzzle()
    {

    }

}
