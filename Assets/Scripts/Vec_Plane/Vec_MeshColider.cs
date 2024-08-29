using CustomMath;
using System;
using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;



public class Vec_MeshColider : MonoBehaviour
{
    public int createdPlanes = 0;
    public bool showV_MeshColider;
    public List<Vec_Plane> m_planes;
    public List<Vec3> p_Inside_Mesh;
    public List<Vec3> pointsToCheck;
    public Mesh objMesh;


    public List<Vec3> colP;

    // Struct Ray Para simplificar los Chekeos de puntos
    struct Vec_Ray
    {
        public Vec3 origin;
        public Vec3 destination;

        public Vec_Ray(Vec3 origin, Vec3 destination) 
        {
            this.origin = origin;
            this.destination = destination;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        objMesh = GetComponent<MeshFilter>().mesh;
        p_Inside_Mesh = new List<Vec3>();
        pointsToCheck = new List<Vec3>();
        colP = new List<Vec3>();
        m_planes = new List<Vec_Plane>();
    }

    // Update is called once per frame
    void Update()
    {
        objMesh = GetComponent<MeshFilter>().mesh;
        m_planes.Clear();
        createdPlanes = 0;

        // i += 3 Por que Recorremos los puntos de 3 en 3 Para conseguir los vertices de la mesh
        // Creacion de planos
        for (int i = 0; i < objMesh.GetIndices(0).Length; i += 3)
        {
            Vec3 v1 = new Vec3(transform.TransformPoint(objMesh.vertices[objMesh.GetIndices(0)[i]]));
            Vec3 v2 = new Vec3(transform.TransformPoint(objMesh.vertices[objMesh.GetIndices(0)[i + 1]]));
            Vec3 v3 = new Vec3(transform.TransformPoint(objMesh.vertices[objMesh.GetIndices(0)[i + 2]]));

            Vec_Plane plane = new Vec_Plane(v1, v2, v3);
            plane.normal *= -1;
            //plane.Flip();
            m_planes.Add(plane);
            createdPlanes++;
        }

        // Por las dudas
        //foreach (var item in m_planes)
        //{
        //    item.Flip();
        //}


        // Guadamos Todos los puntos de la grilla para chekearlos despues con la mesh

        pointsToCheck.Clear();

        for (int x = 0; x < Vec_Grid.v_Grid.GetLength(0); x++)
        {
            for (int y = 0; y < Vec_Grid.v_Grid.GetLength(1); y++)
            {
                for (int z = 0; z < Vec_Grid.v_Grid.GetLength(2); z++)
                {
                    pointsToCheck.Add(Vec_Grid.v_Grid[x, y, z]);
                }
            }
        }

        p_Inside_Mesh.Clear();
        colP.Clear();
        // Chekea los puntos y si estan dentro de la mesh los agrega a la lista de p_Inside_Mesh
        foreach (var point in pointsToCheck)
        {
            Vec3 direction = Vec3.Forward * 10f;
            //int sl_Dir = UnityEngine.Random.Range(1,7);
            //Debug.Log("sl_Dir " + sl_Dir);
            //switch (sl_Dir)
            //{
            //    case 1:
            //        direction = Vec3.Up * 10f;
            //        break;
            //    case 2:
            //        direction = Vec3.Down * 10f;
            //
            //        break;
            //    case 3:
            //        direction = Vec3.Left * 10f;
            //        break;
            //    case 4:
            //        direction = Vec3.Right * 10f;
            //        break;
            //    case 5:
            //        direction = Vec3.Forward * 10f;
            //        break;
            //    case 6:
            //        direction = Vec3.Back * 10f;
            //        break;
            //}

            // Hacer un random para la direccion del rayo 
            // En base a eso definir la direccion 
            // SI ALGO SALE MAL MALA TUYA 

            Vec_Ray ray = new Vec_Ray(point, direction);
            int counter = 0;
            
            foreach (var plane in m_planes)
            {
                if (IsPointInPlane(plane, ray, out Vec3 collisionPoint))
                {
                    if (IsValidPlane(plane, collisionPoint))
                    {
                        colP.Add(collisionPoint);
                        counter++;
                    }
                }
            }

            //Debug.Log("counter " + counter);

            if (counter % 2 == 1)
            {
                //Debug.Log("Point cord " + point);
                p_Inside_Mesh.Add(point);
            }
        }

        DrawMeshPlanes();
    }

    public bool IsPointColliding(Vec3 pointToCheck)
    {
        foreach (var item in p_Inside_Mesh)
        {
            if (pointToCheck == item)
            {
                return true;
            }
        }
        return false;
    }

    // http://www.jeffreythompson.org/collision-detection/tri-point.php
    // Triangle Point Collision

    // Arreglar Esto Que parece ser donde esta el problema 
    private bool IsValidPlane(Vec_Plane mesh_P, Vec3 point)
    {
        float x1 = mesh_P.va.x;
        float x2 = mesh_P.vb.x;
        float x3 = mesh_P.vc.x;
        
        float y1 = mesh_P.va.y;
        float y2 = mesh_P.vb.y;
        float y3 = mesh_P.vc.y;
        
        float px = point.x;
        float py = point.y;
        
        // get the area of the triangle
        float areaOrig = Math.Abs((x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1));
        
        // get the area of 3 triangles made between the point
        // and the corners of the triangle
        float area1 = Math.Abs((x1 - px) * (y2 - py) - (x2 - px) * (y1 - py));
        float area2 = Math.Abs((x2 - px) * (y3 - py) - (x3 - px) * (y2 - py));
        float area3 = Math.Abs((x3 - px) * (y1 - py) - (x1 - px) * (y3 - py));
        
        // if the sum of the three areas equals the original,
        // we're inside the triangle!
        //if (area1 + area2 + area3 == areaOrig)
        //{
        //    return true;
        //}
        //return false;
        return Math.Abs(area1 + area2 + area3 - areaOrig) < Vec3.epsilon;
    }

    // Chekea Que punto del ray esta en el plano
    bool IsPointInPlane(Vec_Plane meshPlane, Vec_Ray ray, out Vec3 collisionPoint)
    {
        // Si la variable point Coliciona quiero que me devuelva donde coliciono 
        collisionPoint = Vec3.Zero;

        float denom = Vec3.Dot(meshPlane.normal, ray.destination);

        if (Mathf.Abs(denom) > Vec3.epsilon)
        {
            float t = Vec3.Dot((meshPlane.normal * meshPlane.distance - ray.origin), meshPlane.normal) / denom;
            if (t >= Vec3.epsilon)
            {
                collisionPoint = ray.origin + ray.destination * t;
                return true;
            }
        }
        return false;
    }

    void DrawMeshPlanes()
    {
        if (!showV_MeshColider)
            return;

        foreach (var plane in m_planes)
        {
            plane.DrawPlane(Color.green, Color.red);
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        { return; }

        //Gizmos.color = Color.red;
        //for (int i = 0; i < colP.Count; i++)
        //{
        //    Gizmos.DrawWireSphere(colP[i],0.1f);
        //}

        foreach (var item in pointsToCheck)
        {
            Gizmos.DrawRay(item,Vec3.Forward * 10f);
        }
    }
}
