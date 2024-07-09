using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 横纵断面
{
    /// <summary>
    /// 点类
    /// </summary>
    class Point
    {
        public string name;
        public double X;
        public double Y;
        public double H;
        public double dis;

        public Point(string a,double X,double Y,double H)
        {
            name = a;
            this.X=X;
            this.Y = Y;
            this.H = H;

        }
        public Point()
        {

        }
    }
}
