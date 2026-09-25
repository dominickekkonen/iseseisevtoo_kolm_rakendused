using System.IO;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public static class PildikoguAbi
    {
        public static string KaustaTee()
        {
            string tee = Path.Combine(Application.StartupPath, "Pildikogu");

            if (!Directory.Exists(tee))
            {
                Directory.CreateDirectory(tee);
            }

            return tee;
        }

        public static string PuhastaFailiNimi(string nimi)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                nimi = nimi.Replace(c.ToString(), "");
            }

            return nimi;
        }

        public static string LeiaVabaTee(string kaust, string sooviFailiNimi)
        {
            string laiend = Path.GetExtension(sooviFailiNimi);
            string nimiIlmaLaiendita = Path.GetFileNameWithoutExtension(sooviFailiNimi);

            string tee = Path.Combine(kaust, sooviFailiNimi);
            int loendur = 1;

            while (File.Exists(tee))
            {
                string uusNimi = nimiIlmaLaiendita + "_" + loendur + laiend;
                tee = Path.Combine(kaust, uusNimi);
                loendur++;
            }

            return tee;
        }
    }
}