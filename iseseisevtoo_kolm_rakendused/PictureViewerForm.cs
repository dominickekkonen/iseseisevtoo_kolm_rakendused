using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace iseseisevtoo_kolm_rakendused
{
    public class PictureViewerForm : Form
    {
        private PictureBox pilt;
        private Button openButton;
        private Button previousButton;
        private Button nextButton;
        private Button colorButton;
        private Button saveButton;
        private Button libraryButton;
        private Button closeButton;
        private Button drawButton;
        private Label fileLabel;

        private List<string> imageFiles;
        private int currentImageIndex = -1;

        public PictureViewerForm()
        {
            imageFiles = new List<string>();

            InitializeForm();
            CreateControls();
        }

        private void InitializeForm()
        {
            Text = "Pildi vaatamine";
            Size = new Size(900, 760);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = RakendusSeaded.TaustaVarv ?? Color.LightGray;
        }

        private void CreateControls()
        {
            pilt = new PictureBox
            {
                Location = new Point(50, 70),
                Size = new Size(780, 480),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };

            openButton = new Button
            {
                Text = "Näita pilt",
                Location = new Point(50, 580),
                Size = new Size(130, 40)
            };

            previousButton = new Button
            {
                Text = "⇐",
                Location = new Point(200, 580),
                Size = new Size(120, 40)
            };

            nextButton = new Button
            {
                Text = "⇒",
                Location = new Point(340, 580),
                Size = new Size(120, 40)
            };

            colorButton = new Button
            {
                Text = "Tagaplaan",
                Location = new Point(480, 580),
                Size = new Size(120, 40)
            };

            saveButton = new Button
            {
                Text = "Salvesta pilt",
                Location = new Point(620, 580),
                Size = new Size(120, 40)
            };

            libraryButton = new Button
            {
                Text = "Pildikogu",
                Location = new Point(50, 640),
                Size = new Size(150, 40)
            };

            closeButton = new Button
            {
                Text = "Sulge",
                Location = new Point(220, 640),
                Size = new Size(150, 40)
            };

            drawButton = new Button
            {
                Text = "Joonista",
                Location = new Point(390, 640),
                Size = new Size(150, 40)
            };

            fileLabel = new Label
            {
                Text = "Pilt pole valitud",
                Location = new Point(50, 25),
                AutoSize = true,
                Font = new Font("Arial", 11)
            };

            openButton.Click += OpenImage;
            previousButton.Click += ShowPreviousImage;
            nextButton.Click += ShowNextImage;
            colorButton.Click += ChangeBackgroundColor;
            saveButton.Click += SaveImage;
            libraryButton.Click += OpenLibrary;
            closeButton.Click += CloseForm;
            drawButton.Click += OpenDrawing;

            Controls.Add(pilt);
            Controls.Add(openButton);
            Controls.Add(previousButton);
            Controls.Add(nextButton);
            Controls.Add(colorButton);
            Controls.Add(saveButton);
            Controls.Add(libraryButton);
            Controls.Add(closeButton);
            Controls.Add(drawButton);
            Controls.Add(fileLabel);
        }

        private void OpenImage(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Vali pilt";
            dialog.Filter =
                "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string directory = Path.GetDirectoryName(dialog.FileName);

                if (directory != null)
                {
                    imageFiles.Clear();

                    string[] files = Directory.GetFiles(
                        directory,
                        "*.*"
                    );

                    foreach (string file in files)
                    {
                        string extension =
                            Path.GetExtension(file).ToLower();

                        if (extension == ".jpg" ||
                            extension == ".jpeg" ||
                            extension == ".png" ||
                            extension == ".bmp" ||
                            extension == ".gif")
                        {
                            imageFiles.Add(file);
                        }
                    }

                    imageFiles.Sort();

                    currentImageIndex =
                        imageFiles.IndexOf(dialog.FileName);

                    DisplayCurrentImage();
                }
            }
        }

        private void DisplayCurrentImage()
        {
            if (currentImageIndex < 0 ||
                currentImageIndex >= imageFiles.Count)
            {
                return;
            }

            LoadImage(imageFiles[currentImageIndex]);
        }

        private void LoadImage(string path)
        {
            if (pilt.Image != null)
            {
                pilt.Image.Dispose();
                pilt.Image = null;
            }

            FileStream stream = new FileStream(
                    path,
                    FileMode.Open,
                    FileAccess.Read);

            pilt.Image = Image.FromStream(stream);

            fileLabel.Text = Path.GetFileName(path);
        }

        private void ShowPreviousImage(object sender, EventArgs e)
        {
            if (imageFiles.Count == 0)
                return;

            currentImageIndex--;

            if (currentImageIndex < 0)
                currentImageIndex = imageFiles.Count - 1;

            DisplayCurrentImage();
        }

        private void ShowNextImage(object sender, EventArgs e)
        {
            if (imageFiles.Count == 0)
                return;

            currentImageIndex++;

            if (currentImageIndex >= imageFiles.Count)
                currentImageIndex = 0;

            DisplayCurrentImage();
        }

        private void ChangeBackgroundColor(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pilt.BackColor = dialog.Color;
            }
        }

        private void SaveImage(object sender, EventArgs e)
        {
            if (pilt.Image == null)
            {
                MessageBox.Show(
                    "Pildi ei ole",
                    "Info",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            string vaikimisiNimi = "pilt_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

            NimeSisestusForm nimeVorm = new NimeSisestusForm(vaikimisiNimi);

            if (nimeVorm.ShowDialog() != DialogResult.OK)
                return;

            string sisestatudNimi = nimeVorm.SisestatudNimi;

            if (string.IsNullOrWhiteSpace(sisestatudNimi))
            {
                sisestatudNimi = vaikimisiNimi;
            }

            string kaust = PildikoguAbi.KaustaTee();
            string failiNimi = PildikoguAbi.PuhastaFailiNimi(sisestatudNimi) + ".png";
            string teeFaili = PildikoguAbi.LeiaVabaTee(kaust, failiNimi);

            pilt.Image.Save(teeFaili, System.Drawing.Imaging.ImageFormat.Png);

            MessageBox.Show(
                "Pilt on salvestatud pildikogusse!\n\n" + Path.GetFileName(teeFaili),
                "Salvestatud",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void OpenLibrary(object sender, EventArgs e)
        {
            string kaust = PildikoguAbi.KaustaTee();
            string[] failid = Directory.GetFiles(kaust, "*.*");

            if (failid.Length == 0)
            {
                MessageBox.Show(
                    "Pildikogus pole veel ühtegi pilti",
                    "Pildikogu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            PictureLibraryForm libraryForm = new PictureLibraryForm();

            if (libraryForm.ShowDialog() == DialogResult.OK &&
                libraryForm.SelectedImagePath != null)
            {
                LoadImage(libraryForm.SelectedImagePath);
            }
        }

        private void OpenDrawing(object sender, EventArgs e)
        {
            if (pilt.Image == null)
            {
                DrawingForm joonistusVorm = new DrawingForm(null);
                joonistusVorm.ShowDialog();
                return;
            }

            DialogResult vastus = MessageBox.Show(
                "Kas soovid joonistada praeguse pildi peale?\n\n" +
                "Vali 'Ei', et joonistada tühjale valgele lehele.",
                "Joonistamine",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (vastus == DialogResult.Cancel)
                return;

            if (vastus == DialogResult.Yes)
            {
                DrawingForm joonistusVorm = new DrawingForm(pilt.Image);
                joonistusVorm.ShowDialog();
            }
            else
            {
                DrawingForm joonistusVorm = new DrawingForm(null);
                joonistusVorm.ShowDialog();
            }
        }

        private void CloseForm(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (pilt.Image != null)
            {
                pilt.Image.Dispose();
            }

            base.OnFormClosed(e);
        }
    }
}