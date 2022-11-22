using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm
{
    List<GameObject> go_parts;
    List<Matrix4x4> m_locations;
    List<Matrix4x4> m_scales;
    List<Matrix4x4> m_rotations;
    Vector3[] v3_originals;

    float rotForearm;
    float dir;

    float dirY;
    float deltaY;
    float rotY;

    enum Parts
    {
        RP_SHOULDER, RP_ARM, RP_FOREARM, RP_HAND
    }

    // Start is called before the first frame update
    public void CreateArm(float x)
    {
        rotY = 0;
        deltaY = 0.1f;
        dirY = -1f;

        dir = x;
        rotForearm = 10f * dir;

        go_parts = new List<GameObject>();
        m_scales = new List<Matrix4x4>();
        m_locations = new List<Matrix4x4>();
        m_rotations = new List<Matrix4x4>();

        // SHOULDER
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_SHOULDER].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        go_parts[(int)Parts.RP_SHOULDER].name = "SHOULDER";
        go_parts[(int)Parts.RP_SHOULDER].GetComponent<BoxCollider>().enabled = false;
        v3_originals = go_parts[(int) Parts.RP_SHOULDER].GetComponent<MeshFilter>().mesh.vertices;
        m_scales.Add(Transforms.Scale(0.4f, 0.4f, 0.4f));
        m_locations.Add(Transforms.Translate((0.6f + 0.2f) * x, 0.5f + 0.75f, 0));
        m_rotations.Add(Transforms.RotateZ(0));
        // ARM
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_ARM].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        go_parts[(int)Parts.RP_ARM].name = "ARM";
        go_parts[(int)Parts.RP_ARM].GetComponent<BoxCollider>().enabled = false;
        m_scales.Add(Transforms.Scale(0.5f, 0.4f, 0.4f));
        m_locations.Add(Transforms.Translate((0.2f + 0.25f) * x , 0, 0));
        m_rotations.Add(Transforms.RotateY(0));
        // FOREARM
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_FOREARM].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        go_parts[(int)Parts.RP_FOREARM].name = "FOREARM";
        go_parts[(int)Parts.RP_FOREARM].GetComponent<BoxCollider>().enabled = false;
        m_scales.Add(Transforms.Scale(0.3f, 0.4f, 0.4f));
        m_locations.Add(Transforms.Translate((0.25f + 0.15f) * x , 0, 0f));
        m_rotations.Add(Transforms.RotateY(rotForearm));
        // HAND
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_HAND].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
        go_parts[(int)Parts.RP_HAND].name = "HAND";
        go_parts[(int)Parts.RP_HAND].GetComponent<BoxCollider>().enabled = false;
        m_scales.Add(Transforms.Scale(0.3f, 0.5f, 0.4f));
        m_locations.Add(Transforms.Translate((0.15f + 0.15f) * x, 0, 0f));
        m_rotations.Add(Transforms.RotateY(0));
    }

    public void UpdateArm()
    {
        rotY += deltaY * dirY;
        if(rotY <= -10f || rotY >= 10f) dirY = -dirY;

        Matrix4x4 accumT = Matrix4x4.identity; // Para heredar transformaciones en la jerarquia (execepto escala)
        for (int i = 0; i < go_parts.Count; i++)
        {
            Matrix4x4 m = accumT * m_locations[i] * m_rotations[i] * m_scales[i];

            if(i == (int)Parts.RP_SHOULDER)
            {
                Matrix4x4 t = Transforms.Translate((1.2f / 2f + 0.4f / 2f) * dir, 0.5f + 0.75f, 0);
                Matrix4x4 r1 = Transforms.RotateY(rotY);
                Matrix4x4 r2 = Transforms.RotateZ(-90 * dir);
                Matrix4x4 r3 = Transforms.RotateY(rotY);
                m = accumT * r1 * t * r2 * r3 * m_scales[i];
                accumT *= r1 * t * r2 * r3;
            }
            else if(i == (int)Parts.RP_FOREARM)
            {
                Matrix4x4 t1 = Transforms.Translate((0.5f / 2f) * dir, 0, 0);
                Matrix4x4 r = Transforms.RotateY(rotForearm);
                Matrix4x4 t2 = Transforms.Translate((0.3f / 2f) * dir, 0, 0);
                m = accumT * t1 * r * t2 * m_scales[i];
                accumT *= t1 * r * t2;
            }
            else
            {
                accumT *= m_locations[i] * m_rotations[i];

            }
            go_parts[i].GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform(m,v3_originals);
        }
    }
}
