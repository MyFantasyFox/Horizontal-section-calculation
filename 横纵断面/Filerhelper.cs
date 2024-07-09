using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace 横纵断面
{
    class Filerhelper
    {
        public static List<Point> GetPoints(string path,ref string H)
        {
            List<Point> points = new List<Point>();
            using (StreamReader Sr = new StreamReader(path))
            {
                string[] tmp = Sr.ReadLine().Split(',');
                H = tmp[1];
                Sr.ReadLine();
                Sr.ReadLine();
                while (!Sr.EndOfStream)
                {
                    string[] v = Sr.ReadLine().Split(',');
                    Point p = new Point(v[0], Convert.ToDouble(v[1]), Convert.ToDouble(v[2]), Convert.ToDouble(v[3]));
                    points.Add(p);
                }
            }
            return points;
        }
    }
}
