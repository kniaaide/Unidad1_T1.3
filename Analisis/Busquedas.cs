using System;

namespace Unidad1.Analisis
{
    /// <summary>
    /// Implementaciones de búsqueda que devuelven el índice encontrado y el
    /// número de comparaciones realizadas.
    /// </summary>
    public static class Busquedas
    {
        /// <summary>Búsqueda lineal. Tiempo O(n) en el peor caso. Espacio O(1).</summary>
        public static (int Indice, long Comparaciones) BusquedaLineal(int[] arreglo, int objetivo)
        {
            long comparaciones = 0;

            for (int i = 0; i < arreglo.Length; i++)
            {
                comparaciones++;
                if (arreglo[i] == objetivo)
                    return (i, comparaciones);
            }

            return (-1, comparaciones);
        }

        /// <summary>
        /// Búsqueda binaria iterativa sobre arreglo ordenado ascendentemente.
        /// Tiempo O(log n) en el peor caso. Espacio O(1).
        /// </summary>
        public static (int Indice, long Comparaciones) BusquedaBinaria(int[] arreglo, int objetivo)
        {
            int izquierda = 0;
            int derecha = arreglo.Length - 1;
            long comparaciones = 0;

            while (izquierda <= derecha)
            {
                // Fórmula segura para evitar desbordamiento de enteros.
                int medio = izquierda + (derecha - izquierda) / 2;
                comparaciones++;

                if (arreglo[medio] == objetivo)
                    return (medio, comparaciones);

                if (arreglo[medio] < objetivo)
                    izquierda = medio + 1;   // Descarta la mitad izquierda
                else
                    derecha = medio - 1;     // Descarta la mitad derecha
            }

            return (-1, comparaciones);
        }
    }
}
