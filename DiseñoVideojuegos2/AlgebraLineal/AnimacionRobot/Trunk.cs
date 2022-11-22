using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk
{
    static List<GameObject> go_parts;
    static List<Matrix4x4> m_locations;
    static List<Matrix4x4> m_scales;
    static List<Matrix4x4> m_rotations;
    static Vector3[] v3_originals;

    static float deltaY; // Cuanto se movera entre frames
    static float dirY; // Direccion del mmovimiento +1 o -1
    static float rotY;

    enum Parts
    {
        RP_HEAP, RP_TORSO, RP_CHEST, RP_NECK, RP_HEAD
    }

    // Create the robot body
    public static void CreateTrunk()
    {
        rotY = 0f;
        dirY = -1f;
        deltaY = 0.1f;
        go_parts = new List<GameObject>();
        m_scales = new List<Matrix4x4>();
        m_locations = new List<Matrix4x4>();
        m_rotations = new List<Matrix4x4>();

        // HEAP
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_HEAP].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.gray);
        go_parts[(int)Parts.RP_HEAP].name = "HEAP";
        go_parts[(int)Parts.RP_HEAP].GetComponent<BoxCollider>().enabled = false;
        v3_originals = go_parts[(int) Parts.RP_HEAP].GetComponent<MeshFilter>().mesh.vertices;
        m_scales.Add(Transforms.Scale(1, 0.5f, 1));
        m_locations.Add(Transforms.Translate(0, 0, 0));
        m_rotations.Add(Transforms.RotateY(0));
        // TORSO
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_TORSO].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        go_parts[(int)Parts.RP_TORSO].name = "TORSO";
        go_parts[(int)Parts.RP_TORSO].GetComponent<BoxCollider>().enabled = false;
        m_scales.Add(Transforms.Scale(1, 0.75f, 1));
        m_locations.Add(Transforms.Translate(0, 0.25f + 0.75f / 2.0f, 0));
        m_rotations.Add(Transforms.RotateY(0));
        // CHEST
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_CHEST].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        go_parts[(int)Parts.RP_CHEST].name = "CHEST";
        go_parts[(int)Parts.RP_CHEST].GetComponent<BoxCollider>().enabled = false;
        m_scales.Add(Transforms.Scale(1.2f, 0.4f, 1.2f));
        m_locations.Add(Transforms.Translate(0, 0.75f / 2f + 0.2f, 0));
        m_rotations.Add(Transforms.RotateY(0));
        // NECK
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_NECK].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        go_parts[(int)Parts.RP_NECK].name = "NECK";
        go_parts[(int)Parts.RP_NECK].GetComponent<BoxCollider>().enabled = false;
        m_scales.Add(Transforms.Scale(0.1f, 0.1f, 0.2f));
        m_locations.Add(Transforms.Translate(0f, 0.2f + 0.1f/2f, 0f));
        m_rotations.Add(Transforms.RotateY(0));
        // HEAD
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_HEAD].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
        go_parts[(int)Parts.RP_HEAD].name = "HEAD";
        go_parts[(int)Parts.RP_HEAD].GetComponent<BoxCollider>().enabled = false;
        m_scales.Add(Transforms.Scale(0.5f, 0.5f, 0.5f));
        m_locations.Add(Transforms.Translate(0f, 0.1f/2f + 0.5f/2f, 0f));
        m_rotations.Add(Transforms.RotateY(0));
    }

    // Update is called once per frame
    public static void UpdateTrunk()
    {
        rotY += deltaY * dirY;
        if(rotY <= -10f || rotY >= 10f) dirY = -dirY;

        Matrix4x4 accumT = Matrix4x4.identity; // Para heredar transformaciones en la jerarquia (execepto escala)
        for (int i = 0; i < go_parts.Count; i++)
        {
            Matrix4x4 m = accumT * m_locations[i] * m_rotations[i] * m_scales[i];
            if(i == (int) Parts.RP_CHEST)
            {
                Matrix4x4 t1 = Transforms.Translate(0, 0.75f / 2f, 0);
                Matrix4x4 r1 = Transforms.RotateY(rotY);
                Matrix4x4 t2 = Transforms.Translate(0, 0.4f / 2f, 0);
                m = accumT * t1 * r1 * t2 * m_scales[i];
                accumT *= t1 * r1 * t2;
            }
            else
            {
                accumT *= m_locations[i] * m_rotations[i];
            }
            go_parts[i].GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform(m,v3_originals);
        }
    }
}
