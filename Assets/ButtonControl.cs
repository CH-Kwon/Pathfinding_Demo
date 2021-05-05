using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    //public instance for ButtonControl script to access on the other script
    public static ButtonControl instance;
    //Button status to play the program
    public enum ButtonOption {SET_START, SET_TARGET, DEFAULT, DELETE_START, DELETE_TARGET, DELETE_LINE };
    public ButtonOption buttonOption;
    public bool startFindPath;

    public void Start()
    {
        instance = this;
        buttonOption = ButtonOption.DEFAULT;
    }

    public void SetStart()
    {
       buttonOption = ButtonOption.SET_START;     
    }

    public void SetTarget()
    {
        buttonOption = ButtonOption.SET_TARGET;
    }

    public void DeleteStart()
    {
        buttonOption = ButtonOption.DELETE_START;

        if (buttonOption == ButtonOption.DELETE_START)
        {
            GameObject.Destroy(GameObject.FindGameObjectWithTag("StartPoint"));
            SpawnTarget.instance.isSetStart = false;
        }
    }


public void DeleteTarget()
    {
        buttonOption = ButtonOption.DELETE_TARGET;

        if (buttonOption == ButtonOption.DELETE_TARGET)
        {
            int targetObjCount = SpawnTarget.instance.newTargetList.Count;

            if (targetObjCount != 0)
            {           
                //Destroy target object one by one from last one in the list
               Destroy(SpawnTarget.instance.newTargetList[SpawnTarget.instance.newTargetList.Count-1].gameObject);
                SpawnTarget.instance.newTargetList.RemoveAt(targetObjCount - 1);

                //If targets are deleted completely, no longer delete them
                if(SpawnTarget.instance.newTargetList.Count == 0)
                {
                    SpawnTarget.instance.isSetTarget = false;
                }
            }
        }
    }

 
    public void Movement()
    {
        //toggle button for start pathfinding
        buttonOption = ButtonOption.DEFAULT;
        startFindPath = true;
        Move.instance.speed = 5f;
    }
}
