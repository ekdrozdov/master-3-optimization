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
            //TODO: fix this check, problem with Columns for SpraseMatrix
            if (a.ColumnsCount.Max() != b.RowCount)
            {
                throw new Exception("Cant multiplicate two matrix because ColumnsCount not Equal RowsCount");
            }

            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();
            //TODO: fix this allocation, problem with Columns for SpraseMatrix

            var matrixC = new Matrix<T>(a.RowCount, b.ColumnsCount.Max());

            for (var i = 0; i < a.RowCount; i++)
            {
                for (var k = 0; k < a.ColumnsCount[i]; k++)
                {
                    for (var j = 0; j < b.ColumnsCount[k]; j++)
                    {

                        matrixC[i, j] = la.Sum(matrixC[i, j], la.Mult(a[i, k], b[k, j]));
                    }
                }

            }

            return matrixC;
        }
        public static IVector<T> Mult<T>(this IMatrix<T> a, IVector<T> b) where T : unmanaged
        {
            //TODO: fix this check, problem with Columns for SpraseMatrix

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
            //TODO: fix this allocation, problem with Columns for SpraseMatrix

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
            //TODO: fix this allocation, problem with Columns for SpraseMatrix
            var mat = new Matrix<T>(matrix.RowCount - 1, matrix.ColumnsCount.Max() - 1);
            var r = -1;

            for (var i = 0; i < matrix.RowCount; i++)
            {
                if (i == excludingRow)
                {
                    continue;
                }

                r++;
                var c = -1;

                for (var j = 0; j < matrix.ColumnsCount[i]; j++)
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
            //TODO: fix this check
            if (matrix.RowCount != matrix.ColumnsCount.Max())
                throw new Exception("matrix need to be square.");

            if (matrix.RowCount == 1 && matrix.ColumnsCount.Max() == 1)
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

            return Transpose<T>(Cofactor<T>(matrix)).MultWithCopy(la.Div(la.Cast(1.0), Determinant(matrix)));
        }
        public static IMatrix<T> Mult<T>(this IMatrix<T> matrix, T constant) where T : unmanaged
        {
            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();
            for (var i = 0; i < matrix.RowCount; i++)
            {
                for (var j = 0; j < matrix.ColumnsCount[i]; j++)
                {
                    matrix[i, j] = la.Mult(matrix[i][j], constant);
                }
            }
            return matrix;
        }
        public static IMatrix<T> MultWithCopy<T>(this IMatrix<T> matrix, T constant) where T : unmanaged
        {
            return (matrix.Clone() as IMatrix<T>).Mult(constant);
        }

        public static IMatrix<T> Cofactor<T>(this IMatrix<T> matrix) where T : unmanaged
        {
            var mat = new Matrix<T>(matrix.RowCount, matrix.ColumnsCount.Count);
            var la = LinearAlgebraFactory.GetLinearAlgebra<T>();

            static int ChangeSign(int i) => i % 2 == 0 ? 1 : -1;

            for (var i = 0; i < matrix.RowCount; i++)
            {
                for (var j = 0; j < matrix.ColumnsCount[i]; j++)
                {
                    mat[i, j] = la.Mult(la.Cast(ChangeSign(i) * ChangeSign(j)), Determinant(CreateSubMatrix(matrix, i, j)));
                }
            }

            return mat;
        }
    }
}