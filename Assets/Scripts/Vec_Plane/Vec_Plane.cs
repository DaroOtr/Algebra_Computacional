using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMath
{
    public struct Vec_Plane 
    {
        internal const int size = 16;

        private Vec3 p_Normal;

        private float p_Distance;

        public Vec3 va;
        public Vec3 vb;
        public Vec3 vc;


        //The direction in which the plane is pointing
        public Vec3 normal
        {
            get
            {
                return p_Normal;
            }
            set
            {
                p_Normal = value;
            }
        }

        // The distance from 00 of the world to the plane
        public float distance
        {
            get
            {
                return p_Distance;
            }
            set
            {
                p_Distance = value;
            }
        }

        // flip the plane
        public Vec_Plane flipped => new Vec_Plane(-p_Normal, 0f - p_Distance);

        public Vec_Plane(Vec3 inNormal, Vec3 inPoint)
        {
            p_Normal = inNormal.normalized;
            p_Distance = 0f - Vec3.Dot(p_Normal, inPoint);
            va = Vec3.Zero; vb = Vec3.Zero; vc = Vec3.Zero;
        }

        public Vec_Plane(Vec3 inNormal, float d)
        {
            p_Normal = inNormal.normalized;
            p_Distance = d;
            va = Vec3.Zero; vb = Vec3.Zero; vc = Vec3.Zero;
        }

        public Vec_Plane(Vec3 a, Vec3 b, Vec3 c)
        {
            va = a; vb = b; vc = c;
            Vec3 aux = Vec3.Cross(b - a, c - a);
            p_Normal = aux.normalized;
            p_Distance = 0f - Vec3.Dot(p_Normal, a);
        }

        public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
        {
            p_Normal = inNormal.normalized;
            p_Distance = 0f - Vec3.Dot(inNormal, inPoint);
        }

        public void Set3Points(Vec3 a, Vec3 b, Vec3 c)
        {
            Vec3 aux = Vec3.Cross(b - a, c - a);
            p_Normal = aux.normalized;
            p_Distance = 0f - Vec3.Dot(p_Normal, a);
        }

        public void Flip()
        {
            p_Normal = -p_Normal;
            p_Distance = 0f - p_Distance;
        }

        public void Translate(Vec3 translation)
        {
            p_Distance += Vec3.Dot(p_Normal, translation);
        }

        public static Vec_Plane Translate(Vec_Plane plane, Vec3 translation)
        {
            return new Vec_Plane(plane.p_Normal, plane.p_Distance += Vec3.Dot(plane.p_Normal, translation));
        }

        public Vec3 ClosestPointOnPlane(Vec3 point)
        {
            float num = Vec3.Dot(p_Normal, point) + p_Distance;
            return point - p_Normal * num;
        }

        public float GetDistanceToPoint(Vec3 point)
        {
            return Vec3.Dot(p_Normal, point) + p_Distance;
        }

        public bool GetSide(Vec3 point)
        {
            return Vec3.Dot(p_Normal, point) + p_Distance > 0f;
        }

        public bool SameSide(Vec3 inPt0, Vec3 inPt1)
        {
            float distanceToPoint = GetDistanceToPoint(inPt0);
            float distanceToPoint2 = GetDistanceToPoint(inPt1);
            return (distanceToPoint > 0f && distanceToPoint2 > 0f) || (distanceToPoint <= 0f && distanceToPoint2 <= 0f);
        }

        public void DrawPlane(Color p_Color, Color r_Color)
        {
            Vec3 position = this.p_Normal * this.p_Distance;
            Vec3 normal = this.normal;

            Vector3 v3;
            if (normal.normalized != Vector3.forward)
                v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
            else
                v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;
            var corner0 = position + v3;
            var corner2 = position - v3;
            var q = Quaternion.AngleAxis(90.0f, normal);
            v3 = q * v3;
            var corner1 = position + v3;
            var corner3 = position - v3;
            Debug.DrawLine(corner0, corner2, p_Color);
            Debug.DrawLine(corner1, corner3, p_Color);
            Debug.DrawLine(corner0, corner1, p_Color);
            Debug.DrawLine(corner1, corner2, p_Color);
            Debug.DrawLine(corner2, corner3, p_Color);
            Debug.DrawLine(corner3, corner0, p_Color);
            Debug.DrawRay(position, normal, r_Color);
        }
    }
}
