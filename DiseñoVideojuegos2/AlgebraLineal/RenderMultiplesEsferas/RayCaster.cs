using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RayCaster : MonoBehaviour
{
    // Objects
    GameObject plane;
    Camera cam;

    GameObject lightGO;

    // Ilumination
    Vector3 Id;
    Vector3 Ia;
    Vector3 Is;

    // Constants
    List<Vector3> kd;
    List<Vector3> ka;
    List<Vector3> ks;

    // Alpha
    List<float> alpha;

    // Spheres
    int spheresCount;
    List<float> radius;
    List<Vector3> spheresCenters;

    // Background image
    public Texture2D background;

    // Render Texture
    Texture2D texture; 

    // Start is called before the first frame update
    void Start()
    {
        // Ilumination
        Id = new Vector3(0.8f, 0.8f, 1.0f);
        Ia = new Vector3(0.7f, 0.7f, 0.7f);
        Is = new Vector3(1.0f, 1.0f, 1.0f);

        // Constants
        kd = new List<Vector3>();
        ka = new List<Vector3>();
        ks = new List<Vector3>();

        // Alpha
        alpha = new List<float>();

        // Spheres
        spheresCount = 20;
        radius = new List<float>();
        spheresCenters = new List<Vector3>();

        // Plane
        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = new Vector3(0, 5, -2);
        plane.transform.rotation = Quaternion.Euler(90, 0, 0);
        plane.transform.localScale = new Vector3(1, 1, 1);

        // Camera
        cam = Camera.main;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.fieldOfView = 65;
        cam.nearClipPlane = 1;
        cam.farClipPlane = 20;
        cam.transform.position = new Vector3(0, 4, 5.5f);
        cam.transform.rotation = Quaternion.Euler(0, 0, 0);
        cam.transform.localScale = new Vector3(1, 1, 1);

        // Light

        // Search for lights existing on scene
        if (GameObject.FindObjectsOfType<Light>().Length != 0)
        {
            foreach (Light l in GameObject.FindObjectsOfType<Light>())
            {
                Destroy(l.gameObject);
            }
        }

        // Create a new light
        lightGO = new GameObject("PointLight");
        Light light = lightGO.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(Id.x, Id.y, Id.z);
        lightGO.transform.position = new Vector3(0, 7.5f, 3);
        light.intensity = 2.5f;

        // Texture
        texture = new Texture2D(640, 480, TextureFormat.ARGB32, false);

        // Create spheres
        CreateSpheres();

        // Sort spheres
        SortSpheres();

        // Render
        CreateRender();
    }

    // Create the spheres
    void CreateSpheres()
    {
        for(int i = 0; i < spheresCount; i++)
        {
            // Create sphere
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Set sphere position and store center
            sphere.transform.position = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(2.0f, 6.0f), Random.Range(8.0f, 10.0f));
            spheresCenters.Add(sphere.transform.position);

            // Set sphere scale and store radius
            float rad = Random.Range(0.1f, 0.35f);
            radius.Add(rad);
            sphere.transform.localScale = new Vector3(rad*2, rad*2, rad*2);

            // Set sphere color and store kd
            Vector3 tmpKd = new Vector3(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
            kd.Add(tmpKd);
            sphere.GetComponent<Renderer>().material.color = new Color(tmpKd.x, tmpKd.y, tmpKd.z);

            // Set sphere ks and store it
            ks.Add(new Vector3(tmpKd.x / 3.0f, tmpKd.y / 3.0f, tmpKd.z / 3.0f));
            sphere.GetComponent<Renderer>().material.SetColor("_SpecColor", new Color(ks[i].x, ks[i].y, ks[i].z));

            // Store ka
            ka.Add(new Vector3(tmpKd.x / 5.0f, tmpKd.y / 5.0f, tmpKd.z / 5.0f));

            // Store alpha
            alpha.Add(Random.Range(500.0f, 600.0f));

        }
    }

    // Sort spheres respect to center
    void SortSpheres()
    {
        // Sort spheres
        for(int i = 0; i < spheresCount; i++)
        {
            for(int j = 0; j < spheresCount - 1; j++)
            {
                if(spheresCenters[j].z < spheresCenters[j + 1].z)
                {
                    // Swap spheres centers
                    Vector3 tmpSphereCenter = spheresCenters[j];
                    spheresCenters[j] = spheresCenters[j + 1];
                    spheresCenters[j + 1] = tmpSphereCenter;

                    // Swap spheres radius
                    float tmpSphereRadius = radius[j];
                    radius[j] = radius[j + 1];
                    radius[j + 1] = tmpSphereRadius;

                    // Swap spheres kd
                    Vector3 tmpSphereKd = kd[j];
                    kd[j] = kd[j + 1];
                    kd[j + 1] = tmpSphereKd;

                    // Swap spheres ka
                    Vector3 tmpSphereKa = ka[j];
                    ka[j] = ka[j + 1];
                    ka[j + 1] = tmpSphereKa;

                    // Swap spheres ks
                    Vector3 tmpSphereKs = ks[j];
                    ks[j] = ks[j + 1];
                    ks[j + 1] = tmpSphereKs;

                    // Swap spheres alpha
                    float tmpSphereAlpha = alpha[j];
                    alpha[j] = alpha[j + 1];
                    alpha[j + 1] = tmpSphereAlpha;
                }
            }
        }
    }

    // Create Render texture
    void CreateRender()
    {
        // Copy background
        CopyBackground();
        
        for(int k = 0; k < spheresCount; k++)
        {
            for (int i = 0; i < 640; i++)
            {
                for (int j = 0; j < 480; j++)
                {
                    Vector3 c = Cast(i + 1, j + 1, k);
                    if(c != Vector3.zero)
                    {
                        texture.SetPixel(i, 479-j, new Color(c.x, c.y, c.z));
                    }
                }
            }
        }

        // Apply texture and settings
        texture.Apply();
        texture.filterMode = FilterMode.Point;

        Renderer rend = plane.GetComponent<Renderer>();
        Shader shader = Shader.Find("Unlit/Texture");
        
        rend.material.shader = shader;
        rend.material.mainTexture = texture;

        // Encode texture into PNG
        byte[] bytes = texture.EncodeToPNG();

        // Write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/../Assets/Images/render.png", bytes);
    }

    // Copy background image to texture
    void CopyBackground()
    {
        for(int i = 0; i < 640; i++)
        {
            for(int j = 0; j < 480; j++)
            {
                float u = i / 640.0f;
                float v = j / 480.0f;
                Color color = background.GetPixelBilinear(u, v);
                texture.SetPixel(i, j, color);
            }
        }
    }

    // Raycast
    Vector3 Cast(float w, float h, int sphereIndex)
    {
        cam = Camera.main;

        float frustrumHeight = 2.0f * cam.nearClipPlane * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustrumWidth = frustrumHeight * cam.aspect;
        float pixelWidth = frustrumWidth / 640f;
        float pixelHeight = frustrumHeight / 480f;
        Vector3 center = FindTopLeftFrustrumNear();
        center += +(pixelWidth / 2f) * cam.transform.right; 
        center -= (pixelHeight / 2f) * cam.transform.up;

        center += w * pixelWidth * cam.transform.right;
        center -= h * pixelHeight * cam.transform.up;

        Vector3 u = (center - cam.transform.position).normalized;
        Vector3 o = cam.transform.position;
        Vector3 c = spheresCenters[sphereIndex];
        float r = radius[sphereIndex];
        float a = Mathf.Pow(Vector3.Dot(u, o - c), 2) - (Vector3.Dot(o - c, o - c) - Mathf.Pow(r, 2));

        Vector3 x;

        Vector3 color;

        if(a < 0)
        {
            color = Vector3.zero;
        }
        else if(a == 0)
        {
            float d = -Vector3.Dot(u, o - c);
            x = o + (d * u);
            color = Illumination(x, sphereIndex);
        }
        else if (a > 0)
        {
            float dN = -Vector3.Dot(u, o - c) - Mathf.Sqrt(a);
            float dP = -Vector3.Dot(u, o - c) + Mathf.Sqrt(a);
            float d = Mathf.Min(dN, dP);
            x = o + (d * u);
            color = Illumination(x, sphereIndex);
        }
        else
        {
            color = Vector3.zero;
        }

        return color;
    }

    // Find illumination on given point
    Vector3 Illumination(Vector3 PoI, int sphereIndex)
    {
        Vector3 A = new Vector3(ka[sphereIndex].x * Ia.x, ka[sphereIndex].y * Ia.y, ka[sphereIndex].z * Ia.z);
        Vector3 D = new Vector3(kd[sphereIndex].x * Id.x, kd[sphereIndex].y * Id.y, kd[sphereIndex].z * Id.z);
        Vector3 S = new Vector3(ks[sphereIndex].x * Is.x, ks[sphereIndex].y * Is.y, ks[sphereIndex].z * Is.z);

        Vector3 n = PoI - spheresCenters[sphereIndex];

        Vector3 l = lightGO.transform.position - PoI;
        Vector3 v = cam.transform.position - PoI;
        float dotNuLu = Vector3.Dot(n.normalized, l.normalized);
        float dotNuL = Vector3.Dot(n.normalized, l);

        Vector3 lp = n * dotNuL;
        Vector3 lo = l - lp;
        Vector3 r = lp-lo;
        D *= dotNuLu;
        if(float.IsNaN(Mathf.Pow(Vector3.Dot(v.normalized,r.normalized),alpha[sphereIndex])))
        {
            S *= 0;
        }
        else
        {
            S *= Mathf.Pow(Vector3.Dot(v.normalized,r.normalized),alpha[sphereIndex]);
        }

        return A + D + S;
    }

    // Find top left frustrum near
    Vector3 FindTopLeftFrustrumNear()
    {
        Camera cam = Camera.main;
        //localizar camara
        Vector3 o = cam.transform.position;
        //mover hacia adelante
        Vector3 p = o + cam.transform.forward * cam.nearClipPlane;
        //obtener dimenciones del frustum
        float frustrumHeight = 2.0f * cam.nearClipPlane * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustrumWidth = frustrumHeight * cam.aspect;
        //mover hacia arriba, media altura
        p += cam.transform.up * frustrumHeight / 2.0f;
        //mover a la izquierda, medio ancho
        p -= cam.transform.right * frustrumWidth / 2.0f;

        return p;
    }
}
