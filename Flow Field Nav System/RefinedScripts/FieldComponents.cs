using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using DotsFlowField;

//namespace DotsFlowField
//{
    public struct FlowFieldData : IComponentData//I dont currently need this so wont use this
    {
        public int2 gridSize;
        public float cellRadius;
        public float3 Destination;  // I dont think I need this on here will have it separate as DestinationCellData
        public int2 DestinationCell;
        public CurrentFlowfield currenfield; //TODO Need to figure out if this should be a separate value on the same entity or just combined into this one
        public int MaxFlowLayers;
    
    }
    public struct FlowfieldMemberOf : IComponentData
    {
        public CurrentFlowfield myflowfeld;

    }

    public enum CurrentFlowfield //TODO This is enough for five flowfield can increase to more as needed or maybe I should do it differently I don't know I want to keep the flowfield number limited don't want to go over max 20 say
    {
        Flowfield1,
        Flowfield2,
        Flowfield3,
        Flowfield4,
        Flowfield5,
        Flowfield6,
        Flowfield7,
        Flowfield8,
        Flowfield9,
        Flowfield10
    }

    
//public struct CellData : IComponentData    
//{
        
//    public float3 worldPos;
        
//    public int2 gridIndex;
        
//    public byte cost;
        
//    public ushort bestCost;
        
//    public int2 bestDirection;
    
//}

    public struct CellDataBuffer : IBufferElementData
    {
        public CellData celldata;

        public static implicit operator CellData(CellDataBuffer cellbooferelement)
        {
            return cellbooferelement.celldata;
        }

        public static implicit operator CellDataBuffer(CellData e)
        {
            return new CellDataBuffer { celldata = e };
        }
    }

    public struct FlowfieldVertPointsBuff : IBufferElementData
    {
        public float3 Float3points;

        public static implicit operator float3(FlowfieldVertPointsBuff flowvertelem)
        {
            return flowvertelem.Float3points;
        }

        public static implicit operator FlowfieldVertPointsBuff(float3 e)
        {
            return new FlowfieldVertPointsBuff { Float3points = e };
        }


    }

    public struct NewFlowFieldEvent : IComponentData
    { }

    public struct DestinationCellData : IComponentData
    {
        public int2 int2Value;
    }

    public struct CalculateCellCostEventTag : IComponentData { }
//}

//Note A different way of doing an IbufferelmentData that I'm guessing is the same as the above
//public struct MapMemoryBuffer : IBufferElementData
//{
//    public bool Value;
//    public static implicit operator bool(MapMemoryBuffer b) => b.Value;
//    public static implicit operator MapMemoryBuffer(bool v) =>
//        new MapMemoryBuffer { Value = v };
//}

//public struct MaxHitPoints : IComponentData
//{
//    public int Value;
//    public static implicit operator int(MaxHitPoints b) => b.Value;
//    public static implicit operator MaxHitPoints(int v) =>
//        new MaxHitPoints { Value = v };
//}