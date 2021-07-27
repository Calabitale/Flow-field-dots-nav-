using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using DotsFlowField;

//Notes this can go into a seperate system as this can be calculated on its own occasionally I think, but it always has to follow calculatecellcostsystem but I surely don't always need to calculate the cell cost if the terrain objects arent moving then it does not need to be calculated
public class CalcIntegrationFieldSys : SystemBase
{
    public EntityQuery NeighboursQuery;
    public EntityQuery CellDataQuery;
    protected override void OnCreate()
    {
        base.OnCreate();
        //NeighboursQuery = GetEntityQuery(ComponentType.ReadOnly<FlowfieldNeigborEntities>());
        NeighboursQuery = GetEntityQuery(ComponentType.ReadOnly<FlowfieldNeigborEntities>());
        CellDataQuery = GetEntityQuery(typeof(CellData));
        RequireSingletonForUpdate<CalcintegrationFieldEvent>();
        Enabled = false;
    }

    protected override void OnStartRunning()
    {
        
        RequireSingletonForUpdate<CalcintegrationFieldEvent>();
    }

    protected override void OnUpdate()
    {
        Debug.Log("This is going into the old Integration field for some reason");
        if (HasSingleton<CalcintegrationFieldEvent>())
        {

            var tempevent = GetSingletonEntity<CalcintegrationFieldEvent>();
            EntityManager.DestroyEntity(tempevent);
            //Debug.Log("It's not destroying the entity for some reason");
        }
        //commandBuffer.RemoveComponent<CalculateFlowFieldTag>(entity);

        //DynamicBuffer<Entity> entityBuffer = buffer.Reinterpret<Entity>();
        //NativeArray<CellData> cellDataContainer = new NativeArray<CellData>(entityBuffer.Length, Allocator.TempJob);

        //int2 gridSize = flowFieldData.gridSize;

        //for (int i = 0; i < entityBuffer.Length; i++)
        //{
        //    cellDataContainer[i] = GetComponent<CellData>(entityBuffer[i]);
        //}

        //int flatDestinationIndex = FlowFieldHelper.ToFlatIndex(destinationCellData.destinationIndex, gridSize.y);
        //CellData destinationCell = cellDataContainer[-];
        //destinationCell.cost = 0;
        //destinationCell.bestCost = 0;
        //cellDataContainer[flatDestinationIndex] = destinationCell;

        //NativeQueue<int2> indicesToCheck = new NativeQueue<int2>(Allocator.TempJob);
        //NativeList<int2> neighborIndices = new NativeList<int2>(Allocator.TempJob);

        //indicesToCheck.Enqueue(destinationCellData.destinationIndex);

        //// Integration Field
        //while (indicesToCheck.Count > 0)
        //{
        //    int2 cellIndex = indicesToCheck.Dequeue();
        //    int cellFlatIndex = FlowFieldHelper.ToFlatIndex(cellIndex, gridSize.y);
        //    CellData curCellData = cellDataContainer[cellFlatIndex];
        //    neighborIndices.Clear();
        //    FlowFieldHelper.GetNeighborIndices(cellIndex, GridDirection.CardinalDirections, gridSize, ref neighborIndices);
        //    foreach (int2 neighborIndex in neighborIndices)
        //    {
        //        int flatNeighborIndex = FlowFieldHelper.ToFlatIndex(neighborIndex, gridSize.y);
        //        CellData neighborCellData = cellDataContainer[flatNeighborIndex];
        //        if (neighborCellData.cost == byte.MaxValue)
        //        {
        //            continue;
        //        }

        //        if (neighborCellData.cost + curCellData.bestCost < neighborCellData.bestCost)
        //        {
        //            neighborCellData.bestCost = (ushort)(neighborCellData.cost + curCellData.bestCost);
        //            cellDataContainer[flatNeighborIndex] = neighborCellData;
        //            indicesToCheck.Enqueue(neighborIndex);
        //        }
        //    }

        Entities.WithName("ResetCostandBestCost").ForEach((ref CellData celldudes) =>
        {
            //celldudes.
            if (celldudes.cost != 255)
            {
                celldudes.cost = 1;
            }
            
            //Debug.Log("The current cell cost is" + celldudes.cost);

            //celldudes.cost = 1;

            celldudes.bestCost = byte.MaxValue;
            //if(celldudes.cost == 0)
            //{
            //    //Debug.Log($"The celldudes positios is {celldudes.gridIndex}");
            //}
            //Debug.Log($"The celldutes best cost are: {celldudes.bestCost}");

        }).ScheduleParallel();

        //this.CompleteDependency();//It seems to need this dependencies are not automatic 
        //var tempfloof = GetSingleton<FlowFieldData>();
        var celldatacount = CellDataQuery.CalculateEntityCount();
        NativeArray<CellData> TempCelldata = new NativeArray<CellData>(celldatacount, Allocator.TempJob);

        Entities.WithName("CollectAllCellData").ForEach((int entityInQueryIndex, ref CellData celldudes) =>
        {

            TempCelldata[entityInQueryIndex] = celldudes;

        }).ScheduleParallel();
 
        //var TempCelldata = CellDataQuery.ToComponentDataArray<CellData>(Allocator.TempJob);

        //var TempCellentity = CellDataQuery.ToEntityArray(Allocator.Temp);
        var tempNeigbourindices = new GetNeighborIndices();

        var TempFlatIndex = new ToFlatIndex();
        //FixedList64<int2> tempint2list = new FixedList64<int2>();
        //tempNeigbourindices.Execute(tempcelldata.gridIndex, griddsiz, ref tempint2list);
        //Debug.Log("How many times is it acctually going in here");
        NativeQueue<int2> indicesToCheck = new NativeQueue<int2>(Allocator.TempJob);


        Entities.WithName("CalculateIntegrationField").WithDisposeOnCompletion(indicesToCheck).ForEach((in FlowFieldData flooydata) =>
        {
           


            int flatdestindex = TempFlatIndex.Execute(flooydata.DestinationCell, flooydata.gridSize.y);

            var tempcelldestinate = TempCelldata[flatdestindex];

            if (tempcelldestinate.cost == 255) return;//THis just exits the job if the destination is accidently set to a obstacle

            tempcelldestinate.cost = 0;
            tempcelldestinate.bestCost = 0;

            TempCelldata[flatdestindex] = tempcelldestinate;

            //TODO Just get rid of the fooking Neigbourentities just calculate them as and when I need them

            //Debug.Log("The index value is" + tempdebug);

            //NativeQueue<int2> indicesToCheck = new NativeQueue<int2>(Allocator.TempJob);
            indicesToCheck.Enqueue(flooydata.DestinationCell);
            //The calculates the ascending values according to distance
            while (indicesToCheck.Count > 0)
            {
                int2 cellindex = indicesToCheck.Dequeue();
                var cellfatindex = TempFlatIndex.Execute(cellindex, flooydata.gridSize.y);
                var curCelldata = TempCelldata[cellfatindex];
                FixedList128<int2> tempint2list = new FixedList128<int2>();
                tempNeigbourindices.Execute(cellindex, flooydata.gridSize, true, ref tempint2list);

                foreach (int2 neeborindex in tempint2list)
                {
                    //Debug.Log("The neeborindex is " + neeborindex);
                    int flatneigbourindex = TempFlatIndex.Execute(neeborindex, flooydata.gridSize.y);
                    //Debug.Log("The flatneighbour index is" + flatneigbourindex);

                    var neigbourcelldata = TempCelldata[flatneigbourindex];

                    if (neigbourcelldata.cost == byte.MaxValue)
                    {
                        //Debug.Log("Did it work and find the max value");
                        continue;
                    }
                    //Debug.Log("neigbhour cell cost is " + neigbourcelldata.cost);
                    //Debug.Log("currcell cost is  " + curCelldata.bestCost);
                    //Debug.Log("neigbhour best cost is " + neigbourcelldata.bestCost);

                    if (neigbourcelldata.cost + curCelldata.bestCost < neigbourcelldata.bestCost)
                    {

                        neigbourcelldata.bestCost = (ushort)(neigbourcelldata.cost + curCelldata.bestCost);
                        TempCelldata[flatneigbourindex] = neigbourcelldata;
                        indicesToCheck.Enqueue(neeborindex);
                        //Debug.Log($"The neigbourcelldutes cost are: {celldudes.cost}");
                    }

                }


            }
            //This calculates the Direction
            for (int i = 0; i < TempCelldata.Length; i++)
            {
                CellData curCullData = TempCelldata[i];
                //var cellfatindex = TempFlatIndex.Execute(curCullData.gridIndex, flooydata.gridSize);
                //var curCelldata = TempCelldata[cellfatindex];
                ushort currbestCost = curCullData.bestCost;
                int2 currbestDirection = int2.zero;
                FixedList128<int2> tempint2list = new FixedList128<int2>();
                tempNeigbourindices.Execute(curCullData.gridIndex, flooydata.gridSize, false, ref tempint2list);
                foreach(int2 neebor in tempint2list)
                {
                    var flatusindicus = TempFlatIndex.Execute(neebor, flooydata.gridSize.y);
                    CellData neeborcellindex = TempCelldata[flatusindicus];
                    if(neeborcellindex.bestCost < currbestCost)
                    {
                        currbestCost = neeborcellindex.bestCost;
                        currbestDirection = neeborcellindex.gridIndex - curCullData.gridIndex;
                        //currbestDirection = curCullData.gridIndex - neeborcellindex.gridIndex;
                        //Debug.Log($"The currbest cost is: {neeborcellindex.gridIndex} {curCullData.gridIndex}");

                    }

                }
                //Debug.Log($"The currbest cost is: {currbestDirection}");
                //curCullData.bestDirection = currbestDirection;
                TempCelldata[i] = curCullData;

            }


                //indicesToCheck.Dispose();
        }).Schedule();//ToDo I should figure out how to make this parrallel when if I have multiple flow fields 

        //indicesToCheck.Dispose();

        //TODO Here is my copyback job
        Entities.WithName("CopyBackTempCelldata").WithDisposeOnCompletion(TempCelldata).ForEach((int entityInQueryIndex, ref CellData celldudes) =>
        {
            celldudes = TempCelldata[entityInQueryIndex];
        }).ScheduleParallel();

        //var othertempcelldata = CellDataQuery.ToComponentDataArrayAsync

        //TODO I'm not sure if this is the best way to do this, I need these all to be together, because I don't think they need to be calculated on their own, but
        //This job needs the result of the previous job, and so I will have to re get the data from, this is all very confusing and probably sub optimal
        //Entities.WithName("CalculateCellDirection").ForEach((in FlowFieldData flooydata) =>
        //{


        //}).WithoutBurst().Run();


            //if (HasSingleton<CalcintegrationFieldEvent>())
            //{

            //    var tempevent = GetSingletonEntity<CalcintegrationFieldEvent>();
            //    EntityManager.DestroyEntity(tempevent);
            //    //Debug.Log("It's not destroying the entity for some reason");
            //}
            //    // Flow Field
            //    for (int i = 0; i < cellDataContainer.Length; i++)
            //    {
            //        CellData curCullData = cellDataContainer[i];
            //        neighborIndices.Clear();
            //        FlowFieldHelper.GetNeighborIndices(curCullData.gridIndex, GridDirection.AllDirections, gridSize, ref neighborIndices);
            //        ushort bestCost = curCullData.bestCost;
            //        int2 bestDirection = int2.zero;
            //        foreach (int2 neighborIndex in neighborIndices)
            //        {
            //            int flatNeighborIndex = FlowFieldHelper.ToFlatIndex(neighborIndex, gridSize.y);
            //            CellData neighborCellData = cellDataContainer[flatNeighborIndex];
            //            if (neighborCellData.bestCost < bestCost)
            //            {
            //                bestCost = neighborCellData.bestCost;
            //                bestDirection = neighborCellData.gridIndex - curCullData.gridIndex;
            //            }
            //        }
            //        curCullData.bestDirection = bestDirection;
            //        cellDataContainer[i] = curCullData;
            //    }

            //    GridDebug.instance.ClearList();

            //    for (int i = 0; i < entityBuffer.Length; i++)
            //    {
            //        commandBuffer.SetComponent(entityBuffer[i], cellDataContainer[i]);
            //        commandBuffer.AddComponent<AddToDebugTag>(entityBuffer[i]);
            //    }

            //    neighborIndices.Dispose();
            //    cellDataContainer.Dispose();
            //    indicesToCheck.Dispose();
            //    commandBuffer.AddComponent<CompleteFlowFieldTag>(entity);
            //}).WithoutBurst().Run();
            //this.CompleteDependency();


            //CellDataQuery.CopyFromComponentDataArray(TempCelldata);

            //indicesToCheck.Dispose();
            //TempCelldata.Dispose();
            //TempCellentity.Dispose();

        }



}

public struct ToFlatIndex
{
    public int Execute(int2 index2D, int height)
    {



        return height * index2D.x + index2D.y;
    }
}

//public struct CalcintegrationFieldEvent : IComponentData { }
