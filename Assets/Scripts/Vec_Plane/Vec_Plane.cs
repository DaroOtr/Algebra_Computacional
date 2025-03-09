using UnityEngine;

namespace CustomMath
{
    /// <summary>
    /// Un plano es un objeto matematico compuesto por una sucecion infinita de rectas que dividen el espacio en 2
    /// </summary>
    public struct Vec_Plane 
    {
        
        // El tema de las coliciones tambien pasa por aca

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
        public Vec_Plane flipped => new Vec_Plane(-p_Normal, - p_Distance);

        public Vec_Plane(Vec3 inNormal, Vec3 inPoint)
        {
            /*
             * Solo puede pasar un plano que pase por este punto y la normal este a 90 grados
             */
            p_Normal = inNormal.normalized;
            p_Distance = - Vec3.Dot(p_Normal, inPoint);
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
            /*
             * Teniendo 3 puntos sabemos que solo 1 plano puede pasar por estos mismos al mismo tiempo
             * Con estos 3 puntos se busca un vector perpendicular a los vectores de (b - a y c - a) simultaneamente
             * este mismo vector es la normal del plano (la cara positiva)
             * Y despues se hace el producto punto para calcular la distancia desde el 00 del mundo al plano
             *
             * Si quiero dar vuelta la normal puedo pasarle los puntos al revez para que de esta manera la normal del plano quede al revez
             */
            
            va = a; vb = b; vc = c;
            Vec3 aux = Vec3.Cross(b - a, c - a);
            p_Normal = aux.normalized;
            p_Distance = - Vec3.Dot(p_Normal, a);
        }

        public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
        {
            p_Normal = inNormal.normalized;
            p_Distance = - Vec3.Dot(inNormal, inPoint);
        }

        public void Set3Points(Vec3 a, Vec3 b, Vec3 c)
        {
            Vec3 aux = Vec3.Cross(b - a, c - a);
            p_Normal = aux.normalized;
            p_Distance = - Vec3.Dot(p_Normal, a);
        }

        public void Flip()
        {
            p_Normal = -p_Normal;
            p_Distance = - p_Distance;
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
            /*
             * Devuelve el punto mas cercano del plano hacia este punto
             * p_Normal * num devuelve el punto mas cercano dentro del plano
             */
            float num = GetDistanceToPoint(point);
            return point - p_Normal * num;
        }

        public float GetDistanceToPoint(Vec3 point)
        {
            /*
             * Devuelve la distancia que hay entre el punto y al plano + la distancia del plano hacia el origen del mundo
             */
            return Vec3.Dot(p_Normal, point) + p_Distance;
        }

        public bool GetSide(Vec3 point)
        {
            /*
             * si la distancia es + esta del lado positivo del plano (Direccion donde apunta la normal)
             * si la distancia es - estas del lado negativo del plano (Direccion contratria a la normal del plano)
             */
            return GetDistanceToPoint(point) > 0f;
        }

        public bool SameSide(Vec3 inPt0, Vec3 inPt1)
        {
            /*
             * Esta funcion chequea si ambos puntos estan del mismo lado del plano
             * Si estan en lados opuestos devuelve falso, sino es verdadero
             */
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
