using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class Collision_Manager : MonoBehaviour
{
    public List<Vec_MeshColider> objs_Mesh;
    public Material collMaterial;
    public Material normalMaterial;
    void Update()
    {
        foreach (Vec3 item in objs_Mesh[0].p_Inside_Mesh)
        {
            if (objs_Mesh[1].IsPointColliding(item))
            {
                objs_Mesh[0].gameObject.GetComponent<MeshRenderer>().material = collMaterial;
                objs_Mesh[1].gameObject.GetComponent<MeshRenderer>().material = collMaterial;
            }
            else
            {
                objs_Mesh[0].gameObject.GetComponent<MeshRenderer>().material = normalMaterial;
                objs_Mesh[1].gameObject.GetComponent<MeshRenderer>().material = normalMaterial;
            }
        }
    }
}