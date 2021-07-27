using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

public class FlowFieldAddLayerSystem : SystemBase
{
    public EntityQuery DestinationQuery;

    
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<AddLayerSystemEvent>();
        RequireSingletonForUpdate<CellDestinationsBuffer>();
        RequireSingletonForUpdate<FlowFieldData>();       
        //Enabled = false;
    }


    protected override void OnUpdate()
    {      

        var execute66 = HasSingleton<AddLayerSystemEvent>() ? GetSingletonEntity<AddLayerSystemEvent>() : Entity.Null ; //Note I think this works just as good as the above maybe better?
        EntityManager.DestroyEntity(execute66);
        
        var Flowfielddude = GetSingleton<FlowFieldData>();

        var tempentity = GetSingletonEntity<CellDestinationsBuffer>();        

        var tempDestinatebuffo = GetBuffer<CellDestinationsBuffer>(tempentity);

        if (tempDestinatebuffo.Length >= Flowfielddude.MaxFlowLayers) return;//TODO Need to keep an eye this I may need a more robust checking system but this may work for the whole thing in theory as long as I only do one add in this

        int2 tempint2 = new int2(0, 0);

        Entities.ForEach((DynamicBuffer<CellBestDirectionBuff> obbybuffer) =>
        {
            int2 tempint2 = new int2(0, 0);
            obbybuffer.Add(tempint2);

        }).ScheduleParallel();
        
        tempDestinatebuffo.Add(tempint2);

    }
}

//TODO I won't use this currently no need to remove layers as long as I keep them max layers quite limited Maybe I can just reset the layers on a scene change or something, maybe change this to Reset layers System or maybe it would just suffice with the flowfield recreation
public class FlowFieldRemoveLayerSystem : SystemBase //TODO Will I ever want to remove a layer and how exactly and where from will I want to remove a layer, it would surely have to be at a specific index for a specific bunch of entities
{
    public EntityQuery DestinationQuery;

    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<RemoveLayerSystemEvent>();
        RequireSingletonForUpdate<CellDestinationsBuffer>();
        RequireSingletonForUpdate<FlowFieldData>();
    }

    protected override void OnUpdate()
    {
        if (HasSingleton<RemoveLayerSystemEvent>())
        {
            var execute66 = GetSingletonEntity<RemoveLayerSystemEvent>();
            EntityManager.DestroyEntity(execute66);

        }
        
        var Flowfielddude = GetSingleton<FlowFieldData>();

        var tempentity = GetSingletonEntity<CellDestinationsBuffer>();

        var tempDestinatebuffo = GetBuffer<CellDestinationsBuffer>(tempentity);

        if (tempDestinatebuffo.Length >= Flowfielddude.MaxFlowLayers) return;//Note It seems to work ok so far with just this check
                

        Entities.ForEach((DynamicBuffer<CellBestDirectionBuff> obbybuffer) =>
        {
           

        }).ScheduleParallel();

        
    }

}


//public struct PlayerJobContext
//{
//    public Entity entity;
//    ComponentDataFromEntity<Position> posFromEntity;
//    BufferFromEntity<MapViewBuffer> mapViewFromEntity;

//    public PlayerJobContext(SystemBase system, bool readOnly)//THis is the constructor or whatever you idiot
//    {
//        entity = system.GetSingletonEntity<Player>();
//        posFromEntity = system.GetComponentDataFromEntity<Position>(readOnly);
//        mapViewFromEntity = system.GetBufferFromEntity<MapViewBuffer>(readOnly);
//    }

//    public int2 Position
//    {
//        get => posFromEntity[entity];
//        set => posFromEntity[entity] = value;
//    }

//    public GridData2D<bool> GetMapView(int2 mapSize)
//    {
//        var viewArr = mapViewFromEntity[entity].Reinterpret<bool>().AsNativeArray();
//        var grid = new GridData2D<bool>(viewArr, mapSize);
//        return grid;
//    }
//}