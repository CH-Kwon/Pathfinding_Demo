using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [HideInInspector]
    //public instance for Pathfinding script to access on the other script
    public static Pathfinding instance;

    public GameObject startObj;

    //Start position of start obj to find path
    public List<Transform> StartPosition;
    //Target poisiton of each target 
    public List<Transform> TargetPosition;

    //Temporary list of nodes in each target to calculate next target 
    public List<Node> TempFinalPath;

    //To check if find path of current target
    public bool FinalPathCalculation;
    //To check program has been started
    public bool IsStartCalculation;

    //List of all nodes of evert target objects
    public List<Node> allNodes;

    private void Awake()
    {
        instance = this;

        StartPosition = new List<Transform>();
        TargetPosition = new List<Transform>();
        TempFinalPath = new List<Node>();

        FinalPathCalculation = false;
        IsStartCalculation = false;
       
        allNodes = new List<Node>();
    }

    private void Update()
    {
       GetEveryPath(Move.instance.pathIndex);
    }

    private void GetEveryPath(int pathIndex)
    {
        //If move button clicked, then start to find path
        if (ButtonControl.instance.startFindPath)
        {
            //Check if calcualtion of pathfinding has not been started
            if (!FinalPathCalculation)
            {
                //Start find start object
                startObj = GameObject.FindGameObjectWithTag("StartPoint");

                //Save start object for pathfinding
                StartPosition.Add(startObj.transform);
                //Save target objects based on pathindex which indicates what target points is start point heading to
                TargetPosition.Add(SpawnTarget.instance.newTargetList[pathIndex].transform);

                //Setting the start and target position
                StartPosition[pathIndex].position = startObj.transform.position;
                TargetPosition[pathIndex].position = SpawnTarget.instance.newTargetList[pathIndex].transform.position;

                //Start find path
                FindPath(StartPosition[pathIndex].position, TargetPosition[pathIndex].position);//Find a path to the goal3     
            }
        }
    }

    void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        //Get start nodes in world position to find closest target
        Node StartNode = Grid.instance.WorldNode(a_StartPos);
        //Get target nodes in world position to find closest target
        Node TargetNode = Grid.instance.WorldNode(a_TargetPos);

        //List of node
        List<Node> openList = new List<Node>();
        //Hashset to check if node is in the list
        HashSet<Node> closedList = new HashSet<Node>();

        //Initialize start node in new list
        openList.Add(StartNode);

        //If there is node in new list
        while (openList.Count > 0)
        {
            ////Create new node as first item of new list
            Node CurrentNode = openList[0];

           // CheckNodeCost(openList, closedList, CurrentNode);
            //looping from second object from new list
            for (int i = 1; i < openList.Count; i++)
            {
                //if F,H cost in the openlist are less then cost of current node
                if (openList[i].F_Cost <= CurrentNode.F_Cost &&
                    openList[i].H_Cost < CurrentNode.H_Cost)
                {
                    //Assign the current node to openlist item
                    CurrentNode = openList[i];
                }
            }
            
            openList.Remove(CurrentNode);
            closedList.Add(CurrentNode);

            //If current node is get to target node
            if (CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }

            foreach (Node NeighborNode in Grid.instance.GetNeighboringNodes(CurrentNode))
            {
                //Check if neighbor is wall or already checked
                if (!NeighborNode.isWall_ || closedList.Contains(NeighborNode))
                {
                    continue;
                }

                //Calculation F cost of neighbor
                //Manhatten distance between curNode and neighbornode
                int ManhDist = GetManhattenDistance(CurrentNode, NeighborNode);
                int curNodec_gCost = CurrentNode.G_Cost;

                int F_Cost = GetMoveCost(curNodec_gCost, ManhDist);

                //If g cost is greater then f cost or not in openlist
                if (F_Cost < NeighborNode.G_Cost || !openList.Contains(NeighborNode))
                {
                    //Assign G cost as F cost
                    NeighborNode.G_Cost = F_Cost;
                    //Assign H cost
                    NeighborNode.H_Cost = GetManhattenDistance(NeighborNode, TargetNode);

                    //Set prev node for tracing back 
                    NeighborNode.PrevNode = CurrentNode;

                    //If neighbor node is not in openlist
                    if (!openList.Contains(NeighborNode))
                    {
                        //Add in open list
                        openList.Add(NeighborNode);
                    }
                }
            }
        }
    }

    void GetFinalPath(Node startNode, Node endNode)
    {
        //Save current node which is checked
        Node CurrentNode = endNode;

        //Finalpath list to save nodes
        List<Node> FinalPath = new List<Node>();

        //Looping all prev node to start node
        while (CurrentNode != startNode)
        {
            FinalPath.Add(CurrentNode);
            //Move back into current node of prev node
            CurrentNode = CurrentNode.PrevNode;
        }

        //Reverse the path to get correct path
        FinalPath.Reverse();

        //Clear the final path of every calculation to get each of final path to reach target position
        if (Grid.instance.FinalPath != null)
            Grid.instance.FinalPath.Clear();

        //Get final path
        Grid.instance.FinalPath = FinalPath;

        //Checking final calculation of last target.
        //Stop calculation of pathfinding if start node finally get to last node of last target
        if (!FinalPathCalculation)
        {
            //save each path of each target
            TempFinalPath.Clear();
            TempFinalPath = FinalPath;

            //Save all nodes that start node has been through in all nodes list to render line of all the path
            for(int i = 0; i < TempFinalPath.Count; i++)
            {
                allNodes.Add(TempFinalPath[i]);
            }

            FinalPathCalculation = true;
            IsStartCalculation = true;
        }

    }

    int GetMoveCost(int curNode_gCost, int distance)
    {
        return curNode_gCost + distance;
    }

    int GetManhattenDistance(Node Node_A, Node Node_B)
    {
        //Get distance between node of x axis
        int xDist = Mathf.Abs(Node_B.Grid_xPos - Node_A.Grid_xPos);
        //Get distance between node of y axis
        int yDist = Mathf.Abs(Node_B.Grid_yPos - Node_A.Grid_yPos);
        int result = xDist + yDist;

        return result;//Return the sum
    }
   
}

