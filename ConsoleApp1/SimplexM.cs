using Mehroz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
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
    internal class SimplexM
    {


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

        public void Solve(object[,] test_array, Dictionary<string, Fraction> coeff, List<string> artifical_variables, bool isMaxF = true)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            var basis = new List<string>();
            int iter = 1;
            var number_row = new Dictionary<string, Fraction>();
            var M_row = new Dictionary<string, Fraction>();

            bool IsMInBasis = false;
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("Ітерація №{0}", iter++);
            Console.WriteLine(new string('-', 80));
            PrintMatrix(test_array);

            foreach (var b in basis)
            {
                if (Regex.IsMatch(b, @".*M"))
                {
                    IsMInBasis = true;
                    break;
                }
                else
                {
                    IsMInBasis = false;
                }
            }


            while (!BasisMultiplyOnVectors(test_array, coeff, ref number_row, ref M_row, basis, artifical_variables, IsM: IsMInBasis, isMax:isMaxF))
            {
                Console.WriteLine(new string('-', 80));
                Console.WriteLine("Ітерація №{0}", iter++);
                Console.WriteLine(new string('-', 80));
                PrintMatrix(test_array);

                foreach (var b in basis)
                {
                    if (Regex.IsMatch(b, @".*M"))
                    {
                        IsMInBasis = true;
                        break;
                    }
                    else
                    {
                        IsMInBasis = false;
                    }
                }

            }
        }

        private bool BasisMultiplyOnVectors(object[,] table, Dictionary<string, Fraction> P_coefficients, ref Dictionary<string, Fraction> number_row, ref Dictionary<string, Fraction> M_row, List<string> basis, List<string> art_var, bool IsM = false, bool isMax=true)
        {
            
            if(IsM)
            {
                int p_count = 0;
                M_row = new Dictionary<string, Fraction>();
                for (int columns = 2; columns < table.GetLength(1); columns++)
                {
                    Fraction scalar_product = new Fraction();
                    for (int rows = 2; rows < table.GetLength(0); rows++)
                    {
                        //Console.WriteLine((table[rows, columns] as Fraction).ToString());
                        scalar_product += (table[rows, columns] as Fraction) * (table[rows, 1] as Fraction);
                    }
                    scalar_product -= P_coefficients[$"P{p_count++}"];

                    if (p_count > 0)
                    {
                        M_row.Add($"P{p_count - 1}", scalar_product);
                    }
                    //Console.WriteLine(scalar_product);

                }

                string new_basis_vector = GetVectorThatGoesToBasis(M_row);
                //PrintMatrix(table);


                int removing_vector_row = (GetVectorThatRemovesFromBasis(table, new_basis_vector, out string vector));

                Console.WriteLine($"З базису виводиться вектор {vector}\nУ базис вводиться вектор {new_basis_vector}");

                //PrintMatrix(table);
                Gauss(table, removing_vector_row, int.Parse(new_basis_vector.Substring(1)), new_basis_vector, P_coefficients);
                FillBasisArray(table, basis, art_var);

            }
            else if(!IsM)
            {
                //DropColumn(ref table, art_var);
                int p_count = 0;
                number_row = new Dictionary<string, Fraction>();
                for (int columns = 2; columns < table.GetLength(1); columns++)
                {
                    Fraction scalar_product = new Fraction();
                    for (int rows = 2; rows < table.GetLength(0); rows++)
                    {
                        //Console.WriteLine((table[rows, columns] as Fraction).ToString());
                        scalar_product += (table[rows, columns] as Fraction) * (table[rows, 1] as Fraction);
                    }
                    scalar_product -= P_coefficients[$"P{p_count++}"];

                    if (p_count > 0)
                    {
                        number_row.Add($"P{p_count - 1}", scalar_product);
                    }
                    //Console.WriteLine(scalar_product);

                }

                if (isMax && isOptimalMax(number_row) || (!isMax && isOptimalMin(number_row)))
                {
                    Console.WriteLine(new string('-', 80));
                    PrintMatrix(table);
                    Console.WriteLine("Оптимальний план знайдено.");
                    return true;
                }
                
                string new_basis_vector = GetVectorThatGoesToBasis(number_row, isMax);
                //PrintMatrix(table);

                
                int removing_vector_row = (GetVectorThatRemovesFromBasis(table, new_basis_vector, out string vector));
                Console.WriteLine($"З базису виводиться вектор {vector}\nУ базис вводиться вектор {new_basis_vector}");

                //PrintMatrix(table);
                Gauss(table,removing_vector_row, int.Parse(new_basis_vector.Substring(1)), new_basis_vector, P_coefficients);
            }
            return false;
        }

        private void DropColumn(ref object[,] array, List<string> art_var)
        {
            for(int column = 0; column<array.GetLength(1); column++)
            {
                if (art_var.Contains(array[0, column]))
                {
                    DropColumn(ref array, column);
                }
            }
        }

        static void DropColumn(ref object[,] array, int columnIndex)
        {
            // Get the lengths of the original array
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            // Create a new array with one less column
            object[,] newArray = new object[rows, cols - 1];

            // Copy elements from the original array to the new array
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0, k = 0; j < cols; j++)
                {
                    if (j != columnIndex)
                    {
                        newArray[i, k] = array[i, j];
                        k++;
                    }
                }
            }
            array = newArray;
        }

        private void FillBasisArray(object[,] table, List<string> basis, List<string> art_var)
        {
            basis.Clear();
            for(int row = 2; row< table.GetLength(0); row++)
            {
                if (art_var.Contains(table[row, 0] as string))
                {
                    basis.Add(table[row, 0] as string + "M");

                }
                else
                {
                    basis.Add(table[row, 0] as string);
                }
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
                    if (kvp.Value >0 && kvp.Value.ToDouble() >= abs_max && kvp.Key != "P0")
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
