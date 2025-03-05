using UnityEngine;

/** This script uses code from a YouTube tutorial found at https://www.youtube.com/watch?v=kkAjpQAM-jE&t=1s
 *  It details the instantiation of the grid using grid tile prefabs, how to select tiles on the grid
 */

public class Grid : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private GridTile gridTile;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {

                var tile = Instantiate(gridTile, new Vector3(x, 0, z), Quaternion.identity);
                tile.name = $"Tile {x}, {z}";
            }
        }
    }
}
