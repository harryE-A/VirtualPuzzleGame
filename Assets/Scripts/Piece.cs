using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
/**
 * This script uses code from the following YouTube tutorial - https://www.youtube.com/watch?v=kWRyZ3hb1Vc
 * Specifically for dragging the pieces and working out where they should move relative to the camera.
 * I have adapted the code and added my own sections that interlink to solve my specific technical issues.
 * 
 * The code has been marked with comments, the rest of this file contains my own code.
 */

public class Piece : MonoBehaviour
{           
    private float newX; //Values for X and Z for snapping
    private float newZ;

    //Which way the piece is flipped:
    [SerializeField] private bool toggled = false; //True = side with 2 protruding balls. False = 1 protruding

    [SerializeField] private bool placed;   //Is the piece on the board already
    [SerializeField] private bool locked;   //Is the player allowed to move the piece
    [SerializeField] private bool dragging; //Is the piece currently being dragged

    private bool colliding;

    Vector3 mousePos; 

    //Input Actions
    InputAction rotateAction;
    InputAction toggleAction;

    

    //Tutorial Code:
    private Vector3 GetMousePos() //Get where the mouse is relative to the camera
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown() //Called when the user clicks on a collider
    {
        mousePos = Input.mousePosition - GetMousePos();
        
    }

    private void OnMouseDrag() //Moves the object being dragged by setting it's position to where the mouse is relative to the camera
    {
        dragging = true;
        placed = false;

        transform.parent.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos);
    //Tutorial Code End.
        
        transform.parent.position = new Vector3(transform.position.x, 0, transform.position.z); //Lock Y Axis
        
        //Rotation
        if (rotateAction.WasPerformedThisFrame()) {RotatePiece();}

        //Toggling (flipping)
        if (toggleAction.WasPerformedThisFrame()) {TogglePiece();}
   
    }

    private void OnMouseUp()
    {
        Vector3 pieceLocation = transform.parent.position;
        Vector3 roundedPieceLocation = new Vector3(Mathf.Round(pieceLocation.x), 0, Mathf.Round(pieceLocation.z)); //Round to integer

        CalculateNewX(pieceLocation, roundedPieceLocation);
        CalculateNewZ(pieceLocation, roundedPieceLocation);

        transform.parent.position = new Vector3(newX, 0, newZ); //Set new coordinates

        dragging = false;
    }

    private void Update()
    {
        if(!dragging && colliding && !placed) 
        {
            transform.parent.position = GetComponentInParent<PiecePos>().ToStartPos(); //Dont allow, send back to edge of board   
        }
        if(!dragging && !colliding)
        {
            placed = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliding = true;

        //Debug.Log("Colliding True for: " + this.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        colliding = true;
        //Debug.Log("Colliding True for: " + this.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
        //Debug.Log("Colliding False for: " + this.name);
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

        if(toggled) //Two Protruding balls
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
        toggleAction = InputSystem.actions.FindAction("Toggle");
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
}
