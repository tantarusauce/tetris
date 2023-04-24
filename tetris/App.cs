using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.ComponentModel.Design;

class tetris : Form
{
    private Image[] img = new Image[14];
    private Image[,] bg = new Image[17, 24];
    private Image backg, holdImg, scoreSheet, bcgr;
    private Mino m;
    private Game gam;
    private Label titleLabel;
    private Label scoreLabel;
    private Label startLabel;
    private Label retryLabel;
    private Label exitLabel;
    Random rn = new Random();
    private Image minom1img;
    private int scene = -1;//-1:titleinit 0:title 1:None 2:gameinit 3:game 
    private int timer = 0;
    private int rc = 0;
    private int rct = 0;
    private int count = 0;
    private int timerCount = 0;
    private bool keysUp = false;
    private bool startMouseEnter = false;
    private bool retryMouseEnter = false;
    private bool exitMouseEnter = false;
    private int[] a = { 1, 2, 3, 4, 5, 5, 4, 3, 2, 1 };
    System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();

    public static void Main()
    {
        Application.Run(new tetris());
    }

    public tetris()
    {
        this.Text = "faketetris";
        this.ClientSize = new Size(1280, 720);
        this.DoubleBuffered = true;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Paint += new PaintEventHandler(fm_Paint);
        this.KeyDown += new KeyEventHandler(fm_KeyDown);
        tm.Tick += new EventHandler(tm_Tick);
        tm.Start();
    }

    public void loadImage()
    {
        minom1img = Image.FromFile(".\\resources\\mino_-1.png");
        scoreSheet = Image.FromFile(".\\resources\\scoreInd.png");
        backg = Image.FromFile(".\\resources\\background.png");
        bcgr = Image.FromFile(".\\resources\\background0.png");
        holdImg = Image.FromFile(".\\resources\\hold.png");
        for (int i = 0; i <= 13; i++)
        {
            img[i] = Image.FromFile(".\\resources\\mino_" + i + ".png");
        }
    }
    public void titleInit()
    {
        loadImage();
        titleLabel = new Label();
        titleLabel.Font = new Font("MS UI Gothic", 50);
        titleLabel.Size = new Size(1280, 400);
        titleLabel.Location = new Point(300, 100);
        titleLabel.BackColor = Color.Transparent;
        titleLabel.Parent = this;
        titleLabel.Text = "　PCの性能がいいほど\n難易度が上がるテトリス";
        startLabel = new Label();
        startLabel.Font = new Font("MS UI Gothic", 50);
        startLabel.Size = new Size(500, 100);
        startLabel.Location = new Point(500, 500);
        startLabel.BackColor = Color.Transparent;
        startLabel.Parent = this;
        startLabel.Text = "S T A R T";
    }
    public void fm_Paint(Object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        switch (scene)
        {
            case 0://title
                g.DrawImage(bcgr, this.ClientRectangle);
                g.DrawImage(scoreSheet, 280, 30, 700, 300);
                int transp = startMouseEnter ? 7 : 0;
                for (int j = 0; j < 5; j++)
                {
                    g.DrawImage(img[transp], j * 60 + 500, 500, 64, 64);
                }

                break;

            case 3://gameloop
                Point flm = m.fallingMino;
                g.DrawImage(bcgr, this.ClientRectangle);
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
                        g.DrawImage(m.minoImage1[j, i], m.point.X + j * 30, (flm.Y + i + fallShadow() - 4) * 30, 32, 32);
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        g.DrawImage(m.minoImage0[j, i], m.point.X + j * 30, m.point.Y + i * 30, 32, 32);
                    }
                }
                if (!(gam.combo == 0))
                {
                    if (gam.score != 0)
                    {
                        scoreLabel.Text = "SCORE:" + gam.score.ToString() + "0\n\nLINE(S):" + gam.deletedRow.ToString() +
                    "\n\nLEVEL:" + gam.level.ToString() + "\n\nCOMBO:" + gam.combo.ToString();
                    }
                    else
                    {
                        scoreLabel.Text = "SCORE:" + gam.score.ToString() + "\n\nLINE(S):" + gam.deletedRow.ToString() +
                    "\n\nLEVEL:" + gam.level.ToString() + "\n\nCOMBO:" + gam.combo.ToString();
                    }
                }
                else
                {
                    if (gam.score != 0)
                    {
                        scoreLabel.Text = "\nSCORE:" + gam.score.ToString() + "0\n\nLINE(S):" + gam.deletedRow.ToString() +
                            "\n\nLEVEL:" + gam.level.ToString();
                    }
                    else
                    {
                        scoreLabel.Text = "\nSCORE:" + gam.score.ToString() + "\n\nLINE(S):" + gam.deletedRow.ToString() +
                            "\n\nLEVEL:" + gam.level.ToString();
                    }
                }
                g.DrawImage(scoreSheet, 30, 30, 600, 300);
                g.DrawImage(holdImg, 500, 200, 128, 128);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        g.DrawImage(m.minoImageHold[j, i], 535 + j * 15, 245 + i * 15, 16, 16);
                    }
                }
                break;
            case 5:
                flm = m.fallingMino;
                g.DrawImage(bcgr, this.ClientRectangle);
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
                        g.DrawImage(m.minoImage1[j, i], m.point.X + j * 30, (flm.Y + i + fallShadow() - 4) * 30, 32, 32);
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        g.DrawImage(m.minoImage0[j, i], m.point.X + j * 30, m.point.Y + i * 30, 32, 32);
                    }
                }
                if (!(gam.combo == 0))
                {
                    if (gam.score != 0)
                    {
                        scoreLabel.Text = "SCORE:" + gam.score.ToString() + "0\n\nLINE(S):" + gam.deletedRow.ToString() +
                    "\n\nLEVEL:" + gam.level.ToString() + "\n\nCOMBO:" + gam.combo.ToString();
                    }
                    else
                    {
                        scoreLabel.Text = "SCORE:" + gam.score.ToString() + "\n\nLINE(S):" + gam.deletedRow.ToString() +
                    "\n\nLEVEL:" + gam.level.ToString() + "\n\nCOMBO:" + gam.combo.ToString();
                    }
                }
                else
                {
                    if (gam.score != 0)
                    {
                        scoreLabel.Text = "\nSCORE:" + gam.score.ToString() + "0\n\nLINE(S):" + gam.deletedRow.ToString() +
                            "\n\nLEVEL:" + gam.level.ToString();
                    }
                    else
                    {
                        scoreLabel.Text = "\nSCORE:" + gam.score.ToString() + "\n\nLINE(S):" + gam.deletedRow.ToString() +
                            "\n\nLEVEL:" + gam.level.ToString();
                    }
                }
                g.DrawImage(scoreSheet, 30, 30, 600, 300);
                g.DrawImage(holdImg, 500, 200, 128, 128);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        g.DrawImage(m.minoImageHold[j, i], 535 + j * 15, 245 + i * 15, 16, 16);
                    }
                }
                int transp1 = retryMouseEnter ? 8 : 1;
                int transp2 = exitMouseEnter ? 9 : 2;
                for (int j = 0; j < 5; j++)
                {
                    g.DrawImage(img[transp1], j * 60 + 600, 240, 64, 64);
                }
                for (int j = 0; j < 4; j++)
                {
                    g.DrawImage(img[transp2], j * 55 + 640, 480, 60, 60);
                }
                break;

        }

    }
    public void fm_KeyDown(Object sender, KeyEventArgs e)
    {
        if ((e.KeyCode == Keys.Right) & (rightOk()))
        {
            Point flm = m.fallingMino;
            flm.X += 1;
            m.fallingMino = flm;
            Point point = m.point;
            point.X += 30;
            m.point = point;
        }
        else if ((e.KeyCode == Keys.Left) & (leftOk()))
        {
            Point flm = m.fallingMino;
            flm.X -= 1;
            m.fallingMino = flm;
            Point point = m.point;
            point.X -= 30;
            m.point = point;
        }
        else if (e.KeyCode == Keys.Up)
        {
            keysUp = true;
            while (lookBottom() == true)
            {
                fall(0);
            }
        }
        else if (e.KeyCode == Keys.Down)
        {
            if (fallOk() == true)
            {
                gam.veryfast = true;
                fall(0);
                gam.veryfast = false;
            }
        }
        else if ((e.KeyCode == Keys.Z) & (rotaterightOk()))
        {
            if (m.deg == 3)
            {
                m.deg = 0;
            }
            else
            {
                m.deg++;
            }
            minoDeg();
        }
        else if ((e.KeyCode == Keys.X) & (rotateleftOk()))
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
        else if (e.KeyCode == Keys.C)
        {
            if (holdOk())
            {
                if (gam.hold == -3)
                {
                    gam.hold = gam.kind;
                    gam.kind = gam.hidHold;
                }
                else
                {
                    int temp;
                    temp = gam.hold;
                    gam.hold = gam.kind;
                    gam.kind = temp;
                }
                if (gam.holded == false)
                {
                    m.fallingMino = new Point(6, 0);
                    Point minop = new Point(740, -90);
                    m.point = minop;
                    gam.holded = true;
                }
                minoDeg();
            }
        }
    }
    public void fm_MouseEnter(Object sender, EventArgs e)
    {
        startMouseEnter = true;
    }
    public void fm_MouseLeave(Object sender, EventArgs e)
    {
        startMouseEnter = false;
    }
    public void fm_MouseClick(Object sender, EventArgs e)
    {
        scene = 2;
    }
    public void retry_MouseEnter(Object sender, EventArgs e)
    {
        retryMouseEnter = true;
    }
    public void retry_MouseLeave(Object sender, EventArgs e)
    {
        retryMouseEnter = false;
    }
    public void retry_MouseClick(Object sender, EventArgs e)
    {
        scene = 1;
    }
    public void exit_MouseEnter(Object sender, EventArgs e)
    {
        exitMouseEnter = true;
    }
    public void exit_MouseLeave(Object sender, EventArgs e)
    {
        exitMouseEnter = false;
    }
    public void exit_MouseClick(Object sender, EventArgs e)
    {
        scene = -2;
    }
    public void gameDispose()
    {
        scoreLabel.Dispose();
        exitLabel.Dispose();
        retryLabel.Dispose();
    }
    public void titleLoop()
    {
        startLabel.MouseEnter += new EventHandler(fm_MouseEnter);
        startLabel.MouseLeave += new EventHandler(fm_MouseLeave);
        startLabel.MouseClick += new MouseEventHandler(fm_MouseClick);
    }
    public void init()
    {
        tm.Interval = 20;
        titleLabel.Dispose();
        startLabel.Dispose();
        m = new Mino();
        gam = new Game();
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
                           {-2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2}};
        int d = 0;
        int k = rn.Next(7);
        m.deg = d;
        m.fallingMino = new Point(6, 0);
        Point minop = new Point(740, -90);
        m.point = minop;
        gam.placedMino = initMino;
        gam.kind = rn.Next(7);
        gam.hidHold = rn.Next(7);
        gam.hold = -3;
        gam.veryfast = false;
        gam.score = 0;
        gam.deletedRow = 0;
        gam.level = 0;
        gam.combo = 0;
        gam.holded = false;
        gam.tickCount = 0;
        gam.fallTick = 0;
        int[] initDRN = {-1, -1, -1, -1};
        gam.deletedRowNum = initDRN;
        scoreLabel = new Label();
        scoreLabel.Font = new Font("MS UI Gothic", 30);
        scoreLabel.Size = new Size(500, 300);
        scoreLabel.Location = new Point(30, 30);
        scoreLabel.BackColor = Color.Transparent;
        scoreLabel.Parent = this;
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
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                d = m.deg;
                if (m.minoShape[gam.kind, d, i, j] == 1)
                {
                    m.minoImage1[j, i] = img[gam.kind + 7];
                }
                else
                {
                    m.minoImage1[j, i] = minom1img;
                }
            }
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                d = m.deg;
                if(gam.hold == -3)
                {
                    m.minoImageHold[j, i] = backg;
                }
                else
                {
                    if (m.minoShape[gam.hold, d, i, j] == 1)
                    {
                        m.minoImageHold[j, i] = img[gam.hold];
                    }
                    else
                    {
                        m.minoImageHold[j, i] = backg;
                    }
                }
                
            }
        }
    }

    public void gameLoop()
    {
        if (gam.tickCount != 2)
        {
            gam.tickCount += 1;
        }
        if (lookBottom() == true)
        {
            if (gam.deletedRow < 10) gam.level = 0;
            else if (gam.deletedRow < 20) gam.level = 1;
            else if (gam.deletedRow < 30) gam.level = 2;
            else if (gam.deletedRow < 40) gam.level = 3;
            else if (gam.deletedRow < 50) gam.level = 4;
            else if (gam.deletedRow < 60) gam.level = 5;
            else if (gam.deletedRow < 70) gam.level = 6;
            else if (gam.deletedRow < 80) gam.level = 7;
            else if (gam.deletedRow < 90) gam.level = 8;
            else if (gam.deletedRow < 100) gam.level = 9;
            else if (gam.deletedRow < 110) gam.level = 10;
            else if (gam.deletedRow < 120) gam.level = 11;
            else if (gam.deletedRow < 130) gam.level = 12;
            else if (gam.deletedRow < 140) gam.level = 13;
            else gam.level = 14;
            switch (gam.level)
            {
                case 0:
                    gam.fallTick = 20;
                    break;
                case 1:
                    gam.fallTick = 15;
                    break;
                case 2:
                    gam.fallTick = 10;
                    break;
                case 3:
                    gam.fallTick = 5;
                    break;
                case 4:
                    gam.fallTick = 0;
                    break;
                case 5:
                    gam.veryfast = true;
                    gam.fallTick = 9;
                    break;
                case 6:
                    gam.veryfast = true;
                    gam.fallTick = 8;
                    break;
                case 7:
                    gam.veryfast = true;
                    gam.fallTick = 7;
                    break;
                case 8:
                    gam.veryfast = true;
                    gam.fallTick = 6;
                    break;
                case 9:
                    gam.veryfast = true;
                    gam.fallTick = 5;
                    break;
                case 10:
                    gam.veryfast = true;
                    gam.fallTick = 4;
                    break;
                case 11:
                    gam.veryfast = true;
                    gam.fallTick = 3;
                    break;
                case 12:
                    gam.veryfast = true;
                    gam.fallTick = 2;
                    break;
                case 13:
                    gam.veryfast = true;
                    gam.fallTick = 1;
                    break;
                case 14:
                    gam.veryfast = true;
                    gam.fallTick = 0;
                    break;
            }
            fall(gam.fallTick);
        }
        else
        {
            if ((timerCount >= gam.fallTick) || (keysUp))
            {
                keysUp = false;
                timerCount = 0;
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
                gam.holded = false;
                bgPaint();
                gam.kind = rn.Next(7);
                m.fallingMino = new Point(6, 0);
                m.point = new Point(740, -90);
                m.deg = 0;
                minoDeg();
                rct = deleteRow();
                rc = rct;
                while (!(rct == 0))
                {
                    rct = deleteRow();
                    rc += rct;
                }
                scoreCal(rc, gam.combo);
                if (!(rc == 0)) gam.combo += 1; else gam.combo = 0;
            }
            else
            {
                timerCount++;
            }
        }
        if (gameOver()) scene = 4;
        bgPaint();
    }
    public void tm_Tick(Object sender, EventArgs e)
    {
        switch (scene)
        {
            case -2:
                gameDispose();
                scene = -1;
                break;
            case -1:
                titleInit();
                scene = 0;
                break;
            case 0:
                titleLoop();
                break;
            case 1:
                gameDispose();
                scene = -1;
                break;
            case 2:
                init();
                scene = 3;
                break;
            case 3:
                gameLoop();
                break;
            case 4:
                gameOverInit();
                scene = 5;
                break;
            case 5:
                gameOverLoop();
                break;
        }
        Invalidate();
    }
    public void gameOverInit()
    {
        retryLabel = new Label();
        retryLabel.Font = new Font("MS UI Gothic", 50);
        retryLabel.Size = new Size(400, 100);
        retryLabel.Location = new Point(600, 240);
        retryLabel.BackColor = Color.Transparent;
        retryLabel.Parent = this;
        retryLabel.Text = "R E T R Y";
        exitLabel = new Label();
        exitLabel.Font = new Font("MS UI Gothic", 50);
        exitLabel.Size = new Size(400, 100);
        exitLabel.Location = new Point(600, 480);
        exitLabel.BackColor = Color.Transparent;
        exitLabel.Parent = this;
        exitLabel.Text = "  E X I T ";
    }
    public void gameOverLoop()
    {
        exitLabel.MouseEnter += new EventHandler(exit_MouseEnter);
        exitLabel.MouseLeave += new EventHandler(exit_MouseLeave);
        exitLabel.MouseClick += new MouseEventHandler(exit_MouseClick);
        retryLabel.MouseEnter += new EventHandler(retry_MouseEnter);
        retryLabel.MouseLeave += new EventHandler(retry_MouseLeave);
        retryLabel.MouseClick += new MouseEventHandler(retry_MouseClick);
    }
    public bool gameOver()
    {
        for (int i = 3; i < 13; i++)
        {
            if(!(gam.placedMino[5, i] == -1)) return true;
        }
        return false;
    }
    public void scoreCal(int rc, int ratio)
    {
        gam.deletedRow += rc;
        ratio++;
        switch(rc)
        {
            case 1:
                gam.score += 1 * ratio;
                break;
            case 2:
                gam.score += 5 * ratio;
                break;
            case 3:
                gam.score += 10 * ratio;
                break;
            case 4:
                gam.score += 50 * ratio;
                break;
        }

    }
    public bool rightOk()
    {
        Point flm = m.fallingMino;
        bool rightOk = true;
        int d = m.deg;
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                if (m.minoShape[gam.kind, d, j, i] == 1)
                {
                    if (!(gam.placedMino[flm.Y + j, flm.X + i + 1] == -1 || gam.placedMino[flm.Y + j, flm.X + i + 1] == -3))
                    {
                        rightOk = false;
                    }
                }

            }
        }
        return rightOk;
    }

    public bool leftOk()
    {
        Point flm = m.fallingMino;
        bool leftOk = true;
        int d = m.deg;
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                if (m.minoShape[gam.kind, d, j, i] == 1)
                {
                    if (!(gam.placedMino[flm.Y + j, flm.X + i - 1] == -1 || gam.placedMino[flm.Y + j, flm.X + i - 1] == -3))
                    {
                        leftOk = false;
                    }
                }

            }
        }
        return leftOk;
    }

    public bool rotaterightOk()
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
        return rotateok;
    }
    public bool rotateleftOk()
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
    public bool holdOk()
    {
        Point flm = m.fallingMino;
        bool fallok = true;
        Parallel.For(0, 8, id =>
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m.minoShape[(gam.hold == -3) ? gam.hidHold: gam.hold, m.deg, j, i] == 1)
                    {
                        if (!(gam.placedMino[flm.Y + j, flm.X + i] == -1 || gam.placedMino[flm.Y + j, flm.X + i] == -3))
                        {
                            fallok = false;
                        }
                    }

                }
            }
        });
        return fallok;
    }
    public int fallShadow()
    {
        bool fallok = true;
        int fallS = 0;
        do
        {
            Point flm = m.fallingMino;
            Parallel.For(0, 8, id =>
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (m.minoShape[gam.kind, m.deg, j, i] == 1)
                        {
                            if (!(gam.placedMino[flm.Y + j + 1 + fallS, flm.X + i] == -1 || gam.placedMino[flm.Y + j + 1 + fallS, flm.X + i] == -3))
                            {
                                fallok = false;
                            }
                        }
                    }
                }
            });
            fallS++;
        } while (fallok);
        return fallS;
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
        //gam.veryfast = true;
        //timer = time;
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
    public int deleteRow()
    {
        int rowCount = 0;
            for (int i = 20; i >= 0; i--)
            {
                if (deleteRowOK(i))
                {
                    deleteRowR(i);
                    rowCount++;
            }
        }
        rowCount -= 1;
        return rowCount;
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
class Game
{
    public int[,] placedMino = new int[17, 24];
    public int kind;
    public int hold;
    public int hidHold;
    public bool veryfast;
    public int score;
    public int deletedRow;
    public int level;
    public bool holded;
    public int combo;
    public int tickCount;
    public int fallTick;
    public int[] deletedRowNum = new int[4];
}
class Mino
{
    public Image[,] minoImage0 = new Image[4, 4];
    public Image[,] minoImage1 = new Image[4, 4];
    public Image[,] minoImageHold = new Image[4, 4];
    //Iミノ
    public int[,,,] minoShape = new int[7, 4, 4, 4]    {{{{0,0,0,0},
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
                                                          {1,0,0,0},
                                                          {1,1,1,0},
                                                          {0,0,0,0}},

                                                         {{0,0,0,0},
                                                          {0,1,0,0},
                                                          {0,1,0,0},
                                                          {1,1,0,0}},

                                                         {{0,0,0,0},
                                                          {0,0,0,0},
                                                          {1,1,1,0},
                                                          {0,0,1,0}},

                                                         {{0,0,0,0},
                                                          {0,1,1,0},
                                                          {0,1,0,0},
                                                          {0,1,0,0}}},


        //Lミノ2
                                                        {{{0,0,0,0},
                                                          {0,0,1,0},
                                                          {1,1,1,0},
                                                          {0,0,0,0}},

                                                         {{0,0,0,0},
                                                          {1,1,0,0},
                                                          {0,1,0,0},
                                                          {0,1,0,0}},

                                                         {{0,0,0,0},
                                                          {0,0,0,0},
                                                          {1,1,1,0},
                                                          {1,0,0,0}},

                                                         {{0,0,0,0},
                                                          {0,1,0,0},
                                                          {0,1,0,0},
                                                          {0,1,1,0}}},



                                                        {{{0,0,0,0},
                                                          {0,1,0,0},
                                                          {1,1,1,0},
                                                          {0,0,0,0}},

                                                         {{0,0,0,0},
                                                          {0,1,0,0},
                                                          {1,1,0,0},
                                                          {0,1,0,0}},

                                                         {{0,0,0,0},
                                                          {0,0,0,0},
                                                          {1,1,1,0},
                                                          {0,1,0,0}},

                                                         {{0,0,0,0},
                                                          {0,1,0,0},
                                                          {0,1,1,0},
                                                          {0,1,0,0}}}};
    public int deg;
    public Point point;
    public Point fallingMino;
}