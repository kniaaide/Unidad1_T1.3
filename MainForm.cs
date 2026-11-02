using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Unidad1.Analisis;

namespace Unidad1
{
    /// <summary>
    /// Formulario de la Tarea 1.3. Mide comparaciones y tiempo promedio de la
    /// búsqueda lineal y la binaria en el peor caso (valor ausente).
    /// </summary>
    public class MainForm : Form
    {
        private readonly Label lblRepeticiones = new Label
        {
            Text = "Repeticiones por medición:",
            Location = new Point(20, 22),
            AutoSize = true
        };

        private readonly NumericUpDown nudRepeticiones = new NumericUpDown
        {
            Location = new Point(200, 18),
            Size = new Size(90, 25),
            Minimum = 1,
            Maximum = 1000,
            Value = 100
        };

        private readonly Button btnMedir = new Button
        {
            Text = "Medir",
            Location = new Point(310, 16),
            Size = new Size(120, 30)
        };

        private readonly DataGridView dgv = new DataGridView
        {
            Location = new Point(20, 60),
            Size = new Size(740, 340),
            ReadOnly = true,
            AllowUserToAddRows = false,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        private readonly Label lblConclusion = new Label
        {
            Location = new Point(20, 415),
            Size = new Size(740, 90),
            Text = "Peor caso (valor ausente): la búsqueda lineal realiza n comparaciones, mientras que " +
                   "la binaria realiza aproximadamente log2(n). La binaria exige un arreglo ordenado; " +
                   "la lineal no tiene esa precondición."
        };

        public MainForm()
        {
            Text = "Tarea 1.3 - Búsqueda lineal vs. búsqueda binaria";
            ClientSize = new Size(780, 520);
            StartPosition = FormStartPosition.CenterScreen;

            ConfigurarTabla();
            Controls.AddRange(new Control[]
            {
                lblRepeticiones, nudRepeticiones, btnMedir, dgv, lblConclusion
            });

            btnMedir.Click += (s, e) => Medir();
        }

        private void ConfigurarTabla()
        {
            dgv.Columns.Add("colN", "n");
            dgv.Columns.Add("colCompLineal", "Comparaciones lineal");
            dgv.Columns.Add("colCompBinaria", "Comparaciones binaria");
            dgv.Columns.Add("colLog", "log2(n)");
            dgv.Columns.Add("colTiempoLineal", "Tiempo lineal promedio (ms)");
            dgv.Columns.Add("colTiempoBinaria", "Tiempo binaria promedio (ms)");
        }

        private void Medir()
        {
            int[] tamanios = { 1_000, 10_000, 100_000, 1_000_000 };
            int repeticiones = (int)nudRepeticiones.Value;

            Cursor = Cursors.WaitCursor;
            btnMedir.Enabled = false;
            try
            {
                dgv.Rows.Clear();

                foreach (int n in tamanios)
                {
                    // Arreglo ordenado: prerrequisito de la búsqueda binaria.
                    int[] datos = Enumerable.Range(1, n).ToArray();

                    // Peor caso: el valor no existe, así que se recorre la colección completa.
                    int objetivo = n + 1;

                    long compLineal = 0;
                    long compBinaria = 0;

                    var cronoLineal = Stopwatch.StartNew();
                    for (int r = 0; r < repeticiones; r++)
                    {
                        compLineal = Busquedas.BusquedaLineal(datos, objetivo).Comparaciones;
                    }
                    cronoLineal.Stop();

                    var cronoBinaria = Stopwatch.StartNew();
                    for (int r = 0; r < repeticiones; r++)
                    {
                        compBinaria = Busquedas.BusquedaBinaria(datos, objetivo).Comparaciones;
                    }
                    cronoBinaria.Stop();

                    dgv.Rows.Add(
                        n,
                        compLineal,
                        compBinaria,
                        Math.Log2(n).ToString("F2"),
                        (cronoLineal.Elapsed.TotalMilliseconds / repeticiones).ToString("F4"),
                        (cronoBinaria.Elapsed.TotalMilliseconds / repeticiones).ToString("F4"));
                }
            }
            finally
            {
                Cursor = Cursors.Default;
                btnMedir.Enabled = true;
            }
        }
    }
}
