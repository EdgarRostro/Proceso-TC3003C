using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    Robot robot;
    // Start is called before the first frame update
    public void Start()
    {
        robot = new Robot();
        robot.Create();
    }

    // Update is called once per frame
    public void Update()
    {
        robot.Move();
    }
}
