using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public class DrawingForm : Form
    {
        private PictureBox joonistusAla;
        private Bitmap joonistusPilt;

        private Point viimanePunkt;
        private bool joonistan;

        private Color valitudVarv = Color.Black;

        private Button varviNupp;
        private Label paksusLabel;
        private TrackBar paksusTriib;
        private Label paksusVaartusLabel;

        private Button puhastaNupp;
        private Button salvestaNupp;
        private Button sulgeNupp;

        public DrawingForm(Image algnePilt)
        {
            InitializeForm();
            CreateControls();
            LooJoonistusPilt(algnePilt);
        }

        private void InitializeForm()
        {
            Text = "Joonistamine";
            Size = new Size(720, 700);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = RakendusSeaded.TaustaVarv ?? Color.LightGray;
        }

        private void CreateControls()
        {
            joonistusAla = new PictureBox
            {
                Location = new Point(20, 20),
                Size = new Size(660, 460),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            joonistusAla.MouseDown += JoonistusAla_MouseDown;
            joonistusAla.MouseMove += JoonistusAla_MouseMove;
            joonistusAla.MouseUp += JoonistusAla_MouseUp;

            varviNupp = new Button
            {
                Text = "Värv",
                Location = new Point(20, 495),
                Size = new Size(150, 40),
                BackColor = valitudVarv,
                ForeColor = Color.White
            };

            paksusLabel = new Label
            {
                Text = "Joone paksus:",
                Font = new Font("Arial", 10),
                AutoSize = true,
                Location = new Point(190, 505)
            };

            paksusTriib = new TrackBar
            {
                Location = new Point(310, 490),
                Size = new Size(220, 45),
                Minimum = 1,
                Maximum = 20,
                Value = 3,
                TickFrequency = 1
            };

            paksusVaartusLabel = new Label
            {
                Text = "3",
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(540, 505)
            };

            paksusTriib.ValueChanged += PaksusTriib_ValueChanged;

            puhastaNupp = new Button
            {
                Text = "Puhasta",
                Location = new Point(20, 550),
                Size = new Size(150, 40)
            };

            salvestaNupp = new Button
            {
                Text = "Salvesta joonistus",
                Location = new Point(190, 550),
                Size = new Size(180, 40)
            };

            sulgeNupp = new Button
            {
                Text = "Sulge",
                Location = new Point(500, 550),
                Size = new Size(150, 40)
            };

            varviNupp.Click += VarviNupp_Click;
            puhastaNupp.Click += PuhastaNupp_Click;
            salvestaNupp.Click += SalvestaNupp_Click;
            sulgeNupp.Click += SulgeNupp_Click;

            Controls.Add(joonistusAla);
            Controls.Add(varviNupp);
            Controls.Add(paksusLabel);
            Controls.Add(paksusTriib);
            Controls.Add(paksusVaartusLabel);
            Controls.Add(puhastaNupp);
            Controls.Add(salvestaNupp);
            Controls.Add(sulgeNupp);
        }

        private void LooJoonistusPilt(Image algnePilt)
        {
            joonistusPilt = new Bitmap(joonistusAla.Width, joonistusAla.Height);

            using (Graphics g = Graphics.FromImage(joonistusPilt))
            {
                g.Clear(Color.White);

                if (algnePilt != null)
                {
                    g.DrawImage(
                        algnePilt,
                        new Rectangle(0, 0, joonistusAla.Width, joonistusAla.Height));
                }
            }

            joonistusAla.Image = joonistusPilt;
        }

        private void VarviNupp_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                valitudVarv = dialog.Color;
                varviNupp.BackColor = valitudVarv;
            }
        }

        private void PaksusTriib_ValueChanged(object sender, EventArgs e)
        {
            paksusVaartusLabel.Text = paksusTriib.Value.ToString();
        }

        private void JoonistusAla_MouseDown(object sender, MouseEventArgs e)
        {
            joonistan = true;
            viimanePunkt = e.Location;
        }

        private void JoonistusAla_MouseMove(object sender, MouseEventArgs e)
        {
            if (!joonistan)
                return;

            int paksus = paksusTriib.Value;

            using (Graphics g = Graphics.FromImage(joonistusPilt))
            {
                using (Pen pliiats = new Pen(valitudVarv, paksus))
                {
                    g.DrawLine(pliiats, viimanePunkt, e.Location);
                }
            }

            viimanePunkt = e.Location;

            joonistusAla.Invalidate();
        }

        private void JoonistusAla_MouseUp(object sender, MouseEventArgs e)
        {
            joonistan = false;
        }

        private void PuhastaNupp_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(joonistusPilt))
            {
                g.Clear(Color.White);
            }

            joonistusAla.Invalidate();
        }

        private void SalvestaNupp_Click(object sender, EventArgs e)
        {
            string vaikimisiNimi = "joonistus_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

            NimeSisestusForm nimeVorm = new NimeSisestusForm(vaikimisiNimi);

            if (nimeVorm.ShowDialog() != DialogResult.OK)
                return;

            string sisestatudNimi = nimeVorm.SisestatudNimi;

            if (string.IsNullOrWhiteSpace(sisestatudNimi))
            {
                sisestatudNimi = vaikimisiNimi;
            }

            string kaust = PildikoguAbi.KaustaTee();
            string failiNimi = PildikoguAbi.PuhastaFailiNimi(sisestatudNimi) + ".png";
            string teeFaili = PildikoguAbi.LeiaVabaTee(kaust, failiNimi);

            joonistusPilt.Save(teeFaili, System.Drawing.Imaging.ImageFormat.Png);

            MessageBox.Show(
                "Joonistus on salvestatud pildikogusse!\n\n" + Path.GetFileName(teeFaili),
                "Salvestatud",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void SulgeNupp_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}