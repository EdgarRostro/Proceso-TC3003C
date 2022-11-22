using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    GameObject C1;
    GameObject C2;
    GameObject C3;

    Vector3[] vCube;
    float rotZ;
    bool bajando;

    // Start is called before the first frame update
    void Start()
    {
        rotZ = 0;
        bajando = false;
        C1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        C2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        C3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        vCube = C1.GetComponent<MeshFilter>().mesh.vertices;
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 t = Transforms.Translate(0.5f, 0, 0);
        Matrix4x4 t2 = Transforms.Translate(1f, 0, 0);
        // Matrix4x4 t3 = Transforms.Translate(2.5f, 0, 0);
        Matrix4x4 s = Transforms.Scale(1f, 0.5f, 0.5f);
        Matrix4x4 r;

        if(rotZ <= 45 && !bajando)
        {
            rotZ += 1;
            r = Transforms.RotateZ(rotZ);
        }
        else if(rotZ >= -45 && bajando)
        {
            rotZ -= 1;
            r = Transforms.RotateZ(rotZ);
        }
        else
        {
            r = Transforms.RotateZ(rotZ);
            bajando = !bajando;
        }

        C1.GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform( r * t * s, vCube);
        C2.GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform( r * t * t * r * t * s, vCube);
        C3.GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform( r * t * t * r * t * t * r * t * s, vCube);

    }
}
