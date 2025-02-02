using Random = System.Random;

// Code shamelessly copied from here 
// https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
static class RandomExtension
{
    // Fisher-Yates algorithm for array shuffling
    public static void Shuffle<T> (this Random randomNum, T[] array)
    {
        int n = array.Length;
        while(n > 1)
        {
            int k = randomNum.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
