using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using DotsFlowField;
public class CalculateCellCostSystem : SystemBase
{
    public EntityQuery ObstacleEnt;

    public NativeArray<ObstacleCollisionVerts> tempbuff;

    public EntityQuery TerrainObsQuery;

    //public EntityQuery CellDataplusEntity;

    protected override void OnCreate()
    {
        Enabled = false;
    }


    protected override void OnStartRunning()
    {
        RequireSingletonForUpdate<CalculateCellCostEventTag>();
        ObstacleEnt = GetEntityQuery(typeof(Translation), ComponentType.ReadOnly<ObstacleTag>());

        EntityQueryDesc queryDescription = new EntityQueryDesc();
        queryDescription.All = new[] { ComponentType.ReadOnly<ObstacleCollisionVerts>() };
        TerrainObsQuery = GetEntityQuery(queryDescription);

        //CellDataplusEntity = GetEntityQuery(ComponentType.ReadOnly<CellData>());



        //queryDescription.All = new[] { ComponentType.ReadOnly<MyBufferElement>() };
        //query = GetEntityQuery(queryDescription);
       

    }

    protected override void OnUpdate()   
    {
       

        Debug.Log("CalculateCellCost original is still running for some reason  ");
        
        if (HasSingleton<CalculateCellCostEventTag>())
        {
            var tempent = GetSingletonEntity<CalculateCellCostEventTag>();
            EntityManager.DestroyEntity(tempent);
        }
        //var comparisonArray = new NativeArray<ObstacleCollisionVerts>(< number of entities with ObstacleCollisionVerts * length of each buffer >, Allocator.TempJob);

        //Entities.WithParallelRestrictionDisabled(comparisonArray).ForEach((in < Some sort of ComponentData containing a unique index >, in DynamicBuffer < ObstacleCollisionVerts > OCV) => { }).ScheduleParallel;

        //ObstacleEnt = GetEntityQuery(typeof(Translation), ComponentType.ReadOnly<ObstacleTag>());

        //var colliderpoints = EntityManager.CreateEntityQuery(typeof(ObstacleCollisionVerts));
        //var colliderents = colliderpoints.ToEntityArray(Allocator.TempJob);

        var colliderents = TerrainObsQuery.ToEntityArray(Allocator.TempJob);

        Debug.Log("The number of obstaclevert entities in the old CalculateCellCost system are" + TerrainObsQuery.CalculateEntityCount());
        //if (colliderents.Length == 0)
        //    return;

        //var tempbuff = new NativeArray<ObstacleCollisionVerts>(colliderents.Length, Allocator.TempJob);
        //for(int i = 0; i < colliderents.Length; i++)
        //{

        //    var ompbuff = GetBuffer<ObstacleCollisionVerts>(colliderents[i]);
        //    var somebuff = ompbuff.ToNativeArray(Allocator.TempJob);
        //    //tempbuff = 

        //if (colliderents.Length > 0)
        //{
        //    int tempcounter = 0;

        //    //obstaclebuffers = new NativeArray<ObstacleCollisionVerts>(colliderents.Length, Allocator.Persistent);
        //    obstacleverts = new NativeArray<float3>(colliderents.Length * 8, Allocator.Persistent);  //TODO Maybe this is not the right length or something but there are errant values that should not exist either on the objects or within the 
        //    for (int i = 0; i < colliderents.Length; i++)
        //    {
        //        var tempbuffer = currentmanager.GetBuffer<ObstacleCollisionVerts>(colliderents[i]);

        //        //obstacleverts = new NativeArray<float3>(collideren, Allocator.Persistent);
        //        for (int j = 0; j < tempbuffer.Length; j++)
        //        {
        //            obstacleverts[tempcounter] = tempbuffer[j];
        //            tempcounter++;
        //        }

        //    }

        //}

        //var comparisonArray = new NativeArray<ObstacleCollisionVerts>(colliderents.Length * 2, Allocator.TempJob);
        var comparisonArray = new NativeArray<float3x2>(colliderents.Length, Allocator.TempJob);

        
        ////Debug.Log("The comparison array size is " + comparisonArray.Length);

        ////Debug.Log("The comparisonarray length is " + comparisonArray.Length);
        //int jobcounter = 0;
        //TODO If I wanted to make this multithreaded I could use int nativeThreadIndex instead of int entityInQueryIndex, will this work multithreaded I'm not so sure it will
        Entities.ForEach((int entityInQueryIndex, DynamicBuffer<ObstacleCollisionVerts> obbybuffer) =>
        {
            float3x2 tempfloat = new float3x2 { c0 = obbybuffer[0].float3verts, c1 = obbybuffer[1].float3verts };

            comparisonArray[entityInQueryIndex] = tempfloat;



        }).ScheduleParallel();//Not sure if this should single main thread or multithreaded it seems to currently be a bit slower wit multithread but when I add more collision obstacles it may work better with multithread will leave as multithread for now though
                              //TODO Maybe it would be simpler if I just put the obstaclecollisionverts buffer as a float3x2 will leave that for a while and maybe do it later or probably not at all, then again the way I have it now it makes it easier to add and deduct vert points


        //for (int i = 0; i < comparisonArray.Length; i++)
        //{
        //    Debug.Log("The values in the comparisonarray are" + comparisonArray[i].c0);
        //    Debug.Log("The values in the comparisonarray are" + comparisonArray[i].c1);

        //}
        Debug.Log("The original array size is " + comparisonArray.Length);
        

        Entities.ForEach((ref CellData cooldata, in DynamicBuffer<FlowfieldVertPointsBuff> flowverts) =>
        {
            //Debug.Log("Is it even going into here");

            #region Old Commented out code


            //for (int i = 0; i < colliderents.Length; i++)
            //{
            //    //Debug.Log("Is it going into here");
            //    //var tempbuff = GetBuffer<ObstacleCollisionVerts>(colliderents[i]);

            //    //TODO I need to figure this out this is incorrect 
            //    //var tempbool = (flowverts[0].Float3points.x <= tempbuff[1].float3verts.x && flowverts[1].Float3points.x >= tempbuff[0].float3verts.x)
            //    //&& (flowverts[0].Float3points.y <= tempbuff[1].float3verts.y && flowverts[1].Float3points.y >= tempbuff[0].float3verts.y)
            //    //&& (flowverts[0].Float3points.z <= tempbuff[1].float3verts.z && flowverts[1].Float3points.z >= tempbuff[0].float3verts.z);

            //    var tumpbuff = GetBufferFromEntity<ObstacleCollisionVerts>(true);
            //    var tempbuff = tumpbuff[colliderents[i]];
            //    //var tempbuff = comparisonArray[colliderents[i]];

            //    var tempbool = (flowverts[0].Float3points.x <= tempbuff[1].float3verts.x && flowverts[1].Float3points.x >= tempbuff[0].float3verts.x)
            //    && (flowverts[0].Float3points.y <= tempbuff[1].float3verts.y && flowverts[1].Float3points.y >= tempbuff[0].float3verts.y)
            //    && (flowverts[0].Float3points.z <= tempbuff[1].float3verts.z && flowverts[1].Float3points.z >= tempbuff[0].float3verts.z);


            //    //var wbuu = tempterrainobstacles[i].Value;
            //    //var currcollider = colliderents[i];
            //    //var colliderdunts = colliderpoints[i]
            //    //var currdist = math.distance(cooldata.worldPos, wbuu);

            //    if (tempbool)
            //    {
            //        //Debug.Log("The celldata is close to an obstacle");
            //        //var tempAABB = new MinMaxAABB();

            //        //Debug.Log("Is it even going into here");

            //        cooldata.cost = byte.MaxValue;
            //    }//TODO Perhaps need to add other ifs and maybe make it so that you can have different types of terrain that slows the character down like swamp water perhaps but then again maybe not


            //    //cooldata.worldPos

            //}
            #endregion

            cooldata.cost = 1;
            for (int j = 0; j < comparisonArray.Length; j++)
            {
                //Debug.Log("The flowvers values are " + flowverts[0].Float3points + flowverts[1].Float3points);

                var tempobsbuff = comparisonArray[j];

                //NOTE With this I have to make sure the min and max match exactly because for some reason if they are off or arent in the right order then this does not work at all
                var tempbool = (flowverts[0].Float3points.x <= tempobsbuff.c0.x && flowverts[1].Float3points.x >= tempobsbuff.c1.x)
                            && (flowverts[0].Float3points.y <= tempobsbuff.c0.y && flowverts[1].Float3points.y >= tempobsbuff.c1.y)
                            && (flowverts[0].Float3points.z <= tempobsbuff.c0.z && flowverts[1].Float3points.z >= tempobsbuff.c1.z);

                if (tempbool)
                {


                    cooldata.cost = byte.MaxValue;
                    //Debug.Log("The values are" + cooldata.cost);

                }


            }


        }).ScheduleParallel();

        

        //var tempcelldata = CellDataplusEntity.ToComponentDataArray<CellData>(Allocator.TempJob);
        #region FindNeighbourCellJob       

        //var tempNeigbourindices = new GetNeighborIndices();
       
        ////TODO NEed to figure out how to get the entities that are surrounding this entity
        //Entities.WithName("FindNeigbourCell").ForEach((DynamicBuffer<FlowfieldNeigborEntities> flowbuff, in FlowFieldData fluffydata, in CellData tempcelldata) =>
        //{
        //    NativeList<int2> tempint2list = new NativeList<int2>(Allocator.Temp);

        //    tempNeigbourindices.Execute(tempcelldata.gridIndex, fluffydata.gridSize, ref tempint2list);           

        //    for (int i = 0; i < tempint2list.Length; i++)
        //    {
        //        flowbuff.Add(tempint2list[i]);
        //    }

        //    tempint2list.Dispose();

        //}).ScheduleParallel();

        #endregion

        this.CompleteDependency();//TODO This way works for now its not the proper maybe trying using an entitycommandbuffer or something.


        colliderents.Dispose();

        comparisonArray.Dispose();

        //if (HasSingleton<CalculateCellCostEventTag>())
        //{
        //    var tempent = GetSingletonEntity<CalculateCellCostEventTag>();
        //    EntityManager.DestroyEntity(tempent);
        //}
        

    }

    //function intersect(a, b)
    //{
    //    return (a.minX <= b.maxX && a.maxX >= b.minX) &&
    //           (a.minY <= b.maxY && a.maxY >= b.minY) &&
    //           (a.minZ <= b.maxZ && a.maxZ >= b.minZ);
    //}

    //    bool AABBtoAABB(const TAABB& tBox1, const TAABB& tBox2)
    //{

    ////Check if Box1's max is greater than Box2's min and Box1's min is less than Box2's max
    //    return(tBox1.m_vecMax.x > tBox2.m_vecMin.x &&
    //    tBox1.m_vecMin.x<tBox2.m_vecMax.x &&
    //    tBox1.m_vecMax.y> tBox2.m_vecMin.y &&
    //    tBox1.m_vecMin.y<tBox2.m_vecMax.y &&
    //    tBox1.m_vecMax.z> tBox2.m_vecMin.z &&
    //    tBox1.m_vecMin.z<tBox2.m_vecMax.z);

    //    //If not, it will return false

}

//public class FindNeighbourSystem : SystemBase
//{
//    public EntityQuery Flowfieldquery;

//    protected override void OnCreate()
//    {
//        base.OnCreate();
//        Enabled = false;
//    }

//    protected override void OnStartRunning()
//    {
//        //RequireSingletonForUpdate<FindNeighborCellEvent>();
//        //Flowfieldquery = GetEntityQuery(ComponentType.ReadOnly<DotsFlowField.FlowFieldData>());
//        Enabled = false;
//    }

//    protected override void OnUpdate()
//    {
//        Enabled = false;

//        var tempflowfield = Flowfieldquery.ToComponentDataArray<FlowFieldData>(Allocator.TempJob);

//        if (HasSingleton<FindNeighborCellEvent>())
//        {

//            var tempevent = GetSingletonEntity<FindNeighborCellEvent>();
//            EntityManager.DestroyEntity(tempevent);
//            //Debug.Log("It's not destroying the entity for some reason");
//        }

//        var griddsiz = tempflowfield[0].gridSize;

//        var tempNeigbourindices = new GetNeighborIndices();

//        //TODO Need to figure out why this is not faster multithreaded its actually faster singlethreaded for some reason
//        //TODO Try turing this into a separate system like I was going to do in the first place at least it will work to check because the mulithreaded may be hangups from the rest of the System
//        Entities.WithName("FindNeigbourCell").ForEach((DynamicBuffer<FlowfieldNeigborEntities> flowbuff, in FlowfieldMemberOf fluffydata, in CellData tempcelldata) =>
//        {
//            //NativeList<int2> tempint2list = new NativeList<int2>(Allocator.Temp);
//            FixedList128<int2> tempint2list = new FixedList128<int2>();
//            //NativeArray<int2> tempint2list = new NativeArray<int2>(4, Allocator.Temp);

//            tempNeigbourindices.Execute(tempcelldata.gridIndex, griddsiz, true, ref tempint2list);

//            for (int i = 0; i < tempint2list.Length; i++)
//            {
//                flowbuff.Add(tempint2list[i]);
//            }

//            //tempint2list.Dispose();



//        }).ScheduleParallel();

//        tempflowfield.Dispose();
//        //TODO Burst seems to be run really inconsistantly
//        //this.CompleteDependency();

//        //if (HasSingleton<FindNeighborCellEvent>())
//        //{
            
//        //    var tempevent = GetSingletonEntity<FindNeighborCellEvent>();
//        //    EntityManager.DestroyEntity(tempevent);
//        //    //Debug.Log("It's not destroying the entity for some reason");
//        //}


//    }
//}

public struct GetNeighborIndices
{
    public int2 None;
    public int2 North;
    public int2 South;
    public int2 East;
    public int2 West;
    public int2 NorthEast;
    public int2 NorthWest;
    public int2 SouthEast;
    public int2 SouthWest;

    //public NativeArray<int2> thegroddirections;
    public static readonly int2[] cardinaldirections = new int2[4] {new int2(0, 1), new int2(0, -1), new int2(1, 0), new int2(-1, 0) };

    public static readonly int2[] alldirections;// = new int2[9] {new int2()}

    //public int2[] currentdirections;
    //public NativeList<int2> results;

    static GetNeighborIndices()
    {
        alldirections = new int2[]
        {
            new int2(0, 0),
            new int2(0, 1),
            new int2(0, -1),
            new int2(1, 0),
            new int2(-1, 0),
            new int2(1, 1),
            new int2(-1, 1),
            new int2(1, -1),
            new int2 (-1, -1)
            
        };
    }   

    public int2[] GetNeigbouralldirections()
    {

        return alldirections;
    }

    public FixedList128<int2> Execute(int2 originIndex, int2 gridSize, bool WhatDirections, ref FixedList128<int2> results)
    {
        //thegroddirections = new NativeArray<int2>();
        //NativeList<int2> = new NativeList<int2>()
        //thegroddirections = new int2[4]();

        //None = new int2(0, 0);
        //North = new int2(0, 1);
        //South = new int2(0, -1);
        //East = new int2(1, 0);
        //West = new int2(-1, 0);
        //NorthEast = new int2(1, 1);
        //NorthWest = new int2(-1, 1);
        //SouthEast = new int2(1, -1);
        //SouthWest = new int2(-1, -1);

        //thegroddirections[0] = None;
        //thegroddirections[0] = North;
        //thegroddirections[1] = South;
        //thegroddirections[2] = East;
        //thegroddirections[3] = West;

        //int2[] thegroddirections = new int2[] { };
        //thegroddirections[5] = NorthEast;
        //thegroddirections[6] = NorthWest;
        //thegroddirections[7] = SouthEast;
        //thegroddirections[8] = SouthWest;  
        int2[] currentdirections;


        if (WhatDirections)
        {
            currentdirections = cardinaldirections;
        }
        else
        {
            currentdirections = alldirections;
        }

        
        //int2 neighborIndex;
        foreach (int2 curDirection in currentdirections)            //might have to use for with burst
        {
            int2 neighborIndex;
            neighborIndex = originIndex + curDirection;
            neighborIndex = neighborIndex.x < 0 || neighborIndex.x >= gridSize.x || neighborIndex.y < 0 || neighborIndex.y >= gridSize.y ? new int2(-1, -1) : neighborIndex;
            if (neighborIndex.x >= 0)
            {
                results.Add(neighborIndex);//just discard the -1,-1 in the calling method

            }
        }


        //foreach (int2 curDirection in thegroddirections)
        //{
        //    int2 neighborIndex = GetIndexAtRelativePosition(originIndex, curDirection, gridSize);

        //    if (neighborIndex.x >= 0)
        //    {
        //        results.Add(neighborIndex);
        //    }
        //}
        return results;
    }

    //private int2 GetIndexAtRelativePosition(int2 originPos, int2 relativePos, int2 gridSize)
    //{

    //    int2 finalPos = originPos + relativePos;
    //    if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
    //    {
    //        return new int2(-1, -1);
    //    }
    //    else
    //    {
    //        return finalPos;
    //    }


    //}



}

public struct FlowfieldNeigborEntities : IBufferElementData
{
    public int2 Neighbent;

    public static implicit operator int2(FlowfieldNeigborEntities Neigbelem)
    {
        return Neigbelem.Neighbent;
    }

    public static implicit operator FlowfieldNeigborEntities(int2 e)
    {
        return new FlowfieldNeigborEntities { Neighbent = e };
    }

}

//public struct CalculateCellCostEventTag : IComponentData { }

