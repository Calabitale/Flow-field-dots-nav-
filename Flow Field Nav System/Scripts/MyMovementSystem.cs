using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using System;
using Unity.Collections;
using DotsFlowField;
using Unity.Jobs.LowLevel.Unsafe;


public class MyMovementSystem : SystemBase
{

    public float3 Target;
    public float Speed;
    public EntityQuery CelldataQuery;
    public EntityQuery flowfielddata;
    public Unity.Mathematics.Random Rundomnumcreator;
    protected override void OnCreate()
    {
        base.OnCreate();
        Target = new float3(-5, 0, 5);
        Speed = 15f;
        
        CelldataQuery = GetEntityQuery(ComponentType.ReadOnly<CellsBestDirection>());        
        RequireSingletonForUpdate<FlowFieldData>();
        Rundomnumcreator = new Unity.Mathematics.Random();

        Enabled = false;
    }

    protected override void OnStartRunning()
    {

    }

    protected override void OnDestroy()
    {
        
    }

    protected override void OnUpdate()
    {
        var Rundomnumcreator = new NativeArray<Unity.Mathematics.Random>(JobsUtility.MaxJobThreadCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        var r = (uint)UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        for (int i = 0; i < JobsUtility.MaxJobThreadCount; i++)
        {
            Rundomnumcreator[i] = new Unity.Mathematics.Random(r == 0 ? r + 1 : r);
        }
        
        //Rundomnumcreator.Length;
        //Debug.Log("The length of the rundom is " + numbofthreads);
        //var tempcelldata = CelldataQuery.ToComponentDataArray<CellsBestDirection>(Allocator.TempJob);
        var cellentitys = CelldataQuery.ToEntityArray(Allocator.TempJob);
        
        var tempflowfield = GetSingleton<FlowFieldData>();
        var spood = Speed;
        var temptarget = Target;

        var Timmydelta = Time.DeltaTime;       

        //Entity tempvent = GetSingletonEntity<TargetAuthoring>();

        //var temptargpos = GetComponent<Translation>(tempvent);
        //float3 temptargfloat = temptargpos.Value;
        //temptargfloat.y = 0;

        //var currfielddata = GetSingleton<FlowFieldData>();
        var cellindexworldpos = new GetCellIndexFromWorldPos();

        var tempflatus = new ToFlatIndex();

        //Todod I probably need more of method to collect all the layers and put them into their specific
        //TODO It might be best to create all these layers as soon as they are required
        //Entities.WithName("CollectSpecificLayer").ForEach((DynamicBuffer<CellBestDirectionBuff> dudedirection) =>
        //{


        //}).ScheduleParallel();

        //var rndum = Rundomnumcreator[0];
        //var ultimaterand = rndum.NextInt(0, 2);
        var TempBufferDirections = GetBufferFromEntity<CellBestDirectionBuff>(true);

        //No Physics, keep it simple physics can come in another game that will never be made just like this game just do simple translation movement
        Entities.WithReadOnly(TempBufferDirections).WithReadOnly(cellentitys).WithNativeDisableParallelForRestriction(Rundomnumcreator).WithAll<TestMoveojbectTag>().WithDisposeOnCompletion(Rundomnumcreator).WithDisposeOnCompletion(cellentitys).ForEach((int nativeThreadIndex, ref Translation trunslation, ref Rotation rutate, in CellBDLayer dooby) =>
        {
            var rndum = Rundomnumcreator[nativeThreadIndex];
            var ultimaterand = rndum.NextInt(0, 2);
            //float3 movedir = math.normalize(temptargfloat - trunslation.Value);
            var currcellpos = cellindexworldpos.Execute(trunslation.Value, tempflowfield.gridSize, tempflowfield.cellRadius * 2);

            var tempgridpos = tempflatus.Execute(currcellpos, tempflowfield.gridSize.y);
            //float2 moveDirection = tempcelldata[tempgridpos].bestDirection
            var currententity = cellentitys[tempgridpos];
            var idonevenknow = TempBufferDirections[currententity];
            float2 moveDirection = idonevenknow[dooby.intVal].bestDirection;
            float3 movedir = new float3(moveDirection.x, 0, moveDirection.y);

            #region Old Junk code I don't even know what this does anymore, why is this even here why am I even keeeping it nobody knows
            //float3 movedir = math.normalize(tempwaypoints[curryway.value] - curmovpos.Value);
            //if (math.distance(curmovpos.Value, tempwaypointscurr[curryway.value].Position) > 1.0f)
            //{
            //    float3 prevpos = curmovpos.Value;
            //    curmovpos.Value += movedir * movespeed[0] * deltatime;
            //    //var babeque = movespeed[0] * deltatime;
            //    //Debug.Log("The main distance moved is" + babeque);
            //    //This makes sure it doesn't jump beyond the waypoint no matter how high the speed, maybe use math.min instead of distance?

            //    //Need to replace this with a system that move it to the next waypoint with the remaindter of the distance
            //    //if (math.distance(prevpos, curmovpos.Value) > math.distance(curmovpos.Value, tempwaypointscurr[curryway.value].Position))
            //    //I'm pretty sure this isn't working exactly how it should ''
            //    var othervalue = movespeed[0] * deltatime;

            //    var tempadprevcurrpos = math.distance(prevpos, curmovpos.Value);
            //    //Debug.Log("This distance between the prevpos and current pos" + tempadprevcurrpos +"and movespeed * deltatime" + othervalue);
            //    if (math.distance(prevpos, curmovpos.Value) > math.distance(prevpos, tempwaypointscurr[curryway.value].Position))
            //    {
            //        var tempdistance = math.distance(curmovpos.Value, tempwaypointscurr[curryway.value].Position);
            //        curmovpos.Value = tempwaypointscurr[curryway.value].Position;
            //        curryway.value += 1;

            //        movedir = math.normalize((tempwaypointscurr[curryway.value].Position - curmovpos.Value));
            //        curmovpos.Value += movedir * tempdistance;
            //    }
            //}

            #endregion

            //float3 temptargetheight = temptargfloat;
            //temptargetheight.y = 0;

            float3 tempentheight = trunslation.Value;
            tempentheight.y = 0;

            //var tempother = new float2(0, 0);
            //var tempbool = math.Equals(moveDirection, tempother);
            //if (moveDirection.Equals(tempother))
            //{
            //TODO Its hitting this but its not the reason they aren't moving
            //Debug.Log($"This code has been executed {moveDirection}");
            //return;
            //} 
            //if (math.distance(tempentheight, ) < 0.5)
            //if(tempbool)
            //{                
            //    return;
            //}

            trunslation.Value += movedir * spood * Timmydelta;// * math.forward(rutate.Value);
            Rundomnumcreator[0] = rndum;

        }).ScheduleParallel();

        #region OLd rough code to keep just in case
        //    if (_flowFieldEntity.Equals(Entity.Null)) {return;}
        //    float deltaTime = Time.DeltaTime;
        //    FlowFieldData flowFieldData = _flowFieldData;
        //    int2 destinationCell = _destinationCellData.destinationIndex;
        //    JobHandle jobHandle = new JobHandle();
        //    jobHandle = Entities.ForEach((ref PhysicsVelocity physVelocity, ref EntityMovementData entityMovementData, 
        //        ref Translation translation) =>
        //    {
        //        int2 curCellIndex = FlowFieldHelper.GetCellIndexFromWorldPos(translation.Value, flowFieldData.gridSize,
        //            flowFieldData.cellRadius * 2);

        //        if (curCellIndex.Equals(destinationCell))
        //        {
        //            entityMovementData.destinationReached = true;
        //        }

        //        int flatCurCellIndex = FlowFieldHelper.ToFlatIndex(curCellIndex, flowFieldData.gridSize.y);
        //        float2 moveDirection = _cellDataContainer[flatCurCellIndex].bestDirection;
        //        float finalMoveSpeed = (entityMovementData.destinationReached ? entityMovementData.destinationMoveSpeed : entityMovementData.moveSpeed) * deltaTime;

        //        physVelocity.Linear.xz = moveDirection * finalMoveSpeed;
        //        //translation.Value.y = 0f;

        //    }).ScheduleParallel(jobHandle);
        //    jobHandle.Complete();
        //}

        //protected override void OnDestroy()
        //{
        //    _cellDataContainer.Dispose();
        //}
        #endregion

    }
}

public class DestroyEntitysatDest : SystemBase 
{
    //Todo There's one problem with this maybe more, it has to check every loop whether the entitys are in the destination square the more entity's there are the more cycles this uses and its just a waste it would be best to mark entity's
    //somehow to be destroyed, the more entities there are the more work this does and the majority of the time it will not be destoying the entities it will jut be checking that they need to be destroyed and not much else, better to tag them or whatever when I ever do it properly later on
    public EndSimulationEntityCommandBufferSystem endcommbuff;
    //public EntityQuery celldestbuffQuery;
    //public DynamicBuffer<CellDestinationsBuffer> dudesBuffer;
    protected override void OnCreate()
    {
        base.OnCreate();
        endcommbuff = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        Enabled = false;

        //celldestbuffQuery = GetEntityQuery(ComponentType.ReadOnly<CellDestinationsBuffer>());
    }

    protected override void OnUpdate()
    {
        var GetCellindpos = new GetCellIndexFromWorldPos();

        var ecb = endcommbuff.CreateCommandBuffer().AsParallelWriter();

        //var destbuffEntitys = celldestbuffQuery.ToEntityArray(Allocator.Temp);
        //var destbuffSingle = HasSingleton<CellDestinationsBuffer>() ? GetSingletonEntity<CellDestinationsBuffer>() : default;
       
        
        //if (destbuffSingle == Entity.Null)
        //{
        //    return;

        //}
       
        //var dudesBuffer = GetBuffer<CellDestinationsBuffer>(destbuffSingle);
        //Debug.Log("The distinations buffer are" + dudesBuffer[0].Destination + dudesBuffer[3].Destination);
        var targ1 = new float3(35, 0, 70);
        var targ2 = new float3(35, 0, -70);


        Entities.ForEach((Entity entity, int entityInQueryIndex, in Translation currentpos) =>
        {
           
            var curcelpos = GetCellindpos.Execute(currentpos.Value, 100, 1);

            //for (int i = 0; i < dudesBuffer.Length; i++)
            //{
            //    if (curcelpos.Equals(dudesBuffer[i].Destination))
            //    {
            //        //Debug.Log($"Its not woring correctly {curcelpos }, {destinatipos}");
            //        ecb.DestroyEntity(entityInQueryIndex, entity);
            //    }
            //}

            if (math.distance(currentpos.Value, targ1) < 2)
            {
                //Debug.Log($"Its not woring correctly {curcelpos }");
                ecb.DestroyEntity(entityInQueryIndex, entity);
            }
            else if (math.distance(currentpos.Value, targ2) < 2)
            {
                ecb.DestroyEntity(entityInQueryIndex, entity);
            }

        }).ScheduleParallel();

        endcommbuff.AddJobHandleForProducer(this.Dependency);
    }
}

public struct GetCellIndexFromWorldPos
{

    public int2 Execute(float3 worldPos, int2 gridSize, float cellDiameter)
    {
        float percentX = worldPos.x / (gridSize.x * cellDiameter);
        float percentY = worldPos.z / (gridSize.y * cellDiameter);

        percentX = math.clamp(percentX, 0f, 1f);
        percentY = math.clamp(percentY, 0f, 1f);

        int2 cellIndex = new int2
        {
            x = math.clamp((int)math.floor((gridSize.x) * percentX), 0, gridSize.x - 1),
            y = math.clamp((int)math.floor((gridSize.y) * percentY), 0, gridSize.y - 1)
        };

        return cellIndex;
    }
}
