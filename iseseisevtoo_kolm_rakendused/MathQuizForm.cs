using System;
using System.Drawing;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public class MathQuizForm : Form
    {
        private const int RiviArv = 4;
        private const int AegSekundites = 30;

        private char[] tehted = { '+', '-', '*', '/' };

        private int[] esimesedNumbrid = new int[RiviArv];
        private int[] teisedNumbrid = new int[RiviArv];
        private int[] oigedVastused = new int[RiviArv];

        private Label[] esimeseNumbriLabelid = new Label[RiviArv];
        private Label[] teiseNumbriLabelid = new Label[RiviArv];
        private NumericUpDown[] vastuseKastid = new NumericUpDown[RiviArv];

        private Label ajaLabel;
        private Button alustaNupp;
        private Button lopetaNupp;

        private Random juhuslik;
        private Timer ajastaja;
        private int aegaJarel;

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
            BackColor = RakendusSeaded.TaustaVarv ?? Color.WhiteSmoke;
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
                Text = "Aega jäänud:",
                Font = new Font("Arial", 12),
                AutoSize = true,
                Location = new Point(150, 70)
            };

            ajaLabel = new Label
            {
                Text = AegSekundites + " sekundit",
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(270, 70)
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
                    Enabled = false
                };

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
                Location = new Point(110, algusY + RiviArv * 45 + 20),
                Size = new Size(140, 40)
            };

            lopetaNupp = new Button
            {
                Text = "Lõpeta",
                Font = new Font("Arial", 12),
                Location = new Point(260, algusY + RiviArv * 45 + 20),
                Size = new Size(140, 40),
                Enabled = false
            };

            alustaNupp.Click += AlustaNupp_Click;
            lopetaNupp.Click += LopetaNupp_Click;

            Controls.Add(alustaNupp);
            Controls.Add(lopetaNupp);
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

            aegaJarel = AegSekundites;
            ajaLabel.Text = aegaJarel + " sekundit";

            LubaVastuseKastid(true);
            LahtestaVarvid();

            alustaNupp.Enabled = false;
            lopetaNupp.Enabled = true;

            ajastaja.Start();
        }

        private void Ajastaja_Tick(object sender, EventArgs e)
        {
            aegaJarel--;
            ajaLabel.Text = aegaJarel + " sekundit";

            if (aegaJarel <= 0)
            {
                ajastaja.Stop();
                LopetaViktoriin();
            }
        }

        private void LopetaNupp_Click(object sender, EventArgs e)
        {
            ajastaja.Stop();
            LopetaViktoriin();
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

        private void LahtestaVarvid()
        {
            for (int i = 0; i < RiviArv; i++)
            {
                vastuseKastid[i].BackColor = Color.White;
            }
        }

        private void LopetaViktoriin()
        {
            LubaVastuseKastid(false);

            alustaNupp.Enabled = true;
            alustaNupp.Text = "Proovi uuesti";
            lopetaNupp.Enabled = false;

            int oigeidVastuseid = 0;

            for (int i = 0; i < RiviArv; i++)
            {
                int antudVastus = (int)vastuseKastid[i].Value;

                if (antudVastus == oigedVastused[i])
                {
                    vastuseKastid[i].BackColor = Color.LightGreen;
                    oigeidVastuseid++;
                }
                else
                {
                    vastuseKastid[i].BackColor = Color.LightCoral;
                }
            }

            MessageBox.Show(
                "Viktoriin lõpetatud!\n\n" +
                "Õigeid vastuseid: " + oigeidVastuseid + "/" + RiviArv,
                "Tulemus",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}