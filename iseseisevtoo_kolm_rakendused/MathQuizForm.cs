using System;
using System.Drawing;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public class MathQuizForm : Form
    {
        private const int RiviArv = 4;

        private char[] tehted = { '+', '-', '*', '/' };

        private int[] esimesedNumbrid = new int[RiviArv];
        private int[] teisedNumbrid = new int[RiviArv];
        private int[] oigedVastused = new int[RiviArv];

        private Label[] esimeseNumbriLabelid = new Label[RiviArv];
        private Label[] teiseNumbriLabelid = new Label[RiviArv];
        private NumericUpDown[] vastuseKastid = new NumericUpDown[RiviArv];
        private bool[] vastatud = new bool[RiviArv];

        private Label ajaLabel;
        private Button alustaNupp;

        private Random juhuslik;
        private Timer ajastaja;
        private int aegaMoodunud;

        public MathQuizForm()
        {
            juhuslik = new Random();

            SeadistaVorm();
            LooKomponendid();
            LooAjastaja();
        }

        private void SeadistaVorm()
        {
            Text = "Matemaatika viktoriin";
            Size = new Size(500, 400);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
        }

        private void LooKomponendid()
        {
            Label pealkiri = new Label
            {
                Text = "Matemaatika viktoriin",
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(100, 20)
            };

            Label ajaTekstLabel = new Label
            {
                Text = "Möödunud aeg:",
                Font = new Font("Arial", 12),
                AutoSize = true,
                Location = new Point(140, 70)
            };

            ajaLabel = new Label
            {
                Text = "0 sekundit",
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(280, 70)
            };

            Controls.Add(pealkiri);
            Controls.Add(ajaTekstLabel);
            Controls.Add(ajaLabel);

            int algusY = 120;

            for (int i = 0; i < RiviArv; i++)
            {
                int y = algusY + i * 45;

                Label esimeneLabel = new Label
                {
                    Text = "0",
                    Font = new Font("Arial", 16),
                    AutoSize = true,
                    Location = new Point(80, y)
                };

                Label tehteLabel = new Label
                {
                    Text = tehted[i].ToString(),
                    Font = new Font("Arial", 16, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(130, y)
                };

                Label teineLabel = new Label
                {
                    Text = "0",
                    Font = new Font("Arial", 16),
                    AutoSize = true,
                    Location = new Point(170, y)
                };

                Label vordubLabel = new Label
                {
                    Text = "=",
                    Font = new Font("Arial", 16, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(220, y)
                };

                NumericUpDown vastuseKast = new NumericUpDown
                {
                    Location = new Point(260, y),
                    Size = new Size(100, 30),
                    Font = new Font("Arial", 14),
                    Minimum = -1000,
                    Maximum = 1000,
                    Enabled = false,
                    Tag = i
                };

                vastuseKast.KeyDown += VastuseKast_KeyDown;

                esimeseNumbriLabelid[i] = esimeneLabel;
                teiseNumbriLabelid[i] = teineLabel;
                vastuseKastid[i] = vastuseKast;

                Controls.Add(esimeneLabel);
                Controls.Add(tehteLabel);
                Controls.Add(teineLabel);
                Controls.Add(vordubLabel);
                Controls.Add(vastuseKast);
            }

            alustaNupp = new Button
            {
                Text = "Alusta viktoriini",
                Font = new Font("Arial", 12),
                Location = new Point(150, algusY + RiviArv * 45 + 20),
                Size = new Size(180, 40)
            };

            alustaNupp.Click += AlustaNupp_Click;

            Controls.Add(alustaNupp);
        }

        private void LooAjastaja()
        {
            ajastaja = new Timer();
            ajastaja.Interval = 1000;
            ajastaja.Tick += Ajastaja_Tick;
        }

        private void AlustaNupp_Click(object sender, EventArgs e)
        {
            LooKusimused();

            aegaMoodunud = 0;
            ajaLabel.Text = "0 sekundit";

            for (int i = 0; i < RiviArv; i++)
            {
                vastatud[i] = false;
            }

            LubaVastuseKastid(true);
            alustaNupp.Enabled = false;

            ajastaja.Start();
        }

        private void Ajastaja_Tick(object sender, EventArgs e)
        {
            aegaMoodunud++;
            ajaLabel.Text = aegaMoodunud + " sekundit";
        }

        private void LooKusimused()
        {
            for (int i = 0; i < RiviArv; i++)
            {
                int esimene = 0;
                int teine = 0;
                int vastus = 0;

                if (tehted[i] == '+')
                {
                    esimene = juhuslik.Next(1, 51);
                    teine = juhuslik.Next(1, 51);
                    vastus = esimene + teine;
                }
                else if (tehted[i] == '-')
                {
                    esimene = juhuslik.Next(20, 100);
                    teine = juhuslik.Next(1, esimene);
                    vastus = esimene - teine;
                }
                else if (tehted[i] == '*')
                {
                    esimene = juhuslik.Next(1, 13);
                    teine = juhuslik.Next(1, 13);
                    vastus = esimene * teine;
                }
                else
                {
                    teine = juhuslik.Next(1, 13);
                    vastus = juhuslik.Next(1, 13);
                    esimene = teine * vastus;
                }

                esimesedNumbrid[i] = esimene;
                teisedNumbrid[i] = teine;
                oigedVastused[i] = vastus;

                esimeseNumbriLabelid[i].Text = esimene.ToString();
                teiseNumbriLabelid[i].Text = teine.ToString();
                vastuseKastid[i].Value = 0;
            }
        }

        private void LubaVastuseKastid(bool luba)
        {
            for (int i = 0; i < RiviArv; i++)
            {
                vastuseKastid[i].Enabled = luba;
            }
        }

        private void VastuseKast_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            NumericUpDown kast = (NumericUpDown)sender;
            int index = (int)kast.Tag;

            vastatud[index] = true;

            SelectNextControl(kast, true, true, true, true);

            if (KoikVastatud())
            {
                ajastaja.Stop();
                LopetaViktoriin();
            }
        }

        private bool KoikVastatud()
        {
            for (int i = 0; i < RiviArv; i++)
            {
                if (!vastatud[i])
                    return false;
            }

            return true;
        }

        private void LopetaViktoriin()
        {
            LubaVastuseKastid(false);

            int oigeidVastuseid = 0;

            for (int i = 0; i < RiviArv; i++)
            {
                int antudVastus = (int)vastuseKastid[i].Value;

                if (antudVastus == oigedVastused[i])
                {
                    oigeidVastuseid++;
                }
            }

            alustaNupp.Enabled = true;
            alustaNupp.Text = "Proovi uuesti";

            MessageBox.Show(
                "Kõik ülesanded lahendatud!\n\n" +
                "Õigeid vastuseid: " + oigeidVastuseid + "/" + RiviArv + "\n" +
                "Aeg kulus: " + aegaMoodunud + " sekundit",
                "Tulemus",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
