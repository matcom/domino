namespace Table;

public static class CircularArray<T>
{
    public static T[] Circular(T[] array, int ind)
    {
        T[] aux = new T[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            if (ind == array.Length) ind = 0;
            aux[i] = array[ind];
            ind++;
        }
        return aux;
    }
}