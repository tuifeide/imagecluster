using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageClustering
{
    class Program
    {

        static void Main(string[] args)
        {
            StreamReader files = new StreamReader("D:\\ex\\pre_flickr\\dic.txt");//("D:\\ex\\data\\re\\subgraph2.txt");
            StreamReader dicfile = new StreamReader("D:\\ex\\pre_flickr\\p.txt");//("D:\\ex\\data\\re\\pics.txt");
            StreamWriter clus = new StreamWriter("D:\\ex\\pre_flickr\\idx_tk.txt");//("D:\\ex\\data\\re\\idx.txt");
            StreamReader kmdata = new StreamReader("D:\\ex\\pre_flickr\\data.txt");//("D:\\ex\\data\\re\\kmdata.txt");
            StreamWriter scofile = new StreamWriter("D:\\ex\\pre_flickr\\scofile1_t.txt");//("D:\\ex\\data\\re\\scofile1.txt");
            StreamWriter scofile2 = new StreamWriter("D:\\ex\\pre_flickr\\scofile2_t.txt");//("D:\\ex\\data\\re\\scofile2.txt");
            //Hashtable images;
            int k = 2;// k clusters
            int m = 774, n = 783, p = 10368;
            int[] mk = new int[k];
            mk[0] = 445;
            mk[1] = 329;
            string str;
            str = kmdata.ReadLine();
            string[] a;
            a = str.Split(' ');
            //Console.Out.WriteLine(a[0].Length);
            Matrix links = new Matrix(m, n);
            Matrix imgfea = new Matrix(m, p);
            Hashtable dic = new Hashtable();
            Matrix socre;
            Matrix socre2;
            int di = 0, dj = 0;

            int[] idx = new int[m];

            double tras = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    tras = 0;
                    for (int l = 7; l > 1; l--)
                    {
                        tras += a[i * p + j][l] - 48;
                        tras /= 10;
                    }
                    tras += a[i * p + j][0] - 48;
                    imgfea[i, j] = tras;
                }
            }


            while (!dicfile.EndOfStream)
            {
                str = dicfile.ReadLine();
                a = str.Split('\n');
                dic.Add(a[0], di);
                di++;
            }
            while (!files.EndOfStream)
            {
                str = files.ReadLine();
                a = str.Split(' ');
                for (int i = 0; i < a.Length - 1; i++)
                {
                    links[(int)dic[a[i]], dj]++;
                }
                dj++;
            }
            //初始化
            //for (int i = 0; i < mk[0]; i++)
            //{
            //    idx[i] = 1;
            //}
            //for (int i = mk[0]; i < m; i++)
            //{
            //    idx[i] = 0;
            //}
            //for (int i = 0; i < m; i++)
            //{
            //    idx[i] = i % 2;
            //}
            Random ra = new Random();
            for (int i = 0; i < m; i++)
            {
                if (ra.NextDouble() > 0.5)
                {
                    idx[i] = 0;
                }
                else
                {
                    idx[i] = 1;
                }
            }
            //Iteration
            for (int it = 0; it < 30; it++)
            {

                socre = Rank.rank(idx, links);
                socre2 = Rank.rank(idx, imgfea);

                Matrix fea = new Matrix(4, m);
                //Matrix fea = new Matrix(2, m);
                genFea(idx, links, socre, fea, 0);
                genFea(idx, imgfea, socre2, fea, 2);


                Matrix cet = new Matrix(4, 2);
                int clus_c = 0;

                for (int i = 0; i < 2; i++)
                {
                    clus_c = 0;
                    for (int j = 0; j < m; j++)
                    {
                        if (idx[j] == i)
                        {
                            clus_c++;
                            for (int l = 0; l < 4; l++)
                            {
                                cet[l, i] += fea[l, j];
                            }
                        }
                    }
                    for (int l = 0; l < 4; l++)
                    {
                        cet[l, i] /= clus_c;
                    }
                }
                double[] distance = new double[2];
                for (int i = 0; i < m; i++)
                {
                    distance = cet.getDistance(fea[i]);
                    if (distance[0] > distance[1])
                    {
                        idx[i] = 1;
                    }
                    else if (distance[1] > distance[0])
                    {
                        idx[i] = 0;
                    }
                }

                //for (int i = 0; i < m; i++)
                //{
                //    if (fea[0, i] > fea[1, i])
                //    {
                //        idx[i] = 0;
                //        //clus.Write(fea[0, i]);
                //        //clus.Write('\t');
                //        //clus.Write(fea[1, i]);
                //        //clus.Write('\n');
                //    }
                //    else
                //    {
                //        idx[i] = 1;
                //    }
                //}
                scofile.WriteLine("---------------------------");
                scofile.WriteLine("New Iteration");
                scofile.WriteLine(it);
                scofile.WriteLine(" ");
                scofile2.WriteLine("---------------------------");
                scofile2.WriteLine("New Iteration");
                scofile2.WriteLine(it);
                scofile2.WriteLine(" ");
                for (int i = 0; i < m; i++)
                {
                    scofile.WriteLine(socre[0, i]);
                    scofile2.WriteLine(socre[1, i]);
                }


            }
            for (int i = 0; i < m; i++)
            {
                //clus.Write(i);
                //clus.Write("......\t");
                clus.WriteLine(idx[i]);
            }

            double[][] nmi = new double[k][];
            for (int i = 0; i < k; i++)
            {
                nmi[i] = new double[k];
            }

            double[] p1 = new double[k];
            double[] p2 = new double[k];
            double tar = 0, tmptar1 = 0, tmptar2 = 0; 
            for (int i = 0; i < m; i++)
            {
                if (i < 445)
                {
                    nmi[0][idx[i]]++;
                }
                else
                {
                    nmi[1][idx[i]]++;
                }
            }
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    nmi[i][j] /= m;
                }
            }

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    p1[i] += nmi[i][j];
                    p2[j] += nmi[i][j];
                }
            }

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    tar += nmi[i][j] * Math.Log(nmi[i][j] / (p1[j] * p2[i]));
                }
                tmptar1 += p1[i] * Math.Log(p1[i]);
                tmptar2 += p2[i] * Math.Log(p2[i]);
            }

            tar = tar / Math.Sqrt(tmptar1 * tmptar2);

            Console.Out.WriteLine(tar);
            Console.In.Read();
            kmdata.Close();
            clus.Close();
            files.Close();
            dicfile.Close();
            scofile.Close();
            scofile2.Close();
        }

        public static void genFea(int[] idx, Matrix links, Matrix socre, Matrix fea, int fl)
        {
            int linkrow = links.getRow();
            int linkcol = links.getCol();
            double[] pz = new double[2];
            double[] npz = new double[2];
            double sum = 0, sumt = 0;

            double sum1 = 0, sum2 = 0;
            pz[0] = 0.5;
            pz[1] = 0.5;

            for (int it2 = 0; it2 < 5; it2++)
            {
                sum = 0;
                sumt = 0;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < linkrow; j++)
                    {
                        if (idx[j] == i)
                        {
                            for (int k = 0; k < linkcol; k++)
                            {
                                sumt += socre[i, k + linkrow] * links[j, k];
                            }
                        }
                        sum += sumt * socre[i, j];
                        sumt = 0;
                    }
                    npz[i] = sum * pz[i];
                    sum = 0;
                }
                sum = npz[0] + npz[1];
                npz[0] /= sum;
                npz[1] /= sum;
                pz[0] = npz[0];
                pz[1] = npz[1];
            }

            sum1 = 0;
            sum2 = 0;

            for (int i = 0; i < linkrow; i++)
            {
                fea[fl, i] = socre[0, i] * npz[0];
                fea[fl + 1, i] = socre[1, i] * npz[1];
                sum1 += fea[fl, i];
                sum2 += fea[fl + 1, i];
            }
            for (int i = 0; i < linkrow; i++)
            {
                fea[fl, i] /= sum1;
                fea[fl + 1, i] /= sum2;
            }
        }
    }
}
