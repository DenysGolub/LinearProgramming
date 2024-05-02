using Mehroz;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class NewSimplex
    {
       
        public void Solve(object[,] table, List<string> artifical_variables, Dictionary<string, Fraction> P_coefficients, bool IsMax=true)
        {
            List<string> basis = new List<string>();
            bool IsArt = artifical_variables.Count > 0 ? true : false;
            AddCheckingRows(ref table, IsArt);


            if (IsMax)
            {
                int checking_row = 0;
                while(true)
                {
                    FillCheckingRows(ref table, artifical_variables, P_coefficients);
                    FillBasisArray(table, basis, artifical_variables);
                    bool isArtInBasis = IsArtInBasis(table, artifical_variables, basis);

                    if (!isArtInBasis)
                    {
                        DropArtificalCheckingRowsAndColumns(ref table, artifical_variables);
                    }

                    GetCheckingRowsIndex(table, out int ordinary, out int art);
                    checking_row = isArtInBasis ? art : ordinary;


                    if (isOptimalMax(table, checking_row))
                    {
                        break;
                    }


                    int column_with_new_vector = GetNewBasisVector(table, checking_row, out string vector);

                    Console.WriteLine("У базис вводиться вектор {0}", vector);

                    int row_with_old_vector = GetRemovingVector(table, checking_row, column_with_new_vector, out string removing_vector);

                    Console.WriteLine("З базису виводиться {0}", removing_vector);

                    Gauss(table, row_with_old_vector, column_with_new_vector, vector, P_coefficients);
                }
            }
            else if(!IsMax)
            {
                int checking_row = 0;
                while (true)
                {
                    FillCheckingRows(ref table, artifical_variables, P_coefficients);
                    FillBasisArray(table, basis, artifical_variables);
                    bool isArtInBasis = IsArtInBasis(table, artifical_variables, basis);

                    if(!isArtInBasis)
                    {
                        DropArtificalCheckingRowsAndColumns(ref table, artifical_variables);
                    }

                    GetCheckingRowsIndex(table, out int ordinary, out int art);
                    checking_row = isArtInBasis ? art : ordinary;


                    if (isOptimalMin(table, checking_row))
                    {
                        break;
                    }


                    int column_with_new_vector = GetNewBasisVector(table, checking_row, out string vector, false);

                    Console.WriteLine("У базис вводиться вектор {0}", vector);

                    int row_with_old_vector = GetRemovingVector(table, checking_row, column_with_new_vector, out string removing_vector, false);

                    Console.WriteLine("З базису виводиться {0}", removing_vector);

                    Gauss(table, row_with_old_vector, column_with_new_vector, vector, P_coefficients);
                }
            }

            Console.WriteLine();
            Console.WriteLine(new string('*', 30) + "Розв'язок" + new string('*', 30));
            Console.WriteLine();
            PrintMatrix(table);
            Console.WriteLine(new string('-', 50));

            Console.WriteLine("Оптимальний план знайдено");
            
            
            

        }


        private void DropColumn(ref object[,] array, List<string> art_var)
        {
            for (int column = 0; column < array.GetLength(1); column++)
            {
                if (art_var.Contains(array[0, column]))
                {
                    DropColumn(ref array, column);
                }
            }
        }

        private void DropArtificalCheckingRowsAndColumns(ref object[,] array, List<string> art_var)
        {
            //array = ResizeArray(array, array.GetLength(0) - 1, array.GetLength(1));
            int originalColumnCount = array.GetLength(1);
            List<int> columnsToRemove = new List<int>();

            // Find columns to remove
            for (int column = 0; column < originalColumnCount; column++)
            {
                string columnHeader = array[0, column]?.ToString(); // Assuming headers are strings

                if (columnHeader != null && art_var.Contains(columnHeader))
                {
                    columnsToRemove.Add(column);
                }
            }

            // If no columns to remove, exit
            if (columnsToRemove.Count == 0)
                return;

            int newColumnCount = originalColumnCount - columnsToRemove.Count;

            // Create a new array without the columns to be removed
            object[,] newArray = new object[array.GetLength(0), newColumnCount];
            int newIndex = 0;

            for (int oldColumn = 0; oldColumn < originalColumnCount; oldColumn++)
            {
                if (!columnsToRemove.Contains(oldColumn))
                {
                    for (int row = 0; row < array.GetLength(0); row++)
                    {
                        newArray[row, newIndex] = array[row, oldColumn];
                    }
                    newIndex++;
                }
            }

            //newArray = ResizeArray(newArray, newArray.GetLength(0) - 1, newArray.GetLength(1));

            array = newArray;
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


        private int GetRemovingVector(object[,]table, int checking_row, int column_with_new_vector, out string removing_vector, bool isMax=true)
        {
            int removing_vector_row = 0;
            removing_vector = "";

            GetCheckingRowsIndex(table, out int ord, out int art);
            Fraction fraction = null;
            fraction = new Fraction(Int32.MaxValue, 1);


            for (int row = 2; row < ord; row++)
            {
                if((table[row, column_with_new_vector] as Fraction)!=0)
                {
                    var napr_vect = (table[row, 2] as Fraction) / (table[row, column_with_new_vector] as Fraction);

                    if (napr_vect.ToDouble() > 0 && napr_vect < fraction)
                    {
                        removing_vector_row = row;
                        fraction = napr_vect;
                        removing_vector = table[row, 0] as string;
                    }
                }
                
            }
            return removing_vector_row;
        }

        private int GetNewBasisVector(object[,]table, int checking_row, out string vector, bool isMax=true)
        {
            Fraction fraction = null;
            fraction = new Fraction(0, 1);

            int new_vector_column = 0;


            for (int columns = 3; columns < table.GetLength(1); columns++)
            {
                if ((table[checking_row, columns] as Fraction) < 0 && isMax)
                {
                    if(Math.Abs((table[checking_row, columns] as Fraction).ToDouble())>Math.Abs(fraction.ToDouble()))
                    {
                        fraction = table[checking_row, columns] as Fraction;
                        new_vector_column = columns;
                    }
                }
                else if ((table[checking_row, columns] as Fraction) > 0 && !isMax)
                {
                    if((table[checking_row, columns] as Fraction)>fraction)
                    {
                        fraction = table[checking_row, columns] as Fraction;
                        new_vector_column = columns;
                    }
                }
            }
            vector = table[0, new_vector_column] as string;
            return new_vector_column;
        }

        private void GetCheckingRowsIndex(object[,] table, out int ordinary, out int artifical)
        {
            ordinary = 0;

            artifical = 0;
            for(int i=0; i<table.GetLength(0); i++)
            {
                if (table[i,0] == "m+1")
                {
                    ordinary = i;
                }
                else if (table[i,0]== "m+2")
                {
                    artifical = i;
                }
            }
        }

        public void FillCheckingRows(ref object[,] table, List<string> artifical_variables, Dictionary<string, Fraction> P_coefficients)
        {
            //PrintMatrix(table);


            int p_count = 0;
            bool IsArt = artifical_variables.Count > 0 ? true : false;

          

            GetCheckingRowsIndex(table, out int checking, out int art);

            for (int columns = 2; columns < table.GetLength(1); columns++)
            {

                Fraction scalar_product = new Fraction();

                Fraction articical_scalar = new Fraction();
                for (int rows = 2; rows < checking; rows++)
                {
                    if (table[rows, columns] is Fraction && artifical_variables.Contains(table[rows, 0]) == true)
                    {
                        articical_scalar += (table[rows, columns] as Fraction) * (table[rows, 1] as Fraction);
                    }
                    else if (table[rows, columns] is Fraction)
                    {

                        var a = (table[rows, columns]);
                        var b = (table[rows, 1]);
                        scalar_product += (table[rows, columns] as Fraction) * (table[rows, 1] as Fraction);
                    }
                }
                if(artifical_variables.Contains($"P{p_count}"))
                {
                    articical_scalar -= P_coefficients[$"P{p_count}"]; 
                }
                else
                {
                    scalar_product -= P_coefficients[$"P{p_count}"];
                }

                p_count++;
                if (IsArt)
                {
                    table[art, columns] = articical_scalar;
                    table[checking, columns] = scalar_product;
                }
                else if(!IsArt)
                {
                    table[checking, columns] = scalar_product;
                }
            }
            PrintMatrix(table);
        }

        private bool IsArtInBasis(object[,]table, List<string> artifical_variables, List<string> basis)
        {
            foreach(var b in basis)
            {
                if (Regex.IsMatch(b, @".*M"))
                {
                    return true;
                }
            }
            return false;
        }
       
        private void ChangeBasis(object[,] table, bool IsArt)
        {

        }


        private bool isOptimalMax(object[,] table, int row)
        {
            for(int column = 3; column<table.GetLength(1); column++)
            {
                if ((table[row, column] as Fraction) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool isOptimalMin(object[,] table, int row)
        {
            for (int column = 3; column < table.GetLength(1); column++)
            {
                if ((table[row, column] as Fraction) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void PrintMatrix(object[,] table)
        {
            const int columnWidth = 10; // Width of each column
            int rows = table.GetLength(0);
            int columns = table.GetLength(1);

            // Print matrix
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    string value = table[row, col].ToString();
                    Console.Write(value.PadRight(columnWidth));
                }
                Console.WriteLine();

                // Insert dashed line after the second row
                if (row == 1)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        Console.Write(new string('-', 10).PadRight(columnWidth));
                    }
                    Console.WriteLine();
                }
            }
        }

        private void AddCheckingRows(ref object[,] matrix, bool IsArtificalVariable=false)
        {
            if(IsArtificalVariable)
            {
                matrix = ResizeArray(matrix, matrix.GetLength(0) + 2, matrix.GetLength(1));
                matrix[matrix.GetLength(0)-2, 0] = "m+1";
                matrix[matrix.GetLength(0) - 1, 0] = "m+2";

                matrix[matrix.GetLength(0) - 2, 1] = "-";
                matrix[matrix.GetLength(0) - 1, 1] = "-";

            }
            else if(!IsArtificalVariable)
            {
                matrix = ResizeArray(matrix, matrix.GetLength(0) + 1, matrix.GetLength(1));

                matrix[matrix.GetLength(0) - 1, 0] = "m+1";
                matrix[matrix.GetLength(0) - 1, 1] = "-";

            }
        }

        private object[,] ResizeArray<T>(T[,] original, int rows, int cols)
        {
            var newArray = new object[rows, cols];

            for(int i =0; i<newArray.GetLength(0); i++)
            {
               
                for(int j =0; j<newArray.GetLength(1); j++)
                {
                    newArray[i, j] = 0;

                }
            }

            int minRows = Math.Min(rows, original.GetLength(0));
            int minCols = Math.Min(cols, original.GetLength(1));
            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
                    newArray[i, j] = original[i, j];
            return newArray;
        }

        private void FillBasisArray(object[,] table, List<string> basis, List<string> art_var, bool isM=false)
        {
            basis.Clear();
            int minus_row = isM ? table.GetLength(0)-3: table.GetLength(0)-2;
            for (int row = 2; row <= minus_row; row++)
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
        private void Gauss(object[,] table, int old_vector_row_number, int new_vector_column, string new_vector_name, Dictionary<string, Fraction> p_coeff)
        {
            var element = table[old_vector_row_number, new_vector_column] as Fraction;
            table[old_vector_row_number, new_vector_column] = new Fraction(1, 1);

            GetCheckingRowsIndex(table, out int ordinary, out int art);

            for (int column = 2; column < table.GetLength(1); column++)
            {
                if (column != new_vector_column)
                {
                    table[old_vector_row_number, column] = (table[old_vector_row_number, column]) as Fraction / element;
                }
            }

            for (int row = 2; row < ordinary; row++)
            {
                Fraction multiplier = table[row, new_vector_column] as Fraction;
                if (row != old_vector_row_number)
                {
                    AddRow(table, old_vector_row_number, row, multiplier * (-1));
                }
            }



            table[old_vector_row_number, 0] = new_vector_name;
            table[old_vector_row_number, 1] = p_coeff[new_vector_name];
            //PrintMatrix(table);

        }

        private void AddRow(object[,] table, int from_row, int to_row, Fraction multiplier)
        {
            for (int columns = 2; columns < table.GetLength(1); columns++)
            {
                var b = table[to_row, columns];
                var a = (table[from_row, columns] as Fraction);
                table[to_row, columns] = table[to_row, columns] as Fraction + (table[from_row, columns] as Fraction) * (multiplier);
                var c = table[to_row, columns];
            }
        }
    }
}
