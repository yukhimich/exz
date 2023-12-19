using System;

namespace exz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "matrix_file.dat";
            MatrixFileManager matrixFileManager = new MatrixFileManager(filePath);

            Matrix matrix1 = new Matrix(3, 3);
            matrix1[0, 0] = 1;
            matrix1[1, 1] = 2;
            matrix1[2, 2] = 3;

            Matrix matrix2 = new Matrix(2, 2);
            matrix2[0, 0] = 4;
            matrix2[1, 1] = 5;

            matrixFileManager.WriteMatrixToFile(matrix1);

            Matrix readMatrix = matrixFileManager.ReadMatrixFromFile(0);

            if (readMatrix != null)
            {
                Console.WriteLine("Read Matrix:");
                PrintMatrix(readMatrix);
            }
            else
            {
                Console.WriteLine("Matrix not found.");
            }

            matrixFileManager.ReplaceMatrixInFile(0, matrix2);

            matrixFileManager.DeleteMatrixFromFile(0);

            matrixFileManager.ClearFile();
        }

        static void PrintMatrix(Matrix matrix)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
