using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor;
using Unity.Collections;
using System;
using Unity.Rendering;

[UpdateAfter(typeof(CreateTestObjects))]
public class EnvironmentCollisionDetection : SystemBase
{
    
    public EntityQuery m_QueryBobstacle;
    public EntityQuery m_QueryTranlutions;

    //[ReadOnly]
    //public ComponentDataFromEntity<Translation> Obstaclepositions;

    protected override void OnCreate()
    {
        base.OnCreate();
        //Enabled = false;
        //Addressables.LoadAssetAsync<GenerationSettings>("GenerationSettings")
        //  .Completed += handle => {
        //      Settings = handle.Result;
        //      Enabled = True;
        //  };

        Enabled = false;
    }

    protected override void OnStartRunning()
    {

        //var obstucle = GetSingleton<DotPrefabinator>();
        //var soment = obstucle.TestObjectPrefab;

        //var dudent = EntityManager.Instantiate(soment);
        //EntityManager.SetComponentData<Translation>(dudent, new Translation { Value = new float3(1.1f, 1, 0) });
        //EntityManager.SetName(dudent, "TheDudeObstacle");

        m_QueryBobstacle = GetEntityQuery(ComponentType.ReadOnly<ObstacleCreation>());
        m_QueryTranlutions = GetEntityQuery(ComponentType.ReadOnly<Translation>());

    }

    protected override void OnStopRunning()
    {
        //m_Query.Dispose();
        //m_QueryBobstacle.Dispose();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        //m_QueryBobstacle.Dispose();
    }

    //struct VelocityJob : IJobParallelFor
    struct CheckDistanceJob
    {

    }

    protected override void OnUpdate()
    {

        var dudents = m_QueryBobstacle.ToEntityArray(Allocator.TempJob);
        var trundles = m_QueryTranlutions.ToComponentDataArray<Translation>(Allocator.TempJob);
       
        Entities.WithReadOnly(dudents).WithAll<TestEntAuthorTag>().ForEach((Entity inty, ref IsCollisiontrue isitthough) =>
        {

            if (dudents.Length <= 0)
                return;

            for (int i = 0; i < dudents.Length; i++)
            {
                var toppy = GetComponent<Translation>(dudents[i]);
                var currentdude = GetComponent<Translation>(inty);

                var currdistance = math.distance(currentdude.Value, toppy.Value);
                
                if (currdistance < 1)
                {
                    isitthough.Value = true;
                }
            }
        }).ScheduleParallel();

        //TODO This is how I should dispose of a nativearray
        dudents.Dispose(Dependency);
        //TODO I dont think I need to dispose of this
        trundles.Dispose(); 
    }
    public struct Intersects//OBB First, OBB Second)
    {

        public bool Execute(float3 [] firstob, float3 []secondob)
        {
            

                
            //#region pre test calcs and declarations
                
            //float rf, rs;
                
            //float[,] R = new float[2, 2];
                
            //float[,] AbsR = new float[2, 2];

                
            //compuet rotation matrix by expressing second in terms of first
                
            //also create common sub expressions
                
            //for (int i = 0; i < 2; i++)                    
            //    for (int j = 0; j < 2; j++)                    
            //    {                        
            //        R[i, j] = Vector2.Dot(First.Axis[i], Second.Axis[j]);                        
            //        AbsR[i, j] = Math.Abs(R[i, j]) + EPSILON;
                    
            //    }
            //    //create translation vector                
            
            //    Vector2 translation = Second.Origin - First.Origin;
                
            //    //bring translation into First's local coordinate system                
            //    translation = new Vector2(Vector2.Dot(translation, First.Axis[0]),  
            //    Vector2.Dot(translation, First.Axis[1]));
            //#endregion                
            ////Test if axes FirstX or FirstY separate the OBBs
            //    for (int i = 0; i < 2; i++)
            //    {
            //        rf = First.HalfWidths.Index(i);
            //        rs = Second.HalfWidths.X * AbsR[i, 0] + Second.HalfWidths.Y * AbsR[i, 1];

            //        if (Math.Abs(translation.Index(i)) > (rf + rs))
            //            return false;
            //    }

            //    //Test if axes SecondX or SecondY separate the OBBs
            //    for (int i = 0; i < 2; i++)
            //    {
            //        rf = First.HalfWidths.Index(0) * AbsR[0, i] + First.HalfWidths.Index(1) * AbsR[1, i];
            //        rs = Second.HalfWidths.Index(i);

            //        if (Math.Abs(translation.Index(0) * R[0, i] + translation.Index(1) * R[1, i]) > (rf + rs))
            //            return false;
            //    }

            



            return true;
        }

        //#region pre test calcs and declarations
        //float rf, rs;
        //float[,] R = new float[2, 2];
        //float[,] AbsR = new float[2, 2];

        
        ////compuet rotation matrix by expressing second in terms of first
        ////also create common sub expressions
        //for (int i = 0; i < 2; i++)
        //    for (int j = 0; j < 2; j++)
        //    {
        //        R[i, j] = Vector2.Dot(First.Axis[i], Second.Axis[j]);
        //        AbsR[i, j] = math.abs(R[i, j]) + EPSILON;
        //    }

        ////create translation vector
        //Vector2 translation = Second.Origin - First.Origin;

        ////bring translation into First's local coordinate system
        //translation = new Vector2(Vector2.Dot(translation, First.Axis[0]),
        //    Vector2.Dot(translation, First.Axis[1]));
        //#endregion

        ////Test if axes FirstX or FirstY separate the OBBs
        //for (int i = 0; i < 2; i++)
        //{
        //    rf = First.HalfWidths.Index(i);
        //    rs = Second.HalfWidths.X * AbsR[i, 0] + Second.HalfWidths.Y * AbsR[i, 1];

        //    if (Math.Abs(translation.Index(i)) > (rf + rs))
        //        return false;
        //}

        ////Test if axes SecondX or SecondY separate the OBBs
        //for (int i = 0; i < 2; i++)
        //{
        //    rf = First.HalfWidths.Index(0) * AbsR[0, i] + First.HalfWidths.Index(1) * AbsR[1, i];
        //    rs = Second.HalfWidths.Index(i);

        //    if (Math.Abs(translation.Index(0) * R[0, i] + translation.Index(1) * R[1, i]) > (rf + rs))
        //        return false;
        //}

        //no separating axis - OBBs must therefore be intersecting
        
    }
}

public class CreateTestObjects : SystemBase
{
    public int Gridsize;
    public float Cellsize;
    protected override void OnCreate()
    {
        base.OnCreate();

        Gridsize = 100;
        Cellsize = 1;
        Enabled = false;
    }

    protected override void OnStartRunning()
    {
        
    }

    protected override void OnUpdate()
    {
        
        var obstucle = GetSingleton<DotPrefabinator>();

        if(!EntityManager.Exists(obstucle.TestObjectPrefab))
        {
            Debug.Log("This bloody enity does not exist");
            return;
        }

        var cellradii = Cellsize / 2;
        //float3 cellWorldPos = new float3(cellDiameter * x + cellRadius, 0, cellDiameter * y + cellRadius);
        //x + (x * spacing)
        float spacing = 0f;

        var soment = obstucle.TestObjectPrefab;

        for (int x = 0; x < Gridsize; x++)
        {
            for (int y = 0; y < Gridsize; y++)
            {
                var dudent = EntityManager.Instantiate(soment);
                //TODO Need to figure out how to make it positioned better like where do I want the first cell to begin or does it matter because am I going to use his cell creation, I need to place the obstacles manually at least
                EntityManager.SetComponentData<Translation>(dudent, new Translation { Value = new float3(x * spacing + x, 1, y * spacing + y) });
                EntityManager.SetName(dudent, "TheDudeObstacle");
                EntityManager.AddComponent(dudent, typeof(IsCollisiontrue));
            }
        }

        Enabled = false; 
    }
}



public struct IsCollisiontrue : IComponentData
{
    public bool Value;
}

/*
public class OBB
{
    #region members and properties
    Vector2 _origin;//centre point of the OBB
    public Vector2 Origin
    {
        get { return _origin; }
        set { _origin = value; }
    }
    Vector2[] _axis;//2d orientation matrix
    public Vector2[] Axis
    {
        get { return _axis; }
    }
    Vector2 _halfWidths;//the +ve extents along each axis
    public Vector2 HalfWidths
    {
        get { return _halfWidths; }
        set { _halfWidths = value; }
    }
    float _angleInRadians;//used for drawing a visuaisation of the OBB
    public float AngleInRadians
    {
        get { return _angleInRadians; }
        set
        {
            if (value < Math.PI && value > -Math.PI) //in the range [-PI..PI]
                UpdateAxis(value);
            if (value >= Math.PI)
                UpdateAxis((float)(Math.Abs((value % Math.PI * 2)) - Math.PI));
            if (value <= -Math.PI)
                UpdateAxis((float)(Math.Abs((value % Math.PI * 2)) + Math.PI));
        }
    }

    public Color DebugColor = Color.White;
    //an epsilon value to counter floating point errors in a parallel situation
    const float EPSILON = 0.00001f;
    #endregion

    /// <summary>
    /// Creates an oriented bounding box for collision detection
    /// </summary>
    /// <param name="Origin">The center of the box</param>
    /// <param name="AngleInRadians">The rotation of the box in the xy plane</param>
    /// <param name="HalfWidths">The half extents of the box in it's X and Y axis</param>
    public OBB(Vector2 Origin, float AngleInRadians, Vector2 HalfWidths)
    {
        _origin = Origin;
        _angleInRadians = AngleInRadians;
        _halfWidths = HalfWidths;

        _axis = new Vector2[2];
        _axis[0] = new Vector2();
        _axis[1] = new Vector2();

        UpdateAxis(AngleInRadians);
    }

    /// <summary>
    /// Returns whether this OBB is intersecting a second
    /// </summary>
    /// <param name="OtherOBB"></param>
    /// <returns></returns>
    public bool Intersects(OBB OtherOBB)
    {
        return OBB.Intersects(this, OtherOBB);
    }

    /// <summary>
    /// Finds where we would draw our debug texture without rotation.
    /// </summary>
    /// <returns>A rectangle corresponging to the unrotated position of the AABB</returns>
    private Rectangle GetDestinationRect()
    {
        int x = (int)(_origin.X);
        int y = (int)(_origin.Y);
        int width = (int)(_halfWidths.X * 2);
        int height = (int)(_halfWidths.Y * 2);
        return new Rectangle(x, y, width, height);
    }

    /// <summary>
    /// Updates the orientation of the OBB
    /// </summary>
    /// <param name="AngleInRadians">The new rotation in radians</param>
    public void UpdateAxis(float AngleInRadians)
    {
        //Standard rotation matrix equation
        _axis[0].X = (float)Math.Cos(AngleInRadians);
        _axis[0].Y = (float)Math.Sin(AngleInRadians);
        _axis[1].Y = (float)Math.Cos(AngleInRadians);
        _axis[1].X = -(float)Math.Sin(AngleInRadians);

        _angleInRadians = AngleInRadians;
    }

    /// <summary>
    /// Prints the properties of the OBB to the console
    /// </summary>
    public void Print()
    {
        Console.WriteLine("origin : " + _origin + "\nAngle : " + _angleInRadians +
        "\nX Axis : " + _axis[0] + "\nY Axis : " + _axis[1]);
    }

    /// <summary>
    /// Draws a visualisation of the OBB, useful for debug purposes
    /// </summary>
    /// <param name="sb">The spritebatch instance to draw with</param>
    /// <param name="NullTexture">A reference to a 1x1 texture which will be draw over the OBB</param>
    public void Draw(Texture2D NullTexture, SpriteBatch sb)
    {
        //OBB's are only ever going to be drawn in debug mode
        //so performance from repeated Begin() End() calls is acceptable

        sb.Begin();

        Vector2 g = _halfWidths;
        g.Normalize();
        sb.Draw(NullTexture, GetDestinationRect(), null, DebugColor,
            _angleInRadians,
            Vector2.One / 2,//the origin of the 1x1 texture i.e. (0.5f,0.5f)
            SpriteEffects.None, 1f);

        sb.End();

    }

    /// <summary>
    /// Tests whether two OBBs intersect. Uses a separating axis implementation.
    /// </summary>
    /// <param name="First">The first OBB</param>
    /// <param name="Second">The second OBB</param>
    /// <returns></returns>
    public static bool Intersects(OBB First, OBB Second)
    {
        #region pre test calcs and declarations
        float rf, rs;
        float[,] R = new float[2, 2];
        float[,] AbsR = new float[2, 2];

        //compuet rotation matrix by expressing second in terms of first
        //also create common sub expressions
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 2; j++)
            {
                R[i, j] = Vector2.Dot(First.Axis[i], Second.Axis[j]);
                AbsR[i, j] = Math.Abs(R[i, j]) + EPSILON;
            }

        //create translation vector
        Vector2 translation = Second.Origin - First.Origin;

        //bring translation into First's local coordinate system
        translation = new Vector2(Vector2.Dot(translation, First.Axis[0]),
            Vector2.Dot(translation, First.Axis[1]));
        #endregion

        //Test if axes FirstX or FirstY separate the OBBs
        for (int i = 0; i < 2; i++)
        {
            rf = First.HalfWidths.Index(i);
            rs = Second.HalfWidths.X * AbsR[i, 0] + Second.HalfWidths.Y * AbsR[i, 1];

            if (Math.Abs(translation.Index(i)) > (rf + rs))
                return false;
        }

        //Test if axes SecondX or SecondY separate the OBBs
        for (int i = 0; i < 2; i++)
        {
            rf = First.HalfWidths.Index(0) * AbsR[0, i] + First.HalfWidths.Index(1) * AbsR[1, i];
            rs = Second.HalfWidths.Index(i);

            if (Math.Abs(translation.Index(0) * R[0, i] + translation.Index(1) * R[1, i]) > (rf + rs))
                return false;
        }

        //no separating axis - OBBs must therefore be intersecting
        return true;
    }
}
    
}

namespace Extensions
{
    /// <summary>
    /// Adds the ability to index a Vector2 for cheeky code saving
    /// </summary>
    public static class Vector2Extensions
    {
        public static float Index(this Vector2 v, int i)
        {
            switch (i)
            {
                case 0:
                    return v.X;
                case 1:
                    return v.Y;
                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }
}
*/
