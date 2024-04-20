using Mehroz;
namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Fraction a = new Fraction(1,5);
            Fraction b = new Fraction(2,1);

            var c = a * b;
            Simplex smp = new Simplex();

            smp.Solve();
        }
    }
}
