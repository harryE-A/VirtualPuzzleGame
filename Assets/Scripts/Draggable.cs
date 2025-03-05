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
    private void OnMouseUp()
    {
        //Find every gameobject that is overlapping with the dropped piece
        //Get snapping points
        //Set the piece location
        //Set the snapping points to not let anything else snap

    }
}
