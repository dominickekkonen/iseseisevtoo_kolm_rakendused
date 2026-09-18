using System;
using System.Drawing;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public class DrawingForm : Form
    {
        private PictureBox joonistusAla;
        private Bitmap joonistusPilt;

        private Point viimanePunkt;
        private bool joonistan;

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
            Size = new Size(720, 640);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.LightGray;
        }

        private void CreateControls()
        {
            joonistusAla = new PictureBox
            {
                Location = new Point(20, 20),
                Size = new Size(660, 470),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            joonistusAla.MouseDown += JoonistusAla_MouseDown;
            joonistusAla.MouseMove += JoonistusAla_MouseMove;
            joonistusAla.MouseUp += JoonistusAla_MouseUp;

            puhastaNupp = new Button
            {
                Text = "Puhasta",
                Location = new Point(20, 510),
                Size = new Size(150, 40)
            };

            salvestaNupp = new Button
            {
                Text = "Salvesta joonistus",
                Location = new Point(190, 510),
                Size = new Size(180, 40)
            };

            sulgeNupp = new Button
            {
                Text = "Sulge",
                Location = new Point(500, 510),
                Size = new Size(150, 40)
            };

            puhastaNupp.Click += PuhastaNupp_Click;
            salvestaNupp.Click += SalvestaNupp_Click;
            sulgeNupp.Click += SulgeNupp_Click;

            Controls.Add(joonistusAla);
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

        private void JoonistusAla_MouseDown(object sender, MouseEventArgs e)
        {
            joonistan = true;
            viimanePunkt = e.Location;
        }

        private void JoonistusAla_MouseMove(object sender, MouseEventArgs e)
        {
            if (!joonistan)
                return;

            using (Graphics g = Graphics.FromImage(joonistusPilt))
            {
                using (Pen pliiats = new Pen(Color.Black, 3))
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
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter =
                "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string extension = System.IO.Path.GetExtension(dialog.FileName).ToLower();

                if (extension == ".jpg")
                {
                    joonistusPilt.Save(
                        dialog.FileName,
                        System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else if (extension == ".bmp")
                {
                    joonistusPilt.Save(
                        dialog.FileName,
                        System.Drawing.Imaging.ImageFormat.Bmp);
                }
                else
                {
                    joonistusPilt.Save(
                        dialog.FileName,
                        System.Drawing.Imaging.ImageFormat.Png);
                }

                MessageBox.Show("Joonistus on salvestatud!");
            }
        }

        private void SulgeNupp_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}