using UnityEngine;

public class PiecePos : MonoBehaviour
{
    [SerializeField] Vector3 startPos;

    [SerializeField] int PieceId;

    public Vector3 ToStartPos()
    {
        return startPos;
    }
}
