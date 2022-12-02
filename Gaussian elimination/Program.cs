using System;
using System.Linq;

namespace GaussAlgorithm
{
    public class Solver
    {
        public double[] Solve(double[][] matrix, double[] freeMembers)
        {
            var width = matrix[0].Length;
            var height = matrix.Length;
            MakingRowEchelonForm(matrix, freeMembers);
            var result = Solving(matrix, freeMembers);
            return result;
        }
        void MakingRowEchelonForm(double[][] matrix, double[] freeMembers, double delta = 1.0e-3)
        {
            var width = matrix[0].Length;
            var height = matrix.Length;
            for (int i = 0; i < width; i++)
            {
                double[] basic = new double[width];
                bool isBaselineFound = false;
                double currenFreeMember = 0.0;
                for (int j = 0; j < height; j++)
                {
                    var currentLine = matrix[j];
                    bool isZerosInBeggining = currentLine.Take(i).All(el => Math.Abs(el) <= delta);
                    bool isThenNonZero = currentLine.Skip(i).First() != 0;
                    if (isZerosInBeggining && isThenNonZero)
                    {
                        isBaselineFound = true;
                        basic = currentLine;
                        currenFreeMember = freeMembers[j];
                        break;
                    }
                }
                if (!isBaselineFound) continue;

                var firstBaseElement = basic[i];
                for (int j = 0; j < height; j++)
                {
                    var currentLine = matrix[j];
                    if (currentLine == basic) continue;
                    var firstElement = currentLine[i];
                    if (firstElement == 0) continue;
                    var scale = firstElement / firstBaseElement;
                    matrix[j] = currentLine.Zip(basic, (first, second) => first -= second * scale).ToArray();
                    freeMembers[j] -= scale * currenFreeMember;
                }
            }
        }

        double[] Solving(double[][] matrix, double[] freeMembers, double delta = 1.0e-3)
        {
            var width = matrix[0].Length;
            var height = matrix.Length;
            var result = new double[width];
            for (int i = 0; i < height; i++)
            {
                bool isFound = false;
                for (int j = 0; j < width; j++)
                {
                    if (Math.Abs(matrix[i][j]) >= delta)
                    {
                        result[j] = freeMembers[i] / matrix[i][j];
                        isFound = true;
                        break;
                    }
                }
                if (!isFound && freeMembers[i] != 0) throw new NoSolutionException("Solution does not exist!");

            }
            return result;
        }
    }



}
