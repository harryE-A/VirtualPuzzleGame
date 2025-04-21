using UnityEngine;

public class GridPoint : MonoBehaviour
{
    [SerializeField] public bool filled; //Is this specific point on the grid filled?

    private void OnTriggerEnter(Collider other)
    {
        filled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        filled = false;
    }
}
