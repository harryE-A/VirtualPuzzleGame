using UnityEngine;

public class GridPoint : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Touching", collision.gameObject);

        this.name = collision.gameObject.name;
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Touching: {0}", collision.gameObject);
        this.name = "Point";
    }
}
