using Mehroz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
/*                   P1   P2  P3  P4  P5  
 * Базис   Сб    P0  3     7   0   0   0  
    P3      0    14  1     2   1   0   0  
    P4      0    15  -5    3   0   1   0   
    P5      0    12  2     3   0   0   1 
4 рядок     -     0  -3    -7  0   0   0
5 рядок     -     0   0    0   0   0   0
*/
    internal class Simplex
    {
        object[,] test_array =
        {
            { new Fraction(0,1), new Fraction(14,1), new Fraction(1,1), new Fraction(2,1), new Fraction(1,1), new Fraction(0,1), new Fraction(0,1)},
            { new Fraction(0,1), new Fraction(15,1), new Fraction(-5,1), new Fraction(3, 1), new Fraction(0,1), new Fraction(1,1), new Fraction(0,1)},
            { new Fraction(0,1), new Fraction(12, 1), new Fraction(2,1), new Fraction(3,1), new Fraction(0,1), new Fraction(0,1), new Fraction(1,1)}
        };

        public void Solve()
        {
            var coeff = new Dictionary<string, Fraction>()
            {
                {"P0", new Fraction(0,1) },
                {"P1", new Fraction(3,1) },
                {"P2", new Fraction(7,1) },
                {"P3", new Fraction(0,1) },
                {"P4", new Fraction(0,1) },
                {"P5", new Fraction(0,1) }
            };
            BasisMultiplyOnVectors(test_array, coeff);
        }

        private void BasisMultiplyOnVectors(object[,] table, Dictionary<string, Fraction> P_coefficients,bool IsM = false)
        {
            if(IsM)
            {

            }
            else if(!IsM)
            {
                int p_count = 0;
                Dictionary<string, Fraction> fourth_row = new Dictionary<string, Fraction>();
                for (int columns = 1; columns < table.GetLength(1); columns++)
                {
                    Fraction scalar_product = table[0, 0] as Fraction;
                    for (int rows = 0; rows < table.GetLength(0); rows++)
                    {
                        //Console.WriteLine((table[rows, columns] as Fraction).ToString());
                        scalar_product += (table[rows, columns] as Fraction) * (table[rows, 0] as Fraction);
                    }
                    scalar_product -= P_coefficients[$"P{p_count++}"];

                    if (p_count > 0)
                    {
                        fourth_row.Add($"P{p_count - 1}", scalar_product);
                    }
                    Console.WriteLine(scalar_product);

                }
                string new_basis_vector = GetVectorThatGoesToBasis(fourth_row);

                int removing_vector_row = (GetVectorThatRemovesFromBasis(table, new_basis_vector));
                PrintMatrix(table);
                Console.WriteLine("***");
                Gauss(table,removing_vector_row, new_basis_vector.Substring(1));
            }
        }

        private string GetVectorThatGoesToBasis(Dictionary<string, Fraction> fourth_row, bool isMax=true)
        {
            string vector = "";
            if (isMax)
            {
                Fraction abs_max = new Fraction();
                foreach(var kvp in fourth_row)
                {
                    if(kvp.Value.ToDouble() < 0)
                    {
                        if (Math.Abs(kvp.Value.ToDouble()) > Math.Abs(abs_max.ToDouble()))
                        {
                            abs_max = kvp.Value;
                            vector = kvp.Key;
                        }
                    }
                }
            }
            else if(!isMax)
            {
                Fraction abs_max = new Fraction();
                foreach (var kvp in fourth_row)
                {
                    if (kvp.Value.ToDouble() > abs_max.ToDouble())
                    {
                        abs_max = kvp.Value;
                        vector = kvp.Key;
                    }
                }
            }
            return vector;
        }

        private int GetVectorThatRemovesFromBasis(object[,]table, string new_basis_vector)
        {
            int removing_vector_row = 0;

            int number = int.Parse(new_basis_vector.Substring(1));

            Fraction fraction = (table[0, 1] as Fraction) / (table[0, number + 1] as Fraction);
            Console.WriteLine();

            for(int row = 0; row<table.GetLength(0); row++)
            {
                var napr_vect = (table[row, 1] as Fraction) / (table[row, number + 1] as Fraction);

                if(napr_vect.ToDouble()>0)
                {
                    if (napr_vect < fraction)
                    {
                        removing_vector_row = row;
                        fraction = napr_vect;
                    }
                }
            }
            return removing_vector_row;
        }

        private void Gauss(object[,]table, int old_vector_row_number, string new_vector)
        {
            var element = table[old_vector_row_number, int.Parse(new_vector)+1] as Fraction;
            table[old_vector_row_number, int.Parse(new_vector)+1] = new Fraction(1, 1);

            for(int column = 1; column<table.GetLength(1); column++)
            {
                if(column!=int.Parse(new_vector)+1)
                {
                    table[old_vector_row_number, column] = (table[old_vector_row_number, column]) as Fraction / element;
                }
                //ДАЛІ РОБИТИ ДОДАВАННЯ РЯДОЧКІВ
            }

            PrintMatrix(table);

            Console.WriteLine("***");

            for (int row_to_up = old_vector_row_number-1; row_to_up>=0; row_to_up--)
            {

                Console.WriteLine((table[row_to_up, int.Parse(new_vector) + 1] as Fraction).ToString());
            }

        }

        private void PrintMatrix(object[,] table) 
        {
            for(int row = 0; row<table.GetLength(0); row++)
            {
                for(int j = 0; j<table.GetLength(1); j++)
                {
                    Console.Write(table[row, j].ToString() + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
