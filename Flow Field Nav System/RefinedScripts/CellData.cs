using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct CellData : IComponentData
{
    public float3 worldPos;
    public int2 gridIndex;
    public byte cost;
    public ushort bestCost;
    //public int2 bestDirection;
}

public struct CellsBestDirection : IComponentData
{
    public int2 bestDirection;
}

public struct CellBestDirectionBuff : IBufferElementData
{
    public int2 bestDirection;

    public static implicit operator int2(CellBestDirectionBuff Cellbofdirection)
    {
        return Cellbofdirection.bestDirection;
    }

    public static implicit operator CellBestDirectionBuff(int2 e)
    {
        return new CellBestDirectionBuff { bestDirection = e };
    }
}

//Note I can have several layers on a single Flowfield each layer has a single BestDirection which is a bufferelementdata so that I can have multiple 
public struct CellBDLayer : IComponentData
{
    public int intVal;
}

public struct CellBDLayerToCalc : IComponentData
{
    public int intVal;
}

public struct CellDestinationsBuffer : IBufferElementData
{
    public int2 Destination;

    public static implicit operator int2(CellDestinationsBuffer CellDestinateBuff)
    {
        return CellDestinateBuff.Destination;
    }

    public static implicit operator CellDestinationsBuffer(int2 e)
    {
        return new CellDestinationsBuffer { Destination = e };
    }
}

public struct DestinationChangedTag : IComponentData { }

//TODO DO I really need this, this is probably more a waste of time and won't be more performant at all
public struct CellDestinationChangedBuff : IBufferElementData
{
    public bool HasItChanged;

    public static implicit operator bool(CellDestinationChangedBuff CellDestinateChangeBuff)
    {
        return CellDestinateChangeBuff.HasItChanged;
    }

    public static implicit operator CellDestinationChangedBuff(bool e)
    {
        return new CellDestinationChangedBuff { HasItChanged = e };
    }

}

public struct AddLayerSystemEvent : IComponentData { }

public struct RemoveLayerSystemEvent : IComponentData { }

public struct AddLayerTempDestinationsValueBuff : IBufferElementData
{


}

public struct AddLayerTempBestDirectValsBuff : IBufferElementData
{


}