using System;
using System.Linq;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;

namespace OOPT.Optimization.Algebra.Extensions
{
    public static class MatrixExtension
    {
        public static IMatrix<T> Mult<T>(this IMatrix<T> a, IMatrix<T> b) where T : unmanaged
        {
            if (a.ColumnsCount.Max() != b.RowCount)
            {
                throw new Exception("Cant multiplicate two matrix because ColumnsCount not Equal RowsCount");
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            var matrixC = new Matrix<T>(a.RowCount, b.ColumnsCount.Max());

            for (var i = 0; i < a.RowCount; i++)
            {
                for (var j = 0; j < b.ColumnsCount[i]; j++)
                {
                    for (var k = 0; k < a.ColumnsCount.Count; k++)
                    {
                        matrixC[i, j] = la.Sum(matrixC[i,j], la.Mult(a[i, k], b[k, j]));
                    }
                }
            }

            return matrixC;
        }
        public static IVector<T> Mult<T>(this IMatrix<T> a, IVector<T> b) where T : unmanaged
        {
            if (a.ColumnsCount.Max() != b.Count)
            {
                throw new Exception("Cant multiplicate matrix and vector because ColumnsCount not Equal Count");
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            var vector = new Vector<T>(a.RowCount);

            for (var i = 0; i < a.RowCount; i++)
            {
                for (var j = 0; j < a.ColumnsCount[i]; j++)
                {
                    vector[i] = la.Sum(vector[i], la.Mult(a[i, j], b[j]));
                }
            }

            return vector;
        }
        public static IMatrix<T> Transpose<T>(this IMatrix<T> a) where T : unmanaged
        {
            var matrixC = new Matrix<T>(a.ColumnsCount.Max(), a.RowCount);

            for (var i = 0; i < a.RowCount; i++)
            {
                for (var j = 0; j < a.ColumnsCount[i]; j++)
                {
                    matrixC[j, i] = a[i, j];
                }
            }

            return matrixC;
        }

        public static IMatrix<T> CreateSubMatrix<T>(this IMatrix<T> matrix, int excludingRow, int excludingCol) where T : unmanaged
        {
            var mat = new Matrix<T>(matrix.RowCount - 1, matrix.ColumnsCount.Count - 1);
            var r = -1;

            for (var i = 0; i < matrix.RowCount; i++)
            {
                if (i == excludingRow)
                {
                    continue;
                }

                r++;
                var c = -1;

                for (var j = 0; j < matrix.ColumnsCount.Count; j++)
                {
                    if (j == excludingCol)
                        continue;

                    mat[r, ++c] = matrix[i, j];
                }
            }

            return mat;
        }

        public static T Determinant<T>(this IMatrix<T> matrix) where T : unmanaged
        {
            if (matrix.RowCount != matrix.ColumnsCount.Count)
                throw new Exception("matrix need to be square.");

            if (matrix.RowCount == 1 && matrix.ColumnsCount.Count == 1)
            {
                return matrix[0, 0];
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            if (matrix.RowCount == 2)
            {
                return la.Sub(la.Mult(matrix[0, 0], matrix[1, 1]), la.Mult(matrix[0, 1], matrix[1, 0]));
            }

            T sum = la.GetZeroValue();

            static int ChangeSign(int i) => i % 2 == 0 ? 1 : -1;

            for (int i = 0; i < matrix.ColumnsCount.Count; i++)
            {
                sum = la.Sum(sum, la.Mult(la.Mult(la.Cast((double)ChangeSign(i)), matrix[0, i]), Determinant(CreateSubMatrix(matrix, 0, i))));
            }

            return sum;
        }

        //TODO: need implement normal method for this
        public static IMatrix<T> Inverse<T>(this IMatrix<T> matrix) where T : unmanaged
        {
            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            return Transpose<T>(Cofactor<T>(matrix)).MultWithCopy(la.Div(la.Cast(1.0) , Determinant(matrix)));
        }
        public static IMatrix<T> Mult<T>(this IMatrix<T> matrix, T constant) where T : unmanaged
        {
            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();
            for (var i = 0; i < matrix.RowCount; i++)
            {
                for (var j = 0; j < matrix.ColumnsCount.Count; j++)
                {
                    matrix[i, j] = la.Mult(matrix[i][j], constant);
                }
            }
            return matrix;
        }
        public static IMatrix<T> MultWithCopy<T>(this IMatrix<T> matrix, T constant) where T : unmanaged
        {
            return matrix.Clone().Mult(constant);
        }

        public static IMatrix<T> Cofactor<T>(this IMatrix<T> matrix) where T : unmanaged
        {
            var mat = new Matrix<T>(matrix.RowCount, matrix.ColumnsCount.Count);
            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            static int ChangeSign(int i) => i % 2 == 0 ? 1 : -1;

            for (var i = 0; i < matrix.RowCount; i++)
            {
                for (var j = 0; j < matrix.ColumnsCount.Count; j++)
                {
                    mat[i, j] = la.Mult(la.Cast(ChangeSign(i) * ChangeSign(j)), Determinant(CreateSubMatrix(matrix, i, j)));
                }
            }

            return mat;
        }
    }

    public static class VectorExtension
    {
        public static (T minimum, long index) MinWithIndex<T>(this IVector<T> self) where T : unmanaged
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (self.Count == 0)
            {
                throw new ArgumentException("List is empty.", nameof(self));
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            var min = self[0];
            var minIndex = 0;

            for (var i = 1; i < self.Count; ++i)
            {
                if (la.Compare(self[i], min) >= 0)
                {
                    continue;
                }

                min = self[i];
                minIndex = i;
            }

            return (min, minIndex);
        }

        public static IVector<T> Add<T>(this IVector<T> a, IVector<T> b) where T : unmanaged
        {
            if (a.Count != b.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            for (var i = 0; i < a.Count; i++)
            {
                a[i] = la.Sum(a[i], b[i]);
            }

            return a;
        }

        public static IVector<T> Sub<T>(this IVector<T> a, IVector<T> b) where T : unmanaged
        {
            if (a.Count != b.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            for (var i = 0; i < a.Count; i++)
            {
                a[i] = la.Sub(a[i], b[i]);
            }

            return a;
        }

        public static IVector<T> AddWithCloning<T>(this IVector<T> a, IVector<T> b) where T : unmanaged
        {
            return a.Clone().Add(b);
        }

        public static IVector<T> SubWithCloning<T>(this IVector<T> a, IVector<T> b) where T : unmanaged
        {
            return a.Clone().Sub(b);
        }

        public static IVector<T> Mult<T>(this IVector<T> a, T b) where T : unmanaged
        {
            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            for (var i = 0; i < a.Count; i++)
            {
                a[i] = la.Mult(a[i], b);
            }

            return a;
        }

        public static IVector<T> MultWithCloning<T>(this IVector<T> a, T b) where T : unmanaged
        {
            return a.Clone().Mult(b);
        }

        public static IVector<T> Div<T>(this IVector<T> a, T b) where T : unmanaged
        {
            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            for (var i = 0; i < a.Count; i++)
            {
                a[i] = la.Div(a[i], b);
            }

            return a;
        }

        public static IVector<T> SetValue<T>(this IVector<T> a, T b)
        {
            for (var i = 0; i < a.Count; i++)
            {
                a[i] = b;
            }

            return a;
        }
    }
}