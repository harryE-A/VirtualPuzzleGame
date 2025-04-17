using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using UnityEngine;

public class PiecePos : MonoBehaviour
{
    [SerializeField] public int pieceId;

    public Vector3 pos;
    public Quaternion q;

    [SerializeField] private Vector3 startPos;

    public Vector3 ToStartPos() //The starting location of the piece (off the board)
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

    //Using Awake() rather than Start() means only when scene is loaded, e.g play mode, not in editor.
    private void Awake() //When scene is loaded, position piece
    {
        transform.position = pos;
        transform.rotation = q;
    }
}
