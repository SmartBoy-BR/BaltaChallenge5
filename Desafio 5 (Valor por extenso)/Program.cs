using System; // Já implícita com o .net6

internal class Program
{
    static void Main(string[] args)
    {
        NumberToWordsConverter numbersInWords = new();

        Console.WriteLine("Digite um valor em reais:");
        decimal valor = decimal.Parse(Console.ReadLine());

        string valorPorExtenso = numbersInWords.WriteNumberInFull(valor);

        Console.WriteLine(valorPorExtenso);
    }
}