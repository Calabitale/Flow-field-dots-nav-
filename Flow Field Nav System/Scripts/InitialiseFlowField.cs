using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Burst;
using DotsFlowField;

//public class InitialiseFlowField : SystemBase
//{
    //    public int FieldGridsize;
    //    public float CellDiameter;
    //    public float CellRadii;

    //    private EntityCommandBufferSystem _ecbSystem;

    //    public Entity FlowfieldEntity;

    //    //TODO Should this be on an entity perhaps I don't know either way its no good or is good I don't know anything
    //    public float3 FieldOriginPosition;

    //    public EntityQuery TerrainObsQuery;

    //    public EntityQuery TerrainObsTranslation;

    //    public EntityArchetype FlowFieldDataType;

    //    public EntityQuery Flowdfieldquery;



    //    protected override void OnCreate()
    //    {
    //        Enabled = false;
    //        //TODO This minimum size should be just a bit larger than the smallest entity that uses the flowfield, So if an entity size is 1 then this should probably be 1.1 or something like that, so I need to automate this somehow
    //        CellDiameter = 1f;
    //        //TODO This should be big enough to cover the whole screen I will need a better way of calculating this, like defining the player and fitting this within that somehow but will just do this like this for now, also need to automate this
    //        FieldGridsize = 100;
    //        _ecbSystem = World.GetOrCreateSystem<EntityCommandBufferSystem>();
    //        //RequireForUpdate(type(NewFlowFieldData));
    //        //RequireSingletonForUpdate<NewFlowFieldData>();

    //        //TODO Need to figure out how I set this, where do I want to define the origin position for the flowfield.
    //        FieldOriginPosition = new float3(0, 1, 0);
    //        //EntityArchetype PeopleArchetypes = entymanger.CreateArchetype(
    //        //    typeof(MoneyEntityInfo),
    //        //    typeof(Translation),
    //        //    typeof(RenderMesh),
    //        //    typeof(LocalToWorld),
    //        //    typeof(Person),
    //        //    typeof(NonUniformScale),
    //        //    typeof(PersonEntityInfo)
    //        //    );

    //        FlowFieldDataType = EntityManager.CreateArchetype(
    //            typeof(FlowfieldMemberOf),
    //            typeof(CellData)

    //            ); 
    //    }

    //    protected override void OnStartRunning()
    //    {
    //        //TODO For some unknown fucking reason this does not work in Oncreate 
    //        RequireSingletonForUpdate<NewFlowFieldEvent>();
    //        //GetEntityQuery(typeof(RotationQuaternion), ComponentType.ReadOnly<RotationSpeed>());


    //        TerrainObsQuery = GetEntityQuery(typeof(Translation), ComponentType.ReadOnly<ObstacleTag>() );
    //        //TerrainObsTranslation = GetEntityQuery(typeof(Translation));

    //        //Flowdfieldquery = GetEntityQuery(typeof(FlowFieldData));

    //        Enabled = false;

    //    }

    //    protected override void OnUpdate()
    //    {
    //        Enabled = false;
    //        CellRadii = CellDiameter / 2;

    //        //var prefabsinit = GetSingleton<DotPrefabinator>();

    //        //var debugfieldvisual = prefabsinit.TestObjectPrefab;
    //        EntityCommandBuffer combuffer = _ecbSystem.CreateCommandBuffer();

    //        var tempterrainobstacles = TerrainObsQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

    //        //var tempterrainTranslations = TerrainObsTranslation.ToComponentDataArray<Translation>(Allocator.TempJob);
    //        //TODO Need to figure out how to get the Translation component from the obstacle entity's


    //        //TODO I need like an event entity that calls this system, so I can recreate the field whenever I want
    //        var tempfieldgridpos = FieldOriginPosition;

    //        //int fildGridsize = FieldGridsize;

    //        //TODO I shall use Flowfielddata as the boolean thing that denotes whether the field exists or not I don't need a boolean or anything that kind of misses the point of DOTS If 
    //        //if(!HasSingleton<FlowFieldData>())
    //        //{
    //            //FlowfieldEntity = EntityManager.CreateEntity();
    //            //EntityManager.AddComponent(FlowfieldEntity, typeof(FlowFieldData));
    //            //EntityManager.AddBuffer<CellDataBuffer>(FlowfieldEntity);
    //            //EntityManager.SetName(FlowfieldEntity, "FlowfieldEntity");

    //        //}
    //        //else //TODO Maybe I should if the flowfielddata already exists destroy the current one and then create a new one?
    //        //{
    //            //Debug.Log("It is going into here and defaulting the whole damn ");
    //            //return;
    //        //}

    //        //DynamicBuffer<CellDataBuffer> fieldBuffer = EntityManager.GetBuffer<CellDataBuffer>(FlowfieldEntity);
    //        //var tempbuffer = GetSingletonEntity<CellDataBuffer>();

    //        //DynamicBuffer<CellDataBuffer> fieldbuffer = EntityManager.GetBuffer<CellDataBuffer>(tempbuffer);

    //        //DynamicBuffer<MyBufferElement> buffer
    //        //Entities.ForEach(( ref DynamicBuffer<CellDataBuffer> fieldBuffer) =>
    //        //{
    //        //    //Debug.Log("So it went into the flowfield job but didn't do anything why though");

    //        //    for (int x = 0; x < fildGridsize; x++)
    //        //    {
    //        //        for (int y = 0; y < fildGridsize; y++)
    //        //        {
    //        //            float3 cellWorldPos = new float3(CellDiameter * -x + -CellRadii, 0, CellDiameter * y + CellRadii);//TODO I'm not sure if this is the correct way of placing it in the required direction correctly but it seems to work
    //        //            float3 othercellworldpos = cellWorldPos + tempfieldgridpos;

    //        //            //byte cellCost = CostFieldHelper.instance.EvaluateCost(cellWorldPos, cellRadius);//TODO This is the fucking problems you cant access an instance like this in a bursted entities job also need to figure out a better way of doing this in entities
    //        //            CellData newCellData = new CellData
    //        //            {
    //        //                worldPos = othercellworldpos,
    //        //                gridIndex = new int2(x, y),
    //        //                //cost = cellCost,
    //        //                bestCost = ushort.MaxValue,
    //        //                bestDirection = int2.zero
    //        //            };

    //        //            fieldBuffer.Add(newCellData);
    //        //        }

    //        //    }


    //        //}).WithoutBurst().Run();

    //        //TODO I'll do the fooking Flowfield data without a buffer I don't think I need a buffer that just makes it more complicade I can have as many flowfield entity's as I want and tehy are easier to iterate through

    //        //var tempcount = Flowdfieldquery.CalculateEntityCount();
    //        //Debug.Log("The number of flowfield entity's is " + tempcount);

    //        //var tempdest = EntityManager.CreateEntity(typeof(DestinationCellData));

    //        //TODO The number of entities seem to be correctly made so far if I end up with an error then I may encapsulate it in an if statement first checking if there is enough but otherwise will leave alone
    //        //TODO I need to be aware of a possible bug where I have double the number of entities instead 
    //        for (int x = 0; x < FieldGridsize; x++)
    //        {
    //            for (int y = 0; y < FieldGridsize; y++)
    //            {
    //                var Fluffyentity = EntityManager.CreateEntity(FlowFieldDataType);
    //                EntityManager.SetName(Fluffyentity, "FlowFieldDataEntity");

    //                float3 cellWorldPos = new float3(CellDiameter * x + CellRadii, 0, CellDiameter * y + CellRadii);//TODO I'm not sure if this is the correct way of placing it in the required direction correctly but it seems to work
    //                float3 othercellworldpos = cellWorldPos + tempfieldgridpos;

    //                CellData newCelldata = new CellData
    //                {
    //                    worldPos = othercellworldpos,
    //                    gridIndex = new int2(x, y),
    //                    //cost = cellcost
    //                    bestCost = ushort.MaxValue,
    //                    bestDirection = int2.zero

    //                };

    //                EntityManager.SetComponentData<CellData>(Fluffyentity, newCelldata);
    //                EntityManager.AddComponent<FlowfieldVertPointsBuff>(Fluffyentity);
    //                EntityManager.AddComponent<FlowfieldNeigborEntities>(Fluffyentity);

    //                var tempboff = EntityManager.GetBuffer<FlowfieldVertPointsBuff>(Fluffyentity);

    //                //TODO Need to just get the min and max points, does it matter which is the minimun and maximum as in does it matter in which order they are 

    //                //tempboff.Add(new float3(othercellworldpos.x - CellRadii, othercellworldpos.y - CellRadii, othercellworldpos.z + CellRadii));
    //                //tempboff.Add(new float3(othercellworldpos.x + CellRadii, othercellworldpos.y - CellRadii, othercellworldpos.z + CellRadii));
    //                //tempboff.Add(new float3(othercellworldpos.x + CellRadii, othercellworldpos.y - CellRadii, othercellworldpos.z - CellRadii));
    //                tempboff.Add(new float3(othercellworldpos.x - CellRadii, othercellworldpos.y - CellRadii, othercellworldpos.z - CellRadii));

    //                //tempboff.Add(new float3(othercellworldpos.x - CellRadii, othercellworldpos.y + CellRadii, othercellworldpos.z + CellRadii));
    //                tempboff.Add(new float3(othercellworldpos.x + CellRadii, othercellworldpos.y + CellRadii, othercellworldpos.z + CellRadii));
    //                //tempboff.Add(new float3(othercellworldpos.x + CellRadii, othercellworldpos.y + CellRadii, othercellworldpos.z - CellRadii));
    //                //tempboff.Add(new float3(othercellworldpos.x - CellRadii, othercellworldpos.y + CellRadii, othercellworldpos.z - CellRadii));

    //                //FlowFieldData tempflowfield = new FlowFieldData
    //                //{
    //                //    cellRadius = CellRadii,
    //                //    currenfield = CurrentFlowfield.Flowfield1,
    //                //    gridSize = FieldGridsize

    //                //};

    //                FlowfieldMemberOf tempflowfield = new FlowfieldMemberOf
    //                {

    //                    myflowfeld = CurrentFlowfield.Flowfield1
    //                };

    //                EntityManager.SetComponentData<FlowfieldMemberOf>(Fluffyentity, tempflowfield);

    //            }

    //        }



    //        var tempdude = EntityManager.CreateEntity(typeof(FlowFieldData));

    //        FlowFieldData realflowfield = new FlowFieldData
    //        {
    //            cellRadius = CellRadii,
    //            //currenfield = CurrentFlowfield.Flowfield1,
    //            gridSize = FieldGridsize,
    //            Destination = new float3(0, 0, 0)


    //        };

    //        EntityManager.SetComponentData<FlowFieldData>(tempdude, realflowfield);



    //        //this.CompleteDependency();

    //        //I need to get all the entities that are not in the way of the thing that is not worknig and I don't not and cannot know w hy I am doing this and everything is shit and son I am derp
    //        //TODO NEed to figure out how to get the entities that are surrounding this entity


    //        //var tempNeigbourindices = new GetNeighborIndices();

    //        //TODO Need to figure out why this is not faster multithreaded its actually faster singlethreaded for some reason
    //        //TODO Try turing this into a separate system like I was going to do in the first place at least it will work to check because the mulithreaded may be hangups from the rest of the System
    //        //Entities.WithName("FindNeigbourCell").ForEach((DynamicBuffer<FlowfieldNeigborEntities> flowbuff, in FlowFieldData fluffydata, in CellData tempcelldata) =>
    //        //{
    //        //    NativeList<int2> tempint2list = new NativeList<int2>(Allocator.Temp);

    //        //    tempNeigbourindices.Execute(tempcelldata.gridIndex, fluffydata.gridSize, ref tempint2list);

    //        //    for (int i = 0; i < tempint2list.Length; i++)
    //        //    {
    //        //        flowbuff.Add(tempint2list[i]);
    //        //    }

    //        //    tempint2list.Dispose();



    //        //}).ScheduleParallel();

    //        //this.CompleteDependency();
    //        //HasSingleton<CellData>();

    //        if (!HasSingleton<DestinationCellData>())//TODO Not sure if this a good place to create it but will leave this here for now may move it somewhere else later
    //        {
    //            EntityManager.CreateEntity(typeof(DestinationCellData));
    //        }
    //        #region Old rough code
    //                //for (int i = 0; i < dudents.Length; i++)
    //        //{
    //        //    var toppy = GetComponent<Translation>(dudents[i]);
    //        //    var currentdude = GetComponent<Translation>(inty);

    //        //    var currdistance = math.distance(currentdude.Value, toppy.Value);
    //        //    //Debug.Log("The distance between them is " + currdistance);
    //        //    if (currdistance < 1)
    //        //    {
    //        //        //Debug.Log("The distance is too damn close");
    //        //        isitthough.Value = true;

    //        //    }

    //        //}

    //        //TODO THis is where I check the distance the cell and all the obstacle entities to see how close they are, I could also add a type to the obstacle so I have different speeds through them but I will leave that for later I don't see a need for that currently, I don't need terrain that slows things down
    //        //Entities.ForEach((ref CellData cooldata, ref DynamicBuffer<FlowfieldVertPointsBuff> flowverts) =>
    //        //{
    //        //    for (int i = 0; i < tempterrainobstacles.Length; i++)
    //        //    {
    //        //        var wbuu = tempterrainobstacles[i].Value;
    //        //        var currdist = math.distance(cooldata.worldPos, wbuu);

    //        //        if(currdist < 1)
    //        //        {
    //        //            //Debug.Log("The celldata is close to an obstacle");
    //        //            //var tempAABB = new MinMaxAABB();


    //        //            //cooldata.cost = byte.MaxValue;
    //        //        }


    //        //        //cooldata.worldPos
    //        //    }


    //        //}).WithoutBurst().Run();

    //        //function intersect(a, b)
    //        //{
    //        //    return (a.minX <= b.maxX && a.maxX >= b.minX) &&
    //        //           (a.minY <= b.maxY && a.maxY >= b.minY) &&
    //        //           (a.minZ <= b.maxZ && a.maxZ >= b.minZ);
    //        //}


    //        //Entities.ForEach((ref DynamicBuffer<CellDataBuffer> fieldBuffer) =>
    //        //{
    //        //    for(int i = 0; i < fieldBuffer.Length; i++)
    //        //    {
    //        //        var tempval = fieldBuffer[i].celldata;
    //        //        var tempcurrentpos = tempval.worldPos;
    //        //        //var dudebaker = tempterrainTranslations[


    //        //        tempval.cost = (byte)i;
    //        //       fieldBuffer[i] = tempval;
    //        //    }


    //        //}).WithoutBurst().Run();
    //        #endregion

    //        if (HasSingleton<NewFlowFieldEvent>())
    //        {

    //            var tempent = GetSingletonEntity<NewFlowFieldEvent>();

    //            EntityManager.DestroyEntity(tempent);

    //        }

    //        //if (!HasSingleton<FindNeighborCellEvent>())
    //        //{

    //        //    EntityManager.CreateEntity(typeof(FindNeighborCellEvent));
    //        //}

    //        //TODO I may need to make sure with an if check that I create only one of these entity's I have a feeling it might work without, but there might be cases where multiple are created and so should probably check if one already exists before creating
    //        //Actually its probably best to always check if one already exists before creating another one, if I want to guarantee only one exists because other systems could create some or something
    //        if (!HasSingleton<CalculateCellCostEventTag>())
    //        {
    //            //Debug.Log("Is it even going into the newflowfieldevent thingy");
    //            EntityManager.CreateEntity(typeof(CalculateCellCostEventTag));
    //        }

    //        //if(!HasSingleton<CalcintegrationFieldEvent>())
    //        //{
    //        //    EntityManager.CreateEntity(typeof(CalcintegrationFieldEvent));
    //        //}


    //        tempterrainobstacles.Dispose();

    //        //_ecbSystem.AddJobHandleForProducer(this.Dependency);

    //        //combuffer.Playback(EntityManager);


    //    }
    //    /// <summary>
    //    /// Determine if box A intersects box B.
    //    /// </summary>
    //    //public bool Intersects(ref BoundingBox box)
    //    //{
    //    //    Vector3 boxCenter = (box.Max + box.Min) * 0.5f;
    //    //    Vector3 boxHalfExtent = (box.Max - box.Min) * 0.5f;

    //    //    Matrix mb = Matrix.CreateFromQuaternion(Orientation);
    //    //    mb.Translation = Center - boxCenter;

    //    //    return ContainsRelativeBox(ref boxHalfExtent, ref HalfExtent, ref mb) != ContainmentType.Disjoint;
    //    //}




    //}



    //[UpdateInGroup(typeof(SimulationSystemGroup))]
    //public class TestFlowfieldSystem : SystemBase
    //{
    //    public int Testint;
    //    protected override void OnCreate()
    //    {
    //        base.OnCreate();
    //        Enabled = false;
    //    }

    //    protected override void OnStartRunning()
    //    {


    //    }

    //    protected override void OnUpdate()
    //    {
    //        Enabled = false;
    //        Debug.Log("The thing has gone in here");
    //        //int tempgrid = 20;
    //        //var Timelapsed = Time.ElapsedTime;        

    //        //float repeatRate = 5f;

    //        //float timer = 0;

    //        //if (timer < 0)
    //        //{
    //        //    // Do something
    //        //    timer = repeatRate;
    //        //}

    //        //timer -= Time.DeltaTime;

    //      if (!HasSingleton<NewFlowFieldEvent>())
    //        {
    //            var tempent = EntityManager.CreateEntity();
    //            EntityManager.AddComponent<NewFlowFieldEvent>(tempent);

    //        }


    //        //var GetNeighbordudes = new GetNeighborIndices();

    //        //NativeList<int2> tempnatlist = new NativeList<int2>(Allocator.Temp);
    //        //int2 orginiind = new int2(1, 1);


    //        //GetNeighbordudes.Execute(orginiind, tempgrid, ref tempnatlist);
    //        ////Enabled = false;
    //        //foreach(int2 somink in tempnatlist)
    //        //{
    //        //    Debug.Log("the current values are " + somink);

    //        //}


    //        //tempnatlist.Dispose();

    //        //if(HasSingleton<TimeTickEvent>())
    //        //{
    //        //var tempnt = EntityManager.CreateEntity();
    //        //EntityManager.AddComponent<NewFlowFieldEvent>(tempnt);
    //        //Debug.Log("The new flowfield has been created");
    //        //}


    //    }

    //}


    //public struct FindNeighborCellEvent : IComponentData { }

    //public struct DestinationCellData : IComponentData
    //{
    //    public int2 int2Value;
    //}

    ////TODO DO I need this as a bool or should I just create an event entity I do kind of need an event entity to tell whether the flowfield exists though or do I the data that exists within
    //public struct NewFlowFieldEvent : IComponentData
    //{ }


    ////TODO I may need to revise this also do I even need this as entity data its only going to be a singleton and will anything need it outside the job, 
    //public struct FlowfieldVertPointsBuff : IBufferElementData
    //{
    //    public float3 Float3points;

    //    public static implicit operator float3(FlowfieldVertPointsBuff flowvertelem)
    //    {
    //        return flowvertelem.Float3points;
    //    }

    //    public static implicit operator FlowfieldVertPointsBuff(float3 e)
    //    {
    //        return new FlowfieldVertPointsBuff { Float3points = e };
    //    }


    //}

    ////public struct FlowFieldData : IComponentData//I dont currently need this so wont use this
    ////{
    ////    public int gridSize;
    ////    public float cellRadius;
    ////    public float3 Destination;  // I dont think I need this on here will have it separate as DestinationCellData
    ////    public int2 DestinationCell;
    ////    public CurrentFlowfield currenfield; //TODO Need to figure out if this should be a separate value on the same entity or just combined into this one
    ////}

    //public struct FlowfieldMemberOf : IComponentData
    //{
    //    public CurrentFlowfield myflowfeld;

    //}

    ////TODO No this enum won't really work for or maybe it could I'd have to do an early out or something but if I have it this way then I will have to iterate through all the cells of every flow field which kind of defeats the purpose of it
    ////TODO Although how the hell do I do it so I can select the flowfield tags won't really work because that is to specific, I can't agnosticlly select them probably is the best
    //public enum CurrentFlowfield //TODO This is enough for five flowfield can increase to more as needed or maybe I should do it differently I don't know I want to keep the flowfield number limited don't want to go over max 20 say
    //{
    //    Flowfield1,
    //    Flowfield2,
    //    Flowfield3,
    //    Flowfield4,
    //    Flowfield5,
    //    Flowfield6,
    //    Flowfield7,
    //    Flowfield8,
    //    Flowfield9,
    //    Flowfield10

    //}

    //TODO This is the structure the other guy has I may want to refine this a bit put members off into their own componentdatas to make them more efficient perhaps I'll see how its used first how often the members are used
    //public struct CellData : IComponentData
    //{
    //    public float3 worldPos;
    //    public int2 gridIndex;
    //    public byte cost;
    //    public ushort bestCost;
    //    public int2 bestDirection;
    //}

    //TODO I may not need it this big but will keep it like this for now
    //[InternalBufferCapacity(400)]
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

//}

//[InternalBufferCapacity(250)]
//public struct EntityBufferElement : IBufferElementData
//{
//    public Entity entity;

//    public static implicit operator Entity(EntityBufferElement entityBufferElement)
//    {
//        return entityBufferElement.entity;
//    }

//    public static implicit operator EntityBufferElement(Entity e)
//    {
//        return new EntityBufferElement { entity = e };
//    }
//}