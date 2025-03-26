using UnityEngine;

public class PiecePos : MonoBehaviour
{
    [SerializeField] Vector3 startPos;

    //Back to start if illegal placement
    public Vector3 ToStartPos()
    {
        return startPos;
    }
}
