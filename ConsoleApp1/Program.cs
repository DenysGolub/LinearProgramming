using Mehroz;
using System.Net.Http.Headers;
namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Dictionary<string, Fraction> coeff = new Dictionary<string, Fraction>()
            {
                {"P0", new Fraction(0,1) },
                {"P1", new Fraction(2,1) },
                {"P2", new Fraction(3,1) },
                {"P3", new Fraction(0,1) },
                {"P4", new Fraction(0,1) },
                {"P5", new Fraction(0,1) },




            };

            List<string> artifical_variables = new List<string>()
            {
              
            };

            NewSimplex sm = new NewSimplex();
            object[,] test_array =
            {
                {"", "", "", coeff.ElementAt(1).Key, coeff.ElementAt(2).Key, coeff.ElementAt(3).Key, coeff.ElementAt(4).Key, coeff.ElementAt(5).Key},
                {"Базис", "Сб", "P0", coeff["P1"], coeff["P2"], coeff["P3"], coeff["P4"], coeff["P5"]},
                { "P3", new Fraction(), new Fraction(5,1), new Fraction(1,1), new Fraction(1,1), new Fraction(1,1), new Fraction(0,1), new Fraction()},
                { "P4", new Fraction(), new Fraction(9,1), new Fraction(2,1), new Fraction(1,1), new Fraction(0,1), new Fraction(1, 1), new Fraction(0,1) },
                { "P5", new Fraction(), new Fraction(7,1), new Fraction(1,1), new Fraction(2,1), new Fraction(0,1), new Fraction(0, 1), new Fraction(1,1) }

            };
            sm.Solve(test_array, artifical_variables, coeff, true);
        }
    }
}
