using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 横纵断面
{
    class Algo
    {
        /// <summary>
        /// 方位角计算
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetAngle(Point p1,Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            double arfa = Math.Atan(dy / dx);
            if (dx > 0 && dy > 0) arfa = Math.Abs(arfa);
            if (dx < 0 && dy > 0) arfa =Math.PI- Math.Abs(arfa);
            if (dx < 0 && dy < 0) arfa =Math.PI+ Math.Abs(arfa);
            if (dx > 0 && dy < 0) arfa =Math.PI*2- Math.Abs(arfa);
            if (dy>0&&dx==0) arfa = Math.PI / 2;
            if (dy<0&&dx==0) arfa = Math.PI * 3 / 4;
            return arfa;
        }
        /// <summary>
        /// 距离计算
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double Getdiatanse(Point p1,Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            return Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
        }
        /// <summary>
        /// 高程计算
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="p"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double GetH(List<Point> ps, Point p, int n)
        {
            ps.ForEach(x => x.dis = Getdiatanse(p, x)); //遍历计算距离
            List<Point> nearp = ps.OrderBy(x => x.dis).Take(n).ToList();//排序取前n个
            double up = 0,down = 0;
            nearp.ForEach(x => up += x.H / x.dis); //计算up
            nearp.ForEach(x => down += 1 / x.dis);//计算down
            double a = up / down;
            return a;
        }
        /// <summary>
        /// 两点间断面面积计算
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="H"></param>
        /// <returns></returns>
        public static double GetS(Point p1,Point p2,double H)
        {
            double tmp = Getdiatanse(p1, p2);
            return (p1.H + p2.H - 2 * H) * tmp / 2;
        }
        /// <summary>
        /// 纵断面计算
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static List<Point> GetZong(List<Point> ps,List<Point> k)
        {
            List<Point> inpoint = new List<Point>();
            double k0k1 = Getdiatanse(k[0], k[1]);
            double k1k2 = Getdiatanse(k[1], k[2]);
            double k0k2 = Getdiatanse(k[0], k[2]);
            double arfak0k1 = GetAngle(k[0], k[1]);
            double arfak1k2 = GetAngle(k[1], k[2]);
            double d = 10;
            inpoint.Add(k[0]);
            while (d < k0k1)
            {
                Point p = new Point();
                p.dis = d;

                p.X = k[0].X + d * Math.Cos(arfak0k1);
                p.Y = k[0].Y + d * Math.Sin(arfak0k1);
                p.name = $"V-{d}";
                p.H = GetH(ps, p, 5);
                inpoint.Add(p);

                d += 10;
                if (d > k0k1)
                {
                    k[1].dis = k0k1;
                    inpoint.Add(k[1]);
                    break;
                }
            }
            while ( d< k0k2)
            {
                Point p = new Point();
                p.dis = d;

                p.X = k[1].X + (d-k0k1) * Math.Cos(arfak1k2);
                p.Y = k[1].Y + (d-k0k1) * Math.Sin(arfak1k2);
                p.name = $"V-{d}";
                p.H = GetH(ps, p, 5);
                inpoint.Add(p);

                d += 10;
                if (d > k0k2)
                {
                    k[2].dis = k0k2;
                    inpoint.Add(k[2]);
                    break;
                }
            }
            return inpoint;
        }
        /// <summary>
        /// 计算横断面
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        /// <param name="name"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static   List<Point> GetHeng(List<Point> ps, Point k1,Point k2,string name,int n)
        {
            List<Point> interpoint = new List<Point>();
            Point p = zhong(k1, k2, name);
            p.H = GetH(ps, p, 5);
            double al = GetAngle(k1, k2)+Math.PI/2;
            double d = -25;
            double i = 1;
            while (d <= 25)
            {
                if (d == 0)
                {
                    interpoint.Add(p);
                    d += 5;
                    i += 1;
                    continue;
                }
                Point point = new Point();
                point.name = $"P-{i}";
                point.X = p.X + d * Math.Cos(al);
                point.Y = p.Y + d * Math.Sin(al);
                point.dis = d;
                point.H = GetH(ps, point, n);
                interpoint.Add(point);
                d += 5;
                i += 1;
            }
            return interpoint;
        }
        /// <summary>
        /// 计算横断面中点
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Point zhong(Point p1,Point p2,string name)
        {
            Point p = new Point();
            p.name = name;
            p.X = (p1.X + p2.X) / 2;
            p.Y = (p1.Y + p2.Y) / 2;
            return p;
        }
        /// <summary>
        /// 数据输出
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="s"></param>
        /// <param name="l"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetOut(List<Point> ps,string name,double H)
        {
            double s = 0, l = 0;
            for (int i = 0; i < ps.Count-1; i++)
            {
                s += GetS(ps[i], ps[i + 1], H);
            }
            l = Getdiatanse(ps[0], ps[ps.Count-1]);

            string data = "";
            data += $"{name}断面信息 \r\n";
            data += "----------------------------------------------- \r\n";
            data += $"断面面积：{s:f3}\r\n";
            data += $"断面全长：{l:f3}\r\n";
            data += "线路主点\r\n";
            data += "点名    里程    X坐标      Y坐标     H坐标\r\n";
            foreach (var item in ps)
            {
                data += $"{item.name}    {item.dis:f3}    {item.X:f4}     {item.Y:f4}     {item.H:f4}\r\n";

            }
            return data;
        }
        public static void GetRad(double dfm)
        {

        }
    }
}
