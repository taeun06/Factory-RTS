using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public enum dir
{
    Up = 90,
    Down = 270,
    Left = 180,
    Right = 0
}

public class conveyorType
{
    public int Imgtype { get; set; }
    public bool reverse { get; set; }
}

public class ConstructionManager : MonoBehaviour
{
    public Tilemap CollidableLayer;
    public Tilemap nonCollidableLayer;
    public Tilemap MechanicsLayer;
    public Tile[] conveyorImg = new Tile[5];
    public ConveyorTile[] conveyor = new ConveyorTile[4];
    private conveyorType Straight    = new conveyorType {Imgtype = 0 ,reverse = false};
    private conveyorType LCorner     = new conveyorType {Imgtype = 1 ,reverse = true };
    private conveyorType RCorner     = new conveyorType {Imgtype = 1 ,reverse = false};
    private conveyorType LJunction   = new conveyorType {Imgtype = 2 ,reverse = true };
    private conveyorType RJunction   = new conveyorType {Imgtype = 2 ,reverse = false};
    private conveyorType LRJunction  = new conveyorType {Imgtype = 3 ,reverse = false};
    private conveyorType TriJunciton = new conveyorType {Imgtype = 4 ,reverse = false};
    private bool BuildMode = false;
    private dir Direction = dir.Right;

    public void ModeChange(InputAction.CallbackContext context)
    {
        if(context.started) BuildMode = !BuildMode;
    }

    public void Build(InputAction.CallbackContext context)
    {
        if(context.started && BuildMode)
        {
            Vector3Int cellPos = MouseCellPos(nonCollidableLayer);
            MechanicsLayer.SetTile(cellPos ,conveyor[(int)Direction / 90]);
            conveyorType ConveyorType = GetConveyorType(MechanicsLayer ,cellPos);
            ConveyorTile Conveyor = MechanicsLayer.GetTile<ConveyorTile>(cellPos);
            nonCollidableLayer.SetTile(cellPos, conveyorImg[ConveyorType.Imgtype]);
            TransformTile(nonCollidableLayer ,cellPos ,Direction ,ConveyorType);

            conveyorType ForwardType = GetConveyorType(MechanicsLayer ,cellPos + Conveyor.Direction);
            if(ForwardType != null) nonCollidableLayer.SetTile(cellPos + Conveyor.Direction ,conveyorImg[ForwardType.Imgtype]);
        }
    }

    private conveyorType GetConveyorType(Tilemap tilemap, Vector3Int cell)
    {
        ConveyorTile conveyor = tilemap.GetTile<ConveyorTile>(cell);
        if(conveyor == null) return null;
        Vector3Int Forward = conveyor.Direction;
        Matrix4x4 rotation90 = Matrix4x4.Rotate(Quaternion.Euler(0,0,90));
        Vector3Int Left = Vector3Int.RoundToInt(rotation90.MultiplyPoint(Forward));

        ConveyorTile ForwardConveyor = tilemap.GetTile<ConveyorTile>(cell + Forward);
        ConveyorTile BackConveyor    = tilemap.GetTile<ConveyorTile>(cell - Forward);
        ConveyorTile LeftConveyor    = tilemap.GetTile<ConveyorTile>(cell + Left);
        ConveyorTile RightConveyor   = tilemap.GetTile<ConveyorTile>(cell - Left);

        if(RightConveyor != null && RightConveyor.Direction == Left)
        {
            if(LeftConveyor != null && LeftConveyor.Direction == -Left)
            {
                if(BackConveyor != null && BackConveyor.Direction == Forward) return TriJunciton;
                else                                                          return LRJunction;
            }
            else
            {
                if(BackConveyor != null && BackConveyor.Direction == Forward) return RJunction;
                else                                                          return RCorner;
            }
        }
        else
        {
            if(LeftConveyor != null && LeftConveyor.Direction == -Left)
            {
                if(BackConveyor != null && BackConveyor.Direction == Forward) return LJunction;
                else                                                          return LCorner;
            }
            else
            {
                return Straight;
            }
        }
    }

    private void TransformTile(Tilemap tilemap ,Vector3Int cell ,dir direction ,conveyorType Type)
    {
        Matrix4x4 Transformer;
        if(Type.reverse) Transformer = Matrix4x4.Scale(new Vector3(-1,1,1));
        else Transformer = Matrix4x4.identity;
        Transformer *= Matrix4x4.Rotate(Quaternion.Euler(0,0,(int)direction));
        tilemap.SetTransformMatrix(cell,Transformer);
    }

    public void Destroy(InputAction.CallbackContext context)
    {
        if(context.started && BuildMode)
        {
            Vector3Int cellPos = MouseCellPos(nonCollidableLayer);
            nonCollidableLayer.SetTransformMatrix(cellPos,Matrix4x4.identity);
            nonCollidableLayer.SetTile(cellPos,null);
            MechanicsLayer.SetTile(cellPos,null);
        }
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        if(context.started && BuildMode)
        {
        Direction = Clockwise(Direction);
        }
    }

    private dir Clockwise(dir direction)
    {
        switch(direction)
        {
            case dir.Up:
            return dir.Right;
            case dir.Right:
            return dir.Down;
            case dir.Down:
            return dir.Left;
            case dir.Left:
            return dir.Up;
        }
        return dir.Up;
    }

    private dir CounterClockwise(dir direction)
    {
        switch(direction)
        {
            case dir.Up:
            return dir.Left;
            case dir.Left:
            return dir.Down;
            case dir.Down:
            return dir.Right;
            case dir.Right:
            return dir.Up;
        }
        return dir.Up;
    }

    private Vector3Int MouseCellPos(Tilemap tilemap)
    {
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int cellPosition = tilemap.WorldToCell(MouseWorldPos);
        return cellPosition;
    }
}
