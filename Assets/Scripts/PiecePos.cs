using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class PiecePos : MonoBehaviour
{
    [SerializeField] public int pieceId;
    [SerializeField] private Vector3 startPos;

    public Vector3 pos;
    public Quaternion q;
    public bool isToggled;


    public Vector3 GetStartPos() //For returning pieces to their starting location (off the board)
    {
        return startPos;
    }

    public void SetPosRot() //Set the internal values to the values held within the transform component
    {
        pos = transform.position;
        q = transform.rotation;
    }

    public void SetToggled(bool t) //Set toggled
    {
        isToggled = t;
    }

    public void Apply() //Apply internal values back to transform component and piece script
    {
        transform.position = pos;
        transform.rotation = q;
        GetComponentInChildren<Piece>().SetToggled(isToggled);
    }
}
