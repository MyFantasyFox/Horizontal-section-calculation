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

namespace 横纵断面
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Point> points = new List<Point>();
        double H = 0;
        List<Point> K = new List<Point>();
        private void 数据导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "(*.txt)|*.txt";
            if (open.ShowDialog() == DialogResult.OK)
            {
                string h = "";
                points = Filerhelper.GetPoints(open.FileName,ref h);
                H = Convert.ToDouble(h);
                points.ForEach(x => dataGridView1.Rows.Add(x.name, x.X, x.Y, x.H));
                K = points.Where(x => x.name == "K0"||x.name=="K1"||x.name=="K2").ToList();
                MessageBox.Show("数据导入成功");
            }
        }
        List<Point> inpoint = new List<Point>();
        string data = "";
        private void 纵断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inpoint = Algo.GetZong(points, K);
            data = Algo.GetOut(inpoint, "纵",H);
            textBox1.Text = data;
        }
        List<Point> hengduan = new List<Point>();
        List<Point> hengduan1 = new List<Point>();
        private void 横断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hengduan = Algo.GetHeng(points, K[0], K[1], "M", 5);
            data += Algo.GetOut(hengduan,"横",H);
            hengduan1 = Algo.GetHeng(points, K[1], K[2], "N", 5);
            data += Algo.GetOut(hengduan1, "横",H);
            textBox1.Text = data;
        }

        private void 数据导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "(*.txt)|*.txt";
            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter S = new StreamWriter(save.FileName))
                    {
                        S.WriteLine(data);
                        MessageBox.Show("数据保存成功");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据保存失败");
                }
                
            }
        }
    }
}
