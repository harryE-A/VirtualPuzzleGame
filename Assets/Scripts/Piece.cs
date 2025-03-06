using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
/**
 * This script uses code from the following YouTube tutorial - https://www.youtube.com/watch?v=kWRyZ3hb1Vc
 * Specifically for dragging the pieces and working out where they should move relative to the camera.
 * I have adapted the code and added my own sections that interlink to solve my specific technical issues.
 */

//Tutorial Code:
public class Piece : MonoBehaviour
{           
    private float newX; //Values for X and Z for snapping
    private float newZ;

    Vector3 mousePos;

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
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos);

        
        transform.position = new Vector3(transform.position.x, 0, transform.position.z); //Lock vertical
    }
//Tutorial Code End.



//Own Code:  
    private void OnMouseUp()
    {
        Vector3 pieceLocation = transform.position;
        Vector3 roundedPieceLocation = new Vector3(Mathf.Round(pieceLocation.x), 0, Mathf.Round(pieceLocation.z)); //Round to integer

        CalculateNewX(pieceLocation, roundedPieceLocation);
        CalculateNewZ(pieceLocation, roundedPieceLocation);

        transform.position = new Vector3(newX, 0, newZ); //Set new coordinates
    }


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
