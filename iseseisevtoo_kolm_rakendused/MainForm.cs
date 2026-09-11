using System;
using System.Drawing;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public class MainForm : Form
    {
        private Button pictureViewerButton;
        private Button mathQuizButton;
        private Button matchingGameButton;
        private Label titleLabel;

        public MainForm()
        {
            InitializeForm();
            CreateControls();
        }

        private void InitializeForm()
        {
            Text = "Three Applications";
            Size = new Size(500, 400);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
        }

        private void CreateControls()
        {
            titleLabel = new Label
            {
                Text = "Three Applications",
                Font = new Font("Arial", 24, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(125, 40)
            };

            pictureViewerButton = new Button
            {
                Text = "Picture Viewer",
                Size = new Size(250, 55),
                Location = new Point(120, 110),
                Font = new Font("Arial", 12)
            };

            mathQuizButton = new Button
            {
                Text = "Math Quiz",
                Size = new Size(250, 55),
                Location = new Point(120, 180),
                Font = new Font("Arial", 12)
            };

            matchingGameButton = new Button
            {
                Text = "Matching Game",
                Size = new Size(250, 55),
                Location = new Point(120, 250),
                Font = new Font("Arial", 12)
            };

            pictureViewerButton.Click += OpenPictureViewer;
            mathQuizButton.Click += OpenMathQuiz;
            matchingGameButton.Click += OpenMatchingGame;

            Controls.Add(titleLabel);
            Controls.Add(pictureViewerButton);
            Controls.Add(mathQuizButton);
            Controls.Add(matchingGameButton);
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