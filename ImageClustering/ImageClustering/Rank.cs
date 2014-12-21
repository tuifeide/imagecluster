using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageClustering
{
    class Rank
    {
        public static Matrix rank(int[] idx, Matrix link)
        {
            //StreamWriter scores = new StreamWriter("D:\\ex\\data\\scores.txt", true);
            int linkrow = link.getRow();
            int linkcol = link.getCol();
            Matrix score = new Matrix(2, linkrow + linkcol);
            double[] sum = new double[2];
            for (int i = 0; i < linkrow; i++)
            {
                if (idx[i] == 0)
                {
                    for (int j = 0; j < linkcol; j++)
                    {
                        score[0, i] += link[i, j];
                        score[0, j + linkrow] += link[i, j];
                        sum[0] += link[i, j];
                    }
                }
                else
                {
                    for (int j = 0; j < linkcol; j++)
                    {
                        score[1, i] += link[i, j];
                        score[1, j + linkrow] += link[i, j];
                        sum[1] += link[i, j];
                    }
                }
            }
            for (int i = 0; i < 2; i++)
            {
                for (int j = linkrow; j < linkrow + linkcol; j++)
                {
                    score[i, j] /= sum[i];
                }
            }
            for (int i = 0; i < 2; i++)
            {
                sum[i] = 0;
            }
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < linkrow; j++)
                {
                    score[i, j] = 0;
                    for (int k = linkrow; k < linkrow + linkcol; k++)
                    {
                        score[i, j] += link[j, k - linkrow] * score[i, k];
                    }
                    sum[i] += score[i, j];
                }
            }
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < linkrow; j++)
                {
                    score[i, j] /= sum[i];
                }
            }
            return score;
        }
    }
}
