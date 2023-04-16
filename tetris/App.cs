﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

class tetris : Form
{
    private Image[] img = new Image[8];
    private Image[,] bg = new Image[17, 24];
    private Image backg;
    private mino m;
    private game gam;
    private int timer = 0;
    private int count = 0;
    private int[] a = {1,2,3,4,5,5,4,3,2,1};
    Random rn = new Random();
    private Image minom1img;

    public static void Main()
    {
        Application.Run(new tetris());
    }
        
    public tetris()
    {
        this.Text = "faketetris";
        this.ClientSize = new Size(1600, 900);
        this.DoubleBuffered = true;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
        tm.Interval = 20;
        m = new mino();
        gam = new game();
        int[,] initMino = {{-2, -2, -2, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2},
                           {-2, -2, -2, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2},
                           {-2, -2, -2, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2},
                           {-2, -2, -2, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                           {-2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2},
                           {-2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2},
                           {-2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2}
        };
        int d = 0;
        int k = rn.Next(7);
        m.deg = d;
        m.fallingMino = new Point(6, 0);
        Point minop = new Point(740, -90);
        m.point = minop;
        gam.placedMino = initMino;
        gam.kind = rn.Next(7);
        gam.veryfast = false;
        loadImage();
        this.Paint += new PaintEventHandler(fm_Paint);
        this.KeyDown += new KeyEventHandler(fm_KeyDown);
        tm.Tick += new EventHandler(tm_Tick);
        tm.Start();
    }

    public void loadImage()
    {
        minom1img = Image.FromFile(".\\resources\\mino_-1.png");
        backg = Image.FromFile(".\\resources\\background.png");
        for (int i = 0; i <= 6; i++)
            {
                img[i] = Image.FromFile(".\\resources\\mino_" + i + ".png");
            }
        bgPaint();
        minoDeg();
    }
    public void bgPaint()
    {
        for (int j = 0; j < 24; j++)
        {
            for (int i = 0; i < 16; i++)
            {
                switch (gam.placedMino[j, i])
                {
                    case -3:
                        bg[i, j] = minom1img;
                        break;
                    case -2:
                        bg[i, j] = minom1img;
                        break;
                    case -1:
                        bg[i, j] = backg;
                        break;
                    default:
                        bg[i, j] = img[gam.placedMino[j, i]];
                        break;
                }
            }
        }
    }

    public void minoDeg()
    {
        int d;
        d = m.deg;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                d = m.deg;
                if (m.minoShape[gam.kind, d, i, j] == 1)
                {
                    m.minoImage0[j, i] = img[gam.kind];
                }
                else
                {
                    m.minoImage0[j, i] = minom1img;
                }
            }
        }
    }

    public void fm_Paint(Object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        for (int j = 0; j < 24; j++)
        {
            for (int i = 0; i < 16; i++)
            {
                g.DrawImage(bg[i, j], 560 + i * 30, j * 30 - 30 * 3, 32, 32);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                g.DrawImage(m.minoImage0[j, i], m.point.X + j * 30, m.point.Y + i * 30, 32, 32);
            }
        }

    }
    public void fm_KeyDown(Object sender, KeyEventArgs e)
    {
        if((e.KeyCode == Keys.Right) & (Rightok()))
        {
            Point flm = m.fallingMino;
            flm.X += 1;
            m.fallingMino = flm;
            Point point = m.point;
            point.X += 30;
            m.point = point;
        }
        else if ((e.KeyCode == Keys.Left) & (Leftok()))
            {
                Point flm = m.fallingMino;
                flm.X -= 1;
                m.fallingMino = flm;
                Point point = m.point;
                point.X -= 30;
                m.point = point;
            }
        else if(e.KeyCode == Keys.Up)
        {
            while (lookBottom() == true)
            {
                fall(0); 
            }
        }
        else if(e.KeyCode == Keys.Down)
        {
            if(fallOk() == true)
            {
                gam.veryfast = true;
                fall(0);
                gam.veryfast = false;
            }
        }
        else if ((e.KeyCode == Keys.Z) & (rotateRightok()))
        {
            if(m.deg == 3)
            {
                m.deg = 0;
            }
            else
            {
                m.deg++;
            }
            minoDeg();
        }
        else if ((e.KeyCode == Keys.X) & (rotateLeftok()))
        {
            if (m.deg == 0)
            {
                m.deg = 3;
            }
            else
            {
                m.deg--;
            }
            minoDeg();
        }
    }

    public void tm_Tick(Object sender, EventArgs e)
    {
        if (lookBottom() == true)
            {
                fall(10);
            }
            else
            {
            Parallel.For(0, 8, id =>
            {
                Point flm = m.fallingMino;
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (m.minoShape[gam.kind, m.deg, j, i] == 1)
                        {
                            gam.placedMino[flm.Y + j, flm.X + i] = gam.kind;
                        }

                    }
                }
            });
                loadImage();
                gam.kind = rn.Next(7);
                m.fallingMino = new Point(6, 0);
                m.point = new Point(740, -90);
                m.deg = 0;
                minoDeg();
        }
        deleteRow();
        bgPaint();
        Invalidate();
    }
    public bool Rightok()
    {
        Point flm = m.fallingMino;
        bool rightok = true;
        int d = m.deg;
        //Parallel.For(0, 8, id =>
        //{
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m.minoShape[gam.kind, d, j, i] == 1)
                    {
                        if (!(gam.placedMino[flm.Y + j + 1, flm.X + i + 1] == -1 || gam.placedMino[flm.Y + j + 1, flm.X + i + 1] == -3))
                        {
                            rightok = false;
                        }
                    }

                }
            }
        //});
        return rightok;
    }

    public bool Leftok()
    {
        Point flm = m.fallingMino;
        bool leftok = true;
        int d = m.deg;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m.minoShape[gam.kind, d, j, i] == 1)
                    {
                        if (!(gam.placedMino[flm.Y + j + 1, flm.X + i - 1] == -1 || gam.placedMino[flm.Y + j + 1, flm.X + i - 1] == -3))
                        {
                            leftok = false;
                        }
                    }

                }
            }
        return leftok;
    }

    public bool rotateRightok()
    {
        Point flm = m.fallingMino;
        bool rotateok = true;
        int d = m.deg;
        if (d == 3)
        {
            d = 0;
        }
        else
        {
            d++;
        }
        //Parallel.For(0, 8, id =>
        //{
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m.minoShape[gam.kind, d, j, i] == 1)
                    {
                        if (!(gam.placedMino[flm.Y + j + 1, flm.X + i] == -1 || gam.placedMino[flm.Y + j + 1, flm.X + i] == -3))
                        {
                            rotateok = false;
                        }
                    }

                }
            }
        //});
        return rotateok;
    }

    public bool rotateLeftok()
    {
        Point flm = m.fallingMino;
        bool rotateok = true;
        int d = m.deg;
        if (d == 0)
        {
            d = 3;
        }
        else
        {
            d--;
        }
        //Parallel.For(0, 8, id =>
        //{
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m.minoShape[gam.kind, d, j, i] == 1)
                    {
                        if (!(gam.placedMino[flm.Y + j + 1, flm.X + i] == -1 || gam.placedMino[flm.Y + j + 1, flm.X + i] == -3))
                        {
                            rotateok = false;
                        }
                    }

                }
            }
        //});
        return rotateok;
    }
    public bool lookBottom()
    {
        Point flm = m.fallingMino;
        bool fallok = true;
        Parallel.For(0, 8, id =>
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m.minoShape[gam.kind, m.deg, j, i] == 1)
                    {
                        if (!(gam.placedMino[flm.Y + j + 1, flm.X + i] == -1 || gam.placedMino[flm.Y + j + 1, flm.X + i] == -3))
                        {
                            fallok = false;
                        }
                    }

                }
            }
        });
        return fallok;
    }
    public bool fallOk()
    {
        Point flm = m.fallingMino;
        bool fallok = true;
        Parallel.For(0, 8, id =>
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m.minoShape[gam.kind, m.deg, j, i] == 1)
                    {
                        if (!(gam.placedMino[flm.Y + j + 2, flm.X + i] == -1 || gam.placedMino[flm.Y + j + 2, flm.X + i] == -3))
                        {
                            fallok = false;
                        }
                    }

                }
            }
        });
        return fallok;
    }
    public void fall(int time)
    {
        Point minop = m.point;
        Point flMino = m.fallingMino;
        timer += 1;
        if (gam.veryfast == true)
        {
            if (timer >= time)
            {
                flMino.Y += 1;
                minop.Y += 30;
                m.fallingMino = flMino;
                timer = 0;
            }
            }
        else
        {
            if (timer >= time)
            {
                minop.Y += a[count];
                count += 1;
                if (count >= 10)
                {
                    flMino.Y += 1;
                    m.fallingMino = flMino;
                    timer = 0;
                    count = 0;
                }
            }
        }
        m.point = minop;
    }
    public bool deleteRowOK(int j)
    {
        bool OK = true;
        Parallel.For(0, 4, id =>
        {
            for (int i = 3; i <= 12; i++)
                {
                if (gam.placedMino[j + 3, i] == -1)
                {
                    OK = false;
                }
            }
        });
        return OK;
    }
    public void deleteRow()
    {
        Parallel.For(0, 4, id =>
        {
            for (int i = 20; i >= 0; i--)
            {
                if (deleteRowOK(i))
                {
                    deleteRowR(i);
                }
            }
        });
    }

    public int deleteRowR(int j)
    {
        if (j == 0)
        {
            for (int i = 0; i < 16; i++)
            {
                if ((i >= 3) & (i <= 12))
                {
                    gam.placedMino[4, i] = -1;
                }
                else
                {
                    gam.placedMino[4, i] = -2;
                }
            }
            //loadImage();
        }
        else
        {
            for (int i = 0; i < 16; i++)
            {
                gam.placedMino[j + 3, i] = gam.placedMino[j + 2, i];
            }
        }
            return j == 0 ? 0 : deleteRowR(j - 1);
    }
}
class game
    {
        public int[,] placedMino = new int[17, 24];
        public int kind;
        public bool veryfast;
    }
class mino
    {
        public Image[,] minoImage0 = new Image[4, 4];
        //Iミノ
        public int[,,,] minoShape = new int[7, 4, 4, 4]{{{{0,0,0,0},
                                                          {0,0,0,0},
                                                          {1,1,1,1},
                                                          {0,0,0,0}},

                                                         {{0,0,1,0},
                                                          {0,0,1,0},
                                                          {0,0,1,0},
                                                          {0,0,1,0}},

                                                         {{0,0,0,0},
                                                          {0,0,0,0},
                                                          {1,1,1,1},
                                                          {0,0,0,0}},

                                                         {{0,0,1,0},
                                                          {0,0,1,0},
                                                          {0,0,1,0},
                                                          {0,0,1,0}}},


            //一番うざいやつ
                                                        {{{0,0,0,0},
                                                          {0,1,1,0},
                                                          {0,1,1,0},
                                                          {0,0,0,0}},

                                                         {{0,0,0,0},
                                                          {0,1,1,0},
                                                          {0,1,1,0},
                                                          {0,0,0,0}},

                                                         {{0,0,0,0},
                                                          {0,1,1,0},
                                                          {0,1,1,0},
                                                          {0,0,0,0}},

                                                         {{0,0,0,0},
                                                          {0,1,1,0},
                                                          {0,1,1,0},
                                                          {0,0,0,0}}},


        //うざいやつ２
                                                        {{{0,0,0,0},
                                                          {0,0,0,0},
                                                          {0,1,1,0},
                                                          {1,1,0,0}},

                                                         {{0,0,0,0},
                                                          {1,0,0,0},
                                                          {1,1,0,0},
                                                          {0,1,0,0}},

                                                         {{0,0,0,0},
                                                          {0,0,0,0},
                                                          {0,1,1,0},
                                                          {1,1,0,0}},

                                                         {{0,0,0,0},
                                                          {1,0,0,0},
                                                          {1,1,0,0},
                                                          {0,1,0,0}}},


        //うざいやつ３
                                                        {{{0,0,0,0},
                                                          {0,0,0,0},
                                                          {1,1,0,0},
                                                          {0,1,1,0}},

                                                         {{0,0,0,0},
                                                          {0,1,0,0},
                                                          {1,1,0,0},
                                                          {1,0,0,0}},

                                                         {{0,0,0,0},
                                                          {0,0,0,0},
                                                          {1,1,0,0},
                                                          {0,1,1,0}},

                                                         {{0,0,0,0},
                                                          {0,1,0,0},
                                                          {1,1,0,0},
                                                          {1,0,0,0}}},


        //Lミノ1
                                                        {{{0,0,0,0},
                                                          {0,0,0,0},
                                                          {1,0,0,0},
                                                          {1,1,1,0}},

                                                         {{0,0,0,0},
                                                          {0,1,0,0},
                                                          {0,1,0,0},
                                                          {1,1,0,0}},

                                                         {{0,0,0,0},
                                                          {0,0,0,0},
                                                          {1,1,1,0},
                                                          {0,0,1,0}},

                                                         {{0,0,0,0},
                                                          {1,1,0,0},
                                                          {1,0,0,0},
                                                          {1,0,0,0}}},


        //Lミノ2
                                                        {{{0,0,0,0},
                                                          {0,0,0,0},
                                                          {0,0,1,0},
                                                          {1,1,1,0}},

                                                         {{0,0,0,0},
                                                          {1,1,0,0},
                                                          {0,1,0,0},
                                                          {0,1,0,0}},

                                                         {{0,0,0,0},
                                                          {0,0,0,0},
                                                          {1,1,1,0},
                                                          {1,0,0,0}},

                                                         {{0,0,0,0},
                                                          {1,0,0,0},
                                                          {1,0,0,0},
                                                          {1,1,0,0}}},



                                                        {{{0,0,0,0},
                                                          {0,0,0,0},
                                                          {0,1,0,0},
                                                          {1,1,1,0}},

                                                         {{0,0,0,0},
                                                          {0,0,1,0},
                                                          {0,1,1,0},
                                                          {0,0,1,0}},

                                                         {{0,0,0,0},
                                                          {1,1,1,0},
                                                          {0,1,0,0},
                                                          {0,0,0,0}},

                                                         {{0,0,0,0},
                                                          {1,0,0,0},
                                                          {1,1,0,0},
                                                          {1,0,0,0}}}};
        public int deg;
        public Point point;
        public Point fallingMino;
    }