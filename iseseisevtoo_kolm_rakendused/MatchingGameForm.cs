using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public class MatchingGameForm : Form
    {
        private int raskusaste = 0;

        private int ridadeArv;
        private int veergudeArv;
        private int paarideArv;

        private Button[,] kaardid;
        private List<string> sumbolid;

        private static readonly string[] SumbolitePagas =
        {
            "🌟", "♠️", "❄️", "🧩", "♟️", "🎲", "🎱", "⚙️",
            "🍀", "🔥", "🌙", "⚡", "🎯", "🎈", "🍕", "🚀",
            "🐱", "🐶"
        };

        private Button esimeneKaart;
        private Button teineKaart;

        private int leitudPaarid;
        private int kaigudArv;

        private Label kaikudeLabel;
        private Label paaridLabel;
        private ComboBox raskusasteBox;

        private bool kontrollimineKaib;

        private const int Vahe = 8;
        private const int LauaAlgusX = 30;
        private const int LauaAlgusY = 70;

        public MatchingGameForm()
        {
            SeadistaVorm();
            SeadistaRaskusaste(raskusaste);
            AlustaUutMangu();
        }

        private void SeadistaVorm()
        {
            Text = "Mälumäng";
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = RakendusSeaded.TaustaVarv ?? Color.WhiteSmoke;
        }

        private void SeadistaRaskusaste(int tase)
        {
            raskusaste = tase;

            if (tase == 0)

            {
                ridadeArv = 4;
                veergudeArv = 4;
            }
            else if (tase == 1)

            {
                ridadeArv = 4;
                veergudeArv = 6;
            }
            else

            {
                ridadeArv = 6;
                veergudeArv = 6;
            }

            paarideArv = (ridadeArv * veergudeArv) / 2;

            MuudaVormiSuurust();
        }

        private int KaardiSuurus()
        {
            if (raskusaste == 0)
                return 120;
            else if (raskusaste == 1)
                return 95;
            else
                return 85;
        }

        private void MuudaVormiSuurust()
        {
            int kaardiSuurus = KaardiSuurus();

            int lauaLaius = veergudeArv * kaardiSuurus + (veergudeArv - 1) * Vahe;
            int lauaKorgus = ridadeArv * kaardiSuurus + (ridadeArv - 1) * Vahe;

            int vormiLaius = lauaLaius + LauaAlgusX * 2;
            int vormiKorgus = lauaKorgus + LauaAlgusY + 40;

            Size = new Size(Math.Max(vormiLaius, 500), Math.Max(vormiKorgus, 400));
        }

        private void AlustaUutMangu()
        {
            leitudPaarid = 0;
            kaigudArv = 0;
            esimeneKaart = null;
            teineKaart = null;
            kontrollimineKaib = false;

            Controls.Clear();

            LooUlemisedNupud();
            LooSumbolid();
            LooKaardid();

            UuendaSilte();
        }

        private void LooUlemisedNupud()
        {
            kaikudeLabel = new Label
            {
                Text = "Käigud: 0",
                Location = new Point(30, 20),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };

            paaridLabel = new Label
            {
                Text = "Paarid: 0/" + paarideArv,
                Location = new Point(160, 20),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };

            Label raskusasteSilt = new Label
            {
                Text = "Raskusaste:",
                Location = new Point(300, 22),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Regular)
            };

            raskusasteBox = new ComboBox
            {
                Location = new Point(390, 18),
                Size = new Size(110, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            raskusasteBox.Items.Add("Kerge");
            raskusasteBox.Items.Add("Keskmine");
            raskusasteBox.Items.Add("Raske");
            raskusasteBox.SelectedIndex = raskusaste;

            raskusasteBox.SelectedIndexChanged += RaskusasteBox_SelectedIndexChanged;

            Button taaskaivitaNupp = new Button
            {
                Text = "Algusest",
                Location = new Point(ClientSize.Width - 120, 15),
                Size = new Size(90, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            taaskaivitaNupp.Click += TaaskaivitaNupp_Click;

            Controls.Add(kaikudeLabel);
            Controls.Add(paaridLabel);
            Controls.Add(raskusasteSilt);
            Controls.Add(raskusasteBox);
            Controls.Add(taaskaivitaNupp);
        }

        private void RaskusasteBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int valitudTase = raskusasteBox.SelectedIndex;

            if (valitudTase == raskusaste)
                return;

            SeadistaRaskusaste(valitudTase);
            AlustaUutMangu();
        }

        private void LooSumbolid()
        {
            sumbolid = new List<string>();

            for (int i = 0; i < paarideArv; i++)
            {
                sumbolid.Add(SumbolitePagas[i]);
                sumbolid.Add(SumbolitePagas[i]);
            }

            SegaSumbolid();
        }

        private void SegaSumbolid()
        {
            Random juhuslik = new Random();

            for (int i = sumbolid.Count - 1; i > 0; i--)
            {
                int j = juhuslik.Next(i + 1);

                string ajutine = sumbolid[i];
                sumbolid[i] = sumbolid[j];
                sumbolid[j] = ajutine;
            }
        }

        private void LooKaardid()
        {
            kaardid = new Button[ridadeArv, veergudeArv];

            int kaardiSuurus = KaardiSuurus();
            int fondiSuurus = raskusaste == 0 ? 32 : (raskusaste == 1 ? 24 : 18);

            int sumboliIndeks = 0;

            for (int rida = 0; rida < ridadeArv; rida++)
            {
                for (int veerg = 0; veerg < veergudeArv; veerg++)
                {
                    Button kaart = new Button();

                    kaart.Size = new Size(kaardiSuurus, kaardiSuurus);

                    kaart.Location = new Point(
                        LauaAlgusX + veerg * (kaardiSuurus + Vahe),
                        LauaAlgusY + rida * (kaardiSuurus + Vahe)
                    );

                    kaart.Text = "";
                    kaart.Tag = sumbolid[sumboliIndeks];

                    kaart.Font = new Font("Arial", fondiSuurus, FontStyle.Bold);
                    kaart.BackColor = Color.SteelBlue;

                    kaart.Click += Kaart_Click;

                    kaardid[rida, veerg] = kaart;

                    Controls.Add(kaart);

                    sumboliIndeks++;
                }
            }
        }

        private void Kaart_Click(object sender, EventArgs e)
        {
            if (kontrollimineKaib)
                return;

            Button klikitudKaart = sender as Button;

            if (klikitudKaart == null)
                return;

            if (klikitudKaart.BackColor == Color.LightGreen)
                return;

            if (klikitudKaart == esimeneKaart)
                return;

            klikitudKaart.Text = klikitudKaart.Tag.ToString();
            klikitudKaart.BackColor = Color.White;

            if (esimeneKaart == null)
            {
                esimeneKaart = klikitudKaart;
                return;
            }

            teineKaart = klikitudKaart;

            kaigudArv++;
            UuendaSilte();

            KontrolliKaarte();
        }

        private void KontrolliKaarte()
        {
            kontrollimineKaib = true;

            string esimeneSumbol = esimeneKaart.Tag.ToString();
            string teineSumbol = teineKaart.Tag.ToString();

            if (esimeneSumbol == teineSumbol)
            {
                esimeneKaart.BackColor = Color.LightGreen;
                teineKaart.BackColor = Color.LightGreen;

                leitudPaarid++;
                UuendaSilte();

                esimeneKaart = null;
                teineKaart = null;
                kontrollimineKaib = false;

                if (leitudPaarid == paarideArv)
                {
                    MessageBox.Show(
                        "Palju õnne!\n\n" +
                        "Leidsid kõik " + paarideArv + " paari!\n" +
                        "Käike kokku: " + kaigudArv,
                        "Mäng läbi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            else
            {
                Timer ajastaja = new Timer();
                ajastaja.Interval = 700;

                ajastaja.Tick += (s, e) =>
                {
                    esimeneKaart.Text = "";
                    teineKaart.Text = "";

                    esimeneKaart.BackColor = Color.SteelBlue;
                    teineKaart.BackColor = Color.SteelBlue;

                    esimeneKaart = null;
                    teineKaart = null;
                    kontrollimineKaib = false;

                    ajastaja.Stop();
                    ajastaja.Dispose();
                };

                ajastaja.Start();
            }
        }

        private void UuendaSilte()
        {
            if (kaikudeLabel != null)
                kaikudeLabel.Text = "Käigud: " + kaigudArv;

            if (paaridLabel != null)
                paaridLabel.Text = "Paarid: " + leitudPaarid + "/" + paarideArv;
        }

        private void TaaskaivitaNupp_Click(object sender, EventArgs e)
        {
            AlustaUutMangu();
        }
    }
}