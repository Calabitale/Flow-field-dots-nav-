using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using DotsFlowField;

public class FlowFieldVisualDebug : MonoBehaviour
{
    EntityManager currentmanager;
    public float CellDiameter;
    //public InitialiseFlowField Flowfieldsys;
    public int fildGridsize;

    //public DynamicBuffer<CellDataBuffer> celldatas;
    public Transform GridOrigin;

    public NativeArray<Entity> toodledood;

    //public NativeArray<CellDataBuffer> cellDatabuffarray;

    public NativeArray<CellData> cellDataArray;

    public NativeArray<ObstacleCollisionVerts> obstaclebuffers;

    public NativeArray<float3> obstacleverts;

    public NativeArray<float3> flowfieldverts;

    public NativeArray<FlowFieldData> flowfielddatadoots;

    public bool DisplayDirections;

    public Texture testTexture;

    public EntityQuery CellbestyQuery;

    public NativeArray<CellsBestDirection> cellbestydirection;

    public EntityQuery playerinputquery;

    public NativeArray<PlayerInput> playerinput;

    public bool ultimplayeinput;

    // Start is called before the first frame update
    void Start()
    {
        //CellDiameter = 1f;
        //fildGridsize = 10;
        currentmanager = World.DefaultGameObjectInjectionWorld.EntityManager;
        //Flowfieldsys = currentmanager.World.GetOrCreateSystem<InitialiseFlowField>();
        CellbestyQuery = currentmanager.CreateEntityQuery(ComponentType.ReadOnly<CellsBestDirection>());

        playerinputquery = currentmanager.CreateEntityQuery(ComponentType.ReadOnly<PlayerInput>());
    }

    // Update is called once per frame
    void Update()
    {

        //CellBDLayerToCalc LayertoCalc = HasSingleton<CellBDLayerToCalc>() ? GetSingleton<CellBDLayerToCalc>() : default;

        if (playerinputquery.CalculateEntityCount() > 0)
        {
            playerinput = playerinputquery.ToComponentDataArray<PlayerInput>(Allocator.Persistent);
            ultimplayeinput = playerinput[0].fireinput;
            playerinput.Dispose();
        }
        //if(!playerinputentity)

        if (!ultimplayeinput) return;

        var tumbledeedood = currentmanager.CreateEntityQuery(ComponentType.ReadOnly<CellData>());
        //toodledood = tumbledeedood.ToEntityArray(Allocator.Persistent);
        var toodledood = tumbledeedood.CalculateEntityCount();

        var colliderpoints = currentmanager.CreateEntityQuery(ComponentType.ReadOnly<ObstacleCollisionVerts>());
        var colliderents = colliderpoints.ToEntityArray(Allocator.Persistent);

        var flowfielddoots = currentmanager.CreateEntityQuery(ComponentType.ReadOnly<FlowfieldVertPointsBuff>());
        var flowfieldpointents = flowfielddoots.ToEntityArray(Allocator.Persistent);

        var flowfielddata = currentmanager.CreateEntityQuery(ComponentType.ReadOnly<FlowFieldData>());

        var CellbestyQuery = currentmanager.CreateEntityQuery(ComponentType.ReadOnly<CellsBestDirection>());
     
        if(toodledood > 0)
        {
            int tempcounter = 0;

            cellDataArray = tumbledeedood.ToComponentDataArray<CellData>(Allocator.Persistent);
            //TODO FLowfieldData is a singleton can I not get it through this in some manner
            flowfielddatadoots = flowfielddata.ToComponentDataArray<FlowFieldData>(Allocator.Persistent);

            cellbestydirection = CellbestyQuery.ToComponentDataArray<CellsBestDirection>(Allocator.Persistent);

            //if(flowfielddatadoots.
            if (flowfielddatadoots.Length > 0)
            {
                CellDiameter = flowfielddatadoots[0].cellRadius * 2;
            }
            //obstaclebuffers = new NativeArray<ObstacleCollisionVerts>(colliderents.Length, Allocator.Persistent);
            obstacleverts = new NativeArray<float3>(colliderents.Length * 2, Allocator.Persistent);  //TODO Maybe this is not the right length or something but there are errant values that should not exist either on the objects or within the 
            for (int i = 0; i < colliderents.Length; i++)
            {
                var tempbuffer = currentmanager.GetBuffer<ObstacleCollisionVerts>(colliderents[i]);

                //obstacleverts = new NativeArray<float3>(collideren, Allocator.Persistent);
                for (int j = 0; j < tempbuffer.Length; j++)
                {
                   obstacleverts[tempcounter] = tempbuffer[j];                    
                    tempcounter++;
                }
                
            }

        }

        //if(toodledood > 0)
        //{
        //    //celldatas = currentmanager.GetBuffer<CellDataBuffer>(toodledood[0]);
        //    //var tadaa = teede[0].celldata.worldPos;
        //    //cellDataArray = tumbledeedood.ToComponentDataArray<CellData>(Allocator.Persistent);
        //    //cellDatabuffarray = celldatas.ToNativeArray(Allocator.Persistent);
            
        //    //Debug.Log("The world pos of position 1 is" + tadaa);
        //}

        if (flowfieldpointents.Length > 0)
        {
            int othertempcounter = 0;
            flowfieldverts = new NativeArray<float3>(flowfieldpointents.Length * 2, Allocator.Persistent);
            for (int i = 0; i < flowfieldpointents.Length; i++)
            {
                var othertempbuffer = currentmanager.GetBuffer<FlowfieldVertPointsBuff>(flowfieldpointents[i]);
                for(int j = 0; j < othertempbuffer.Length; j++)
                {
                    flowfieldverts[othertempcounter] = othertempbuffer[j];
                    othertempcounter++;
                    //TODO need to get this to display in the editor
                }

            }
        }

        //var teede = currentmanager.GetBuffer<CellDataBuffer>(toodledood[0]);
        //var tadaa = teede[0].celldata.worldPos;

        //Debug.Log("The world pos of position 1 is" + tadaa);

        //tumbledeedood.Dispose();
        //colliderpoints.Dispose();
        //toodledood.Dispose();
        colliderents.Dispose();
        //flowfielddoots.Dispose();
        flowfieldpointents.Dispose();
        //flowfielddatadoots.Dispose();

        //obstacleverts.Dispose();
        //obstaclebuffers.Dispose();
        //obstacleverts.Dispose();
        //if (cellDatabuffarray.IsCreated)
        //{
        //    cellDatabuffarray.Dispose();
        //}


    }

    

    void OnDestroy()
    {
        if (cellDataArray.IsCreated)
        {
            cellDataArray.Dispose();
        }
        if (cellbestydirection.IsCreated)
        {
            cellbestydirection.Dispose();
        }
    }

    private void OnDrawGizmos()
    {
        //var Flowfieldsys = currentmanager.World.GetOrCreateSystem<InitialiseFlowField>();
        //if (toodledood.Length > 0)
        //{
        //    celldatas = currentmanager.GetBuffer<CellDataBuffer>(toodledood[0]);
        //    //var tadaa = teede[0].celldata.worldPos;

        //    //Debug.Log("The world pos of position 1 is" + tadaa);
        //}


        //if (!World.DefaultGameObjectInjectionWorld.IsCreated && World.DefaultGameObjectInjectionWorld == null) return;


        if (World.DefaultGameObjectInjectionWorld == null || !World.DefaultGameObjectInjectionWorld.IsCreated) return;
        //var cellradii = CellDiameter / 2;
        //if (celldatas.IsEmpty)
        //{
        //    return;
        //}
        //CellDiameter = flowfielddatadoots[1].cellRadius * 2;
        //CellBDLayerToCalc LayertoCalc = HasSingleton<CellBDLayerToCalc>() ? GetSingleton<CellBDLayerToCalc>() : default;

        Color randcolor = new Color();

        //var tempnative = cellDataArray.Length > 0 ? cellDataArray.ToArray() : tem;

        for (int i = 0; i < cellDataArray.Length; i++)
        {
          
            Vector3 currentsiz = new Vector3(CellDiameter, CellDiameter, CellDiameter);
            //Vector3 currentsiz = new Vector3(1f, 1f, 1f);
            //    //var rundcolor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            if (cellDataArray[i].cost == 255)
            {

                //Color randcolor = new Color();
                randcolor = Color.red;
                Gizmos.color = randcolor;
            }
            else
            {
                randcolor = Color.white;
                Gizmos.color = randcolor;
            }

            //Gizmos.DrawWireCube(cellDatabuffarray[i].celldata.worldPos, currentsiz);
            Gizmos.DrawWireCube(cellDataArray[i].worldPos, currentsiz);//TODO Draws the cube
            //Gizmos.DrawWireCube(tempnative[i].worldPos, currentsiz);//TODO Draws the cube
            //Debug.Log("The celldatavalues are" + cellDatabuffarray[i].celldata.worldPos);
            //TODO WOOT this works I just need to only dislpayl the text that I want at any one time I could put an enum box into the object
            GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            //Handles.Label(cellDataArray[i].worldPos, cellDataArray[i].cost.ToString(), style);
            
            Handles.Label(cellDataArray[i].worldPos, cellDataArray[i].gridIndex.ToString(), style);
            //cellDataArray[i].

            //int2 tempint = new int2(0, 0);
            
            if(DisplayDirections == true)
            {
                //if(cellDataArray[i].bestDirection)
                //Rect roct = new Rect();
                //roct.width = 5;
                //roct.height = 5;
                //roct.x = cellDataArray[i].worldPos.x;
                //roct.y = cellDataArray[i].worldPos.y;
                //Gizmos.DrawGUITexture(roct, testTexture);
                //if(cellDataArray[i].bestDirection == (0, 0))
                //var tempbool = Equals(cellDataArray[i].bestDirection, (0, 0));
                var tempneighbours = new GetNeighborIndices();
                //tempneighbours.North
                var tempdirections = tempneighbours.GetNeigbouralldirections();
                Vector3 centercubesize = new Vector3(0.08f, 0.08f, 0.08f);
                //Debug.Log(tempdirections[1]);
                if (Equals(cellbestydirection[i].bestDirection, tempdirections[1]))
                {
                    //Debug.Log("The north vector should be " + cellDataArray[i].worldPos);
                    //Gizmos.DrawIcon(cellDataArray[i].worldPos, "northarrow.png");
                    
                    Vector3 northstartvect = cellDataArray[i].worldPos;
                    Vector3 northvect = new Vector3(0, 0, 0.3f);
                    //Vector3 northvect = new Vector3(tempdirections[1].x, 0, tempdirections[1].y);
                    Vector3 northendvect = (Vector3)cellDataArray[i].worldPos - northvect;
                    
                    Gizmos.DrawCube(cellDataArray[i].worldPos, centercubesize);
                    Gizmos.DrawLine(northstartvect, northendvect);


                }
                else if(Equals(cellbestydirection[i].bestDirection, tempdirections[3]))
                {
                    //Gizmos.DrawIcon(cellDataArray[i].worldPos, "eastarrow.png");
                    Vector3 eaststartvect = cellDataArray[i].worldPos;
                    Vector3 eastvect = new Vector3(0.3f, 0, 0);
                    //Vector3 eastvect = new Vector3(tempdirections[3].x, 0, tempdirections[3].y);
                    Vector3 eastendvect = (Vector3)cellDataArray[i].worldPos - eastvect;

                    Gizmos.DrawCube(cellDataArray[i].worldPos, centercubesize);
                    Gizmos.DrawLine(eaststartvect, eastendvect);


                }
                else if (Equals(cellbestydirection[i].bestDirection, tempdirections[2]))
                {
                    //Gizmos.DrawIcon(cellDataArray[i].worldPos, "southarrow.png");
                    Vector3 southstartvect = cellDataArray[i].worldPos;
                    Vector3 southvect = new Vector3(0, 0, -0.3f);
                    //Vector3 southvect = new Vector3(tempdirections[2].x, 0, tempdirections[2].y);
                    Vector3 southendVect = (Vector3)cellDataArray[i].worldPos - southvect;

                    Gizmos.DrawCube(cellDataArray[i].worldPos, centercubesize);
                    Gizmos.DrawLine(southstartvect, southendVect);

                }
                else if (Equals(cellbestydirection[i].bestDirection, tempdirections[4]))
                {
                    //Gizmos.DrawIcon(cellDataArray[i].worldPos, "westarrow.png");
                    Vector3 weststartvect = cellDataArray[i].worldPos;
                    Vector3 westvect = new Vector3(-0.3f, 0, 0);
                    //Vector3 westvect = new Vector3(tempdirections[4].x, 0, tempdirections[4].y);
                    Vector3 westendvect = (Vector3)cellDataArray[i].worldPos - westvect;

                    Gizmos.DrawCube(cellDataArray[i].worldPos, centercubesize);
                    Gizmos.DrawLine(weststartvect, westendvect);


                }
                else if (Equals(cellbestydirection[i].bestDirection, tempdirections[5]))
                {
                    //Gizmos.DrawIcon(cellDataArray[i].worldPos, "northeastarrow.png");
                    Vector3 northeaststartvect = cellDataArray[i].worldPos;
                    Vector3 northeastvect = new Vector3(0.3f, 0, 0.3f);
                    Vector3 northeastendvect = (Vector3)cellDataArray[i].worldPos - northeastvect;

                    Gizmos.DrawCube(cellDataArray[i].worldPos, centercubesize);
                    Gizmos.DrawLine(northeaststartvect, northeastendvect);


                }
                else if (Equals(cellbestydirection[i].bestDirection, tempdirections[6]))
                {
                    //Gizmos.DrawIcon(cellDataArray[i].worldPos, "northwestarrow.png");
                    Vector3 northweststartvect = cellDataArray[i].worldPos;
                    Vector3 northwestvect = new Vector3(-0.3f, 0, 0.3f);
                    Vector3 northwestendvect = (Vector3)cellDataArray[i].worldPos - northwestvect;

                    Gizmos.DrawCube(cellDataArray[i].worldPos, centercubesize);
                    Gizmos.DrawLine(northweststartvect, northwestendvect);

                }
                else if (Equals(cellbestydirection[i].bestDirection, tempdirections[7]))
                {
                    //Gizmos.DrawIcon(cellDataArray[i].worldPos, "southeastarrow.png");
                    Vector3 southeaststartvect = cellDataArray[i].worldPos;
                    Vector3 southeastvect = new Vector3(0.3f, 0, -0.3f);
                    Vector3 southeastendvect = (Vector3)cellDataArray[i].worldPos - southeastvect;

                    Gizmos.DrawCube(cellDataArray[i].worldPos, centercubesize);
                    Gizmos.DrawLine(southeaststartvect, southeastendvect);
                }
                else if (Equals(cellbestydirection[i].bestDirection, tempdirections[8]))
                {
                    //Gizmos.DrawIcon(cellDataArray[i].worldPos, "southwestarrow.png");
                    Vector3 southweststartvect = cellDataArray[i].worldPos;
                    Vector3 southwestvect = new Vector3(-0.3f, 0, -0.3f);
                    Vector3 southwestendvect = (Vector3)cellDataArray[i].worldPos - southwestvect;

                    Gizmos.DrawCube(cellDataArray[i].worldPos, centercubesize);
                    Gizmos.DrawLine(southweststartvect, southwestendvect);
                }



            
            }
            
            
            //Gizmos(cellDataArray[i].bestDirection, )


        }

        //Draws the verts 
        //for(int j= 0; j < obstacleverts.Length; j++)
        //{
        //    Color curol = Color.blue;
        //    Gizmos.color = curol;
        //    var tempval = obstacleverts[j];
        //    Gizmos.DrawSphere(tempval, 0.1f);

        //}

        //for(int i = 0; i < flowfieldverts.Length; i++)
        //{
        //    Color creal = Color.green;
        //    Gizmos.color = creal;
        //    var temppoint = flowfieldverts[i];
        //    Gizmos.DrawWireSphere(temppoint, 0.1f);

        //}

        if(obstacleverts.IsCreated)
        {
            obstacleverts.Dispose();
        }


        //if (cellDataArray.IsCreated)
        //{
        //    //cellDatabuffarray.Dispose();
        //    cellDataArray.Dispose();
        //}

        if(flowfieldverts.IsCreated)
        {
            flowfieldverts.Dispose();
        }

        //if(cellbestydirection.IsCreated)
        //{
        //    cellbestydirection.Dispose();
        //}

        if(flowfielddatadoots.IsCreated)
        {
            flowfielddatadoots.Dispose();
        }

    }

    //private static void DrawGrid(Vector2Int drawGridSize, Color drawColor, float drawCellRadius)
    //{
    //    Gizmos.color = drawColor;
    //    for (int x = 0; x < drawGridSize.x; x++)
    //    {
    //        for (int y = 0; y < drawGridSize.y; y++)
    //        {
    //            Vector3 center = new Vector3(drawCellRadius * 2 * x + drawCellRadius, 0, drawCellRadius * 2 * y + drawCellRadius);
    //            Vector3 size = Vector3.one * drawCellRadius * 2;
    //            Gizmos.DrawWireCube(center, size);
    //        }
    //    }
    //}
}

public enum CellDataTextDisplayType
{
    Cost,
    BestCost,
    Direction


}