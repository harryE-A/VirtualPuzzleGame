using UnityEngine;

public class PiecePos : MonoBehaviour
{
    [SerializeField] Vector3 startPos;
    public Vector3 ToStartPos()
    {
        return startPos;
    }
}
