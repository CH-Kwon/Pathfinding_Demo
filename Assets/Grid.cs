using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [HideInInspector]
    //public instance for Grid script to access on the other script
    public static Grid instance;

    private float node_Diameter;//Twice the amount of the radius (Set in the start function)
    //X,Y size of the grid
    private int x_GridSize;
    private int y_GridSize;

    public float node_Radius;
    public float node_Distance;

    public Node[,] nodeArr;
    //The final path node list
    public List<Node> FinalPath;
                         
    //Layer mask to check a wall
    public LayerMask wallMask;
    //2D Grid to save X and Y size
    public Vector2 gridMap;

    private void Start()//Ran once the program starts
    {
        instance = this;
        InitGridandNode();
        MakeGrid(); 
    }

    private void InitGridandNode()
    {
        node_Diameter = (node_Radius * 2);
        //Get X and Y size of grid by dividing each size of node 
        int GridX = Mathf.RoundToInt(gridMap.x / node_Diameter);
        int GridY = Mathf.RoundToInt(gridMap.y / node_Diameter);

        x_GridSize = GridX;
        y_GridSize = GridY;
    }

    public void MakeGrid()
    {
        nodeArr = new Node[x_GridSize, y_GridSize];//Declare the array of nodes.

        //Calculation of left bottom world position : in the middle postiion of grid, then minus each half size of x and y size.
        Vector3 leftBottom = transform.position - (Vector3.right * gridMap.x * 0.5f) 
                                                - (Vector3.forward * gridMap.y * 0.5f);

        //Looping x and y in the Grid
        for (int x = 0; x < x_GridSize; x++)
        {
            for (int y = 0; y < y_GridSize; y++)
            {
                //Get worldpoint of nodes from left bottom by adding x and y value of radius and diameter
                Vector3 worldPoint = leftBottom + Vector3.right * (x * node_Diameter + node_Radius) + 
                                                  Vector3.forward * (y * node_Diameter + node_Radius);
                bool Wall = true;

                //Collion check for wall when looping all nodes in the grid
                if (Physics.CheckSphere(worldPoint, node_Radius, wallMask))
                {
                    Wall = false;//Object is not a wall
                }

                //Assign new nodes in node array
                nodeArr[x, y] = new Node(Wall, worldPoint, x, y);//Create a new node in the array.
            }
        }
    }

    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        //X and Y position value of neighbor nodes for left,right,bottom,top
        int check_Xpos;
        int check_Ypos;

        //All neighbor node list
        List<Node> neighborNodeList = new List<Node>();

        //Rightside
        check_Xpos = a_NeighborNode.Grid_xPos + 1; 
        check_Ypos = a_NeighborNode.Grid_yPos;

        CalculationNeighbor(neighborNodeList, check_Xpos, check_Ypos);

        //LeftSide
        check_Xpos = a_NeighborNode.Grid_xPos - 1;
        check_Ypos = a_NeighborNode.Grid_yPos;
        CalculationNeighbor(neighborNodeList, check_Xpos, check_Ypos);
 
        //TopSide
        check_Xpos = a_NeighborNode.Grid_xPos;
        check_Ypos = a_NeighborNode.Grid_yPos + 1;
        CalculationNeighbor(neighborNodeList, check_Xpos, check_Ypos);

        //BottomSide
        check_Xpos = a_NeighborNode.Grid_xPos;
        check_Ypos = a_NeighborNode.Grid_yPos - 1;
        CalculationNeighbor(neighborNodeList, check_Xpos, check_Ypos);

        return neighborNodeList;//Return the neighbors list.
    }

    public void CalculationNeighbor(List<Node> nodeList, int xVal, int yVal)
    {
        if (xVal >= 0 && xVal < x_GridSize)
        {
            //check if node is in y grid range
            if (yVal >= 0 && yVal < y_GridSize)
            {
                //Assign neighbor node
                nodeList.Add(nodeArr[xVal, yVal]);
            }
        }
    }

    //Get close node in the world point
    public Node WorldNode(Vector3 worldPos)
    {
        float xPos = ((worldPos.x + gridMap.x / 2) / gridMap.x);
        float yPos = ((worldPos.z + gridMap.y / 2) / gridMap.y);

        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);

        int x = Mathf.RoundToInt((x_GridSize - 1) * xPos);
        int y = Mathf.RoundToInt((y_GridSize - 1) * yPos);

        return nodeArr[x, y];
    }


    //Drawing Gizmo function
    private void OnDrawGizmos()
    {
        //Drawing Cube of all nodes in grid
        Gizmos.DrawWireCube(transform.position, new Vector3(gridMap.x, 1, gridMap.y));

        //If grid is not empty
        if (nodeArr != null)
        {
            //Looping every node 
            foreach (Node n in nodeArr)
            {
                //check wall
                if (n.isWall_)
                {
                    //If it is wall, set color
                    Gizmos.color = Color.black;
                }
                else
                {
                    //If it is not a wall, set color
                    Gizmos.color = Color.red;
                }

                if (FinalPath != null)
                {
                    //Check current node is in final path
                    if (FinalPath.Contains(n))
                    {
                        //set color
                        Gizmos.color = Color.green;

                    }
                    //Render node at the position of node
                    Gizmos.DrawCube(n.nodePosition, Vector3.one * (node_Diameter - node_Distance));
                }
            }
        }
    }
}

