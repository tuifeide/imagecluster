using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageClustering
{
    class Matrix
    {
        double[,] matrix;
        int m, n;
        public Matrix(int i, int j)
        {
            m = i;
            n = j;
            matrix = new double[i, j];
        }
        public double this[int i, int j]
        {
            get
            {
                return matrix[i, j];
            }
            set
            {
                matrix[i, j] = value;
            }
        }
        public double[] this[int i]
        {
            get
            {
                double[] t = new double[m];
                for (int j = 0; j < m; j++)
                {
                    t[j] = matrix[j, i];
                }
                return t;
            }
            //set
            //{
            //    matrix[i] = value;
            //}
        }
        public int getRow()
        {
            return m;
        }
        public int getCol()
        {
            return n;
        }
        public double[] getDistance(double[] m1)
        {
            double d = 0;
            double[] Ed = new double[n];
            for (int i = 0; i < n; i++)
            {
                d = 0;
                for (int j = 0; j < m; j++)
                {
                    d += (m1[j] - matrix[j, i]) * (m1[j] - matrix[j, i]);
                }
                Ed[i] = d / m;
            }
            return Ed;
        }
    }
}
