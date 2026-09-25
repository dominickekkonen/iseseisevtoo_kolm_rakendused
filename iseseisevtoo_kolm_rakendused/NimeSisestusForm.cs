using System;
using System.Drawing;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public class NimeSisestusForm : Form
    {
        private Label sonumLabel;
        private TextBox nimeKast;
        private Button okNupp;
        private Button katkestaNupp;

        public string SisestatudNimi { get; private set; }

        public NimeSisestusForm(string vaikimisiNimi)
        {
            InitializeForm();
            CreateControls(vaikimisiNimi);
        }

        private void InitializeForm()
        {
            Text = "Pildi nimi";
            Size = new Size(400, 180);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = RakendusSeaded.TaustaVarv ?? Color.WhiteSmoke;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private void CreateControls(string vaikimisiNimi)
        {
            sonumLabel = new Label
            {
                Text = "Sisesta pildi nimi:",
                Font = new Font("Arial", 11),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            nimeKast = new TextBox
            {
                Text = vaikimisiNimi,
                Font = new Font("Arial", 11),
                Location = new Point(20, 50),
                Size = new Size(340, 30)
            };

            okNupp = new Button
            {
                Text = "Salvesta",
                Location = new Point(120, 95),
                Size = new Size(110, 35),
                DialogResult = DialogResult.OK
            };

            katkestaNupp = new Button
            {
                Text = "Katkesta",
                Location = new Point(240, 95),
                Size = new Size(110, 35),
                DialogResult = DialogResult.Cancel
            };

            okNupp.Click += OkNupp_Click;

            Controls.Add(sonumLabel);
            Controls.Add(nimeKast);
            Controls.Add(okNupp);
            Controls.Add(katkestaNupp);

            AcceptButton = okNupp;
            CancelButton = katkestaNupp;
        }

        private void OkNupp_Click(object sender, EventArgs e)
        {
            SisestatudNimi = nimeKast.Text.Trim();
        }
    }
}