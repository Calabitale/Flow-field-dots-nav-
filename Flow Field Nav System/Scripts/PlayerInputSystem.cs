using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using Unity.Collections;
using Unity.Transforms;
using DotsFlowField;
using System;

public struct PlayerInput : IComponentData
{
    public float2 moveInput;
    public bool fireinput;
    public bool StartFloofeld;
    public bool Addlayer;
    public bool Calcotherlayer;

}

[UpdateAfter(typeof(PlayerInputStuffSystem))]
public class PlayerTestInput : SystemBase
{
    private Camera _mainCamera;
    public FlowFieldData tempdestination;
    public Entity tempdestentity;

    public EntityQuery DestinationBoofQuery;
    public DotPrefabinator tempmoveobject;

    protected override void OnCreate()
    {
        DestinationBoofQuery = GetEntityQuery(typeof(CellDestinationsBuffer));
        //Enabled = false;

    }

    protected override void OnStartRunning()
    {
        _mainCamera = Camera.main;
        tempmoveobject = HasSingleton<DotPrefabinator>() ? GetSingleton<DotPrefabinator>() : default;
    }

    protected override void OnUpdate()
    {
        var tempinput = GetSingleton<PlayerInput>();
        //var tempmoveobject = GetSingleton<DotPrefabinator>();

        //var tempval = SafeArrayWrites();

        //Debug.Log("The value is" + tempval);

        var currentindexworldpos = new GetCellIndexFromWorldPos();
        //var tempdestination = HasSingleton<DestinationCellData>() ? GetSingleton<DestinationCellData>() : new DestinationCellData().int2Value(new int2(0, 0));
        if (HasSingleton<FlowFieldData>())//DO I actually need this has singleton check
        {
            //var mapEntity = GetSingletonEntity<Map>();
            //var mapStateData = new MapStateJobContext(this, mapEntity, false);

            //var playerEntity = HasSingleton<Player>() ?
            //GetSingletonEntity<Player>() : Entity.Null;
            tempdestination = GetSingleton<FlowFieldData>();
            //tempdestentity = GetSingletonEntity<FlowFieldData>();
        }

        int numoftimes = 0;

        DynamicBuffer<CellDestinationsBuffer> tempDestinatebuffo;

        


    

        
        //Debug.Log("The value in starfield is " + tempinput.StartFloofeld);
        //var dumbinput = tempinput.fireinput;
        //Debug.Log("The current input is" + tempinput.fireinput);
        if (tempinput.fireinput)//TODO One problem it seems as if I have 3 4 or 5 inputs for one button press
        {
            var randmbuff = UnityEngine.Random.Range(0, 4);
            if (HasSingleton<FlowFieldData>())
            {
                //Debug.Log("So does it never go into here");
                //var TempbufferDestinationsentity = DestinationBoofQuery.ToEntityArray(Allocator.TempJob);
                var TempbufferDestinationsentity = GetSingletonEntity<CellDestinationsBuffer>();

                tempDestinatebuffo = GetBuffer<CellDestinationsBuffer>(TempbufferDestinationsentity);

                //var TempBufferDestinate = GetBufferFromEntity<CellDestinationsBuffer>(true);        
                SetSingleton<FlowFieldData>(tempdestination);
                //TempbufferDestinationsentity.Dispose();
                SetSingleton<CellBDLayerToCalc>(new CellBDLayerToCalc { intVal = randmbuff });

            }
            else
            {
                return;
            }

            //TODO Screw it I'm not going to bother with complicated point and click shit I'm not making a game where you select where entities move at least not yet I'll just have a random cell chosen on click 
            //int xnumb = UnityEngine.Random.Range(-20, 20);
            //int ynumb = UnityEngine.Random.Range(-115, -120);

            var tempositkyan = tempdestination.gridSize.x - 1;
            if (randmbuff == 0)
            {
                int2 treenumb = new int2(66, 129);//TODO This is too damn high for some reason
                var tempbuffeee = tempDestinatebuffo[0];//TODO THis just sets the distination I did not think it would work 
                tempbuffeee.Destination = treenumb;
                tempDestinatebuffo[0] = tempbuffeee;

            }
            else if (randmbuff == 1)
            {
                int2 treentwo = new int2(2, 2);

                var tempbuffeee = tempDestinatebuffo[1];
                tempbuffeee.Destination = treentwo;
                tempDestinatebuffo[1] = tempbuffeee;


            }
            else if (randmbuff == 2)
            {
                int2 treentwo = new int2(97, 2);
                var tempbuffeee = tempDestinatebuffo[2];
                tempbuffeee.Destination = treentwo;
                tempDestinatebuffo[2] = tempbuffeee;

            }
            else if (randmbuff == 3)
            {
                int2 treentwo = new int2(2, 97);
                var tempbuffeee = tempDestinatebuffo[3];
                tempbuffeee.Destination = treentwo;
                tempDestinatebuffo[3] = tempbuffeee;

            }

            //Debug.Log("The treenumb is " + treenumb);
            //int2 secondtreenumb = new int2(4, 34);
            //Debug.Log("The random numbers are" + treenumb);

            //tempdestination.DestinationCell = treenumb;

            //var tempintpos = currentindexworldpos.Execute

            //var tempflowfuld = tempdestination;
            //tempflowfuld.DestinationCell = treenumb;
            //tempDestinatebuffo.Add(treenumb);

            //tempdestination = tempflowfuld;
            //fluffy.DestinationCell
            //tempdest.int2Value = treenumb;


            //var tempbufftwoo = tempDestinatebuffo[1];
            //tempbufftwoo.Destination = secondtreenumb;
            //tempDestinatebuffo[1] = tempbufftwoo;

            if (!HasSingleton<CalcintegrationFieldEvent>())
            {
                EntityManager.CreateEntity(typeof(CalcintegrationFieldEvent));
            }
            //Creates a dude to move around this I don't know I can't do this shit I'm tired it's to much work and just years of effort for 
            var tempobject = tempmoveobject.TestObjectPrefab;
            int totalentys = 100;
            NativeArray<Entity> dudees = new NativeArray<Entity>(totalentys, Allocator.Temp);
            EntityManager.Instantiate(tempobject, dudees);
            EntityManager.AddComponent<TestMoveojbectTag>(dudees);
            EntityManager.AddComponent<CellBDLayer>(dudees);
            for (int i = 0; i < dudees.Length; i++)
            {
                int xrandnumpos = UnityEngine.Random.Range(90, 120);
                int zrandnumpos = UnityEngine.Random.Range(-20, -40);

                //var tudue = EntityManager.Instantiate(tempobject);
                //EntityManager.AddComponent<TestMoveojbectTag>(dudees);

                float3 thecreatepos = new float3(xrandnumpos, 1, zrandnumpos);

                
                EntityManager.SetComponentData<Translation>(dudees[i], new Translation { Value = thecreatepos });
                EntityManager.SetComponentData<CellBDLayer>(dudees[i], new CellBDLayer { intVal = randmbuff });

                //var instances = new NativeArray<Entity>(500, Allocator.Temp);
                //EntityManager.Instantiate(entity, instances);

            }

            dudees.Dispose();//Note Have to have dispose otherwise it won't create anymore each time
            #region junk kept just in case
            //var tumbldown = currentindexworldpos.Execute(thecreatepos, tempdestination.gridSize, tempdestination.cellRadius * 2);
            //Debug.Log("The current index world pos is " + tumbldown);
            //EntityManager.SetComponentData<Translation>(tudue, 
            //EntityManager.SetComponentData<FlowFieldData>(tempdestentity, tempdestination);
            //EntityManager.SetComponentData(dodo, new Translation { Value = new float3(tempvals.Value.x, 5, tempvals.Value.z) });

            //SetComponent<FlowFieldData>(tempdestination, tempflowfuld);
            //EntityManager.SetComponentData<DestinationCellData>()
            //Vector3 mouseposinword = new Vector3(tempinput.moveInput.x, tempinput.moveInput.y, 10);
            //Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(mouseposinword);

            //Debug.Log("The worldmousepos is " + worldMousePos);
            //var tempgridpos = GetCellIndexFromWorldPos(worldMousePos, 20, 1);
            //Debug.Log("The tempgridpos" + tempgridpos);
            //var mapEntity = GetSingletonEntity<Map>();
            //var mapStateData = new MapStateJobContext(this, mapEntity, false);

            //var playerEntity = HasSingleton<Player>() ?
            //GetSingletonEntity<Player>() : Entity.Null;
            //float2 currenthitpos = tempinput.moveInput;
            //Debug.Log("The fireinput is true");
            //if(currenthitpos celldata.cost => 255)
            //return
            //else do something
            //tempinput.fireinput = false;
            //if(HasSingleton<CalcintegrationFieldEvent>())
            //{
            //    var tamedent = GetSingletonEntity<CalcintegrationFieldEvent>();
            //    EntityManager.DestroyEntity(tamedent);
            //}

            #endregion

        }



        if (tempinput.StartFloofeld)
        {
            //Debug.Log("It is going into the startfield thingy");
            if (HasSingleton<NewFlowFieldEvent>() || HasSingleton<FlowFieldData>()) return;
            //{
            //Debug.Log("Is it making multiple events");
            EntityManager.CreateEntity(typeof(NewFlowFieldEvent));
            //}
        }

        if(tempinput.Addlayer)
        {
            //var randmbuff = UnityEngine.Random.Range(0, 4);
            //if (HasSingleton<FlowFieldData>())
            //{
            //    //Debug.Log("So does it never go into here");
            //    //var TempbufferDestinationsentity = DestinationBoofQuery.ToEntityArray(Allocator.TempJob);
            //    var TempbufferDestinationsentity = GetSingletonEntity<CellDestinationsBuffer>();

            //    tempDestinatebuffo = GetBuffer<CellDestinationsBuffer>(TempbufferDestinationsentity);

            //    //var TempBufferDestinate = GetBufferFromEntity<CellDestinationsBuffer>(true);        
            //    SetSingleton<FlowFieldData>(tempdestination);
            //    //TempbufferDestinationsentity.Dispose();
            //    SetSingleton<CellBDLayerToCalc>(new CellBDLayerToCalc { intVal = randmbuff });

            //}
            //else
            //{
            //    return;
            //}

            EntityManager.CreateEntity(typeof(StartSpawningSystemEvent));

            if (HasSingleton<FlowFieldData>())
            {
                //Debug.Log("So does it never go into here");
                //var TempbufferDestinationsentity = DestinationBoofQuery.ToEntityArray(Allocator.TempJob);
                var TempbufferDestinationsentity = GetSingletonEntity<CellDestinationsBuffer>();

                tempDestinatebuffo = GetBuffer<CellDestinationsBuffer>(TempbufferDestinationsentity);

                //var TempBufferDestinate = GetBufferFromEntity<CellDestinationsBuffer>(true);        
                SetSingleton<FlowFieldData>(tempdestination);
                //TempbufferDestinationsentity.Dispose();
                SetSingleton<CellBDLayerToCalc>(new CellBDLayerToCalc { intVal = 0 });

            }
            else
            {
                return;
            }

            int2 treenumb = new int2(103, 129);//TODO This is too damn high for some reason
            var tempbuffeee = tempDestinatebuffo[0];//TODO THis just sets the distination I did not think it would work 
            tempbuffeee.Destination = treenumb;
            tempDestinatebuffo[0] = tempbuffeee;

            if (!HasSingleton<CalcintegrationFieldEvent>())
            {
                EntityManager.CreateEntity(typeof(CalcintegrationFieldEvent));
            }

        }

        if(tempinput.Calcotherlayer)
        {
            if (HasSingleton<FlowFieldData>())
            {               
                var TempbufferDestinationsentity = GetSingletonEntity<CellDestinationsBuffer>();

                tempDestinatebuffo = GetBuffer<CellDestinationsBuffer>(TempbufferDestinationsentity);

                SetSingleton<FlowFieldData>(tempdestination);

                //var Layertocalc = EntityManager.CreateEntity(typeof(CellBDLayerToCalc));
                //var layertocalc = GetSingleton<CellBDLayerToCalc>();
                SetSingleton<CellBDLayerToCalc>(new CellBDLayerToCalc { intVal = 1});
                //layertocalc.intVal = 1;
            }
            else
            {
                return;
            }

            //TODO Where is the best place to create this entity I need to use it here but do I need to create it here
            //var Layertocalc = EntityManager.CreateEntity(typeof(CellBDLayerToCalc));

            
            int2 secondtreenumb = new int2(316, 316);

            var tempbufftwoo = tempDestinatebuffo[1];
            tempbufftwoo.Destination = secondtreenumb;
            tempDestinatebuffo[1] = tempbufftwoo;

            if (!HasSingleton<CalcintegrationFieldEvent>())
            {
                EntityManager.CreateEntity(typeof(CalcintegrationFieldEvent));
            }

            var tempobject = tempmoveobject.TestObjectPrefab;

            NativeArray<Entity> dudees = new NativeArray<Entity>(500, Allocator.Temp);
            EntityManager.Instantiate(tempobject, dudees);
            EntityManager.AddComponent<TestMoveojbectTag>(dudees);
            EntityManager.AddComponent<CellBDLayer>(dudees);
            for (int i = 0; i < 500; i++)
            {
                int xrandnumpos = UnityEngine.Random.Range(0, 5);
                int zrandnumpos = UnityEngine.Random.Range(0, 5);

                //var tudue = EntityManager.Instantiate(tempobject);
                //EntityManager.AddComponent<TestMoveojbectTag>(dudees);

                float3 thecreatepos = new float3(xrandnumpos, 0, zrandnumpos);

                EntityManager.SetComponentData<Translation>(dudees[i], new Translation { Value = thecreatepos });
                EntityManager.SetComponentData<CellBDLayer>(dudees[i], new CellBDLayer { intVal = 1 });

                //var instances = new NativeArray<Entity>(500, Allocator.Temp);
                //EntityManager.Instantiate(entity, instances);

            }

            dudees.Dispose();

        }
        //if (HasSingleton<CalcintegrationFieldEvent>())
        //{
        //    var tamedent = GetSingletonEntity<CalcintegrationFieldEvent>();
        //    EntityManager.DestroyEntity(tamedent);
        //}
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        //    Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(mousePos);

        //    _flowFieldControllerEntity = _flowFieldControllerQuery.GetSingletonEntity();

        //    FlowFieldControllerData flowFieldControllerData = EntityManager.GetComponentData<FlowFieldControllerData>(_flowFieldControllerEntity);
        //    GridDebug.instance.FlowFieldControllerData = flowFieldControllerData;

        //    FlowFieldData flowFieldData = new FlowFieldData
        //    {
        //        gridSize = flowFieldControllerData.gridSize,
        //        cellRadius = flowFieldControllerData.cellRadius,
        //        clickedPos = worldMousePos
        //    };

        //    NewFlowFieldData newFlowFieldData = new NewFlowFieldData { isExistingFlowField = true };

        //    if (_flowFieldEntity.Equals(Entity.Null))
        //    {
        //        _flowFieldEntity = EntityManager.CreateEntity();
        //        EntityManager.AddComponent<FlowFieldData>(_flowFieldEntity);
        //        newFlowFieldData.isExistingFlowField = false;
        //    }

        //    EntityManager.AddComponent<NewFlowFieldData>(_flowFieldEntity);
        //    EntityManager.SetComponentData(_flowFieldEntity, flowFieldData);
        //    EntityManager.SetComponentData(_flowFieldEntity, newFlowFieldData);
        //}

        //TempbufferDestinationsentity.Dispose();

    }

    public static int2 GetCellIndexFromWorldPos(float3 worldPos, int2 gridSize, float cellDiameter)
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

[AlwaysUpdateSystem]
public class PlayerInputStuffSystem : SystemBase, PlayerInputActions.IPlayerControlsActions
{
    public PlayerInputActions playerinputacts;
       
    private float2 inputMove;
    private float2 inputLook;
    private bool inputFire;
    private bool startfield;
    private bool addlayer;
    private bool Calcotherlayer;

    //public NativeList<float> FireInputs;

    protected override void OnCreate()
    {
        base.OnCreate();

        playerinputacts = new PlayerInputActions();
        playerinputacts.PlayerControls.SetCallbacks(this);
        //FireInputs = new NativeList<float>(Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        //if (FireInputs.IsCreated)
        {
            //FireInputs.Dispose();
        }
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        playerinputacts.Enable();

        if (HasSingleton<PlayerInput>()) return;

        EntityManager.CreateEntity(typeof(PlayerInput));
    }


    protected override void OnUpdate()
    {

        var move = inputMove;
        bool fire = inputFire;
        bool stootfield = startfield;
        bool oodlayer = addlayer;
        bool calcoddlayer = Calcotherlayer;
        //var look = inputLook;
        //bool jump = controls.Player.Jump.triggered;
        //bool fire = controls.Player.Fire.triggered;
        //if (FireInputs[0] == 1f)
        //{
        //    fire = true;
        //}
        //else
        //{
        //    fire = false;
        //}
                
                
        //Debug.Log("The inputfire is" + fire);

        Entities.ForEach((ref PlayerInput input) =>
        {
            input.moveInput = move;
            //Debug.Log(input.moveInput);
            //input.jumpInput = jump;
            //input.lookInput = look;
            input.fireinput = fire;
            input.StartFloofeld = stootfield;
            input.Addlayer = oodlayer;
            input.Calcotherlayer = calcoddlayer;
        }).Run();

        inputFire = false;
        startfield = false;
        addlayer = false;
        Calcotherlayer = false;
        //FireInputs.Clear();

    }

   
    protected override void OnStopRunning()
    {
        base.OnStartRunning();

        playerinputacts.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>();
         
        //Debug.Log(context.valueType.Name.ToString());

    }

    public void OnFire(InputAction.CallbackContext context)
    {
        var tempval = context.ReadValue<float>();
        //var othertempval = context.action;
        //Debug.Log("The value on the thingy is " + othertempval);
        //var playerEntity = HasSingleton<Player>() ?
        //GetSingletonEntity<Player>() : Entity.Null;
        //Debug.Log("The input value type is " + context.valueType);
        inputFire = tempval == 1 ? inputFire = true : inputFire = false;
        //Debug.Log("The inputfire value is " + inputFire);
        //if(tempval == 1)
        //{
        //    inputFire = true;
        //}
        //else
        //{
        //    inputFire = false;
        //}

        //inputFire = context.performed;

        //Debug.Log("The button has been pressed" + tempval);
        //FireInputs.Add(tempval);
        //e.InputValue = context.ReadValue<float>();

        //ShootInputs.Add(e);
    }

    public void OnStartFlowfield(InputAction.CallbackContext context)
    {
        var anothertempval = context.ReadValue<float>();
        startfield = anothertempval == 1 ? startfield = true : startfield = false;
       
    }

    public void OnAddNewLayer(InputAction.CallbackContext context)
    {
        var anotheothertempval = context.ReadValue<float>();
        addlayer = anotheothertempval == 1 ? addlayer = true : addlayer = false;
        
    }

    public void OnCalcOtherLayer(InputAction.CallbackContext context)
    {
        var otherthee = context.ReadValue<float>();
        Calcotherlayer = otherthee == 1 ? Calcotherlayer = true : Calcotherlayer = false;

    }
}




//public class PlayerOtherInputSystem : SystemBase
//{
//    private ControlActions.BattleActions BattleAction;

//    private InputAction MoveUp;
//    private InputAction MoveDown;
//    private InputAction MoveLeft;
//    private InputAction MoveRight;
//    private InputAction SwitchA;
//    private InputAction SwitchB;
//    private InputAction Skill1;
//    private InputAction Skill2;
//    private InputAction Skill3;
//    private InputAction SupportSkill1;
//    private InputAction SupportSkill2;
//    private InputAction GunFire;

//    protected override void OnCreate()
//    {
//        BattleAction = new ControlActions().Battle;
//    }

//    protected override void OnStartRunning()
//    {
//        BattleAction.Enable();

//        MoveUp = BattleAction.MoveUp;
//        MoveDown = BattleAction.MoveDown;
//        MoveLeft = BattleAction.MoveLeft;
//        MoveRight = BattleAction.MoveRight;

//        SwitchA = BattleAction.SwitchA;
//        SwitchB = BattleAction.SwitchB;

//        GunFire = BattleAction.GunFire;
//        Skill1 = BattleAction.Skill1;
//        Skill2 = BattleAction.Skill2;
//        Skill3 = BattleAction.Skill3;

//        SupportSkill1 = BattleAction.SupportSkill1;
//        SupportSkill2 = BattleAction.SupportSkill2;
//    }

//    protected override void OnStopRunning()
//    {
//        BattleAction.Disable();
//    }

//    protected override void OnUpdate()
//    {
//        Entities
//            .WithAll<PlayerCurrentControlledTag>()
//            .WithNone<PlayerSupportTag>()
//            .WithoutBurst()
//            .ForEach((ref EntityMovementData movedata, ref PlayerData playerdata, ref IsGunFiringData gunfiredata, ref SwitchData switchData, ref ControlledSkillIndexData isCurrentSkillActiveData, ref SupportSkillIndexData isSupportSkillActiveData) =>
//            {
//                movedata.direction.y = MoveDown.WasPressedThisFrame() ? 1 : 0;
//                movedata.direction.y -= MoveUp.WasPressedThisFrame() ? 1 : 0;
//                movedata.direction.x = MoveRight.WasPressedThisFrame() ? 1 : 0;
//                movedata.direction.x -= MoveLeft.WasPressedThisFrame() ? 1 : 0;

//                switchData.value = SwitchA.WasPressedThisFrame() ? 1 : SwitchB.WasPressedThisFrame() ? 2 : 0;

//                gunfiredata.active = GunFire.IsPressed();

//                isCurrentSkillActiveData.value = Skill1.WasPressedThisFrame() ? 1 : Skill2.WasPressedThisFrame() ? 2 : Skill3.WasPressedThisFrame() ? 3 : 0;
//                isSupportSkillActiveData.value = SupportSkill1.WasPressedThisFrame() ? 1 : SupportSkill2.WasPressedThisFrame() ? 2 : 0;

//            }).Run();
//    }
//}