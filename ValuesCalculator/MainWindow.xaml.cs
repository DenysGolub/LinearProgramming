using Gu.Wpf.DataGrid2D;
using Mehroz;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ValuesCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        object[,] table = new Fraction[0, 0];
        public MainWindow()
        {
            InitializeComponent();

            table = ResizeArray(table, 2, 2);
            matrix.SetArray2D(table);

            List<string> list_col = new List<string>();
            for(int i =0; i < table.GetLength(1); i++)
            {
                list_col.Add($"P{i}");
            }

            matrix.SetColumnHeadersSource(list_col);

            List<string> list_row = new List<string>();
            for (int i = 0; i < table.GetLength(0); i++)
            {
                list_row.Add($"{i}");
            }

            matrix.SetRowHeadersSource(list_row);

        }


        private object[,] ResizeArray (object[,] original, int rows, int cols)
        {
            var newArray = new object[rows, cols];

            for (int i = 0; i < newArray.GetLength(0); i++)
            {

                for (int j = 0; j < newArray.GetLength(1); j++)
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

        private void matrix_ValueInCellChanged(object sender, DataGridCellEditEndingEventArgs e)
        {
            var current = e.EditingElement;
            DataGridColumn col1 = e.Column;
            DataGridRow row1 = e.Row;

           
            try
            {
                string number = (current as TextBox).Text;
                int denom = 1;
                int up = 0;
                var match = Regex.Matches(number, @"(\d*)/(\d*)");


                up = int.Parse(match[0].Groups[1].Value);
                denom = int.Parse(match[0].Groups[2].Value);

                if (match[0].Groups[2].Value == "")
                {
                    denom = 1;
                }
                Console.WriteLine();

                table[int.Parse(row1.Header.ToString()), int.Parse(col1.Header.ToString().Substring(1)) ] = new Fraction(up, denom);
            }
            catch(Exception ex)
            {

            }
            Console.WriteLine();
            
            UpdateMatrix();
        }

        private void UpdateMatrix()
        {

        }
    }
}