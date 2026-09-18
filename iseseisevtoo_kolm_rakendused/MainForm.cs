using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace iseseisevtoo_kolm_rakendused
{
    public class MainForm : Form
    {
        private Button pictureViewerButton;
        private Button mathQuizButton;
        private Button matchingGameButton;
        private Label silt;
        private PictureBox pilt;

        public MainForm()
        {
            InitializeForm();
            CreateControls();
        }

        private void InitializeForm()
        {
            Text = "Kolm rakendust";
            Size = new Size(550, 450);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
        }

        private void CreateControls()
        {
            PictureBox pilt = new PictureBox();

            pilt.Dock = DockStyle.Fill;
            pilt.Image = Image.FromFile(@"..\..\Pildid\images.jpg");
            pilt.SizeMode = PictureBoxSizeMode.StretchImage;

            silt = new Label
            {
                Text = "Kolm rakendust",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.LightBlue,
                AutoSize = true,
                Location = new Point(125, 40)
            };

            pictureViewerButton = new Button
            {
                Text = "Pildi vaatamine",
                Size = new Size(250, 55),
                Location = new Point(120, 110),
                Font = new Font("Arial", 12)
            };

            mathQuizButton = new Button
            {
                Text = "Äraarvamismäng",
                Size = new Size(250, 55),
                Location = new Point(120, 180),
                Font = new Font("Arial", 12)
            };

            matchingGameButton = new Button
            {
                Text = "Leidmise mäng",
                Size = new Size(250, 55),
                Location = new Point(120, 250),
                Font = new Font("Arial", 12)
            };

            pictureViewerButton.Click += OpenPictureViewer;
            mathQuizButton.Click += OpenMathQuiz;
            matchingGameButton.Click += OpenMatchingGame;
            Controls.Add(pilt);
            Controls.Add(silt);
            Controls.Add(pictureViewerButton);
            Controls.Add(mathQuizButton);
            Controls.Add(matchingGameButton);
            silt.BringToFront();
            pilt.SendToBack();
        }

        private void OpenPictureViewer(object sender, EventArgs e)
        {
            PictureViewerForm form = new PictureViewerForm();
            form.Show();
        }

        private void OpenMathQuiz(object sender, EventArgs e)
        {
            MathQuizForm form = new MathQuizForm();
            form.Show();
        }

        private void OpenMatchingGame(object sender, EventArgs e)
        {
            MatchingGameForm form = new MatchingGameForm();
            form.Show();
        }
    }
}