using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transforms 
{
    public static Matrix4x4 Translate(float x, float y, float z){
        Matrix4x4 t = Matrix4x4.identity;
        t[0,3] = x;
        t[1,3] = y;
        t[2,3] = z;
        return t;
    }

    public static Matrix4x4 Scale(float x, float y, float z){
        Matrix4x4 t = Matrix4x4.identity;
        t[0,0] = x;
        t[1,1] = y;
        t[2,2] = z;
        return t;
    }

    public static Matrix4x4 RotateX(float a){
        float rad = Mathf.Deg2Rad * a;
        Matrix4x4 t = Matrix4x4.identity;
        t[1,1] = Mathf.Cos(rad);
        t[1,2] = -Mathf.Sin(rad);
        t[2,1] = Mathf.Sin(rad);
        t[2,2] = Mathf.Cos(rad);
        return t;
    }

    public static Matrix4x4 RotateY(float a){
        float rad = Mathf.Deg2Rad * a;
        Matrix4x4 t = Matrix4x4.identity;
        t[0,0] = Mathf.Cos(rad);
        t[0,2] = Mathf.Sin(rad);
        t[2,0] = -Mathf.Sin(rad);
        t[2,2] = Mathf.Cos(rad);
        return t;
    }

    public static Matrix4x4 RotateZ(float a){
        float rad = Mathf.Deg2Rad * a;
        Matrix4x4 t = Matrix4x4.identity;
        t[0,0] = Mathf.Cos(rad);
        t[0,1] = -Mathf.Sin(rad);
        t[1,0] = Mathf.Sin(rad);
        t[1,1] = Mathf.Cos(rad);
        return t;
    }

    public static Vector3[] Transform(Matrix4x4 m, Vector3[] vs)
    {
        // Para almacenar coleccion de vertices transformados
        Vector3[] newVerts = new Vector3[vs.Length];
        // Recorremos cada vertice
        for (int i = 0; i < vs.Length; i++)
        {
            Vector3 vi = vs[i];
            // Creamos un vector homogeneo, con w, 4 dimensiones
            Vector4 newV = new Vector4(vi.x, vi.y, vi.z, 1);
            // Aplicamos la transformacion mediante un producto 
            newV = m * newV;
            // Almacenamos el nuevo vector transformado vector 3 -> vector 4
            newVerts[i] = newV;
        }
        return newVerts;
    }

}
