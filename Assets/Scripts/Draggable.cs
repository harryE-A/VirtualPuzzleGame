using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
/**
 * This script uses code from the following YouTube tutorial - https://www.youtube.com/watch?v=kWRyZ3hb1Vc
 * I have adapted the code and made changes relevant to my specific project
 */

//Tutorial Code:
public class Draggable : MonoBehaviour
{
    Vector3 mousePos;

    //Get where the mouse is relative to the camera
    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    //Called when the user mouses down on a collider
    private void OnMouseDown()
    {
        mousePos = Input.mousePosition - GetMousePos();
    }

    //Moves the object being dragged by setting it's position to where the mouse is relative to the camera
    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos);

        //Lock vertical
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    //Own Code:  
    //When user lets go of drag:
    private void OnMouseUp()
    {
        Vector3 pieceLocation = transform.position;

        float newX;
        float newZ;

        Vector3 roundedPieceLocation = new Vector3(Mathf.Round(pieceLocation.x), 0, Mathf.Round(pieceLocation.z)); //Round to integer

        if (roundedPieceLocation.x % 2 == 0f) //For X Check if divisible by 2
        {
            newX = roundedPieceLocation.x;
        }
        else //Odd number
        {
            float lowerBound = roundedPieceLocation.x - 1;
            float upperBound = roundedPieceLocation.x + 1;

            float midPoint = lowerBound + upperBound / 2;

            if (pieceLocation.x > midPoint) {newX = lowerBound;}
            else {newX = upperBound;}
        }

        if (roundedPieceLocation.z % 2 == 0f) //For Z Check if divisible by 2
        {
            newZ = roundedPieceLocation.z;
        }
        else //Odd number
        {
            float lowerBound = roundedPieceLocation.z - 1;
            float upperBound = roundedPieceLocation.z + 1;

            float midPoint = lowerBound + upperBound / 2;

            if (pieceLocation.z > midPoint) {newZ = lowerBound;}
            else {newZ = upperBound;}
        }

        transform.position = new Vector3(newX, 0, newZ); //Set new coordinates
    }
}
