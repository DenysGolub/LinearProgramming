using Mehroz;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
            {"", "", "", "P1", "P2", "P3", "P4", "P5"},
            {"Базис", "Сб", "P0", "3", "7", "0", "0", "0"},
            { "P3", new Fraction(0,1), new Fraction(14,1), new Fraction(1,1), new Fraction(2,1), new Fraction(1,1), new Fraction(0,1), new Fraction(0,1)},
            { "P4", new Fraction(0,1), new Fraction(15,1), new Fraction(-5,1), new Fraction(7, 1), new Fraction(0,1), new Fraction(1,1), new Fraction(0,1)},
            { "P5", new Fraction(0,1), new Fraction(12, 1), new Fraction(2,1), new Fraction(3,1), new Fraction(0,1), new Fraction(0,1), new Fraction(1,1)}
        };

        private bool isOptimalMax(Dictionary<string, Fraction> fourth_row)
        {
            foreach(var pair in fourth_row)
            {
                if(pair.Value<0 && pair.Key!= "P0")
                {
                    return false;
                }
            }

            return true;
        }

        private bool isOptimalMin(Dictionary<string, Fraction> fourth_row)
        {
            foreach(var pair in fourth_row)
            {
                if(pair.Value>0 && pair.Key != "P0")
                {
                    return false;
                }
            }
            return true;
        }

        public void Solve(bool isMax=true)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var coeff = new Dictionary<string, Fraction>()
            {
                {"P0", new Fraction(0,1) },
                {"P1", new Fraction(3,1) },
                {"P2", new Fraction(7,1) },
                {"P3", new Fraction(0,1) },
                {"P4", new Fraction(0,1) },
                {"P5", new Fraction(0,1) },
            };

            var basis = new Dictionary<string, Fraction>()
            {

            };
            int iter = 1;
            var fourth_row = new Dictionary<string, Fraction>();
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("Ітерація №{0}", iter++);
            Console.WriteLine(new string('-', 80));
            PrintMatrix(test_array);

            while (!BasisMultiplyOnVectors(test_array, coeff, ref fourth_row))
            {
                Console.WriteLine(new string('-', 80));
                Console.WriteLine("Ітерація №{0}", iter++);
                Console.WriteLine(new string('-', 80));
                PrintMatrix(test_array);
            }
          
        }

        private bool BasisMultiplyOnVectors(object[,] table, Dictionary<string, Fraction> P_coefficients, ref Dictionary<string, Fraction> fourth_row, bool IsM = false, bool isMax=true)
        {
            
            if(IsM)
            {

            }
            else if(!IsM)
            {
                int p_count = 0;
                fourth_row = new Dictionary<string, Fraction>();
                for (int columns = 2; columns < table.GetLength(1); columns++)
                {
                    Fraction scalar_product = table[2, 1] as Fraction;
                    for (int rows = 2; rows < table.GetLength(0); rows++)
                    {
                        //Console.WriteLine((table[rows, columns] as Fraction).ToString());
                        scalar_product += (table[rows, columns] as Fraction) * (table[rows, 1] as Fraction);
                    }
                    scalar_product -= P_coefficients[$"P{p_count++}"];

                    if (p_count > 0)
                    {
                        fourth_row.Add($"P{p_count - 1}", scalar_product);
                    }
                    //Console.WriteLine(scalar_product);

                }

                if (isMax && isOptimalMax(fourth_row) || (!isMax && isOptimalMin(fourth_row)))
                {
                    Console.WriteLine("Оптимальний план знайдено.");
                    return true;
                }
                
                string new_basis_vector = GetVectorThatGoesToBasis(fourth_row);
                //PrintMatrix(table);

                
                int removing_vector_row = (GetVectorThatRemovesFromBasis(table, new_basis_vector, out string vector));
                Console.WriteLine($"З базису виводиться вектор {vector}\nУ базис вводиться вектор {new_basis_vector}");

                //PrintMatrix(table);
                Gauss(table,removing_vector_row, int.Parse(new_basis_vector.Substring(1)), new_basis_vector, P_coefficients);
            }
            return false;
        }

        private string GetVectorThatGoesToBasis(Dictionary<string, Fraction> fourth_row, bool isMax=true)
        {
            string vector = "";
            if (isMax)
            {
                Fraction abs_max = new Fraction();
                foreach(var kvp in fourth_row)
                {
                    if(kvp.Value.ToDouble() < 0 && kvp.Key!= "P0")
                    {
                        if (Math.Abs(kvp.Value.ToDouble()) >= Math.Abs(abs_max.ToDouble()))
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

        private int GetVectorThatRemovesFromBasis(object[,]table, string new_basis_vector, out string vector)
        {
            int removing_vector_row = 0;
            vector = "";

            int number = int.Parse(new_basis_vector.Substring(1));

            Fraction fraction = new Fraction(Int32.MaxValue, 1);

            for(int row = 2; row<table.GetLength(0); row++)
            {
                var napr_vect = (table[row, 2] as Fraction) / (table[row, number + 2] as Fraction);

                if(napr_vect.ToDouble()>0)
                {
                    if (napr_vect <= fraction)
                    {
                        removing_vector_row = row;
                        fraction = napr_vect;
                        vector = table[row, 0] as string;
                        
                    }
                }
            }
            return removing_vector_row;
        }

        private void Gauss(object[,]table, int old_vector_row_number, int new_vector_column, string new_vector_name, Dictionary<string, Fraction> p_coeff)
        {
            var element = table[old_vector_row_number, new_vector_column+2] as Fraction;
            table[old_vector_row_number, new_vector_column+2] = new Fraction(1, 1);


            for(int column = 2; column<table.GetLength(1); column++)
            {
                if(column!=new_vector_column+2)
                {
                    table[old_vector_row_number, column] = (table[old_vector_row_number, column]) as Fraction / element;
                }
            }

            for(int row=2; row<table.GetLength(0); row++)
            {
                Fraction multiplier = table[row, new_vector_column + 2] as Fraction;
                if (row!=old_vector_row_number)
                {
                    AddRow(table, old_vector_row_number, row, multiplier*(-1));
                }
            }



            table[old_vector_row_number, 0] = new_vector_name;
            table[old_vector_row_number, 1] = p_coeff[new_vector_name];
            //PrintMatrix(table);

        }

        private void AddRow(object[,]table, int from_row, int to_row, Fraction multiplier)
        {
            for (int columns=2; columns<table.GetLength(1); columns++)
            {
                var b = table[to_row, columns];
                var a = (table[from_row, columns] as Fraction);
                table[to_row, columns] = table[to_row, columns] as Fraction + (table[from_row, columns] as Fraction) * (multiplier);
                var c = table[to_row, columns];
            }
        }

        private void PrintMatrix(object[,] table)
        {
            const int columnWidth = 10; // Width of each column
            int rows = table.GetLength(0);
            int columns = table.GetLength(1);

           /* // Print header
            Console.Write(" ".PadRight(columnWidth)); // Space for row labels
            for (int col = 0; col < columns; col++)
            {
                Console.Write($"P{col + 1}".PadRight(columnWidth));
            }
            Console.WriteLine();*/

            // Print matrix
            for (int row = 0; row < rows; row++)
            {
                //Console.Write($"P{row + 1}".PadRight(columnWidth)); // Row label
                for (int col = 0; col < columns; col++)
                {
                    string value = table[row, col].ToString();
                    Console.Write(value.PadRight(columnWidth));
                }
                Console.WriteLine();
            }
        }

    }
}
