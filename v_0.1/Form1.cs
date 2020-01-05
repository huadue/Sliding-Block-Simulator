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
using System.Drawing.Drawing2D;

namespace v_0._1
{
    public struct vector//向量结构体
    {
        public float r;//大小
        public float th;//方向
        public float Vx;//x分量
        public float Vy;//y分量
    }
    public struct block//物体
    {
        public string name;//名称
        public Point position;//坐标，位置
        public Size Size;//边长
        public vector[] forces;//受力
        public vector force;
        public float mass;//质量
        public vector velocity;//速度
        public double speed;//速度大小
        public Color Color;//颜色
        public float u1;//摩擦系数、下表面
        public float u2;//摩擦系数、上表面
        public float u3;//摩擦系数、左表面
        public float u4;//摩擦系数、右表面
        //public float[] u
    }
    public struct barrier//屏障
    {
        public string name;//名称
        public Color Color;//颜色
        public Point Point1;//起点
        public Point Point2;//终点
        public float ld;//长度
        //public bool direction;
        public float u;//摩擦力
    }
    public struct barrierpaint
    {
        public Point barrierPointstart;//绘制屏障的起点和终点
        public Point barrierPointend;//绘制屏障的起点和终点
        public bool p;//鼠标按下和松开的过程表示
    }
    //public class blocks//方块链表
    //{
    //    //public int number;
    //    block block;
    //    blocks next;
    //}

    
    public partial class Form1 : Form
    {
        public static int No_1 = 0;//控制物块的序号
        public static int No_2 = 0;//控制屏障的序号
        public barrierpaint barrierpaint;
        public static block[] blocks = new block[30];
        public static barrier[] barriers = new barrier[30];
        //blocks blocks = new blocks();

        public static int ifNo_1 = 0, ifNo_2 = 0;//选定的物块、屏障的序号
        static bool ifblock = false;//判断点击处是否有Block
        static bool ifbarrier = false;//判断点击处是否有Barrier
        static Point movepaint = new Point();//移动绘制Barrier的上一个点
        int ground_x = 160, ground_y = 40;
        
        static float G = 18;//重力加速度
        public float u = 0.8f;//弹性系数
        static bool start = false;//判断是否播放

        static Graphics g;

        Rectangle ground = new Rectangle(160, 40, 800,600);

        public float ptop(Point p1, Point p2)//两点距离的平方
        {
            return (p1.X - p2.X) * (p1.X - p2.X) - (p1.Y - p2.Y) * (p1.Y - p2.Y);
        }

        /// <summary>
        /// 防止加载闪屏
        /// </summary>
        

        private void Create()//创建实体（物块、屏障、弹簧等）
        {
            switch (choose)
            {
                case 1:
                    blocks[No_1].name = "滑块" + Convert.ToString(No_1);
                    blocks[No_1].position = MousePosition;
                    blocks[No_1].Size = new Size(24, 24);
                    blocks[No_1].Color = Color.AliceBlue;
                    blocks[No_1].mass = 1;
                    blocks[No_1].u1 = 0;
                    blocks[No_1].u2 = 0;
                    blocks[No_1].u3 = 0;
                    blocks[No_1].u4 = 0;
                    No_1++;
                    break;
                case 2:
                    barriers[No_2].name = "屏障" + Convert.ToString(No_2);
                    barriers[No_2].u = 0;
                    break;

            }
            
        }
        private void Createblock(Point point)//创建一个滑块
        {
            blocks[No_1].name = "滑块" + Convert.ToString(No_1);
            blocks[No_1].position = point;
            blocks[No_1].Size = new Size(64, 64);
            blocks[No_1].Color = Color.Red;
            blocks[No_1].mass = 1;
            blocks[No_1].u1 = 0;
            blocks[No_1].u2 = 0;
            blocks[No_1].u3 = 0;
            blocks[No_1].u4 = 0;
            No_1++;
        }
        private void Createbarrier(Point p1, Point p2)//创建一个屏障
        {
            barriers[No_2].name = "屏障" + Convert.ToString(No_2);
            barriers[No_2].u = 0;
            barriers[No_2].Point1 = p1;
            barriers[No_2].Point2 = p2;
            barriers[No_2].Color = Color.Black;
            No_2++;
        }

        private void paint_to(Color color)//绘制力矢
        {
            //if (G != 0)
            //{
            //    int a, b, c, d;
            //    a = blocks[ifNo_1].position.X + blocks[ifNo_1].Size.Width / 2;
            //    b = blocks[ifNo_1].position.Y + blocks[ifNo_1].Size.Width / 2;
            //    c = (int)blocks[ifNo_1].force.Vx * 10;
            //    d = (int)blocks[ifNo_1].force.Vy * 10;
            //    Pen p0 = new Pen(color, 4);
            //    g.DrawLine(p0, a, b, a + c, b + d);
            //    g.DrawLine(p0, a + c, b + d, a + c - (int)((16 * c + 12 * d) / Math.Sqrt(c * c + d * d)), b + d - (int)((16 * d + 12 * c) / Math.Sqrt(c * c + d * d)));
            //    g.DrawLine(p0, a + c, b + d, a + c - (int)((16 * c - 12 * d) / Math.Sqrt(c * c + d * d)), b + d - (int)((16 * d - 12 * c) / Math.Sqrt(c * c + d * d)));

            //}
        }
        private void Choose_1()
        {
            paint_l(blocks[ifNo_1].position, blocks[ifNo_1].Size, Color.Black);
            panel1.Visible = true;
            textBox2.Text = blocks[ifNo_1].name;
            textBox3.Text = Convert.ToString(blocks[ifNo_1].mass);
            textBox4.Text = Convert.ToString(blocks[ifNo_1].velocity.Vy);
            comboBox2.SelectedIndex = getColor(blocks[ifNo_1]);
            textBox6.Text = Convert.ToString(blocks[ifNo_1].position.X);
            textBox7.Text = Convert.ToString(blocks[ifNo_1].Size.Width);
            textBox8.Text = Convert.ToString(blocks[ifNo_1].position.Y);
            textBox9.Text = Convert.ToString(blocks[ifNo_1].Size.Height);
            paint_to(Color.DarkOrange);
        }
        private void Choose_2()
        {
            Paint_2(barriers[ifNo_2].Point1, barriers[ifNo_2].Point2, Color.LightBlue, Color.Bisque);
            panel2.Visible = true;
            textBox5.Text = barriers[ifNo_2].name + Convert.ToString(ifbarrier);
            textBox11.Text = Convert.ToString(barriers[ifNo_2].Point1.X);
            textBox10.Text = Convert.ToString(barriers[ifNo_2].Point1.Y);
            textBox13.Text = Convert.ToString(barriers[ifNo_2].Point2.X);
            textBox12.Text = Convert.ToString(barriers[ifNo_2].Point2.Y);
        }

        private void Paint_1(int x, int y, int w, int h)//画一个矩形
        {
            Pen pen = new Pen(Color.Red, 10);
            Rectangle j = new Rectangle(x, y, w, h);
            g.DrawRectangle(pen, j);
            //Rectangle v =new Rectangle()
        }
        private void Paint_2(Point p1,Point p2, Color c1,Color c2)//画一个墙壁
        {
            int x1 = p1.X, y1 = p1.Y, x2 = p2.X, y2 = p2.Y;
            if (x1 == x2 && y1 == y2)
            {
                return;
            }
            g.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
            g.CompositingQuality = CompositingQuality.HighQuality;//再加一点
            Pen pen1 = new Pen(c1, 5);
            Pen pen2 = new Pen(c2, 10);
            //Rectangle j = new Rectangle(x, y, w, h);

            double l = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));//长度
            int xn, yn, xm, ym;
            xn = x1 - (int)((y2 - y1) / l * 8);
            yn = y1 + (int)((x2 - x1) / l * 8);
            xm = x2 - (int)((y2 - y1) / l * 8);
            ym = y2 + (int)((x2 - x1) / l * 8);
            g.DrawLine(pen2, xn, yn, xm, ym);
            g.DrawLine(pen1, x1, y1, x2, y2);
            //int t = (int)l / 8+1;//斜杠数量
            //int xn, yn, xm, ym, n;
            //double l0 = 10;//斜杠长度
            //for (n = 0; n <= t; n++)
            //{
            //    xn = x1 + n * (x2 - x1) / t;
            //    yn = y1 + n * (y2 - y1) / t;
            //    xm = xn - (int)((y2 - y1) / l * l0);//l0为斜杠长度,不是10（十）
            //    ym = yn + (int)((x2 - x1) / l * l0);
            //    g.DrawLine(pen2, xn, yn, xm, ym);
            //}
        }
        private void paint_2_0(Point p1, Point p2, Color c1, Color c2)//画一个大墙壁
        {
            int x1 = p1.X, y1 = p1.Y, x2 = p2.X, y2 = p2.Y;
            if (x1 == x2 && y1 == y2)
            {
                return;
            }
            g.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
            g.CompositingQuality = CompositingQuality.HighQuality;//再加一点
            Pen pen1 = new Pen(c1, 7);
            Pen pen2 = new Pen(c2, 12);
            //Rectangle j = new Rectangle(x, y, w, h);

            double l = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));//长度
            int xn, yn, xm, ym;
            xn = x1 - (int)((y2 - y1) / l * 8);
            yn = y1 + (int)((x2 - x1) / l * 8);
            xm = x2 - (int)((y2 - y1) / l * 8);
            ym = y2 + (int)((x2 - x1) / l * 8);
            g.DrawLine(pen2, xn, yn, xm, ym);
            g.DrawLine(pen1, x1, y1, x2, y2);
            g.Dispose();
        }
        private void paint_blocks(block block)//绘制滑块
        {
            //Pen pen = new Pen(block.Color,4);
            SolidBrush brush = new SolidBrush(block.Color);
            //pen.Brush = brush;
            Rectangle j = new Rectangle(block.position, block.Size);
            g.FillRectangle(brush, j);
        }
        private void paint_barrier(barrier barrier)//绘制屏障
        {
            Paint_2(barrier.Point1, barrier.Point2,Color.Black,Color.DarkGray);
        }
        private void Paint_g()//绘制虚线框
        {
            Graphics gs = this.CreateGraphics();
            Pen pen0 = new Pen(Color.Blue, 3);
            pen0.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            pen0.DashPattern = new float[] { 5, 5 };
            gs.DrawLine(pen0, 260, 60, this.Width - 40, 60);
            gs.DrawLine(pen0, 260, this.Height - 120, 260, 60);
            gs.DrawLine(pen0, 260, this.Height - 120, this.Width - 40, this.Height - 120);
            gs.DrawLine(pen0, this.Width - 40, this.Height - 120, this.Width - 40, 60);
        }
        private void paint_l(Point pp, Size ss,Color color)//绘制实线框for block
        {
            Pen pen = new Pen(color, 1);
            pen.DashStyle = DashStyle.Solid;
            g.DrawLine(pen, pp.X - 3, pp.Y - 3, pp.X + 3 + ss.Width, pp.Y - 3);
            g.DrawLine(pen, pp.X - 3, pp.Y - 3, pp.X - 3, pp.Y + 3 + ss.Height);
            g.DrawLine(pen, pp.X + 3 + ss.Width, pp.Y - 3, pp.X + 3 + ss.Width, pp.Y + 3 + ss.Height);
            g.DrawLine(pen, pp.X - 3, pp.Y + 3 + ss.Height, pp.X + 3 + ss.Width, pp.Y + 3 + ss.Height);
        }
        private void clear_1()//清空绘制
        {
            g.Clear(this.BackColor);
        }
        private double distance(Point p1, Point p2, Point p0)//点与直线距离
        {
            double x1 = (double)p1.X, y1 = (double)p1.Y;
            double x2 = (double)p2.X, y2 = (double)p2.Y;
            double x0 = (double)p0.X, y0 = (double)p0.Y;
            if (y1 == y2)
            {
                return Math.Abs(y0 - y1);
            }
            else
            {
                if (x1 == x2)
                {
                    return Math.Abs(x0 - x1);
                }
                else
                {
                    double k = (x1 - x2) / (y2 - y1);
                    return Math.Sqrt(k * k + 1) * Math.Abs(((y0 - y1) * (x2 - x1) / (y2 - y1) + x1 - x0) / (1 - k * (x2 - x1) / (y2 - y1)));
                    
                }
            }

        }
        private void paint_all()//绘制全部
        {
            int i = 0;
            Paint_g();
            //textBox1.Text = Convert.ToString(choose);
            for (i = 0; i < No_2; i++)
            {
                paint_barrier(barriers[i]);
                textBox1.Text = Convert.ToString(i);
            }
            for (i = 0; i < No_1; i++)
            {
                paint_blocks(blocks[i]);
            }

            if (ifbarrier)
            {
                Paint_2(barriers[ifNo_2].Point1, barriers[ifNo_2].Point2, Color.LightBlue, Color.Bisque);
            }
            if (ifblock)
            {
                paint_l(blocks[ifNo_1].position, blocks[ifNo_1].Size, Color.Black);
                paint_to(Color.DarkOrange);
            }

        }

        private void f_block(int n)//计算滑块受力
        {
            blocks[n].force.Vx = 0;
            blocks[n].force.Vy = 0;
            blocks[n].force.Vy += G * blocks[n].mass;
        }
        private void v_block(int n)//计算滑块速度、加速度
        {
            
            blocks[n].velocity.Vx += blocks[n].force.Vx / blocks[n].mass * timer1.Interval / 1000;
            blocks[n].velocity.Vy += blocks[n].force.Vy / blocks[n].mass * timer1.Interval / 1000;
        }
        private void l_block(int n)//计算滑块位置
        {
            blocks[n].position.X += (int)(blocks[n].velocity.Vx * timer1.Interval);
            blocks[n].position.Y += (int)(blocks[n].velocity.Vy * timer1.Interval);
        }
        private bool Dpeng_12(int n,int m)//判断碰撞
        {
            double dlr, dl1, dl2, ds;
            dlr = Math.Sqrt((barriers[m].Point1.X - barriers[m].Point2.X) * (barriers[m].Point1.X - barriers[m].Point2.X) + (barriers[m].Point1.Y - barriers[m].Point2.Y) * (barriers[m].Point1.Y - barriers[m].Point2.Y));
            dl1 = Math.Sqrt((barriers[m].Point1.X - blocks[n].position.X) * (barriers[m].Point1.X - blocks[n].position.X) + (barriers[m].Point1.Y - blocks[n].position.Y) * (barriers[m].Point1.Y - blocks[n].position.Y));
            dl2 = Math.Sqrt((blocks[n].position.X - barriers[m].Point2.X) * (blocks[n].position.X - barriers[m].Point2.X) + (blocks[n].position.Y - barriers[m].Point2.Y) * (blocks[n].position.Y - barriers[m].Point2.Y));
            ds = Math.Sqrt(blocks[n].Size.Width * blocks[n].Size.Width + blocks[n].Size.Height * blocks[n].Size.Height);
            if (dl1 + dl2 <= dlr + ds)
            {
                if (blocks[n].position.X == barriers[m].Point1.X)
                {
                    if ((blocks[n].position.Y - barriers[m].Point1.X) <= blocks[n].Size.Height && (blocks[n].position.Y - barriers[m].Point1.X) >= -blocks[n].Size.Height)
                    {
                        return true;
                    }
                }
                else
                {
                    if (blocks[n].position.X == barriers[m].Point2.X)
                    {
                        if ((blocks[n].position.Y - barriers[m].Point2.X) <= blocks[n].Size.Height && (blocks[n].position.Y - barriers[m].Point2.X) >= -blocks[n].Size.Height)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        double p1x, p1y, p2x, p2y, pox, poy;
                        p1x = (double)barriers[m].Point1.X;
                        p1y = (double)barriers[m].Point1.Y;
                        p2x = (double)barriers[m].Point2.X;
                        p2y = (double)barriers[m].Point2.Y;
                        pox = (double)blocks[n].position.X;
                        poy = (double)blocks[n].position.Y;

                        double k1, k2, k3, k4;
                        k1 = (poy - p1y) / (pox - p1x) - (p2y - p1y) / (p2x - p1x);
                        k2 = (poy - p1y) / (pox + blocks[n].Size.Width - p1x) - (p2y - p1y) / (p2x - p1x);
                        k3 = (poy + blocks[n].Size.Height - p1y) / (pox - p1x) - (p2y - p1y) / (p2x - p1x);
                        k4 = (poy + blocks[n].Size.Height - p1y) / (pox + blocks[n].Size.Width - p1x) - (p2y - p1y) / (p2x - p1x);
                        if (k1 * k4 <= 0 || k2 * k3 <= 0)//|| k2 * k3 <= 0)
                        {
                            return true;
                        }

                    }

                }


            }
            return false;
        }

        private void peng_12(int n,int m)//碰撞处理
        {
            blocks[n].velocity.r = (float)Math.Pow(blocks[n].velocity.Vx, 2) + (float)Math.Pow(blocks[n].velocity.Vy, 2);
            if (blocks[n].velocity.r <= 20)
            {
                blocks[n].velocity.Vx = 0;
                blocks[n].velocity.Vy = 0;
            }
            else
            {
                float ch;
                float pe1, bo1, pe2, bo2;
                barriers[m].ld = ptop(barriers[m].Point1, barriers[m].Point2);
                ch = blocks[n].velocity.Vx * (barriers[m].Point2.X - barriers[m].Point1.X) + blocks[n].velocity.Vy * (barriers[m].Point2.Y - barriers[m].Point1.Y);
                pe1 = ch / barriers[m].ld * (barriers[m].Point2.X - barriers[m].Point1.X);
                pe2 = ch / barriers[m].ld * (barriers[m].Point2.Y - barriers[m].Point1.Y);
                bo1 = blocks[n].velocity.Vx - pe1;
                bo2 = blocks[n].velocity.Vy - pe2;
                blocks[n].velocity.Vx = u * (pe1 - bo1);
                blocks[n].velocity.Vy = u * (pe2 - bo2);
            }
        }
        public Form1()
        {
            InitializeComponent();
        }    
        public int choose;//下拉选项
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int choose;
            choose = comboBox1.SelectedIndex;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            g = CreateGraphics();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            paint_all();
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            Paint_g();
            barrierpaint.p = false;
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //ground = new Rectangle(160, 40, this.Width - 200, this.Height - 160);
            clear_1();
            paint_all();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (choose == 2)
            {
                Point pbarrier = new Point();
                pbarrier = PointToClient(MousePosition);
                if (pbarrier.X < (this.Width - 40) && pbarrier.Y < (this.Height - 120) && pbarrier.X > 160 && pbarrier.Y > 40) 
                {
                    barrierpaint.barrierPointstart = pbarrier;
                    barrierpaint.p = true;
                    movepaint = PointToClient(MousePosition);
                }

            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (choose == 2)
            {
                Point pbarrier = new Point();
                pbarrier = PointToClient(MousePosition);
                if (pbarrier.X < (this.Width - 40) && pbarrier.Y < (this.Height - 120) && pbarrier.X > 160 && pbarrier.Y > 40)
                {
                    if (barrierpaint.p)
                    {
                        if (pbarrier != barrierpaint.barrierPointstart)
                        {
                            barrierpaint.barrierPointend = pbarrier;
                            Createbarrier(barrierpaint.barrierPointstart, barrierpaint.barrierPointend);
                            paint_barrier(barriers[No_2 - 1]);
                        }
                    }                
                }                
            }
            barrierpaint.p = false;
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (choose == 2)
            //{
            //    Point pbarrier = new Point();
            //    pbarrier = PointToClient(MousePosition);
            //    if (pbarrier.X < (this.Width - 40) && pbarrier.Y < (this.Height - 120) && pbarrier.X > 160 && pbarrier.Y > 40)
            //    {
            //        if (barrierpaint.p)
            //        {
            //            if (pbarrier != barrierpaint.barrierPointstart)
            //            {
            //                paint_2_0(barrierpaint.barrierPointstart, movepaint, this.BackColor, this.BackColor);
            //                Paint_2(barrierpaint.barrierPointstart, pbarrier, Color.DarkGray, Color.LightGray);
            //                paint_all();
            //            }
            //        }
            //    }
            //    movepaint= PointToClient(MousePosition);
            //}
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            SolidBrush brush = new SolidBrush(this.BackColor);
            Rectangle j = new Rectangle(blocks[ifNo_1].position, blocks[ifNo_1].Size);
            g.FillRectangle(brush, j);
            g.Dispose();
            paint_l(blocks[ifNo_1].position, blocks[ifNo_1].Size, this.BackColor);
            int i = ifNo_1;
            for (; i < No_1; i++)
            {
                blocks[i] = blocks[i+1];
            }
            No_1--;
            ifblock = false;
            panel1.Visible = false;
            //paint_l(blocks[ifNo_1].position, blocks[ifNo_1].Size, this.BackColor);
            //Form1_Paint(sender, (PaintEventArgs)e);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)//更改颜色；
        {
            if (ifblock)
            {
                switch (comboBox2.SelectedIndex)
                {
                    case 0: blocks[ifNo_1].Color = Color.Red; break;
                    case 1: blocks[ifNo_1].Color = Color.Orange; break;
                    case 2: blocks[ifNo_1].Color = Color.Yellow; break;
                    case 3: blocks[ifNo_1].Color = Color.Green; break;
                    case 4: blocks[ifNo_1].Color = Color.Blue; break;
                    case 5: blocks[ifNo_1].Color = Color.Purple; break;
                    case 6: blocks[ifNo_1].Color = Color.Black; break;
                    case 7: blocks[ifNo_1].Color = Color.Gray; break;
                    case 8: blocks[ifNo_1].Color = Color.Brown; break;
                }
                paint_blocks(blocks[ifNo_1]);
            }
        }
        private int getColor(block b)
        {
            if (b.Color == Color.Red) return 0;
            if (b.Color == Color.Orange) return 1;
            if (b.Color == Color.Yellow) return 2;
            if (b.Color == Color.Green) return 3;
            if (b.Color == Color.Blue) return 4;
            if (b.Color == Color.Purple) return 5;
            if (b.Color == Color.Black) return 6;
            if (b.Color == Color.Gray) return 7;
            if (b.Color == Color.Brown) return 8;
            return 0;
        }//获取颜色

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ifblock)
            {
                SolidBrush brush = new SolidBrush(this.BackColor);
                Rectangle j = new Rectangle(blocks[ifNo_1].position, blocks[ifNo_1].Size);
                g.FillRectangle(brush, j);
                paint_l(blocks[ifNo_1].position, blocks[ifNo_1].Size, this.BackColor);
                blocks[ifNo_1].name = textBox2.Text;
                blocks[ifNo_1].velocity.Vy = Convert.ToSingle(textBox4.Text);
                blocks[ifNo_1].Size.Height = Convert.ToInt32(textBox9.Text);
                blocks[ifNo_1].Size.Width = Convert.ToInt32(textBox7.Text);
                blocks[ifNo_1].position.Y = Convert.ToInt32(textBox8.Text);
                blocks[ifNo_1].position.X = Convert.ToInt32(textBox6.Text);
                blocks[ifNo_1].mass = Convert.ToSingle(textBox3.Text);
                button2.Enabled = false;
                
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {
            button5.Enabled = true;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            button5.Enabled = true;
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            button5.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ifbarrier)
            {
                paint_2_0(barriers[ifNo_2].Point1, barriers[ifNo_2].Point2, this.BackColor, this.BackColor);
                int i = ifNo_2;
                for (; i < No_2; i++)
                {
                    barriers[i] = barriers[i + 1];
                }
                No_2--;
                ifbarrier = false;
                panel2.Visible = false;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            clear_1();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (ifbarrier)
            {
                Paint_2(barriers[ifNo_2].Point1, barriers[ifNo_2].Point2, this.BackColor, this.BackColor);
                barriers[ifNo_2].name = textBox5.Text;
                barriers[ifNo_2].Point1.X = Convert.ToInt32(textBox11.Text);
                barriers[ifNo_2].Point1.Y = Convert.ToInt32(textBox10.Text);
                barriers[ifNo_2].Point2.X = Convert.ToInt32(textBox13.Text);
                barriers[ifNo_2].Point2.Y = Convert.ToInt32(textBox12.Text);
                button5.Enabled = false;
                Paint_2(barriers[ifNo_2].Point1, barriers[ifNo_2].Point2, Color.LightBlue, Color.Bisque);
            }
        }

        private void textBox10_TextChanged_1(object sender, EventArgs e)
        {
            button5.Enabled = true;
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            button5.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString((double)1000 / (double)timer1.Interval);
            for(int i = 0; i < No_1; i++)
            {
                f_block(i);
            }
            if (start)
            {
                Graphics g = this.CreateGraphics();
                for(int i = 0; i < No_1; i++)
                {
                    v_block(i);
                    for(int j = 0; j < No_2; j++)
                    {
                        if(Dpeng_12(i, j))
                        {
                            peng_12(i, j);
                        }

                    }
                    l_block(i);
                }
                g.Clear(this.BackColor);
                paint_all();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (start)
            {
                start = false;
                button6.Text = "播放";
            }
            else
            {
                start = true;
                button6.Text = "暂停";
            }
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //timer1.Interval = (int)(1000 / Convert.ToSingle(textBox1.Text));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                G = 18;
            }
            else
            {

                G = 0;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            bool ifblock_o = false;//判断画出的block是否与已有的重叠
            int shift_x = 0, shift_y = 0;//与ifblock_o联用，确定坐标的偏移

            Point pbarrier = new Point();
            pbarrier = PointToClient(MousePosition);
            for(int i = 0; i < No_1; i++)
            {
                if(((pbarrier.X - blocks[i].position.X <= blocks[i].Size.Width) && (pbarrier.X - blocks[i].position.X >= -blocks[i].Size.Width))&& ((pbarrier.Y - blocks[i].position.Y <= blocks[i].Size.Height) && (pbarrier.Y - blocks[i].position.Y >= -blocks[i].Size.Height)))
                {
                    if (((pbarrier.X - blocks[i].position.X >= 0) && (pbarrier.X - blocks[i].position.X <= blocks[i].Size.Width))&& ((pbarrier.Y - blocks[i].position.Y >= 0) && (pbarrier.Y - blocks[i].position.Y <= blocks[i].Size.Height)))
                    {
                        if (ifblock)
                        {
                            paint_l(blocks[ifNo_1].position, blocks[ifNo_1].Size, this.BackColor);
                            //paint_to(this.BackColor);
                        }
                        ifblock = true;
                        ifNo_1 = i;
                        break;
                    }
                    //else
                    //{
                    //    ifblock_o = true;
                    //    shift_x = -blocks[i].Size.Width - pbarrier.X + blocks[i].position.X;
                    //    shift_y = -blocks[i].Size.Height - pbarrier.Y + blocks[i].position.Y;
                    //    if (shift_x <= -blocks[i].Size.Width) 
                    //    {
                    //        shift_x = 0;
                    //    }
                    //    if (shift_y <= -blocks[i].Size.Height) 
                    //    {
                    //        shift_y = 0;
                    //    }
                    //}
                }
                else
                {
                    ifblock = false;
                    paint_l(blocks[ifNo_1].position, blocks[ifNo_1].Size, this.BackColor);
                    panel1.Visible = false;
                }
            }
            //int Xb, Yb, px, py, lx, ly, l;

            for (int i = 0; i < No_2; i++)
            {
                //px = pbarrier.X - barriers[i].Point1.X;
                //py = pbarrier.Y - barriers[i].Point1.Y;
                //lx = barriers[i].Point2.X - barriers[i].Point1.X;
                //ly = barriers[i].Point2.Y - barriers[i].Point1.Y;
                //l = (int)Math.Sqrt(lx * lx + ly * ly);
                //Yb = (px * lx + py * ly) / (int)Math.Sqrt(lx * lx + ly * ly);
                //Xb = (int)Math.Sqrt((px-Yb*lx/l)^2+(py-Yb*l));
                if (distance(barriers[i].Point1, barriers[i].Point2, pbarrier)<5) 
                {

                    if (ifbarrier)
                    {
                        Paint_2(barriers[ifNo_2].Point1, barriers[ifNo_2].Point2, Color.Black, Color.DarkGray);
                        
                    }
                    ifbarrier = true;
                    ifNo_2 = i;
                    break;

                }
                else
                {
                    ifbarrier = false;
                    Paint_2(barriers[ifNo_2].Point1, barriers[ifNo_2].Point2, Color.Black, Color.DarkGray);
                    panel2.Visible = false;
                }
            }
            if (choose == 1 && ifblock == false && ifbarrier == false) 
            {
                if (pbarrier.X < (this.Width - 104) && pbarrier.Y < (this.Height - 184) && pbarrier.X > 260 && pbarrier.Y > 60)
                {
                    Point p = PointToClient(MousePosition);
                    if (ifblock_o)
                    {
                        p.X += shift_x;
                        p.Y += shift_y;
                        Createblock(p);
                    }
                    else
                    {
                        Createblock(p);
                    }
                    paint_blocks(blocks[No_1-1]);
                }
            }
            if (ifblock )
            {
                Choose_1();
            }
            if (ifbarrier )
            {
                Choose_2();
            }
            if (No_1 >= 29)
            {
                MessageBox.Show("方块过多！");
                comboBox1.SelectedIndex = 0;
                choose = 0;
                No_1--;
            }
            if ( No_2 >= 29)
            {
                MessageBox.Show("屏障过多！");
                comboBox1.SelectedIndex = 0;
                choose = 0;
                No_2--;
            }
        }

    }
}
