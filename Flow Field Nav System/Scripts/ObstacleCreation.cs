using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

//[GenerateAuthoringComponent]
//public class ObstacleObject : IComponentData
//{

//}
//[RequiresEntityConversion]

//public class PlayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
//{
//    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
//    {
//        dstManager.SetName(entity, name);
//        dstManager.AddComponent(entity, typeof(RotationEulerXYZ));
//        dstManager.AddComponent(entity, typeof(Tag_KeepUpright));
//    }
//}

public class ObstacleCreation : MonoBehaviour, IConvertGameObjectToEntity
{
    //public GameObject thisgamyject;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        //thisgamyject = this.gameObject;

        //var boxcollid = thisgamyject.GetComponent<BoxCollider>();
        //boxcollid.center
        //dstManager.SetName(entity, "The Obstacles");


        //dstManager.AddComponent(entity, typeof(ObstacleAuthoring));
        //dstManager.AddComponentData<ObstacleAuthoring>(entity);
        dstManager.AddComponent<ObstacleTag>(entity);
        //dstManager.AddComponent(entity, typeof(ObstacleCollisionVerts));

        //dstManager.AddBuffer<ObstacleCollisionVerts>(entity);

        //var tempbuffer = dstManager.GetBuffer<ObstacleCollisionVerts>(entity);

        //var thismatrix = boxcollid.transform.localToWorldMatrix;

        //var extents = boxcollid.bounds.extents;

        //tempbuffer.Add(
        //CellData newCellData = new CellData
        ////            {
        ////                worldPos = othercellworldpos,
        ////                gridIndex = new int2(x, y),
        ////                //cost = cellCost,
        ////                bestCost = ushort.MaxValue,
        ////                bestDirection = int2.zero
        ////            };

        //dstManager.SetComponentData<ObstacleCollisionVerts>(entity, new ObstacleCollisionVerts { });



    }



}

public struct ObstacleTag : IComponentData { }

//public struct CellDataBuffer : IBufferElementData
//{
//    public CellData celldata;

//    public static implicit operator CellData(CellDataBuffer cellbooferelement)
//    {
//        return cellbooferelement.celldata;
//    }

//    public static implicit operator CellDataBuffer(CellData e)
//    {
//        return new CellDataBuffer { celldata = e };
//    }
//}

[InternalBufferCapacity(8)]
public struct ObstacleCollisionVerts : IBufferElementData
{
    public float3 float3verts;

    public static implicit operator float3(ObstacleCollisionVerts obstacelem)
    {
        return obstacelem.float3verts;
    }

    public static implicit operator ObstacleCollisionVerts(float3 e)
    {
        return new ObstacleCollisionVerts { float3verts = e };
    }

}
////[RequiresEntityConversion]
//public class WaypointsLine : MonoBehaviour//, IConvertGameObjectToEntity
//{

//    public WaypointsLine[] Dudese;

//    //    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
//    //    {

//    //        dstManager.RemoveComponent<RenderMesh>(entity);
//    //        dstManager.SetName(entity, "Barbeque");
//    //        dstManager.AddComponentData(entity, new WaypointsActual());
//    //        //dstManager.AddComponent<WaypointConnections>(entity);
//    //        //dstManager.SetComponentData(entity, new WaypointConnections.
//    //        dstManager.AddBuffer<WaypointConnections>(entity);
//    //foreach (var diddle in Dudese)
//    //        {
//    //var bufferelement = new WaypointConnections{Connections = diddle.position};

//    //var tempbuff = dstManager.GetBuffer<WaypointConnections>(entity);
//    //float3 temppos = diddle.Dudese;
//    //Dudese
//    //tempbuff.Add(bufferelement);
//    //tempbuff.Add(temppos);

//    //        }


//    //    }

//}

//Vector3[] verts = new Vector3[4];        // Array that will contain the BOX Collider Vertices
//BoxCollider b = go.GetComponent<BoxCollider>();

//verts[0] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
//verts[1] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
//verts[2] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
//verts[3] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;