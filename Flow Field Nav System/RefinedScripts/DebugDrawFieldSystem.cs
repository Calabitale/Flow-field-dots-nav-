using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Drawing;
using Unity.Collections;

public class DebugDrawFieldSystem : SystemBase
{
    public EntityQuery celldataquery;
    //public EntityQuery CellBestDirectionQuery;
    
    protected override void OnCreate()
    {
        base.OnCreate();

        celldataquery = GetEntityQuery(ComponentType.ReadOnly<CellData>());

        //CellBestDirectionQuery = GetEntityQuery(ComponentType.ReadOnly<CellsBestDirection>());
        
        Enabled = false;
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Is the debugdraw even running ");
        var flowfielddudat = GetSingleton<FlowFieldData>();
        var builder = DrawingManager.GetBuilder(true);




        builder.Preallocate(flowfielddudat.gridSize.x * flowfielddudat.gridSize.y);

        Entities.ForEach((DynamicBuffer<CellBestDirectionBuff> buffdood, in CellsBestDirection cellbesty, in CellData cells) =>
        {

            if (cells.cost == 255)
            {
                builder.PushColor(Color.red);
                builder.WireBox(cells.worldPos, flowfielddudat.cellRadius * 2);
                builder.PopColor();
            }
            else
            {
                builder.WireBox(cells.worldPos, flowfielddudat.cellRadius * 2);
                builder.Label2D(cells.worldPos, cellbesty.bestDirection.ToString(), 12f, LabelAlignment.Center, Color.white);
            }


        }).Schedule();

        this.CompleteDependency();

        builder.Dispose();
    }
     

    //TODO Can use static functions like this
    public static bool CheckIfComponentExists(Entity entity)
    {

        return true;
    }

}
