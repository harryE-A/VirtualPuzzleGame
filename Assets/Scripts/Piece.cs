using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
/**
 * This script uses some code from the following YouTube tutorial - https://www.youtube.com/watch?v=axW46wCJxZ0
 * Specifically for dragging the pieces and working out where they should move relative to the camera.
 * I have adapted the code and added my own sections that interlink to solve my specific technical issues.
 * 
 * The code has been marked with comments, the majority of this file contains my own code.
 */

public class Piece : MonoBehaviour
{           
    private float newX; //Values for X and Z for snapping
    private float newZ;
    Vector3 mousePos;

    //Which way the piece is flipped:
    [SerializeField] private bool toggled = false; //True = side with 2 protruding balls. False = 1 protruding

    //Is the piece on the board already, stops pieces from displacing others when moving through them while dragging
    [SerializeField] private bool placed;   

    [SerializeField] private bool locked;   //Is the player not allowed to move the piece? 
    [SerializeField] private bool dragging; //Is the piece currently being dragged?

    [SerializeField] private bool colliding; //Is the piece colliding with another?

    [SerializeField] private string colour; //The colour of this piece, for the text help messages.


    [SerializeField] GameController gameController; //Reference to the single instance of the gamecontroller.

    //Input Actions:
    InputAction rotateAction;
    InputAction flipAction;

    //*Tutorial Code Start:*
    private Vector3 GetMousePos() //Get where the mouse is relative to the camera
    {
        return Camera.main.WorldToScreenPoint(transform.position); 
    } //*End*

    private void OnMouseDown() //Called when the user clicks on a collider
    {
        if (!locked)
        {
            gameController.helpMessage.SetActive(false);
            mousePos = Input.mousePosition - GetMousePos(); //This line uses Tutorial Code
        }

        if(locked)
        {
            gameController.helpMessage.SetActive(true);
            TMP_Text helpText = gameController.helpMessage.GetComponent<TMP_Text>();

            helpText.text = "The " + colour + " Piece can't be moved for this level.";
        }
        
    }

    private void OnMouseDrag() //Moves the object being dragged by setting it's position to where the mouse is relative to the camera
    {
        if(!locked)
        {
            dragging = true;
            placed = false;

            transform.parent.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos); //This line uses Tutorial Code

            //Lock Y Axis so pieces don't clip below the board
            transform.parent.position = new Vector3(transform.parent.position.x, 0, transform.parent.position.z); 
        
            //Rotation
            if (rotateAction.WasPerformedThisFrame()) {RotatePiece();}

            //Toggling (flipping)
            if (flipAction.WasPerformedThisFrame()) {TogglePiece();}
        }
   
    }

    private void OnMouseUp()
    {
        if(!locked)
        {
            Vector3 pieceLocation = transform.parent.position;
            Vector3 roundedPieceLocation = new Vector3(Mathf.Round(pieceLocation.x), 0, Mathf.Round(pieceLocation.z)); //Round to integer

            CalculateNewX(pieceLocation, roundedPieceLocation);
            CalculateNewZ(pieceLocation, roundedPieceLocation);

            transform.parent.position = new Vector3(newX, 0, newZ); //Set new coordinates

            //Update the PiecePos script
            GetComponentInParent<PiecePos>().SetPosRot();

            dragging = false;

            //Check if the puzzle has been completed
            gameController.CheckPuzzle();
        }
    }

    private void Update()
    {
        if (!dragging && colliding && !placed && !locked) 
        {
            transform.parent.position = GetComponentInParent<PiecePos>().GetStartPos(); //Dont allow, send back to edge of board   
        }
        if(!dragging && !colliding)
        {
            placed = true; //The piece must be placed down
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliding = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        colliding = true;

        //Bug Fix (moving mouse too quickly and stop dragging can allow pieces to be placed inside one another)
        //NEEDS EDITING WHEN ADDING LOCKING

        //Two pieces
        try { 
            Piece p = collision.gameObject.GetComponent<Piece>(); 
            if((placed && colliding && !dragging) && (p.placed && p.colliding && !p.dragging))
            {
                Debug.Log("Error Case: Two Pieces");
                if(!locked) //The piece shouldn't move if it's locked.
                {
                    transform.parent.position = GetComponentInParent<PiecePos>().GetStartPos();
                }
            }
        }
        //A piece and the board edge
        catch {
            if((placed && colliding && !dragging) && collision.gameObject.CompareTag("Board"))
            {
                Debug.Log("Error Case: Board Edge");
                transform.parent.position = GetComponentInParent<PiecePos>().GetStartPos();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }

    private void RotatePiece()
    {
        Debug.Log("Rotate");
        GameObject parentObject = transform.parent.gameObject; //Find Parent GameObject

        if(!toggled)
        {
            parentObject.transform.Rotate(0, 90, 0); //Rotate it on Y axis by 90 degrees
        }
        else 
        {
            parentObject.transform.Rotate(90, 0, 0); //Rotate around X axis by 90 degrees
        }
    }

    private void TogglePiece()
    {
        Debug.Log("Toggle");
        GameObject parentObject = transform.parent.gameObject; //Find Parent GameObject

        toggled = !toggled; //Flip boolean
        GetComponentInParent<PiecePos>().SetToggled(toggled); //Update the PiecePos script

        if (toggled) //Two Protruding balls
        {
            parentObject.transform.rotation = Quaternion.Euler(0,0,90); //Set rotation to 90 on Z axis
        }
        else if(!toggled) //One Protruding ball
        {
            parentObject.transform.rotation = Quaternion.Euler(0,0,0); //Set rotation to 0 on Z axis
        }
    }

    private void Start()
    {
        //Initialise inputs
        rotateAction = InputSystem.actions.FindAction("Rotate");
        flipAction = InputSystem.actions.FindAction("Flip");
    }

    //Snapping methods:
    private void CalculateNewX(Vector3 pieceLocation, Vector3 roundedPieceLocation) //Takes the two Vector3 values and returns new value for X
    {
        if (roundedPieceLocation.x % 2 == 0f) //For X: Check if divisible by 2
        {
            newX = roundedPieceLocation.x;
        }
        else //Odd number
        {
            float lowerBound = roundedPieceLocation.x - 1;
            float upperBound = roundedPieceLocation.x + 1;

            float midPoint = lowerBound + upperBound / 2;

            if (pieceLocation.x > midPoint) { newX = lowerBound; }
            else { newX = upperBound; }
        }
    }
    private void CalculateNewZ(Vector3 pieceLocation, Vector3 roundedPieceLocation)
    {
        if (roundedPieceLocation.z % 2 == 0f) //For Z: Check if divisible by 2
        {
            newZ = roundedPieceLocation.z;
        }
        else //Odd number
        {
            float lowerBound = roundedPieceLocation.z - 1;
            float upperBound = roundedPieceLocation.z + 1;

            float midPoint = lowerBound + upperBound / 2;

            if (pieceLocation.z > midPoint) { newZ = lowerBound; }
            else { newZ = upperBound; }
        }
    }

    //Setter for toggled
    public void SetToggled(bool t)
    {
        toggled = t;
    }

    //Setter for locked
    public void SetLocked(bool l)
    {
        locked = l;
    }
}
