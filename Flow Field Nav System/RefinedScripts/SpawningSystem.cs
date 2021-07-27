using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

public class SpawningSystem : SystemBase
{
    public DotPrefabinator Prefabinator; //I can just put any objects into this that I want to convert into entities and instantiate I could the obstacleobject quite easily into this if I wanted
    
    public int NumbtospawnatPoint;
    public int MaxnumbspawnatPoint;

    public EntityQuery SpawnpointsQuery;

    protected override void OnCreate()
    {
        base.OnCreate();
        
        RequireSingletonForUpdate<StartSpawningSystemEvent>();

        NumbtospawnatPoint = 10;

        MaxnumbspawnatPoint = 50;

        SpawnpointsQuery = GetEntityQuery(ComponentType.ReadOnly<SpawnPointAuthoring>(), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<NonUniformScale>());

        //Enabled = false;
    }

    protected override void OnStartRunning()
    {
        Prefabinator = HasSingleton<DotPrefabinator>() ? GetSingleton<DotPrefabinator>() : default;


    }

    protected override void OnUpdate()
    {
        if (HasSingleton<StartSpawningSystemEvent>())
        {
            var destrydishit = GetSingletonEntity<StartSpawningSystemEvent>();
            EntityManager.DestroyEntity(destrydishit);
        }

        int NumbtospawnatPoint = 50;//UnityEngine.Random.Range(1, MaxnumbspawnatPoint);

        var TempSpawnpoints = SpawnpointsQuery.ToComponentDataArray<Translation>(Allocator.Temp);
        var TempNonUniformScale = SpawnpointsQuery.ToComponentDataArray<NonUniformScale>(Allocator.Temp);

        var currenttempscale = TempNonUniformScale[0].Value;
        var Spawn1Rootpoint = TempSpawnpoints[0].Value;
        var positions = new List<float>(); 

        float deviation = 1.1f;  //TODO Need to get this value from the actual prefab object, get the mesh or collider size and then modify and add a little bit onto and then it should work for any mesh as long as I keep them accurate

        for (int i = 0; i < NumbtospawnatPoint; i++)//This makes sure the entitys positions are separated and not too close together or completely inside one another
        {
            float pos = 0;
            int count = 0;           
            do
            {
                if (count > 50)
                {
                    break;
                }
                pos = UnityEngine.Random.Range(Spawn1Rootpoint.x - (currenttempscale.x / 2), Spawn1Rootpoint.x + (currenttempscale.x / 2));

                count++;
            } while (positions.Exists(p => math.abs(p - pos) < deviation));

            if (!positions.Exists(p => math.abs(p - pos) < deviation))
            {
                positions.Add(pos);
            }
        }

        NativeArray<Entity> dudees = new NativeArray<Entity>(positions.Count, Allocator.Temp);

        EntityManager.Instantiate(Prefabinator.TestObjectPrefab, dudees);

        EntityManager.AddComponent<TestMoveojbectTag>(dudees);
        EntityManager.AddComponent<CellBDLayer>(dudees);

        for (int i = 0; i < positions.Count; i++)
        {
            float zrandnumpos = Spawn1Rootpoint.z;//UnityEngine.Random.Range(90, 120);
            //Debug.Log("The count is " + positions.Count);
            
            float xrandnumpos = positions[i];           

            float3 thecreatepos = new float3(xrandnumpos, 0, zrandnumpos);

            EntityManager.SetComponentData<Translation>(dudees[i], new Translation { Value = thecreatepos });
            EntityManager.SetComponentData<CellBDLayer>(dudees[i], new CellBDLayer { intVal = 0 });           
        }

        dudees.Dispose();   

    }
}

public struct StartSpawningSystemEvent : IComponentData { }

//[BurstCompile]
//public class AudioSystem : SystemBase
//{

//    [StructLayout(LayoutKind.Sequential, Size = 132)]
//    public struct AudioMessage
//    {
//        public SystemMessageType type;
//        public FixedString64 audioFile;
//    }


//    private NativeQueue<AudioMessage> _messageQueue;
//    public static NativeQueue<AudioMessage>.ParallelWriter messageIn;

//    protected override void OnCreate()
//    {
//        base.OnCreate();

//        _messageQueue = new NativeQueue<AudioMessage>(Allocator.Persistent);
//        messageIn = _messageQueue.AsParallelWriter();
//    }
//    protected override void OnDestroy()
//    {
//        base.OnDestroy();
//        _messageQueue.Dispose();
//    }

//    /** cache known audio files to avoid GC allocations */
//    private System.Collections.Generic.Dictionary<FixedString64, string> _strLookup = new System.Collections.Generic.Dictionary<FixedString64, string>();
//    /// <summary>
//    /// cache strings to avoid gc pressure
//    /// </summary>
//    /// <param name="fstr"></param>
//    /// <returns></returns>
//    private string _getString(ref FixedString64 fstr)
//    {
//        string toReturn;
//        if (_strLookup.TryGetValue(fstr, out toReturn))
//        {
//            return toReturn;
//        }
//        toReturn = fstr.ToString();
//        _strLookup.Add(fstr, toReturn);
//        return toReturn;
//    }


//    protected override void OnUpdate()
//    {

//        var _messageQueue = this._messageQueue;
//        Job
//        //.WithReadOnly(messageQueueParallel) //guess this is not needed
//        .WithCode(() =>
//        {
//            while (_messageQueue.TryDequeue(out var message))
//            {
//                var audioFileStr = _getString(ref message.audioFile);
//                switch (message.type)
//                {
//                    case SystemMessageType.Audio_Sfx:
//                        AudioManager.instance.PlaySfxRequest(audioFileStr);
//                        break;
//                    case SystemMessageType.Audio_Music:
//                        AudioManager.instance.PlayMusicRequest(audioFileStr);
//                        break;
//                    default:
//                        throw new System.Exception("invalid message type send to audioSystem");
//                }
//            }
//        })
//        .WithoutBurst()
//        .Run();
//    }
//}

//var audioQueue = AudioSystem.messageIn;
//Job
//    .WithCode(() =>
//    {
//        while (onKillQueue.TryDequeue(out var tuple))
//        {
//            var audioMessage = new AudioSystem.AudioMessage() { audioFile = tuple.component.sfxName, type = SystemMessageType.Audio_Sfx };
//            audioQueue.Enqueue(audioMessage);

//        }
//    })

//    .Schedule();