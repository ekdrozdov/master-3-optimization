using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOPT.Optimization.Algebra.Extensions;
using OOPT.Optimization.Algebra.Interfaces;
using OOPT.Optimization.Algebra.LinearAlgebra;

namespace OOPT.Optimization.Algebra.DiophantineEquations
{
    public class DiophantineException : Exception
    {
        public DiophantineException(string message) : base(message) { }
    }

    public class DiophantineEquation
    {
        public int CountOfEquation { get; private set; } //m

        public int CountOfUnknown { get; private set; } //n

        public IMatrix<long> OriginalMatrix { get; private set; }

        public int CountOfFreeVariables { get; private set; }

        private IMatrix<long> Matrix { get; set; }

        public List<string> Messages = new List<string>();

        public DiophantineEquation(int countOfEquation, int countOfUnknown)
        {
            CountOfEquation = countOfEquation;
            CountOfUnknown = countOfUnknown;
            OriginalMatrix = new Matrix<long>(CountOfEquation, CountOfUnknown + 1);
            Matrix = new Matrix<long>(CountOfEquation + CountOfUnknown, CountOfUnknown + 1);
        }
        public void SetMatrix(IEnumerable<IEnumerable<long>> matrix)
        {
            var enumerable = matrix.ToArray();

            if (enumerable.Length != CountOfEquation)
            {
                throw new ArgumentException(nameof(CountOfEquation));
            }

            for (var i = 0; i < CountOfEquation; i++)
            {
                if (OriginalMatrix[i].Count != enumerable.ElementAt(i).Count())
                {
                    throw new ArgumentException(nameof(CountOfUnknown));
                }
                OriginalMatrix[i] = new Vector<long>(enumerable.ElementAt(i));
            }
        }

        private void PrepareMatrix()
        {
            for (var i = 0; i < CountOfEquation; ++i)
                Matrix[i] = OriginalMatrix[i].Clone();

            for (var i = 0; i < CountOfUnknown; ++i)
            {
                for (var j = 0; j < CountOfUnknown + 1; ++j)
                    Matrix[i + CountOfEquation][j] = 0;

                Matrix[i + CountOfEquation][i] = 1;
            }
        }

        public void WriteSolutionIntoConsole()
        {
            var la = LinearAlgebraFactory.GetLinearAlgebra<long>();

            if (Messages.Any())
            {
                Messages.ForEach(Console.WriteLine);
            }
            else
            {
                Console.WriteLine($"Count of free variables : {CountOfFreeVariables}");

                for (var i = 0; i < CountOfUnknown; ++i)
                {
                    var stringBuilder = new StringBuilder($"Unknown({i})=");
                    // Particular solution of the equation for i row
                    stringBuilder.Append($"{Matrix[i + CountOfEquation][CountOfUnknown]}");

                    if (CountOfFreeVariables <= 0)
                    {
                        continue;
                    }

                    // Free variables for i row
                    for (var j = 0; j < CountOfFreeVariables - 1; ++j)
                    {
                        stringBuilder.Append(la.Sign(Matrix[i + CountOfEquation][CountOfUnknown - CountOfFreeVariables + j]) >= 0 ? " + " : " - ");
                        stringBuilder.Append($"{Math.Abs(Matrix[i + CountOfEquation][CountOfUnknown - CountOfFreeVariables + j])}*Variable({j})");
                    }

                    stringBuilder.Append(la.Sign(Matrix[i + CountOfEquation][CountOfUnknown - 1]) >= 0 ? " + " : " - ");
                    stringBuilder.Append($"{Math.Abs(Matrix[i + CountOfEquation][CountOfUnknown - 1])}*Variable({CountOfFreeVariables})");
                    Console.WriteLine(stringBuilder.ToString());
                }
            }
        }

        public bool CheckResult()
        {
            var testValues = new long[] { 0, 2, 5, 31, 54 };

            var freeVars = new Matrix<long>(testValues.Length + 1, CountOfFreeVariables, testValues.Append(0L));
            var random = new Random();

            freeVars[testValues.Length] = new Vector<long>(Enumerable.Repeat(0, CountOfFreeVariables)
                                                               .Select(x => Convert.ToInt64(random.Next())));

            var differenceUnknownAndFree = CountOfUnknown - CountOfFreeVariables;

            foreach (var freeRow in (IEnumerable<IVector<long>>)freeVars)
            {
                var solution = new Vector<long>(CountOfUnknown);

                for (var i = 0; i < solution.Count; ++i)
                {
                    var matrixRowEquationPlusOne = Matrix[CountOfEquation + i];
                    solution[i] = matrixRowEquationPlusOne[CountOfUnknown];

                    for (var j = 0; j < CountOfFreeVariables; ++j)
                    {
                        solution[i] += freeRow[j] * matrixRowEquationPlusOne[differenceUnknownAndFree + j];
                    }
                }

                foreach (var row in (IEnumerable<IVector<long>>)OriginalMatrix)
                {
                    var result = row[^1];

                    for (var j = 0; j < row.Count - 1; ++j)
                    {
                        result += row[j] * solution[j];
                    }

                    if (result != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool Solve()
        {
            try
            {
                PrepareMatrix();

                var linearDependentRowsCount = 0;

                for (var iRow = 0; iRow < CountOfEquation; iRow++)
                {
                    var iRowVector = Matrix[iRow];

                    while (true)
                    {
                        var indexOfMinimal = 0;
                        var minimumNotZeroWasFound = false;

                        // We are looking for the minimum element in the string, taking into account that it should not be zero
                        for (var jColumn = iRow; jColumn < CountOfUnknown; ++jColumn)
                        {
                            if (!minimumNotZeroWasFound)
                            {
                                if (iRowVector[jColumn] == 0)
                                {
                                    continue;
                                }

                                minimumNotZeroWasFound = true;
                                indexOfMinimal = jColumn;
                            }
                            else
                            {
                                if (Math.Abs(iRowVector[indexOfMinimal]) > Math.Abs(iRowVector[jColumn]) && iRowVector[jColumn] != 0)
                                {
                                    indexOfMinimal = jColumn;
                                }
                            }
                        }

                        //If all elements in row is zero, this row is linear combination of other. We don`t need this row.
                        if (!minimumNotZeroWasFound)
                        {
                            iRowVector.SetValue(0L);

                            linearDependentRowsCount++;

                            break;
                        }

                        for (var jColumn = iRow; jColumn < CountOfUnknown + 1; ++jColumn)
                        {
                            if (jColumn == indexOfMinimal || Matrix[iRow][jColumn] == 0)
                            {
                                continue;
                            }

                            if (iRowVector[indexOfMinimal] == 0)
                            {
                                throw new DiophantineException($"Division by zero:Matrix[cLine][ind_min] == {Matrix[iRow][indexOfMinimal]};\n" +
                                                               "Solution in long does not exist");
                            }

                            var del = iRowVector[jColumn] / iRowVector[indexOfMinimal];

                            for (var kRow = iRow; kRow < Matrix.RowCount; ++kRow)
                            {
                                Matrix[kRow][jColumn] -= Matrix[kRow][indexOfMinimal] * del;
                            }
                        }

                        var elementOnIRowIRowPlace = iRowVector[iRow];

                        if (elementOnIRowIRowPlace == 0)
                        {
                            for (var kRow = iRow; kRow < Matrix.RowCount; ++kRow)
                            {
                                Matrix[kRow][iRow] += Matrix[kRow][indexOfMinimal];
                            }
                        }

                        minimumNotZeroWasFound = false;

                        for (var j = iRow + 1; j < CountOfUnknown; ++j)
                        {
                            if (iRowVector[j] == 0)
                            {
                                continue;
                            }

                            minimumNotZeroWasFound = true;

                            break;
                        }

                        if (minimumNotZeroWasFound)
                        {
                            continue;
                        }

                        if (elementOnIRowIRowPlace == 0)
                        {
                            throw new DiophantineException($"Division by zero: elementOnIRowIRowPlace == {elementOnIRowIRowPlace};\n" +
                                                           "Solution in long does not exist");
                        }

                        if (iRowVector[CountOfUnknown] % elementOnIRowIRowPlace != 0)
                        {
                            throw new DiophantineException($"iRowVector[CountOfUnknown]:{iRowVector[CountOfUnknown]} has residue when division on elementOnIRowIRowPlace: {elementOnIRowIRowPlace}\n" +
                                                           $"Residue: {iRowVector[CountOfUnknown] % elementOnIRowIRowPlace}\n" +
                                                           "Solution in long does not exist");
                        }

                        if (iRowVector[CountOfUnknown] == 0)
                            break;
                    }
                }

                CountOfFreeVariables = CountOfUnknown - (CountOfEquation - linearDependentRowsCount);

                if (CountOfFreeVariables < 0)
                {
                    throw new DiophantineException($"Count of free variables: {CountOfFreeVariables};\n" +
                                                   "Solution in long does not exist");
                }

                return CheckResult();
            }
            catch (DiophantineException e)
            {
                Messages.Add(e.Message);

                return false;
            }
        }
    }
}