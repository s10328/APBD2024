// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

Console.WriteLine("Modyfikacja 1");
Console.WriteLine("Modyfikacja 2");
Console.WriteLine("Modyfikacja 2");

static double CalculateAverage(int[] change)
{
    if (change == null || change.Length == 0)
    {
        throw new ArgumentException("Tablica nie może być pusta.");
    }

    long sumaMain = 0;
    foreach (int number in change)
    {
        sumaMain += number;
    }

    return (double)sumaMain / change.Length;
}

static int FindMaximum(int[] numbers)
{
    if (numbers == null || numbers.Length == 0)
    {
        throw new ArgumentException("Tablica nie może być pusta.");
    }

    int max = numbers[0];
    foreach (int number in numbers)
    {
        if (number > max)
        {
            max = number;
        }
    }

    return max;
}


