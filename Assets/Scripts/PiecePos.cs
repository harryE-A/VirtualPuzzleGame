using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PiecePos : MonoBehaviour
{
    [SerializeField] public Vector3 startPos;

    [SerializeField] int pieceId;

    //The starting location of the piece in the editor (off the board)
    public Vector3 ToStartPos()
    {
        return startPos;
    }

    public Vector3 GetPos() //Returns the current pos
    {
        return transform.position;
    }

    public Quaternion GetRot() //Returns current rotation
    {
        return transform.rotation;
    }

    public int GetId() //Get piece Id
    {
        return pieceId;
    }
}
