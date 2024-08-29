using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* https://referencesource.microsoft.com/#System.Numerics/System/Numerics/Matrix4x4.cs */
/* https://es.wikipedia.org/wiki/Cuaterni%C3%B3n */
/* https://learn.microsoft.com/en-us/dotnet/api/system.double.nan?view=net-7.0 */

namespace CustomMath
{
    public struct Matrix
    {
        #region Variables
        public float m00;
        public float m10;
        public float m20;
        public float m30;
        public float m01;
        public float m11;
        public float m21;
        public float m31;
        public float m02;
        public float m12;
        public float m22;
        public float m32;
        public float m03;
        public float m13;
        public float m23;
        public float m33;

        #endregion

        #region Constructor
        public Matrix(Vector4 col1, Vector4 col2, Vector4 col3, Vector4 col4)
        {
            m00 = col1.x;
            m01 = col2.x;
            m02 = col3.x;
            m03 = col4.x;
            m10 = col1.y;
            m11 = col2.y;
            m12 = col3.y;
            m13 = col4.y;
            m20 = col1.z;
            m21 = col2.z;
            m22 = col3.z;
            m23 = col4.z;
            m30 = col1.w;
            m31 = col2.w;
            m32 = col3.w;
            m33 = col4.w;
        }
        #endregion

        #region Operators
        private static readonly Matrix Zero = new Matrix(new Vector4(0f, 0f, 0f, 0f), new Vector4(0f, 0f, 0f, 0f), new Vector4(0f, 0f, 0f, 0f), new Vector4(0f, 0f, 0f, 0f));
        private static readonly Matrix Identity = new Matrix(new Vector4(1f, 0f, 0f, 0f), new Vector4(0f, 1f, 0f, 0f), new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 0f, 1f));

        /// <summary>
        /// Select a index between 0 and 15
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
                    case 0:
                        return m00;
                    case 1:
                        return m10;
                    case 2:
                        return m20;
                    case 3:
                        return m30;
                    case 4:
                        return m01;
                    case 5:
                        return m11;
                    case 6:
                        return m21;
                    case 7:
                        return m31;
                    case 8:
                        return m02;
                    case 9:
                        return m12;
                    case 10:
                        return m22;
                    case 11:
                        return m32;
                    case 12:
                        return m03;
                    case 13:
                        return m13;
                    case 14:
                        return m23;
                    case 15:
                        return m33;
                    default:
                        throw new IndexOutOfRangeException("Index out of Range!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        m00 = value;
                        break;
                    case 1:
                        m10 = value;
                        break;
                    case 2:
                        m20 = value;
                        break;
                    case 3:
                        m30 = value;
                        break;
                    case 4:
                        m01 = value;
                        break;
                    case 5:
                        m11 = value;
                        break;
                    case 6:
                        m21 = value;
                        break;
                    case 7:
                        m31 = value;
                        break;
                    case 8:
                        m02 = value;
                        break;
                    case 9:
                        m12 = value;
                        break;
                    case 10:
                        m22 = value;
                        break;
                    case 11:
                        m32 = value;
                        break;
                    case 12:
                        m03 = value;
                        break;
                    case 13:
                        m13 = value;
                        break;
                    case 14:
                        m23 = value;
                        break;
                    case 15:
                        m33 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Index out of Range!");
                }
            }
        }

        public float this[int row, int column]
        {
            get
            {
                return this[row + column * 4];
            }
            set
            {
                this[row + column * 4] = value;
            }
        }
        public static bool operator ==(Matrix lhs, Matrix rhs)
        {
            return lhs.GetColumn(0) == rhs.GetColumn(0) &&
                   lhs.GetColumn(1) == rhs.GetColumn(1) &&
                   lhs.GetColumn(2) == rhs.GetColumn(2) &&
                   lhs.GetColumn(3) == rhs.GetColumn(3);
        }

        public static bool operator !=(Matrix lhs, Matrix rhs) => !(lhs == rhs);

        /// <summary>
        /// Multiply two M4x4 Row by Column (Component to Component)
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix lhs, Matrix rhs)
        {
            Matrix returnM = Zero;

            returnM.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30;
            returnM.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31;
            returnM.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32;
            returnM.m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33;
            returnM.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30;
            returnM.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31;
            returnM.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32;
            returnM.m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33;
            returnM.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30;
            returnM.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31;
            returnM.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32;
            returnM.m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33;
            returnM.m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30;
            returnM.m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31;
            returnM.m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32;
            returnM.m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33;

            return returnM;
        }

        /// <summary>
        /// multiply an M4x4 by an M1x4
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector4 operator *(Matrix lhs, Vector4 vector)
        {
            Vector4 returnV = Vector4.zero;

            returnV.x = lhs.m00 * vector.x + lhs.m01 * vector.y + lhs.m02 * vector.z + lhs.m03 * vector.w;
            returnV.y = lhs.m10 * vector.x + lhs.m11 * vector.y + lhs.m12 * vector.z + lhs.m13 * vector.w;
            returnV.z = lhs.m20 * vector.x + lhs.m21 * vector.y + lhs.m22 * vector.z + lhs.m23 * vector.w;
            returnV.w = lhs.m30 * vector.x + lhs.m31 * vector.y + lhs.m32 * vector.z + lhs.m33 * vector.w;

            return returnV;
        }
        #endregion

        /// <summary>
        /// Get The matrix rotation
        /// </summary>
        /// <returns></returns>
        public Quat rotation => GetRotation();

        /// <summary>
        /// Get The matrix rotation
        /// </summary>
        /// <returns></returns>
        private Quat GetRotation()
        {
            Matrix matr = this;
            Quat returnQ = new Quat();

            returnQ.wq = Mathf.Sqrt(Mathf.Max(0, 1 + matr[0, 0] + matr[1, 1] + matr[2, 2])) / 2;
            returnQ.xq = Mathf.Sqrt(Mathf.Max(0, 1 + matr[0, 0] - matr[1, 1] - matr[2, 2])) / 2;
            returnQ.yq = Mathf.Sqrt(Mathf.Max(0, 1 - matr[0, 0] + matr[1, 1] - matr[2, 2])) / 2;
            returnQ.zq = Mathf.Sqrt(Mathf.Max(0, 1 - matr[0, 0] - matr[1, 1] + matr[2, 2])) / 2;
            returnQ.xq *= Mathf.Sign(returnQ.xq * (matr[2, 1] - matr[1, 2]));
            returnQ.yq *= Mathf.Sign(returnQ.yq * (matr[0, 2] - matr[2, 0]));
            returnQ.zq *= Mathf.Sign(returnQ.zq * (matr[1, 0] - matr[0, 1]));

            return returnQ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static Matrix Rotate(Quat q)
        {
            float x = q.xq * 2f;
            float y = q.yq * 2f;
            float z = q.zq * 2f;
            float x2 = q.xq * x;
            float y2 = q.yq * y;
            float z2 = q.zq * z;
            float xy = q.xq * y;
            float xz = q.xq * z;
            float yz = q.yq * z;
            float wx = q.wq * x;
            float wy = q.wq * y;
            float wz = q.wq * z;

            Matrix resultM = Zero;

            resultM.m00 = 1f - (y2 + z2);
            resultM.m10 = xy + wz;
            resultM.m20 = xz - wy;
            resultM.m30 = 0f;
            resultM.m01 = xy - wz;
            resultM.m11 = 1f - (x2 + z2);
            resultM.m21 = yz + wx;
            resultM.m31 = 0f;
            resultM.m02 = xz + wy;
            resultM.m12 = yz - wx;
            resultM.m22 = 1f - (x2 + y2);
            resultM.m32 = 0f;
            resultM.m03 = 0f;
            resultM.m13 = 0f;
            resultM.m23 = 0f;
            resultM.m33 = 1f;

            return resultM;
        }

        /// <summary>
        /// Attempts to get a scale value from the matrix. (Read Only)
        /// Scale can only be represented correctly by a 3x3 matrix instead of a 3 component vector, if the given matrix has been skewed for example. lossyScale 
        /// is a convenience property which attempts to match the scale from the matrix as much as possible. If the given matrix is orthogonal, the value will be correct.
        /// </summary>
        /// <returns></returns>
        public Vec3 lossyScale => GetLosszScale();

        /// <summary>
        /// Attempts to get a scale value from the matrix. (Read Only)
        /// Scale can only be represented correctly by a 3x3 matrix instead of a 3 component vector, if the given matrix has been skewed for example. lossyScale 
        /// is a convenience property which attempts to match the scale from the matrix as much as possible. If the given matrix is orthogonal, the value will be correct.
        /// </summary>
        /// <returns></returns>
        private Vec3 GetLosszScale()
        {
            return new Vec3(GetColumn(1).magnitude, GetColumn(2).magnitude, GetColumn(3).magnitude);
        }

        /// <summary>
        /// Returns whether the matrix is the identity matrix.
        /// </summary>
        private bool IsIdentity()
        {
            return this == Identity;
        }

        /// <summary>
        ///  the determinant is a scalar value that is a function of the entries of a square matrix.
        /// </summary>
        public float determinant => Determinant(this);
        /// <summary>
        ///  the determinant is a scalar value that is a function of the entries of a square matrix.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static float Determinant(Matrix m)
        {
            return
                m[0, 3] * m[1, 2] * m[2, 1] * m[3, 0] - m[0, 2] * m[1, 3] * m[2, 1] * m[3, 0] -
                m[0, 3] * m[1, 1] * m[2, 2] * m[3, 0] + m[0, 1] * m[1, 3] * m[2, 2] * m[3, 0] +
                m[0, 2] * m[1, 1] * m[2, 3] * m[3, 0] - m[0, 1] * m[1, 2] * m[2, 3] * m[3, 0] -
                m[0, 3] * m[1, 2] * m[2, 0] * m[3, 1] + m[0, 2] * m[1, 3] * m[2, 0] * m[3, 1] +
                m[0, 3] * m[1, 0] * m[2, 2] * m[3, 1] - m[0, 0] * m[1, 3] * m[2, 2] * m[3, 1] -
                m[0, 2] * m[1, 0] * m[2, 3] * m[3, 1] + m[0, 0] * m[1, 2] * m[2, 3] * m[3, 1] +
                m[0, 3] * m[1, 1] * m[2, 0] * m[3, 2] - m[0, 1] * m[1, 3] * m[2, 0] * m[3, 2] -
                m[0, 3] * m[1, 0] * m[2, 1] * m[3, 2] + m[0, 0] * m[1, 3] * m[2, 1] * m[3, 2] +
                m[0, 1] * m[1, 0] * m[2, 3] * m[3, 2] - m[0, 0] * m[1, 1] * m[2, 3] * m[3, 2] -
                m[0, 2] * m[1, 1] * m[2, 0] * m[3, 3] + m[0, 1] * m[1, 2] * m[2, 0] * m[3, 3] +
                m[0, 2] * m[1, 0] * m[2, 1] * m[3, 3] - m[0, 0] * m[1, 2] * m[2, 1] * m[3, 3] -
                m[0, 1] * m[1, 0] * m[2, 2] * m[3, 3] + m[0, 0] * m[1, 1] * m[2, 2] * m[3, 3];
        }


        /// <summary>
        /// Returns the transpose of this matrix (Read Only).
        /// The transposed matrix is the one that has the Matrix4x4's columns exchanged with its rows.
        /// </summary>
        /// <returns></returns>
        public Matrix transpose => Transpose(this);

        /// <summary>
        /// Returns the transpose of this matrix (Read Only).
        /// The transposed matrix is the one that has the Matrix4x4's columns exchanged with its rows.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private Matrix Transpose(Matrix m)
        {
            float aux;

            aux = m.m01;
            m.m01 = m.m10;
            m.m10 = aux;

            aux = m.m02;
            m.m02 = m.m20;
            m.m20 = aux;

            aux = m.m03;
            m.m03 = m.m30;
            m.m30 = aux;

            aux = m.m12;
            m.m12 = m.m21;
            m.m21 = aux;

            aux = m.m13;
            m.m13 = m.m31;
            m.m31 = aux;

            aux = m.m23;
            m.m23 = m.m32;
            m.m32 = aux;

            return m;
        }

        public Matrix inverse => Inverse(this);
        /// <summary>
        /// The inverse of this matrix. (Read Only)
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private Matrix Inverse(Matrix m)
        {
            float detA = Determinant(m);
            if (detA == 0)
                return Zero;

            Matrix aux = new Matrix()
            {
                // Primera Columna
                m00 = m.m11 * m.m22 * m.m33 + m.m12 * m.m23 * m.m31 + m.m13 * m.m21 * m.m32 - m.m11 * m.m23 * m.m32 - m.m12 * m.m21 * m.m33 - m.m13 * m.m22 * m.m31,
                m01 = m.m01 * m.m23 * m.m32 + m.m02 * m.m21 * m.m33 + m.m03 * m.m22 * m.m31 - m.m01 * m.m22 * m.m33 - m.m02 * m.m23 * m.m31 - m.m03 * m.m21 * m.m32,
                m02 = m.m01 * m.m12 * m.m33 + m.m02 * m.m13 * m.m32 + m.m03 * m.m11 * m.m32 - m.m01 * m.m13 * m.m32 - m.m02 * m.m11 * m.m33 - m.m03 * m.m12 * m.m31,
                m03 = m.m01 * m.m13 * m.m22 + m.m02 * m.m11 * m.m23 + m.m03 * m.m12 * m.m21 - m.m01 * m.m12 * m.m23 - m.m02 * m.m13 * m.m21 - m.m03 * m.m11 * m.m22,
                // Segunda Columna				     								    
                m10 = m.m10 * m.m23 * m.m32 + m.m12 * m.m20 * m.m33 + m.m13 * m.m22 * m.m30 - m.m10 * m.m22 * m.m33 - m.m12 * m.m23 * m.m30 - m.m13 * m.m20 * m.m32,
                m11 = m.m00 * m.m22 * m.m33 + m.m02 * m.m23 * m.m30 + m.m03 * m.m20 * m.m32 - m.m00 * m.m23 * m.m32 - m.m02 * m.m20 * m.m33 - m.m03 * m.m22 * m.m30,
                m12 = m.m00 * m.m13 * m.m32 + m.m02 * m.m10 * m.m33 + m.m03 * m.m12 * m.m30 - m.m00 * m.m12 * m.m33 - m.m02 * m.m13 * m.m30 - m.m03 * m.m10 * m.m32,
                m13 = m.m00 * m.m12 * m.m23 + m.m02 * m.m13 * m.m20 + m.m03 * m.m10 * m.m22 - m.m00 * m.m13 * m.m22 - m.m02 * m.m10 * m.m23 - m.m03 * m.m12 * m.m20,
                // Tercera Columna				     								    
                m20 = m.m10 * m.m21 * m.m33 + m.m11 * m.m23 * m.m30 + m.m13 * m.m20 * m.m31 - m.m10 * m.m23 * m.m31 - m.m11 * m.m20 * m.m33 - m.m13 * m.m31 * m.m30,
                m21 = m.m00 * m.m23 * m.m31 + m.m01 * m.m20 * m.m33 + m.m03 * m.m21 * m.m30 - m.m00 * m.m21 * m.m33 - m.m01 * m.m23 * m.m30 - m.m03 * m.m20 * m.m31,
                m22 = m.m00 * m.m11 * m.m33 + m.m01 * m.m13 * m.m31 + m.m03 * m.m10 * m.m31 - m.m00 * m.m13 * m.m31 - m.m01 * m.m10 * m.m33 - m.m03 * m.m11 * m.m30,
                m23 = m.m00 * m.m13 * m.m21 + m.m01 * m.m10 * m.m23 + m.m03 * m.m11 * m.m31 - m.m00 * m.m11 * m.m23 - m.m01 * m.m13 * m.m20 - m.m03 * m.m10 * m.m21,
                // Cuarta Columna					     								    
                m30 = m.m10 * m.m22 * m.m31 + m.m11 * m.m20 * m.m32 + m.m12 * m.m21 * m.m30 - m.m00 * m.m21 * m.m32 - m.m11 * m.m22 * m.m30 - m.m12 * m.m20 * m.m31,
                m31 = m.m00 * m.m21 * m.m32 + m.m01 * m.m22 * m.m30 + m.m02 * m.m20 * m.m31 - m.m00 * m.m22 * m.m31 - m.m01 * m.m20 * m.m32 - m.m02 * m.m21 * m.m30,
                m32 = m.m00 * m.m12 * m.m31 + m.m01 * m.m10 * m.m32 + m.m02 * m.m11 * m.m30 - m.m00 * m.m11 * m.m32 - m.m01 * m.m12 * m.m30 - m.m02 * m.m10 * m.m31,
                m33 = m.m00 * m.m11 * m.m22 + m.m01 * m.m12 * m.m20 + m.m02 * m.m10 * m.m21 - m.m00 * m.m12 * m.m21 - m.m01 * m.m10 * m.m22 - m.m02 * m.m11 * m.m20
            };

            Matrix ret = new Matrix()
            {
                m00 = aux.m00 / detA,
                m01 = aux.m01 / detA,
                m02 = aux.m02 / detA,
                m03 = aux.m03 / detA,
                m10 = aux.m10 / detA,
                m11 = aux.m11 / detA,
                m12 = aux.m12 / detA,
                m13 = aux.m13 / detA,
                m20 = aux.m20 / detA,
                m21 = aux.m21 / detA,
                m22 = aux.m22 / detA,
                m23 = aux.m23 / detA,
                m30 = aux.m30 / detA,
                m31 = aux.m31 / detA,
                m32 = aux.m32 / detA,
                m33 = aux.m33 / detA

            };
            return ret;
        }

        /// <summary>
        /// Creates a scaling matrix.
        /// Returned matrix is such that scales along coordinate axes by a vector v.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix Scale(Vec3 vector)
        {
            Matrix retMat = Zero;
            retMat.m00 = vector.x;
            retMat.m01 = 0f;
            retMat.m02 = 0f;
            retMat.m03 = 0f;
            retMat.m10 = 0f;
            retMat.m11 = vector.y;
            retMat.m12 = 0f;
            retMat.m13 = 0f;
            retMat.m20 = 0f;
            retMat.m21 = 0f;
            retMat.m22 = vector.z;
            retMat.m23 = 0f;
            retMat.m30 = 0f;
            retMat.m31 = 0f;
            retMat.m32 = 0f;
            retMat.m33 = 1f;
            return retMat;
        }

        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix Translate(Vec3 vector)
        {
            Matrix retMat = Zero;

            retMat.m00 = 1f;
            retMat.m01 = 0f;
            retMat.m02 = 0f;
            retMat.m03 = vector.x;
            retMat.m10 = 0f;
            retMat.m11 = 1f;
            retMat.m12 = 0f;
            retMat.m13 = vector.y;
            retMat.m20 = 0f;
            retMat.m21 = 0f;
            retMat.m22 = 1f;
            retMat.m23 = vector.z;
            retMat.m30 = 0f;
            retMat.m31 = 0f;
            retMat.m32 = 0f;
            retMat.m33 = 1f;

            return retMat;
        }

        /// <summary>
        /// Creates a translation, rotation and scaling matrix.
        /// The returned matrix is such that it places objects at position pos, oriented in rotation q and scaled by s.
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Matrix TRS(Vec3 translation, Quat rotation, Vec3 scale)
        {
            Matrix t = Translate(translation);
            Matrix r = Rotate(rotation);
            Matrix s = Scale(scale);

            return t * r * s;
        }

        /// <summary>
        /// Set the TRS Matrix
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="q"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public void SetTRS(Vec3 pos, Quat q, Vec3 s)
        {
            this = TRS(pos, q, s);
        }

        /// <summary>
        /// Check if the matrix is valid for Rendering
        /// </summary>
        /// <returns></returns>
        public bool ValidTRS()
        {
            if (lossyScale == Vec3.Zero)
                return false;
            else if (m00 == double.NaN && m10 == double.NaN && m20 == double.NaN && m30 == double.NaN &&
                     m01 == double.NaN && m11 == double.NaN && m21 == double.NaN && m31 == double.NaN &&
                     m02 == double.NaN && m12 == double.NaN && m22 == double.NaN && m32 == double.NaN &&
                     m03 == double.NaN && m13 == double.NaN && m23 == double.NaN && m33 == double.NaN)
                return false;
            else if (GetRotation().xq > 1 && GetRotation().xq < - 1 && GetRotation().yq > 1 && GetRotation().yq < -1 && GetRotation().zq > 1 && GetRotation().zq < - 1 && GetRotation().wq > 1 && GetRotation().wq < -1)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Returns the specified column (From 0 to 3)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector4 GetColumn(int index)
        {
            return new Vector4(this[0, index], this[1, index], this[2, index], this[3, index]);
        }

        /// <summary>
        /// Set a new Value for a Column
        /// </summary>
        /// <param name="index"></param>
        /// <param name="column"></param>
        public void SetColumn(int index, Vector4 column)
        {
            this[0, index] = column.x;
            this[1, index] = column.y;
            this[2, index] = column.z;
            this[3, index] = column.w;
        }


        /// <summary>
        ///  Returns the specified row (From 0 to 3)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public Vector4 GetRow(int index)
        {
            switch (index)
            {
                case 0:
                    return new Vector4(m00, m01, m02, m03);
                case 1:
                    return new Vector4(m10, m11, m12, m13);
                case 2:
                    return new Vector4(m20, m21, m22, m23);
                case 3:
                    return new Vector4(m30, m31, m32, m33);
                default:
                    throw new IndexOutOfRangeException("Index out of Range!");
            }
        }

        /// <summary>
        /// Set a new Value for a Row
        /// </summary>
        /// <param name="index"></param>
        /// <param name="row"></param>
        public void SetRow(int index, Vector4 row)
        {
            this[index, 0] = row.x;
            this[index, 1] = row.y;
            this[index, 2] = row.z;
            this[index, 3] = row.w;
        }

        /// <summary>
        /// Transforms a position by this matrix (generic).
        /// Returns a position v transformed by the current fully arbitrary matrix. If the matrix is a regular 3D transformation matrix, it is much faster to use 
        /// MultiplyPoint3x4 instead. MultiplyPoint is slower, but can handle projective transformations as well.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vec3 MultiplyPoint(Vec3 point)
        {
            Vec3 retVec;

            retVec.x = m00 * point.x + m01 * point.y + m02 * point.z + m03;
            retVec.y = m10 * point.x + m11 * point.y + m12 * point.z + m13;
            retVec.z = m20 * point.x + m21 * point.y + m22 * point.z + m23;

            float aux = 1f / m30 * point.x +m31 * point.y + m32 * point.z + m33;

            retVec.x *= aux;
            retVec.y *= aux;
            retVec.z *= aux;

            return retVec;
        }
        /// <summary>
        /// Transforms a position by this matrix (fast).
        /// Returns a position v transformed by the current transformation matrix. This function is a faster version of MultiplyPoint; but it can only handle regular 3D
        /// transformations. MultiplyPoint is slower, but can handle projective transformations as well.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vec3 MultiplyPoint3x4(Vec3 point)
        {
            Vec3 retVec;

            retVec.x = m00 * point.x + m01 * point.y + m02 * point.z + m03;
            retVec.y = m10 * point.x + m11 * point.y + m12 * point.z + m13;
            retVec.z = m20 * point.x + m21 * point.y + m22 * point.z + m23;

            return retVec;
        }
        /// <summary>
        /// Transforms a direction by this matrix.
        /// This function is similar to MultiplyPoint;
        /// but it transforms directions and not positions. When transforming a direction, only the rotation part of the matrix 
        /// is taken into account.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Vec3 MultiplyVector(Vec3 vector)
        {
            Vec3 retVec;

            retVec.x = m00 * vector.x + m01 * vector.y + m02 * vector.z;
            retVec.y = m10 * vector.x + m11 * vector.y + m12 * vector.z;
            retVec.z = m20 * vector.x + m21 * vector.y + m22 * vector.z;

            return retVec;
        }

        /// <summary>
        /// Get position vector from the matrix.
        /// </summary>
        /// <returns></returns>
        public Vec3 GetPosition()
        {
            return new Vec3(m03, m13, m23);
        }
    }
}