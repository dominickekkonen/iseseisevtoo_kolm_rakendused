using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public class MatchingGameForm : Form
    {
        private enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }

        private Difficulty currentDifficulty = Difficulty.Easy;

        private int rows;
        private int columns;
        private int totalPairs;
        private Button[,] cards;
        private List<string> symbols;
        private static readonly string[] SymbolPool =
        {
            "🌟", "♠️", "❄️", "🧩", "♟️", "🎲", "🎱", "⚙️",
            "🍀", "🔥", "🌙", "⚡", "🎯", "🎈", "🍕", "🚀",
            "🐱", "🐶"
        };
        private Button firstCard;
        private Button secondCard;
        private int pairsFound;
        private int moves;
        private Label difficultyLabel;
        private Label movesLabel;
        private Label pairsLabel;
        private ComboBox difficultyComboBox;
        private bool checkingCards;
        private const int CardGap = 8;
        private const int BoardStartY = 70;
        private const int BoardStartX = 30;

        private Timer mismatchTimer;

        public MatchingGameForm()
        {
            InitializeForm();
            ApplyDifficulty(currentDifficulty);
            StartNewGame();
        }

        private void InitializeForm()
        {
            Text = "Leidmise mäng";
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
        }

        private void ApplyDifficulty(Difficulty difficulty)
        {
            currentDifficulty = difficulty;

            switch (difficulty)
            {
                case Difficulty.Easy:
                    rows = 4;
                    columns = 4;
                    break;
                case Difficulty.Medium:
                    rows = 4;
                    columns = 6;
                    break;
                case Difficulty.Hard:
                    rows = 6;
                    columns = 6;
                    break;
            }

            totalPairs = (rows * columns) / 2;

            ResizeFormToBoard();
        }

        private int GetCardSize()
        {
            if (columns >= 6 && rows >= 6)
                return 85;
            if (columns >= 6)
                return 95;
            return 120;
        }

        private void ResizeFormToBoard()
        {
            int cardSize = GetCardSize();

            int boardWidth = columns * cardSize + (columns - 1) * CardGap;
            int boardHeight = rows * cardSize + (rows - 1) * CardGap;

            int formWidth = boardWidth + BoardStartX * 2 + 16;
            int formHeight = boardHeight + BoardStartY + 40;

            Size = new Size(Math.Max(formWidth, 500), Math.Max(formHeight, 400));
        }

        private void StartNewGame()
        {
            StopMismatchTimer();

            pairsFound = 0;
            moves = 0;
            firstCard = null;
            secondCard = null;
            checkingCards = false;

            Controls.Clear();

            CreateTopControls();
            CreateSymbols();
            CreateCards();

            UpdateLabels();
        }

        private void StopMismatchTimer()
        {
            if (mismatchTimer != null)
            {
                mismatchTimer.Stop();
                mismatchTimer.Tick -= MismatchTimer_Tick;
                mismatchTimer.Dispose();
                mismatchTimer = null;
            }
        }

        private void CreateTopControls()
        {
            movesLabel = new Label
            {
                Text = "Moves: 0",
                Location = new Point(30, 20),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };

            pairsLabel = new Label
            {
                Text = "Pairs: 0/" + totalPairs,
                Location = new Point(160, 20),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };

            difficultyLabel = new Label
            {
                Text = "Difficulty:",
                Location = new Point(285, 20),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Regular)
            };

            difficultyComboBox = new ComboBox
            {
                Location = new Point(350, 18),
                Size = new Size(100, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            difficultyComboBox.Items.AddRange(new object[] { "Easy", "Medium", "Hard" });
            difficultyComboBox.SelectedIndex = (int)currentDifficulty;
            difficultyComboBox.SelectedIndexChanged += DifficultyComboBox_SelectedIndexChanged;

            Button restartButton = new Button
            {
                Text = "Restart",
                Location = new Point(ClientSize.Width - 100, 15),
                Size = new Size(90, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            restartButton.Click += RestartButton_Click;

            Controls.Add(movesLabel);
            Controls.Add(pairsLabel);
            Controls.Add(difficultyLabel);
            Controls.Add(difficultyComboBox);
            Controls.Add(restartButton);
        }

        private void DifficultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Difficulty selected = (Difficulty)difficultyComboBox.SelectedIndex;

            if (selected == currentDifficulty)
                return;

            ApplyDifficulty(selected);
            StartNewGame();
        }

        private void CreateSymbols()
        {
            if (totalPairs > SymbolPool.Length)
                throw new InvalidOperationException("Not enough symbols in the pool for this difficulty.");

            symbols = new List<string>();

            for (int i = 0; i < totalPairs; i++)
            {
                symbols.Add(SymbolPool[i]);
                symbols.Add(SymbolPool[i]);
            }

            ShuffleSymbols();
        }

        private void ShuffleSymbols()
        {
            Random random = new Random();

            for (int i = symbols.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);

                string temp = symbols[i];
                symbols[i] = symbols[j];
                symbols[j] = temp;
            }
        }

        private void CreateCards()
        {
            cards = new Button[rows, columns];

            int cardSize = GetCardSize();
            int fontSize = cardSize >= 110 ? 32 : (cardSize >= 90 ? 24 : 18);

            int symbolIndex = 0;

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    Button card = new Button();

                    card.Size = new Size(cardSize, cardSize);

                    card.Location = new Point(
                        BoardStartX + column * (cardSize + CardGap),
                        BoardStartY + row * (cardSize + CardGap)
                    );

                    card.Text = "";
                    card.Tag = symbols[symbolIndex];

                    card.Font = new Font(
                        "Arial",
                        fontSize,
                        FontStyle.Bold
                    );

                    card.BackColor = Color.SteelBlue;

                    card.Click += Card_Click;

                    cards[row, column] = card;

                    Controls.Add(card);

                    symbolIndex++;
                }
            }
        }

        private void Card_Click(object sender, EventArgs e)
        {
            if (checkingCards)
                return;

            Button clickedCard = sender as Button;

            if (clickedCard == null)
                return;


            if (clickedCard.BackColor == Color.LightGreen)
                return;


            if (clickedCard == firstCard)
                return;

            clickedCard.Text = clickedCard.Tag.ToString();
            clickedCard.BackColor = Color.White;
            if (firstCard == null)
            {
                firstCard = clickedCard;
                return;
            }
            secondCard = clickedCard;

            moves++;
            UpdateLabels();

            CheckCards();
        }

        private void CheckCards()
        {
            checkingCards = true;

            string firstSymbol = firstCard.Tag.ToString();
            string secondSymbol = secondCard.Tag.ToString();

            if (firstSymbol == secondSymbol)
            {
                firstCard.BackColor = Color.LightGreen;
                secondCard.BackColor = Color.LightGreen;

                pairsFound++;

                UpdateLabels();

                firstCard = null;
                secondCard = null;

                checkingCards = false;

                if (pairsFound == totalPairs)
                {
                    MessageBox.Show(
                        "Congratulations!\n\n" +
                        "You found all " + totalPairs + " pairs!\n" +
                        "Moves: " + moves,
                        "Game Finished",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            else
            {
                StopMismatchTimer();

                mismatchTimer = new Timer();
                mismatchTimer.Interval = 700;
                mismatchTimer.Tick += MismatchTimer_Tick;
                mismatchTimer.Start();
            }
        }

        private void MismatchTimer_Tick(object sender, EventArgs e)
        {
            StopMismatchTimer();

            if (firstCard != null)
            {
                firstCard.Text = "";
                firstCard.BackColor = Color.SteelBlue;
            }

            if (secondCard != null)
            {
                secondCard.Text = "";
                secondCard.BackColor = Color.SteelBlue;
            }

            firstCard = null;
            secondCard = null;

            checkingCards = false;
        }

        private void UpdateLabels()
        {
            if (movesLabel != null)
                movesLabel.Text = "Moves: " + moves;

            if (pairsLabel != null)
                pairsLabel.Text = "Pairs: " + pairsFound + "/" + totalPairs;
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            StopMismatchTimer();
            base.OnFormClosed(e);
        }
    }
}