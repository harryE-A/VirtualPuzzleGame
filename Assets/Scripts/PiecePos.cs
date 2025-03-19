using UnityEngine;


[CreateAssetMenu(fileName = "New PiecePos", menuName = "Scriptable Objects/PiecePos")]
public class PiecePos : ScriptableObject
{   
    [SerializeField] Vector3 position;
    [SerializeField] Quaternion rotation;

    public void setPos(string pieceName)
    {
        GameObject piece = GameObject.Find(pieceName);

        position = piece.transform.parent.position;
        rotation = piece.transform.rotation;

    }

}
