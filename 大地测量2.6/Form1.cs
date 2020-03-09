using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NSMatrix;

namespace 大地测量2._6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main();
             void Text(string src, double[] a)//文件读取并存入数组
            {
                StreamReader streamReader = new StreamReader(src);
                String[] fileLines = System.IO.File.ReadAllLines(src);
                for (int i = 0; i < 14435; i++)
                {
                    a[i] = Convert.ToDouble(fileLines[i]);
                }
            }
             void Main()
            {



                double[] ENU_E = new double[14436];
                double[] ENU_N = new double[14436];
                double[] ENU_U = new double[14436];
                double[] XYZ_X = new double[14436];
                double[] XYZ_Y = new double[14436];
                double[] XYZ_Z = new double[14436];
                string coor_X = @"D:\DATA\XYZ2BLH\XYZ_X.txt";
                string coor_Y = @"D:\DATA\XYZ2BLH\XYZ_Y.txt";
                string coor_Z = @"D:\DATA\XYZ2BLH\XYZ_Z.txt";
                double rad2deg = 180 / Math.PI;
                Text(coor_X, XYZ_X);
                Text(coor_Y, XYZ_Y);
                Text(coor_Z, XYZ_Z);


                double b0 = 0.44397;
                double l0 = 0.11445;
                double h0 = 276.59;
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

                Graphics g = CreateGraphics();
                Point[] pot = new Point[30000];
                Pen pen = new Pen(Color.Orchid, 3);
                for (int i = 0; i < 14436; i++)

                {

                    pot[i].X = (Convert.ToInt32(ENU_N[i] * 800000) );
                    pot[i].Y = (Convert.ToInt32(ENU_E[i] * 13000) - 5000);
                    pot[i + 14435].X = (Convert.ToInt32(ENU_N[i] * 800000) + 1);
                    pot[i + 14435].Y = (Convert.ToInt32(ENU_E[i] * 13000) + 1 - 5000);

                    g.DrawLine(pen, pot[i], pot[i + 14435]);
                }


            }
        }
    }
    }

