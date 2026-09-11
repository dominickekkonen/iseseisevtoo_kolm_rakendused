using System;
using System.Drawing;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public class MathQuizForm : Form
    {
        private Label questionLabel;
        private Label scoreLabel;
        private Label questionNumberLabel;

        private NumericUpDown answerBox;
        private Button checkButton;
        private Button restartButton;

        private ComboBox difficultyBox;

        private Random random;

        private int firstNumber;
        private int secondNumber;
        private int correctAnswer;

        private int score;
        private int questionNumber;

        private const int TotalQuestions = 10;

        public MathQuizForm()
        {
            random = new Random();

            InitializeForm();
            CreateControls();
            StartGame();
        }

        private void InitializeForm()
        {
            Text = "Mathematical Quiz";
            Size = new Size(600, 450);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
        }

        private void CreateControls()
        {
            Label titleLabel = new Label
            {
                Text = "Mathematical Quiz",
                Font = new Font("Arial", 24, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(165, 30)
            };

            difficultyBox = new ComboBox
            {
                Location = new Point(210, 90),
                Size = new Size(170, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            difficultyBox.Items.Add("Easy");
            difficultyBox.Items.Add("Medium");
            difficultyBox.Items.Add("Hard");

            difficultyBox.SelectedIndex = 0;

            difficultyBox.SelectedIndexChanged +=
                DifficultyChanged;

            questionLabel = new Label
            {
                Text = "Question",
                Font = new Font("Arial", 26, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(220, 150)
            };

            answerBox = new NumericUpDown
            {
                Location = new Point(220, 220),
                Size = new Size(160, 40),
                Font = new Font("Arial", 18),
                Minimum = -100000,
                Maximum = 100000
            };

            checkButton = new Button
            {
                Text = "Check Answer",
                Location = new Point(190, 280),
                Size = new Size(220, 45),
                Font = new Font("Arial", 12)
            };

            restartButton = new Button
            {
                Text = "Restart",
                Location = new Point(190, 335),
                Size = new Size(220, 40)
            };

            scoreLabel = new Label
            {
                Text = "Score: 0",
                Location = new Point(30, 30),
                AutoSize = true,
                Font = new Font("Arial", 12)
            };

            questionNumberLabel = new Label
            {
                Text = "Question: 1/10",
                Location = new Point(30, 60),
                AutoSize = true,
                Font = new Font("Arial", 12)
            };

            checkButton.Click += CheckAnswer;
            restartButton.Click += RestartGame;

            Controls.Add(titleLabel);
            Controls.Add(difficultyBox);
            Controls.Add(questionLabel);
            Controls.Add(answerBox);
            Controls.Add(checkButton);
            Controls.Add(restartButton);
            Controls.Add(scoreLabel);
            Controls.Add(questionNumberLabel);
        }

        private void StartGame()
        {
            score = 0;
            questionNumber = 0;

            checkButton.Enabled = true;
            answerBox.Enabled = true;

            UpdateScore();
            NextQuestion();
        }

        private void NextQuestion()
        {
            if (questionNumber >= TotalQuestions)
            {
                FinishGame();
                return;
            }

            questionNumber++;

            int maxNumber = GetMaximumNumber();

            firstNumber = random.Next(1, maxNumber + 1);
            secondNumber = random.Next(1, maxNumber + 1);

            correctAnswer = firstNumber + secondNumber;

            questionLabel.Text =
                firstNumber + " + " + secondNumber + " = ?";

            answerBox.Value = 0;

            questionNumberLabel.Text =
                "Question: " +
                questionNumber +
                "/" +
                TotalQuestions;
        }

        private int GetMaximumNumber()
        {
            switch (difficultyBox.SelectedIndex)
            {
                case 0:
                    return 10;

                case 1:
                    return 50;

                case 2:
                    return 100;

                default:
                    return 10;
            }
        }

        private void CheckAnswer(object sender, EventArgs e)
        {
            int userAnswer = (int)answerBox.Value;

            if (userAnswer == correctAnswer)
            {
                score++;

                MessageBox.Show(
                    "Correct!",
                    "Result",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Wrong! Correct answer: " +
                    correctAnswer,
                    "Result",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            UpdateScore();
            NextQuestion();
        }

        private void UpdateScore()
        {
            scoreLabel.Text = "Score: " + score;
        }

        private void FinishGame()
        {
            checkButton.Enabled = false;
            answerBox.Enabled = false;

            MessageBox.Show(
                "Quiz finished!\n\n" +
                "Your score: " +
                score +
                "/" +
                TotalQuestions,
                "Game Over",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void RestartGame(object sender, EventArgs e)
        {
            StartGame();
        }

        private void DifficultyChanged(object sender, EventArgs e)
        {
            StartGame();
        }
    }
}
