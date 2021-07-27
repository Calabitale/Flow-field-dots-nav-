using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

public class TerrainObstacleCreation : MonoBehaviour, IConvertGameObjectToEntity
{
    public GameObject thisgammyobject;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        thisgammyobject = this.gameObject;

        var thiscollider = thisgammyobject.GetComponent<Collider>();

        var thismatx = thisgammyobject.transform.localToWorldMatrix;

        var extents = thiscollider.bounds.extents;

        //var somink = thiscollider.
        var colliderminpoint = thiscollider.bounds.min;
        var collidermaxpoint = thiscollider.bounds.max;

        dstManager.AddComponent<ObstacleTag>(entity);
        //dstManager.AddComponent(entity, typeof(ObstacleCollisionVerts));
        var dude = dstManager.AddBuffer<ObstacleCollisionVerts>(entity);
        //dstManager.GetBuffer<ObstacleCollisionVerts>(entity);
        //TODO THis is not currently working need to make sure it works
        //dude.Add(new float3(thismatx.MultiplyPoint3x4(extents)));
        dude.Add(new float3(collidermaxpoint));
        //dude.Add(new float3(thismatx.MultiplyPoint3x4(new Vector3(-extents.x, extents.y, extents.z))));
        //dude.Add(new float3(thismatx.MultiplyPoint3x4(new Vector3(extents.x, extents.y, -extents.z))));
        //dude.Add(new float3(thismatx.MultiplyPoint3x4(new Vector3(-extents.x, extents.y, -extents.z))));
        //dude.Add(new float3(thismatx.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, extents.z))));
        //dude.Add(new float3(thismatx.MultiplyPoint3x4(new Vector3(-extents.x, -extents.y, extents.z))));
        //dude.Add(new float3(thismatx.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, -extents.z))));
        //dude.Add(new float3(thismatx.MultiplyPoint3x4(-extents)));
        dude.Add(new float3(colliderminpoint));
        //var someik = thismatx.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, -extents.z));
        //dude.Add(new float())


    }
}


