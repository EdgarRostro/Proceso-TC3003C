using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg
{
    List<GameObject> go_parts;
    List<Matrix4x4> m_locations;
    List<Matrix4x4> m_scales;
    List<Matrix4x4> m_rotations;
    Vector3[] v3_originals;

    float dir;

    float dirX;
    float deltaX;
    float rotX;

    float dirX2;
    float deltaX2;
    float rotX2;

    enum Parts
    {
        RP_THIGH, RP_KNEE, RP_CALF, RP_FOOT
    }

    // Start is called before the first frame update
    public void CreateLeg(float x)
    {
        rotX = 0;
        deltaX = 0.4f;
        dirX = 1f;

        rotX2 = 0;
        deltaX2 = 0.4f;
        dirX2 = 1f;

        dir = x;

        go_parts = new List<GameObject>();
        m_scales = new List<Matrix4x4>();
        m_locations = new List<Matrix4x4>();
        m_rotations = new List<Matrix4x4>();

        // THIGH
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_THIGH].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        go_parts[(int)Parts.RP_THIGH].name = "THIGH";
        go_parts[(int)Parts.RP_THIGH].GetComponent<BoxCollider>().enabled = false;
        v3_originals = go_parts[(int) Parts.RP_THIGH].GetComponent<MeshFilter>().mesh.vertices;
        m_scales.Add(Transforms.Scale(0.3f, 1f, 0.3f));
        m_locations.Add(Transforms.Translate((0.6f - 0.3f) * x, -0.5f, 0));
        m_rotations.Add(Transforms.RotateZ(0));
        // KNEE
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_KNEE].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        go_parts[(int)Parts.RP_KNEE].name = "KNEE";
        go_parts[(int)Parts.RP_KNEE].GetComponent<BoxCollider>().enabled = false;
        m_scales.Add(Transforms.Scale(0.3f, 0.3f, 0.3f));
        m_locations.Add(Transforms.Translate(0, -0.5f - 0.15f, 0));
        m_rotations.Add(Transforms.RotateY(0));
        // CALF
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_CALF].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        go_parts[(int)Parts.RP_CALF].name = "CALF";
        go_parts[(int)Parts.RP_CALF].GetComponent<BoxCollider>().enabled = false;
        m_scales.Add(Transforms.Scale(0.3f, 0.5f, 0.3f));
        m_locations.Add(Transforms.Translate(0, -0.15f - 0.25f, 0f));
        m_rotations.Add(Transforms.RotateY(0));
        // FOOT
        go_parts.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        go_parts[(int)Parts.RP_FOOT].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
        go_parts[(int)Parts.RP_FOOT].name = "FOOT";
        go_parts[(int)Parts.RP_FOOT].GetComponent<BoxCollider>().enabled = false;
        m_scales.Add(Transforms.Scale(0.4f, 0.3f, 0.3f));
        m_locations.Add(Transforms.Translate(0, -0.25f - 0.15f, 0f));
        m_rotations.Add(Transforms.RotateY(0));
    }

    // Update is called once per frame
    public void UpdateLeg()
    {
        rotX += deltaX * dirX;
        if(rotX <= -40f || rotX >= 40f) dirX = -dirX;
        if(rotX2 >= 0 || rotX2 <= -40f) dirX2 = -dirX2;
        rotX2 += deltaX2 * dirX2;

        Matrix4x4 accumT = Matrix4x4.identity; // Para heredar transformaciones en la jerarquia (execepto escala)
        for (int i = 0; i < go_parts.Count; i++)
        {
            Matrix4x4 m = accumT * m_locations[i] * m_rotations[i] * m_scales[i];

            if(i == (int)Parts.RP_THIGH)
            {
                Matrix4x4 t1 = Transforms.Translate((0.6f - 0.3f) * dir, -0.5f, 0);
                Matrix4x4 r1 = Transforms.RotateX(rotX * dir);
                Matrix4x4 t2 = Transforms.Translate(0, - 0.5f / 2f, 0);
                m = accumT * t1 * r1 * t2 * m_scales[i];
                accumT *= t1 * r1 * t2;
            }
            else if(i == (int)Parts.RP_KNEE)
            {
                Matrix4x4 t1 = Transforms.Translate(0, -1f / 2f, 0);
                Matrix4x4 r1 = Transforms.RotateX(rotX2);
                Matrix4x4 t2 = Transforms.Translate(0, - 0.3f / 2f, 0);
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

