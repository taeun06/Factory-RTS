using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "ConveyorTile", menuName = "Scriptable Objects/ConveyorTile")]
public class ConveyorTile : Tile
{
    public Vector3Int Direction;
    public float Speed;
}
