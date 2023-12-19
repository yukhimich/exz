using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using exz;

[Serializable]
public class MatrixFileManager
{
    private string filePath;

    public MatrixFileManager(string filePath)
    {
        this.filePath = filePath;

        // Ініціалізація файлу, якщо він ще не існує
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }

    public void WriteMatrixToFile(Matrix matrix)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Append))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, matrix);
        }
    }

    public Matrix ReadMatrixFromFile(int matrixNumber)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            for (int i = 0; i < matrixNumber; i++)
            {
                if (fs.Position == fs.Length)
                {
                    // Досягли кінця файлу, матриця не знайдена
                    return null;
                }

                Matrix matrix = (Matrix)formatter.Deserialize(fs);
            }

            // Повертаємо знайдену матрицю
            return (Matrix)formatter.Deserialize(fs);
        }
    }

    public void ReplaceMatrixInFile(int matrixNumber, Matrix newMatrix)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            long startPosition = 0;

            for (int i = 0; i < matrixNumber; i++)
            {
                if (fs.Position == fs.Length)
                {
                    // Досягли кінця файлу, матриця не знайдена
                    return;
                }

                Matrix matrix = (Matrix)formatter.Deserialize(fs);
                startPosition = fs.Position;
            }

            fs.Position = startPosition;
            formatter.Serialize(fs, newMatrix);
        }
    }

    public void DeleteMatrixFromFile(int matrixNumber)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            long startPosition = 0;

            for (int i = 0; i < matrixNumber; i++)
            {
                if (fs.Position == fs.Length)
                {
                    // Досягли кінця файлу, матриця не знайдена
                    return;
                }

                Matrix matrix = (Matrix)formatter.Deserialize(fs);
                startPosition = fs.Position;
            }

            fs.Position = startPosition;
            formatter.Serialize(fs, null); // Позначаємо місце як видалене (null)
        }

        // Дефрагментація
        DefragmentFile();
    }

    private void DefragmentFile()
    {
        string tempFilePath = Path.GetTempFileName();

        using (FileStream sourceStream = new FileStream(filePath, FileMode.Open))
        using (FileStream tempStream = new FileStream(tempFilePath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            while (sourceStream.Position < sourceStream.Length)
            {
                Matrix matrix = (Matrix)formatter.Deserialize(sourceStream);
                if (matrix != null)
                {
                    formatter.Serialize(tempStream, matrix);
                }
            }
        }

        File.Delete(filePath);
        File.Move(tempFilePath, filePath);
    }

    public void ClearFile()
    {
        File.WriteAllText(filePath, string.Empty);
    }
}
