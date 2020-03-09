using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NSMatrix;

namespace XYZ2ENU
{
    class Program
    {
        static void Text0(string src, double[] a, double[] b, double[] c)//文件读取并存入数组
        {
            double[,] input = new double[14435, 4];
            StreamReader reader = new StreamReader(src);
            for (int i = 0; i < 14435; i++)
            {
                string str = reader.ReadLine();
                string[] arr = new string[4];
                arr = str.Split(',');
                for (int j = 1; j < 4; j++)
                {
                    input[i, j] = double.Parse(arr[j]);

                }
                a[i] = input[i, 1];
                b[i] = input[i, 2];
                c[i] = input[i, 3];

            }
        }
        static void Main(string[] args)
        {
            

          
                double[] ENU_E = new double[14436];
                double[] ENU_N = new double[14436];
                double[] ENU_U = new double[14436];
                double[] XYZ_X = new double[14436];
                double[] XYZ_Y = new double[14436];
                double[] XYZ_Z = new double[14436];
                
                string path = @"D:\DATA\XYZ2ENU\XYZ2BLHNEU.xyz";
               
            
            Text0(path, XYZ_X, XYZ_Y, XYZ_Z);


                double b0 = 0.44397;
                double l0 = 0.11445;
                double x0 = 6378210.6613;
                double y0 = 12740.1814;
                double z0 = 49093.2052;
                double[] dx = new double[14436];
                double[] dy = new double[14436];
                double[] dz = new double[14436];
                for (int i = 0; i < 14436; i++)
                {
                    dx[i] = XYZ_X[i] - x0;
                    dy[i] = XYZ_Y[i] - y0;
                    dz[i] = XYZ_Z[i] - z0;
                }
                double[] Sdata = new double[9];//变换矩阵元素
                Sdata[0] = -Math.Sin(l0);
                Sdata[1] = Math.Cos(l0);
                Sdata[2] = 0;
                Sdata[3] = -Math.Sin(b0) * Math.Cos(l0);
                Sdata[4] = -Math.Sin(b0) * Math.Sin(l0);
                Sdata[5] = Math.Cos(b0);
                Sdata[6] = Math.Cos(b0) * Math.Cos(l0);
                Sdata[7] = Math.Cos(b0) * Math.Sin(l0);
                Sdata[8] = Math.Sin(b0);
                Matrix S = new Matrix(3, 3, Sdata);
            double[] res = new double[5];
            StreamWriter sw = new StreamWriter(@"D:\DATA\XYZ2ENU\RES.txt");
            Console.SetOut(sw);
            for (int i = 0; i < 14436; i++)
                {
                   // double[] temp = { ENU_E[i], ENU_N[i], ENU_U[i] };
                   // Matrix enu = new Matrix(3, 1, temp);
                    double[] dxyz = { dx[i], dy[i], dz[i] };
                    Matrix xyz = new Matrix(3, 1, dxyz);
                   Matrix enu = S * xyz;
                enu.Give(res);
                ENU_E[i] = res[0];
                ENU_N[i] = res[1];
                ENU_U[i] = res[2];

                Console.Write(i + 1);
                Console.Write(",");
                Console.Write(ENU_E[i]);
                Console.Write(",");
                Console.Write(ENU_N[i]);
                Console.Write(",");
                Console.Write(ENU_U[i]);
                Console.WriteLine();
                
            }

            //Console.ReadKey();
            sw.Flush();
            sw.Close();

        }
        }
    }

