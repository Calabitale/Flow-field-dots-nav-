using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

namespace DotsFlowField
{
    public class CalculateCellCostSystem : SystemBase
    {
        
        public NativeArray<ObstacleCollisionVerts> tempbuff;

        public EntityQuery TerrainObsQuery;

        protected override void OnCreate()
        {
            //Enabled = false;
        }


        protected override void OnStartRunning()
        {
            RequireSingletonForUpdate<CalculateCellCostEventTag>();           

            EntityQueryDesc queryDescription = new EntityQueryDesc();
            queryDescription.All = new[] { ComponentType.ReadOnly<ObstacleCollisionVerts>() };
            TerrainObsQuery = GetEntityQuery(queryDescription);

        }

        protected override void OnUpdate()
        {

            if (HasSingleton<CalculateCellCostEventTag>())
            {               
                var tempent = GetSingletonEntity<CalculateCellCostEventTag>();
                EntityManager.DestroyEntity(tempent);
            }

            
       
            var colliderentsnumb = TerrainObsQuery.CalculateEntityCount();

            var comparisonArray = new NativeArray<float3x2>(colliderentsnumb, Allocator.TempJob);

            Entities.ForEach((int entityInQueryIndex, DynamicBuffer<ObstacleCollisionVerts> obbybuffer) =>
            {
                float3x2 tempfloat = new float3x2 { c0 = obbybuffer[0].float3verts, c1 = obbybuffer[1].float3verts };

                comparisonArray[entityInQueryIndex] = tempfloat;

            }).ScheduleParallel();//Not sure if this should single main thread or multithreaded it seems to currently be a bit slower wit multithread but when I add more collision obstacles it may work better with multithread will leave as multithread for now though

            //if(comparisonArray.Length == 0)

            Entities.WithReadOnly(comparisonArray).WithDisposeOnCompletion(comparisonArray).ForEach((ref CellData cooldata, in DynamicBuffer<FlowfieldVertPointsBuff> flowverts) =>
            {
                //if (comparisonArray.Length == 0) return;
                cooldata.cost = 1;
                for (int j = 0; j < comparisonArray.Length; j++)
                {
                    var tempobsbuff = comparisonArray[j];
                    //NOTE With this I have to make sure the min and max match exactly because for some reason if they are off or arent in the right order then this does not work at all
                    var tempbool = (flowverts[0].Float3points.x <= tempobsbuff.c0.x && flowverts[1].Float3points.x >= tempobsbuff.c1.x)
                                && (flowverts[0].Float3points.y <= tempobsbuff.c0.y && flowverts[1].Float3points.y >= tempobsbuff.c1.y)
                                && (flowverts[0].Float3points.z <= tempobsbuff.c0.z && flowverts[1].Float3points.z >= tempobsbuff.c1.z);

                    if (tempbool)
                    {
                        cooldata.cost = byte.MaxValue;

                    }
                }
            }).ScheduleParallel();

        }
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
    //        RequireSingletonForUpdate<FindNeighborCellEvent>();
    //        //Flowfieldquery = GetEntityQuery(ComponentType.ReadOnly<FlowFieldData>());
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
        public static readonly int2[] cardinaldirections = new int2[4] { new int2(0, 1), new int2(0, -1), new int2(1, 0), new int2(-1, 0) };

        public static readonly int2[] alldirections;// = new int2[9] {new int2()}

        //public int2[] currentdirections;
        //public NativeList<int2> results;

        static GetNeighborIndices()
        {
            alldirections = new int2[]
            {
            new int2(0, 0), //The current Cell
            new int2(0, 1), //Supposedly North 
            new int2(0, -1), //South?
            new int2(1, 0), //East 
            new int2(-1, 0), //West
            new int2(1, 1),  //NorthEast
            new int2(-1, 1),  //NorthWest
            new int2(1, -1),  // SouthEast
            new int2 (-1, -1)  //SouthWest

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

}