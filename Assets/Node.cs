using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public int Grid_xPos;
    public int Grid_yPos;

    //cost for next node(square)
    public int G_Cost;
    //distance for next goal
    public int H_Cost;

    //check the grid if there is a wall
    public bool isWall_;

    //To save node from previously came from to find shortest path for AStar algorithm
    public Node PrevNode;
    //World position of node
    public Vector3 nodePosition;

    //Getter for F_Cost
    public int F_Cost { get { return G_Cost + H_Cost; } }

    //Node Constructor
    public Node(bool isWall, Vector3 Pos, int grid_X, int grid_Y)
    {
        isWall_ = isWall;
        nodePosition = Pos;
        Grid_xPos = grid_X;
        Grid_yPos = grid_Y;
    }
}
