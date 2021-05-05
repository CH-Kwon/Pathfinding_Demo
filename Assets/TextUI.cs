using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    //Texts which indicate the value of nodes and targets
    public Text TotalNodes;
    public Text TotalTargets;
    public Text CurrentTarget;
    public Text NumberOfCurNode;
    public Text CurNode;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TotalNodes.text = "Number of Total Node : " + Pathfinding.instance.allNodes.Count.ToString();
        TotalTargets.text = "Number of Total Target : " + SpawnTarget.instance.newTargetList.Count.ToString();
        CurrentTarget.text = "Current Target : " + Move.instance.pathIndex.ToString();
        NumberOfCurNode.text = "Number of Current Node :  " + Pathfinding.instance.TempFinalPath.Count.ToString();
        CurNode.text = "Current Node : " + Move.instance.current.ToString();
    }
}
