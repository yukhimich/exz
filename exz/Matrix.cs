using System;

[Serializable]
public class Matrix
{
    private int[,] data;

    public Matrix(int rows, int columns)
    {
        data = new int[rows, columns];
    }

    public int Rows
    {
        get { return data.GetLength(0); }
    }

    public int Columns
    {
        get { return data.GetLength(1); }
    }

    public int this[int row, int column]
    {
        get { return data[row, column]; }
        set { data[row, column] = value; }
    }

    // Додаткові методи або властивості, якщо потрібно
}
