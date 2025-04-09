using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace PiratesDataExplorer
{
    public partial class Form1 : Form
    {
        private byte[] fileData; // Yüklenecek veri burada tutulacak
        private int width = 8;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Paint += pictureBox1_Paint;
        }





        private void loadBinary(string filename)
        {
            if (File.Exists(filename))
            {
                fileData = File.ReadAllBytes(filename);
            }
            else
            {
                MessageBox.Show("Dosya bulunamadı.");
            }
        }

        private int imageWidth = 0;
        private int imageHeight = 0;
        private Bitmap renderedBitmap = null;


        private void ShowPirate()
        {
            if (fileData == null || fileData.Length != 1522) return;

            // Tam C64 renk paleti (16 renk)
            int[,] c64Palette = new int[,]
            {
        { 0x00, 0x00, 0x00 }, // 0 - Black
        { 0xFF, 0xFF, 0xFF }, // 1 - White
        { 0x88, 0x00, 0x00 }, // 2 - Red
        { 0xAA, 0xFF, 0xEE }, // 3 - Cyan
        { 0xCC, 0x44, 0xCC }, // 4 - Purple
        { 0x00, 0xCC, 0x55 }, // 5 - Green
        { 0x00, 0x00, 0xAA }, // 6 - Blue
        { 0xEE, 0xEE, 0x77 }, // 7 - Yellow
        { 0xDD, 0x88, 0x55 }, // 8 - Orange
        { 0x66, 0x44, 0x00 }, // 9 - Brown
        { 0xFF, 0x77, 0x77 }, // 10 - Light red
        { 0x33, 0x33, 0x33 }, // 11 - Dark gray
        { 0x77, 0x77, 0x77 }, // 12 - Medium gray
        { 0xAA, 0xFF, 0x66 }, // 13 - Light green
        { 0x00, 0x88, 0xFF }, // 14 - Light blue
        { 0xBB, 0xBB, 0xBB }  // 15 - Light gray
            };

            int colorIndex = fileData[0x4F2] & 0x0F;

            Bitmap bmp = new Bitmap(144, 112); // 18 chars * 8px, 14 rows * 8px

            for (int cy = 0; cy < 14; cy++)
            {
                for (int cx = 0; cx < 18; cx++)
                {
                    int charIndex = fileData[0x3F2 + cy * 18 + cx] - 0x80;
                    colorIndex = fileData[0x4F2 + cy * 18 + cx] & 0x0F;
                    if (charIndex < 0 || charIndex >= 0x7E) continue;

                    int charOffset = 2 + charIndex * 8;

                    for (int row = 0; row < 8; row++)
                    {
                        byte b = fileData[charOffset + row];


                        for (int i = 0; i < 4; i++)
                        {
                            int bits = (b >> (6 - 2 * i)) & 0x03;

                            int paletteIndex = bits switch
                            {
                                0 => 1,    // Black
                                1 => 0,    // Blue
                                2 => 8,//10,    // 
                                3 => colorIndex-8,
                                _ => 0
                            };

                            int px = cx * 8 + i * 2;
                            int py = cy * 8 + row;

                            for (int dx = 0; dx < 2; dx++)
                            {
                                bmp.SetPixel(px + dx, py, Color.FromArgb(
                                    c64Palette[paletteIndex, 0],
                                    c64Palette[paletteIndex, 1],
                                    c64Palette[paletteIndex, 2]
                                ));
                            }
                        }
                    }
                }
            }

            pictureBox1.Image = bmp;
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (renderedBitmap != null)
            {
                e.Graphics.DrawImage(renderedBitmap, 0, 0);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // ListBox1: A/ ve B/ klasöründeki .win dosyaları
            string[] folders = { "A", "B" };
            foreach (string folder in folders)
            {
                if (Directory.Exists(folder))
                {
                    var files = Directory.GetFiles(folder, "*.win");
                    foreach (var file in files)
                    {
                        string relativePath = Path.Combine(folder, Path.GetFileName(file));
                        listBox1.Items.Add(relativePath.Replace("\\", "/"));
                    }

                    var faceFiles = Directory.GetFiles(folder, "face*");
                    foreach (var file in faceFiles)
                    {
                        listBox2.Items.Add(Path.GetFileName(file));
                    }
                }
            }

            // ListBox2: Tüm dizinde "face*" ile başlayan dosyalar (uzantılı ya da uzantısız)

        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {

            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFile = listBox1.Items[listBox1.SelectedIndex].ToString();
            loadBinary(selectedFile);
            ShowPirate();
            if (checkBox1.Checked) saveImage(selectedFile + "_EX.png");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG Image|*.png|Bitmap Image|*.bmp|JPEG Image|*.jpg";
                sfd.Title = "Save Image As";
                sfd.FileName = "pirate";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ImageFormat format = ImageFormat.Png;
                    string ext = Path.GetExtension(sfd.FileName).ToLower();

                    if (ext == ".bmp") format = ImageFormat.Bmp;
                    else if (ext == ".jpg" || ext == ".jpeg") format = ImageFormat.Jpeg;

                    pictureBox1.Image.Save(sfd.FileName, format);
                }
            }
        }

        private void saveImage(string filename)
        {
            pictureBox1.Image.Save(filename, ImageFormat.Png);
        }

    }
}
