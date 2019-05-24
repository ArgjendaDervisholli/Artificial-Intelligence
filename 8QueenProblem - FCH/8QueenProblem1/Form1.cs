using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace _8QueenProblem1
{
    public partial class QueenProblem : Form
    {
        static int N;
        int[,] shahu;
        int locationX = 0;
        int locationY = 0;
        int boxSize = 50;
        int paddingBox = 10;
        int x = 1;
        PictureBox picture;
        public QueenProblem()
        {
            InitializeComponent();
            xTextBox.Text = "1";
            luaj();

        }
        private void btnSolve_Click(object sender, EventArgs e) { }

        public bool VendosMbretreshen(int q, int N)
        {
            if (q == N)
                return true;

            for (int i = 0; i < N; i++)
            {
                // kontrollojm a guxojm me pozicionu ne rreshtin i kolonen q
                if (kontrolloVendosjen(i, q))
                {
                    if (shahu[i, q] != -1)
                    {
                        shahu[i, q] = 1;
                        if (q < N - 1)
                        {
                            for (int j = 0; j < N; j++)
                                if (kontrolloVendosjen(j, q + 1))
                                {
                                    if (shahu[j, q] != -1)
                                    {
                                        if (VendosMbretreshen(q + 1, N))
                                            return true;
                                    }
                                }
                        }
                        else
                        {
                            VendosMbretreshen(q + 1, N);
                            return true;
                        }
                        shahu[i, q] = 0;
                    }
                }
            }
            return false;
        }
        
        public bool kontrolloVendosjen(int rr, int k) //Rreshti dhe Kolona
        {
            for (int j = 0; j < k; j++)
                if (shahu[rr, j] == 1)
                    return false;

            for (int i = rr + 1, j = k - 1; i < shahu.GetLength(0) && j >= 0; i++, j--)
            {
                if (shahu[i, j] == 1)
                    return false;
            }

            for (int i = rr - 1, j = k - 1; i >= 0 && j >= 0; i--, j--)
                if (shahu[i, j] == 1)
                    return false;

            return true;
        }
        public void vizatoBox(int i, int j, bool mbreteresha)
        {
            pictureBox1.Size = new System.Drawing.Size((N * boxSize + 20), (N * boxSize + 20));
            pictureBox1.Location = new System.Drawing.Point(300, 50);
            pictureBox1.Visible = true;
            picture = new PictureBox
            {
                Name = "pictureBox",
                Size = new Size(boxSize, boxSize),
                Location = new Point(locationX + boxSize * j + paddingBox, locationY + boxSize * i + paddingBox)
            };
            if ((i % 2 == 0 && j % 2 == 0) || (i % 2 == 1 && j % 2 == 1))
                picture.BackColor = System.Drawing.Color.Black;
            else
                picture.BackColor = System.Drawing.Color.White;
            if (mbreteresha == true)
                picture.Image = Image.FromFile(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"..\..\..\Images\BQueen.gif");
            if (shahu[i, j] == -1 && mbreteresha == false)
            {
                picture.BackColor = System.Drawing.Color.Black;
                picture.Image = Image.FromFile(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"..\..\..\Images\x.png");
            }
            pictureBox1.Controls.Add(picture);
        }
        public void fshij()
        {
            pictureBox1.Controls.Clear();
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            lblPositionXY.Text = "";
            luaj((int)nrOfBlockedPos.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblPositionXY.Text = "";
            luaj((int)nrOfBlockedPos.Value);
        }

        private void nrOfBlockedPos_ValueChanged(object sender, EventArgs e)
        {
            lblPositionXY.Text = "";
            luaj((int)nrOfBlockedPos.Value);
        }

        public void luaj(int numri=0)
        {
            fshij();
            N = Convert.ToInt32(Math.Round(numericUpDown1.Value, 0));
            shahu = new int[N, N];
            for (int i = 0; i < shahu.GetLength(0); i++)
                for (int j = 0; j < shahu.GetLength(0); j++)
                {
                    shahu[i, j] = 0;
                }

            for (int k = 0; k < numri; k++)
            {
                Random rnd = new Random();
                int i = rnd.Next(0, N);
                Thread.Sleep(50);
                int j = rnd.Next(0, N);
                if (shahu[i, j] != -1)
                {
                    shahu[i, j] = -1;
                    lblPositionXY.Text = lblPositionXY.Text + "i: " +i+ ", j: " +j+ "\n";
                }
                else
                    k--;
            }

            if (Convert.ToInt32(xTextBox.Text) < N)
            {
                shahu[Convert.ToInt32(xTextBox.Text), 0] = 1;
                if (VendosMbretreshen(1, N) == true)
                {
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            if (shahu[i, j] == 1)
                                vizatoBox(i, j, true);
                            else
                                vizatoBox(i, j, false);
                        }
                    }
                    label2.ForeColor = System.Drawing.Color.Green;
                    label2.Text = "Solution exists!";
                }
                else
                {
                    label2.ForeColor = System.Drawing.Color.Red;
                    label2.Text = "Solution doesn't exists!";
                }
            }
        }

        private void QueenProblem_Load(object sender, EventArgs e)
        {

        }
    }
}

