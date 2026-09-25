using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public class PictureLibraryForm : Form
    {
        private ListBox pildiNimekiri;
        private PictureBox eelvaade;
        private Button valiNupp;
        private Button jatkaNupp;
        private Button kustutaNupp;

        private List<string> pildiTeed = new List<string>();

        public string SelectedImagePath { get; private set; }

        public PictureLibraryForm()
        {
            InitializeForm();
            CreateControls();
            TaidaNimekiri();
        }

        private void InitializeForm()
        {
            Text = "Pildikogu";
            Size = new Size(620, 530);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = RakendusSeaded.TaustaVarv ?? Color.LightGray;
        }

        private void CreateControls()
        {
            pildiNimekiri = new ListBox
            {
                Location = new Point(20, 20),
                Size = new Size(220, 455),
                Font = new Font("Arial", 10)
            };

            eelvaade = new PictureBox
            {
                Location = new Point(260, 20),
                Size = new Size(320, 300),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };

            valiNupp = new Button
            {
                Text = "Vali see pilt",
                Location = new Point(260, 335),
                Size = new Size(150, 40),
                Enabled = false
            };

            jatkaNupp = new Button
            {
                Text = "Jätka joonistamist",
                Location = new Point(260, 385),
                Size = new Size(150, 40),
                Enabled = false
            };

            kustutaNupp = new Button
            {
                Text = "Kustuta",
                Location = new Point(260, 435),
                Size = new Size(150, 40),
                Enabled = false,
                BackColor = Color.LightCoral
            };

            pildiNimekiri.SelectedIndexChanged += PildiNimekiri_SelectedIndexChanged;
            valiNupp.Click += ValiNupp_Click;
            jatkaNupp.Click += JatkaNupp_Click;
            kustutaNupp.Click += KustutaNupp_Click;

            Controls.Add(pildiNimekiri);
            Controls.Add(eelvaade);
            Controls.Add(valiNupp);
            Controls.Add(jatkaNupp);
            Controls.Add(kustutaNupp);
        }

        private void TaidaNimekiri()
        {
            pildiNimekiri.Items.Clear();
            pildiTeed.Clear();

            string kaust = PildikoguAbi.KaustaTee();
            string[] failid = Directory.GetFiles(kaust, "*.*");

            foreach (string fail in failid)
            {
                string laiend = Path.GetExtension(fail).ToLower();

                if (laiend == ".png" ||
                    laiend == ".jpg" ||
                    laiend == ".jpeg" ||
                    laiend == ".bmp" ||
                    laiend == ".gif")
                {
                    pildiTeed.Add(fail);
                }
            }

            pildiTeed.Sort();

            foreach (string tee in pildiTeed)
            {
                pildiNimekiri.Items.Add(Path.GetFileName(tee));
            }

            if (eelvaade.Image != null)
            {
                eelvaade.Image.Dispose();
                eelvaade.Image = null;
            }

            valiNupp.Enabled = false;
            jatkaNupp.Enabled = false;
            kustutaNupp.Enabled = false;
        }

        private Image LaePisipilt(string tee)
        {
            byte[] baidid = File.ReadAllBytes(tee);
            MemoryStream muistiVoog = new MemoryStream(baidid);

            return Image.FromStream(muistiVoog);
        }

        private void PildiNimekiri_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = pildiNimekiri.SelectedIndex;

            if (index < 0)
                return;

            if (eelvaade.Image != null)
            {
                eelvaade.Image.Dispose();
                eelvaade.Image = null;
            }

            eelvaade.Image = LaePisipilt(pildiTeed[index]);

            valiNupp.Enabled = true;
            jatkaNupp.Enabled = true;
            kustutaNupp.Enabled = true;
        }

        private void ValiNupp_Click(object sender, EventArgs e)
        {
            int index = pildiNimekiri.SelectedIndex;

            if (index < 0)
                return;

            SelectedImagePath = pildiTeed[index];
            DialogResult = DialogResult.OK;
            Close();
        }

        private void JatkaNupp_Click(object sender, EventArgs e)
        {
            int index = pildiNimekiri.SelectedIndex;

            if (index < 0)
                return;

            Image algnePilt = LaePisipilt(pildiTeed[index]);

            DrawingForm joonistusVorm = new DrawingForm(algnePilt);
            joonistusVorm.ShowDialog();

            TaidaNimekiri();
        }

        private void KustutaNupp_Click(object sender, EventArgs e)
        {
            int index = pildiNimekiri.SelectedIndex;

            if (index < 0)
                return;

            DialogResult vastus = MessageBox.Show(
                "Kas soovid selle pildi pildikogust kustutada?",
                "Kustutamine",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (vastus != DialogResult.Yes)
                return;

            if (eelvaade.Image != null)
            {
                eelvaade.Image.Dispose();
                eelvaade.Image = null;
            }

            File.Delete(pildiTeed[index]);

            TaidaNimekiri();
        }
    }
}