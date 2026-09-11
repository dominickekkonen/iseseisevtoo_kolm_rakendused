using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public class MatchingGameForm : Form
    {
        private const int Rows = 4;
        private const int Columns = 4;

        private Button[,] cards;
        private List<string> symbols;

        private Button firstCard;
        private Button secondCard;

        private int pairsFound;
        private int moves;

        private Label movesLabel;
        private Label pairsLabel;

        private bool checkingCards;

        public MatchingGameForm()
        {
            InitializeForm();
            StartNewGame();
        }

        private void InitializeForm()
        {
            Text = "Matching Game";
            Size = new Size(650, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
        }

        private void StartNewGame()
        {
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
                Text = "Pairs: 0/8",
                Location = new Point(150, 20),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };

            Button restartButton = new Button
            {
                Text = "Restart",
                Location = new Point(480, 10),
                Size = new Size(100, 35)
            };

            restartButton.Click += RestartButton_Click;

            Controls.Add(movesLabel);
            Controls.Add(pairsLabel);
            Controls.Add(restartButton);
        }

        private void CreateSymbols()
        {
            symbols = new List<string>
            {
                "★", "★",
                "♥", "♥",
                "●", "●",
                "■", "■",
                "▲", "▲",
                "♦", "♦",
                "♣", "♣",
                "☀", "☀"
            };

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
            cards = new Button[Rows, Columns];

            int startX = 50;
            int startY = 70;
            int cardSize = 120;
            int gap = 10;

            int symbolIndex = 0;

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    Button card = new Button();

                    card.Size = new Size(cardSize, cardSize);

                    card.Location = new Point(
                        startX + column * (cardSize + gap),
                        startY + row * (cardSize + gap)
                    );

                    card.Text = "";
                    card.Tag = symbols[symbolIndex];

                    card.Font = new Font(
                        "Arial",
                        32,
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

            // Don't allow clicking an already matched card
            if (clickedCard.BackColor == Color.LightGreen)
                return;

            // Don't allow clicking the same card twice
            if (clickedCard == firstCard)
                return;

            clickedCard.Text = clickedCard.Tag.ToString();
            clickedCard.BackColor = Color.White;

            // First card
            if (firstCard == null)
            {
                firstCard = clickedCard;
                return;
            }

            // Second card
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
                // Match!
                firstCard.BackColor = Color.LightGreen;
                secondCard.BackColor = Color.LightGreen;

                pairsFound++;

                UpdateLabels();

                firstCard = null;
                secondCard = null;

                checkingCards = false;

                if (pairsFound == 8)
                {
                    MessageBox.Show(
                        "Congratulations!\n\n" +
                        "You found all 8 pairs!\n" +
                        "Moves: " + moves,
                        "Game Finished",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            else
            {
                // Not a match - hide cards after 700 ms
                Timer timer = new Timer();

                timer.Interval = 700;

                timer.Tick += (s, e) =>
                {
                    firstCard.Text = "";
                    secondCard.Text = "";

                    firstCard.BackColor = Color.SteelBlue;
                    secondCard.BackColor = Color.SteelBlue;

                    firstCard = null;
                    secondCard = null;

                    checkingCards = false;

                    timer.Stop();
                    timer.Dispose();
                };

                timer.Start();
            }
        }

        private void UpdateLabels()
        {
            if (movesLabel != null)
                movesLabel.Text = "Moves: " + moves;

            if (pairsLabel != null)
                pairsLabel.Text = "Pairs: " + pairsFound + "/8";
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }
    }
}
