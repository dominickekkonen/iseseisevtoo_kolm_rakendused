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

        private List<string> pildiTeed;

        public string SelectedImagePath { get; private set; }

        public PictureLibraryForm(List<string> salvestatudPildid)
        {
            pildiTeed = salvestatudPildid;

            InitializeForm();
            CreateControls();
            TaidaNimekiri();
        }

        private void InitializeForm()
        {
            Text = "Pildikogu";
            Size = new Size(620, 450);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.LightGray;
        }

        private void CreateControls()
        {
            pildiNimekiri = new ListBox
            {
                Location = new Point(20, 20),
                Size = new Size(220, 330),
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
                Location = new Point(260, 340),
                Size = new Size(150, 40),
                Enabled = false
            };

            pildiNimekiri.SelectedIndexChanged += PildiNimekiri_SelectedIndexChanged;
            valiNupp.Click += ValiNupp_Click;

            Controls.Add(pildiNimekiri);
            Controls.Add(eelvaade);
            Controls.Add(valiNupp);
        }

        private void TaidaNimekiri()
        {
            foreach (string tee in pildiTeed)
            {
                pildiNimekiri.Items.Add(Path.GetFileName(tee));
            }
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

            FileStream stream = new FileStream(
                pildiTeed[index],
                FileMode.Open,
                FileAccess.Read);

            eelvaade.Image = Image.FromStream(stream);

            valiNupp.Enabled = true;
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
    }
}