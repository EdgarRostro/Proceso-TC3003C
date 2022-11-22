using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot
{   
    Arm armLeft;
    Arm armRight;

    Leg legLeft;
    Leg legRight;

    // Start is called before the first frame update
    public void Create()
    {
        armLeft = new Arm();
        armRight = new Arm();
        legLeft = new Leg();
        legRight = new Leg();

        Trunk.CreateTrunk();

        armLeft.CreateArm(1);
        armRight.CreateArm(-1);

        legLeft.CreateLeg(1);
        legRight.CreateLeg(-1);
    }

    // Update is called once per frame
    public void Move()
    {
        Trunk.UpdateTrunk();

        armLeft.UpdateArm();
        armRight.UpdateArm();
        
        legLeft.UpdateLeg();
        legRight.UpdateLeg();
    }
}
