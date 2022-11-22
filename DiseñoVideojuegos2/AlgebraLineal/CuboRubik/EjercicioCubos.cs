using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjercicioCubos : MonoBehaviour
{
    List<GameObject> cubes;
    List<Vector3> positions;
    List<Matrix4x4> matrices;
    List<Matrix4x4> mOriginales;
    Vector3[] originales;
    float rotZ;
    float rotY;

    // Start is called before the first frame update
    void Start()
    {
        rotZ = 0f;
        rotY = 0f;
        cubes = new List<GameObject>();
        positions = new List<Vector3>();
        mOriginales = new List<Matrix4x4>();
        matrices = new List<Matrix4x4>();
        for (int i = 0; i < 8; i++)
        {
            cubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        }

        float s = 1.0f;
        float h = s/2.0f;
        positions.Add(new Vector3(-h, -h, h));
        positions.Add(new Vector3(h, -h, h));
        positions.Add(new Vector3(h, h, h));
        positions.Add(new Vector3(-h, h, h));
        positions.Add(new Vector3(h, h, -h));
        positions.Add(new Vector3(-h, h, -h));
        positions.Add(new Vector3(h, -h, -h));
        positions.Add(new Vector3(-h, -h, -h));

        originales = cubes[0].GetComponent<MeshFilter>().mesh.vertices;
        
        for (int i = 0; i < cubes.Count; i++)
        {
            mOriginales.Add(Transforms.Translate(positions[i].x, positions[i].y, positions[i].z));
            matrices.Add(mOriginales[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rotZ < 360f)
        {
            rotZ += 1f;
            for(int i = 4; i < 8; i++)
                matrices[i] = Transforms.RotateZ(rotZ) * mOriginales[i];
        }
        else if (rotY < 360f)
        {
            rotY += 1f;
            for(int i = 2; i < 6; i++)
                matrices[i] = Transforms.RotateY(rotY) * mOriginales[i];
        }
        else
        {
            rotZ = rotY = 0f;
            for(int i = 0; i < 8; i++)
                matrices[i] = mOriginales[i];
        }

        for (int i = 0; i < cubes.Count; i++)
            cubes[i].GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform(matrices[i], originales);
    }
}
