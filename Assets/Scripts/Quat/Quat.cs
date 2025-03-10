using System;
using UnityEngine;

namespace CustomMath
{
    /// <summary>
    /// Un quaternion es la representacion de la rotacion de un objeto en un espacio tridimencional
    /// Esta compuesto de tres numero complejo (x,y,z) y un numero real (w)
    /// Cada complejo es un real y un imaginario
    /// La parte real del quaternion (w) es la distancia al origen del objeto
    /// </summary>
    [Serializable]
    public struct Quat
    {
        /*
         * i * j = k
         * j * k = i
         * k * i = j
         * j * i = - k
         * k * j = - i
         * i * k = - j
         */

        #region Variables

        public const float kEpsilon = 1E-06F;
        public float xq, yq, zq, wq;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor For A Quaternion
        /// </summary>
        /// <param name="xq"></param>
        /// <param name="yq"></param>
        /// <param name="zq"></param>
        /// <param name="wq"></param>
        public Quat(float xq, float yq, float zq, float wq)
        {
            this.xq = xq;
            this.yq = yq;
            this.zq = zq;
            this.wq = wq;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Checks if the Quat lhs is equal to rhs
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(Quat lhs, Quat rhs)
        {
            return (lhs.xq == rhs.xq && lhs.yq == rhs.yq && lhs.zq == rhs.zq && lhs.wq == rhs.wq);
        }

        /// <summary>
        /// Check if Quat lhs is unequal to rhs
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(Quat lhs, Quat rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Devuelve un nuevo quaternion basado en la multiplicación de los 2 quaternions previos 
        /// (X*Y) + Z, porque de esta manera se hizo para evitar el gimbal lock, ya que Y es el único componente imaginario que no se toca con los otros 2
        /// , dando lugar a que con el valor de Y puedas recuperar el valor de X y Z.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Quat operator *(Quat lhs, Quat rhs)
        {
            /*
             * Se utiliza la fórmula de Hamilton para obtener la multiplicación de estas cuaterniones,
             * ya que esta fórmula está pensada para este caso particular y es mejor computacionalmete.
             * Se aplica distributiva sobre los componentes X Y Z W de los cuaterniones.
             * esto da como resultado un cuaternion de la adiccion de los dos cuaterniones
             */

            float new_xq = lhs.wq * rhs.xq + lhs.xq * rhs.wq + lhs.yq * rhs.zq - lhs.zq * rhs.yq;
            float new_yq = lhs.wq * rhs.yq + lhs.yq * rhs.wq + lhs.zq * rhs.xq - lhs.xq * rhs.zq;
            float new_zq = lhs.wq * rhs.zq + lhs.zq * rhs.wq + lhs.xq * rhs.yq - lhs.yq * rhs.xq;
            float new_wq = lhs.wq * rhs.wq - lhs.xq * rhs.xq - lhs.yq * rhs.yq - lhs.zq * rhs.zq;

            return new Quat(new_xq, new_yq, new_zq, new_wq);
        }

        /* https://es.wikipedia.org/wiki/Cuaterni%C3%B3n */

        /// <summary>
        ///Applies the rotation to the specified point
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vec3 operator *(Quat rotation, Vec3 point)
        {
            /*
             * Se multiplica por 2 por la cantidad de numeros imaginarios (x y z) que lo afectan al numero real (w)
             * al ser 3 dimenciones , para pasarlo al numero real , necesitamos hacer el proceso inverso que se aplico
             * para pasarlo e numero imaginario (/ 2)
             */

            float rotX = rotation.xq * 2f;
            float rotY = rotation.yq * 2f;
            float rotZ = rotation.zq * 2f;

            float rotX2 = rotation.xq * rotX;
            float rotY2 = rotation.yq * rotY;
            float rotZ2 = rotation.zq * rotZ;

            float rotXY = rotation.xq * rotY;
            float rotXZ = rotation.xq * rotZ;
            float rotYZ = rotation.yq * rotZ;

            float rotWX = rotation.wq * rotX;
            float rotWY = rotation.wq * rotY;
            float rotWZ = rotation.wq * rotZ;

            Vec3 result = Vec3.Zero;

            /*
             * Creamos una matriz de rotacion generada por el cuaternion a las cordenadas del vector
             * el 1f se utiliza para conservar la escala durante la rotacion
             */
            result.x = (1f - (rotY2 + rotZ2)) * point.x + (rotXY - rotWZ) * point.y + (rotXZ + rotWY) * point.z;
            result.y = (rotXY + rotWZ) * point.x + (1f - (rotX2 + rotZ2)) * point.y + (rotYZ - rotWX) * point.z;
            result.z = (rotXZ - rotWY) * point.x + (rotYZ + rotWX) * point.y + (1f - (rotX2 + rotY2)) * point.z;

            return result;
        }

        /// <summary>
        /// Select a Number Between 1 to 4
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 1:
                        return xq;
                        break;
                    case 2:
                        return yq;
                        break;
                    case 3:
                        return zq;
                        break;
                    case 4:
                        return wq;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Index out of Range!");
                }
            }
            set
            {
                switch (index)
                {
                    case 1:
                        xq = value;
                        break;
                    case 2:
                        yq = value;
                        break;
                    case 3:
                        zq = value;
                        break;
                    case 4:
                        wq = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Index out of Range!");
                }
            }
        }

        /// <summary>
        /// Represents the rotation of a quaternion with the axes of the world (The "No Rotation")
        /// la W representa la parte real del quaternion y es 1 por que mantiene la magnitud del quaternion en 1
        /// Ya que si fuera 0 no seria valido para rotaciones
        /// </summary>
        public static Quat identity
        {
            get { return new Quat(0, 0, 0, 1); }
        }

        #endregion

        /// <summary>
        /// Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
        /// </summary>
        /// <param name="xq"></param>
        /// <param name="yq"></param>
        /// <param name="zq"></param>
        /// <returns></returns>
        public static Quat Euler(float xq, float yq, float zq)
        {
            /* https://docs.unity3d.com/es/530/ScriptReference/Quaternion.Euler.html */
            float sin;
            float cos;
            Quat qX, qY, qZ;
            Quat ret = identity;

            /*
             * (Mathf.Deg2Rad * x) agarra el angulo y lo pasa a rad
             * se multimplica por 0.5 por que se utiliza la mitad del angulo para crear quaterniones
             * y luego se crea el quaternion
             */
            sin = Mathf.Sin(Mathf.Deg2Rad * xq * 0.5f); //For the imaginary part, we use Sin
            cos = Mathf.Cos(Mathf.Deg2Rad * xq * 0.5f); //For the real part, we use Cos
            qX = new Quat(sin, 0, 0, cos);

            sin = Mathf.Sin(Mathf.Deg2Rad * yq * 0.5f);
            cos = Mathf.Cos(Mathf.Deg2Rad * yq * 0.5f);
            qY = new Quat(0, sin, 0, cos);

            sin = Mathf.Sin(Mathf.Deg2Rad * zq * 0.5f);
            cos = Mathf.Cos(Mathf.Deg2Rad * zq * 0.5f);
            qZ = new Quat(0, 0, sin, cos);

            /*
             * El orden de las multiplicaciones afectan al resultado
             * Por eso se utilizan en este orden ya que es el utilizado mas comunmente
             */
            ret = qY * qX * qZ;

            return ret;
        }

        /// <summary>
        /// Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order) whit an Angle.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Quat Euler(Vec3 angle)
        {
            return Euler(angle.x, angle.y, angle.z);
        }

        /// <summary>
        /// Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Quat EulerAngles(float x, float y, float z)
        {
            return Euler(x, y, z);
        }

        /// <summary>
        /// Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Quat EulerAngles(Vec3 angle)
        {
            return Euler(angle.x, angle.y, angle.z);
        }

        /// <summary>
        /// Given a quaternion of the form Q = a + bi + cj + dk, the normalized quaternion is defined as Q / √a2 + b2 + c2 + d2.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static Quat Normalize(Quat q)
        {
            /*
             * Primero obtenemos la magnitud al cuadrado del quaternion (Dot(q, q))
             * Luego aplicamos la raiz para obtener la magnitud real (Esto lo hacemos para que el resultado no sea negativo)
             * Despues pregunto si la magnitud es cercana a 0 (practicamente no tiene rotacion por ende es irrelevante)
             * Y al final retorno un nuevo quaternion dividiendo cada uno de sus componentes por la magnitud
             * para que su longitud de como resultado 1
             */
            float mag = Mathf.Sqrt(Dot(q, q));

            if (mag < Mathf.Epsilon)
            {
                return identity;
            }

            return new Quat(q.xq / mag, q.yq / mag, q.zq / mag, q.wq / mag);
        }

        /// <summary>
        /// Given a quaternion of the form Q=a+bi+cj+dk, the normalized quaternion is defined as Q/√a2+b2+c2+d2.
        /// </summary>
        public void Normalize()
        {
            this = Normalize(this);
        }

        /// <summary>
        /// Returns the angle in degrees between two rotations a and b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Angle(Quat a, Quat b)
        {
            /* https://docs.unity3d.com/ScriptReference/Quaternion.Angle.html */
            float dot = Dot(a, b);

            if (dot > 0.999999f)
                return 0f;
            else
                return (Mathf.Acos(Mathf.Min(Mathf.Abs(dot), 1f)) * 2f * Mathf.Rad2Deg);

            /*
             * Primero obtengo el valor absoluto
             * despues busco el minimo valor entre el resultado y 1 (para evitar errores en el acos)
             * despues se multiplica por 2 por que requerimos el doble del angulo del quaternion para obtener el angulo real
             * y para finalizar lo pasamos a grados
             */
        }

        /// <summary>
        /// Returns the rotation of the axis vector usign "Rad"
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static Quat AngleAxis(float angle, Vec3 axis)
        {
            /*
             * Rota un quat en base a un eje una cantidad de grados espesificos
             * primero normalizamos el vector para evitar errores relacionados a su magnitud
             * Mathf.Sin(angle * Mathf.Deg2Rad * 0.5f) esto se hace para sacar los valores ya rotados de los componentes complejos del quat
             * Mathf.Cos(angle * Mathf.Deg2Rad * 0.5f) y esta parte se hace para sacar el componente real del quat (W)
             */

            axis.Normalize();
            axis *= Mathf.Sin(angle * Mathf.Deg2Rad * 0.5f);
            return new Quat(axis.x, axis.y, axis.z, Mathf.Cos(angle * Mathf.Deg2Rad * 0.5f));
        }

        /// <summary>
        /// Returns the rotation of the axis vector usign "Deg"
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Quat AxisAngle(Vec3 axis, float angle)
        {
            Quat ret = identity;
            axis.Normalize();
            axis *= (float)Math.Sin((angle / 2) * Mathf.Deg2Rad);
            ret.xq = axis.x;
            ret.yq = axis.y;
            ret.zq = axis.z;
            ret.wq = (float)Math.Cos((angle / 2) * Mathf.Deg2Rad);
            return Normalize(ret);
        }

        /// <summary>
        /// Return a float value equal to the magnitudes of the two quaternions multiplied together
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Dot(Quat a, Quat b)
        {
            return a.xq * b.xq + a.yq * b.yq + a.zq * b.zq + a.wq * b.wq;
        }

        /// <summary>
        /// Creates a rotation which rotates from fromDirection to toDirection.
        /// </summary>
        /// <param name="fromDirection"></param>
        /// <param name="toDirection"></param>
        /// <returns></returns>
        public static Quat FromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            Vec3 axis = Vec3.Cross(fromDirection, toDirection);
            float angle = Vec3.Angle(fromDirection, toDirection);
            return AngleAxis(angle, axis.normalized);
        }

        /// <summary>
        /// Returns the Inverse of rotation.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Quat Inverse(Quat rotation)
        {
            Quat retQ;
            retQ.xq = -rotation.xq;
            retQ.yq = -rotation.yq;
            retQ.zq = -rotation.zq;
            retQ.wq = rotation.wq;
            return retQ;
        }

        /// <summary>
        /// Interpolates between a and b by t and normalizes the result afterwards. The parameter t is clamped to the range [0, 1].
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Quat Lerp(Quat a, Quat b, float t)
        {
            /* https://docs.unity3d.com/ScriptReference/Quaternion.Lerp.html */

            return LerpUnclamped(a, b, Mathf.Clamp01(t));
        }

        /// <summary>
        /// nterpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Quat LerpUnclamped(Quat a, Quat b, float t)
        {
            /* https://docs.unity3d.com/ScriptReference/Quaternion.LerpUnclamped.html */
            Quat retQ;
            float time = 1 - t;
            retQ.xq = time * a.xq + t * b.xq;
            retQ.yq = time * a.yq + t * b.yq;
            retQ.zq = time * a.zq + t * b.zq;
            retQ.wq = time * a.wq + t * b.wq;

            retQ.Normalize();

            return retQ;
        }

        /// <summary>
        /// Set new Values for "LookRotation"
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public void SetLookRotation(Vec3 view)
        {
            this = LookRotation(view, Vec3.Up);
        }

        /// <summary>
        /// Set new Values for "LookRotation"
        /// </summary>
        /// <param name="view"></param>
        /// <param name="upwards"></param>
        /// <returns></returns>
        public void SetLookRotation(Vec3 view, Vec3 upwards)
        {
            this = LookRotation(view, upwards);
        }

        /// <summary>
        /// The from quaternion is rotated towards to by an angular step of maxDegreesDelta (but note that the rotation will not overshoot).
        /// Negative values of maxDegreesDelta will move away from to until the rotation is exactly the opposite direction.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="maxDegreesDelta"></param>
        /// <returns></returns>
        public static Quat RotateTowards(Quat from, Quat to, float maxDegreesDelta)
        {
            /* https://docs.unity3d.com/ScriptReference/Quaternion.RotateTowards.html */

            float angle = Angle(from, to);

            if (angle == 0f)
            {
                return to;
            }

            return SlerpUnclamped(from, to, Mathf.Min(1f, maxDegreesDelta / angle));
        }

        /// <summary>
        /// Spherically interpolates between quaternions a and b by ratio t. The parameter t is clamped to the range [0, 1].
        /// Use this to create a rotation which smoothly interpolates between the first quaternion a to the second quaternion b, based on the value of the 
        /// parameter t. If the value of the parameter is close to 0, the output will be close to a, if it is close to 1, the output will be close to b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Quat Slerp(Quat a, Quat b, float t)
        {
            return SlerpUnclamped(a, b, Mathf.Clamp01(t));
        }

        /// <summary>
        /// Spherically interpolates between a and b by t. The parameter t is not clamped.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Quat SlerpUnclamped(Quat a, Quat b, float t)
        {
            /* https://docs.unity3d.com/ScriptReference/Quaternion.SlerpUnclamped.html */

            Quat retQ;

            float time = 1 - t;

            float wa, wb;

            float angle = Mathf.Acos(Dot(a, b));

            angle = Mathf.Abs(angle);

            float sn = Mathf.Sin(angle);

            wa = Mathf.Sin(time * angle) / sn;
            wb = Mathf.Sin((1 - time) * angle) / sn;

            retQ.xq = wa * a.xq + wb * b.xq;
            retQ.yq = wa * a.yq + wb * b.yq;
            retQ.zq = wa * a.zq + wb * b.zq;
            retQ.wq = wa * a.wq + wb * b.wq;

            retQ.Normalize();

            return retQ;
        }

        /// <summary>
        /// Set new values for a Quat
        /// </summary>
        /// <param name="new_xq"></param>
        /// <param name="new_yq"></param>
        /// <param name="new_zq"></param>
        /// <param name="new_wq"></param>
        public void Set(float new_xq, float new_yq, float new_zq, float new_wq)
        {
            xq = new_xq;
            yq = new_yq;
            zq = new_zq;
            wq = new_wq;
        }

        /// <summary>
        /// Set New Values for "SetFromToRotation"
        /// </summary>
        /// <param name="fromDirection"></param>
        /// <param name="toDirection"></param>
        public void SetFromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            this = FromToRotation(fromDirection, toDirection);
        }

        /// <summary>
        /// public static Quaternion LookRotation(Vector3 forward, Vector3 upwards = Vector3.up); 
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="upwards"></param>
        /// <returns></returns>
        public static Quat LookRotation(Vec3 forward, Vec3 upwards)
        {
            Vec3 dir = (upwards - forward).normalized;
            Vec3 rotAxis = Vec3.Cross(Vec3.Forward, dir);
            float dot = Vec3.Dot(Vec3.Forward, dir);

            Quat retQ;
            retQ.xq = rotAxis.x;
            retQ.yq = rotAxis.y;
            retQ.zq = rotAxis.z;
            retQ.wq = dot + 1;

            return Normalize(retQ);
        }

        /// <summary>
        /// Converts a rotation to angle-axis representation (angles in degrees).
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="axis"></param>
        public void ToAngleAxis(out float angle, out Vec3 axis)
        {
            angle = 2 * Mathf.Acos(wq);
            if (Mathf.Abs(angle) < kEpsilon)
            {
                angle *= Mathf.Deg2Rad;
                axis = new Vec3(1, 0, 0);
            }

            float div = 1 / Mathf.Sqrt(1 - Mathf.Sqrt(wq));
            angle *= Mathf.Deg2Rad;
            axis = new Vec3(xq * div, yq * div, zq * div);
        }

        /// <summary>
        /// Returns the values of a Quat in a string
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            return new string("Xq Value : " + this.xq + ", Yq Value : " + this.yq + ", Zq Value : " + this.zq +
                              ", Wq Value : " + this.wq);
        }
    }
}