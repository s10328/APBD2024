// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

Console.WriteLine("Modyfikacja 1");
Console.WriteLine("Modyfikacja 2");
Console.WriteLine("Modyfikacja 2");

double CalculateAverage(int[] numbers)
{
    if (numbers == null || numbers.Length == 0)
    {
        throw new ArgumentException("Tablica nie może być pusta.");
    }

    long sum = 0;
    foreach (int number in numbers)
    {
        sum += number;
    }

    return (double)sum / numbers.Length;
}

