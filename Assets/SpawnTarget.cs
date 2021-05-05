using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTarget : MonoBehaviour
{
    [HideInInspector]
    //public instance for SapwnTarget script to access on the other script
    public static SpawnTarget instance;
    public Vector3 clickPosition;

    //objects for start, target instantiation
    public GameObject targetObj;
    public GameObject startObj;

    public GameObject LineDrawer;

    //List for new target lists
    public List<GameObject> newTargetList;
    public List<LineRenderer> lineList;

    public Vector4 PickColor;
    public Vector3 scale;

    public bool isSetStart;
    public bool isSetTarget;

    private void Start()
    {
        instance = this;
        newTargetList = new List<GameObject>();
        lineList = new List<LineRenderer>();
        PickColor = new Vector4();

        isSetStart = false;
        isSetTarget = false;

        scale = new Vector3(0.7f, 0.7f, 0.7f);
    }
    private void Update()
    {
        CreateNewPoints();
    }

    //GetMouse Position in 3 dimension but calculating as 2D for only x,y axis to create a start point on the field
    private Vector3 GetMousePosition()
    {
        clickPosition = -Vector3.one;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            clickPosition = hit.point;
        }

        //Create click position only for x, z value
        Vector3 newClickPosition = new Vector3(clickPosition.x, 0.5f, clickPosition.z);
        
        return newClickPosition;
    }
    private void CreateNewPoints()
    { 
        //Create start object by clicking
       if (Input.GetMouseButtonDown(0))
       {
            //If click the "Set Start Position" button
            if (ButtonControl.instance.buttonOption == ButtonControl.ButtonOption.SET_START && !(isSetStart))
            {
                //Create start point as instantiate game object of cube
                GameObject startObject = Instantiate(startObj) as GameObject;
                Renderer newStartObjRenderer = startObj.GetComponent<Renderer>();

                //setting the transform information and tag to find start obj
                startObject.transform.position = GetMousePosition();
                startObject.transform.localScale = scale;                       
                startObject.tag = "StartPoint";

                isSetStart = true;
                
            }
            //If click the "Set Target Position" button
            if (ButtonControl.instance.buttonOption == ButtonControl.ButtonOption.SET_TARGET && !(isSetTarget))
            {             
                //Create target point as instantiate game objs of cube
                GameObject targetObjects = Instantiate(targetObj) as GameObject;
                Renderer newTargetObjRenderer = targetObjects.GetComponent<Renderer>();
              
                //setting the transform information, tag, and color value
                targetObjects.transform.position = GetMousePosition();
                targetObjects.transform.localScale = scale;
                PickColor = Random.ColorHSV(0.0f, 1f, 1f, 1f);
                newTargetObjRenderer.material.color = PickColor;
                
                targetObjects.tag = "NewTarget";

                GameObject renderLine = Instantiate(LineDrawer) as GameObject;
                LineRenderer lr = renderLine.GetComponent<LineRenderer>();
                             
                //Assign new target points in the list to find path for each points
                newTargetList.Add(targetObjects);
                lineList.Add(lr);
               
            }
       }    
    }

}


