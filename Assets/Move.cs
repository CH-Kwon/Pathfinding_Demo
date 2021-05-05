using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //public instance for move script to access on the other script
    public static Move instance;

    public float speed;
    public float distance;

    public int pathIndex;
    public int current = 0;

    public bool isArrived;
    public bool isFinalGoal;

    public LineRenderer lr;
    public List<Node> target;

    private void Awake()
    {
        pathIndex = 0;
    }
    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        target = new List<Node>();
        instance = this;

        isArrived = false;
        isFinalGoal = false;

        //distance check : check how much start obj is closed to next node
        distance = 0.3f;
        speed = 0;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        //Start to move when pathfinding of each target are completed
        if (Pathfinding.instance.IsStartCalculation)
        {
            Movement(SpawnTarget.instance.newTargetList, Pathfinding.instance.startObj);
            DrawPath();        
        }
    }
    
    //This function actually makes movement for start object
    private void Movement(List<GameObject> pathList, GameObject startPoint)
    {       
        //Save path for each target
        target = Pathfinding.instance.TempFinalPath;

        //If start object is not get to final target 
        if (!isFinalGoal)
        {
            //move target according to nodes of each target position
            Vector3 dir = target[current].nodePosition - startPoint.transform.position;
            startPoint.transform.position += dir * Time.deltaTime * speed;

            //Increase current value to move from current target to next target every frame
            if (dir.magnitude < distance)
            {
                current++;
            }
            //If start object gets to next target
            if (current >= target.Count)
            {
                //If start object is get to final target
                if (pathIndex == SpawnTarget.instance.newTargetList.Count - 1)
                {
                    isFinalGoal = true;
                }
                //Check if start obj is get to next target, mvoe to next target
                else
                {
                    current = 0;
                    pathIndex++;
                    
                    Pathfinding.instance.FinalPathCalculation = false;
                }
            }
        }
    }

    //Drawing every path that start obj has been through
    [System.Obsolete]
    private void DrawPath()
    {
        if (Pathfinding.instance.allNodes != null && (ButtonControl.instance.buttonOption != ButtonControl.ButtonOption.DELETE_LINE))
        {
                //Count of all nodes in the target list
                lr.positionCount = Pathfinding.instance.allNodes.Count;

                //Render color for each node in the target list
                for (int i = 0; i < lr.positionCount; i++)
                {
                    lr.material.color = Color.green;
                    lr.SetWidth(0.2f, 0.2f);
                    lr.SetPosition(i, Pathfinding.instance.allNodes[i].nodePosition);
                }
            }
        }
}
