using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
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

        public class ChrData
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int F { get; set; }
        }

        private static readonly Dictionary<string, ChrData> _data = new()
        {
            ["chr 0.win"] = new ChrData { X = 269 - 192, Y = 81 - 48, F = 4 },
            ["chr 1.win"] = new ChrData { X = 233 - 192, Y = 67 - 48, F = 3 },
            ["chr 2.win"] = new ChrData { X = 254 - 192, Y = 63 - 48, F = 0 },
            ["chr 3.win"] = new ChrData { X = 249 - 192, Y = 70 - 48, F = 0 },
            ["chr 4.win"] = new ChrData { X = 253 - 192, Y = 71 - 48, F = 0 },
            ["chr 5.win"] = new ChrData { X = 246 - 192, Y = 62 - 48, F = 3 },
            ["chr 6.win"] = new ChrData { X = 269 - 192, Y = 70 - 48, F = 1 },
            ["chr 7.win"] = new ChrData { X = 241 - 192, Y = 63 - 48, F = 0 },
            ["chr 8.win"] = new ChrData { X = 251 - 192, Y = 87 - 48, F = 0 },
            ["chr 9.win"] = new ChrData { X = 270 - 192, Y = 72 - 48, F = 4 },
            ["chr 10.win"] = new ChrData { X = 251 - 192, Y = 62 - 48, F = 0 },
            ["chr 11.win"] = new ChrData { X = 245 - 192, Y = 56 - 48, F = 1 },
            ["chr 12.win"] = new ChrData { X = 253 - 192, Y = 63 - 48, F = 0 },
            ["chr 13.win"] = new ChrData { X = 237 - 192, Y = 74 - 48, F = 4 },
            ["chr 14.win"] = new ChrData { X = 260 - 192, Y = 64 - 48, F = 2 },
            ["chr 15.win"] = new ChrData { X = 252 - 192, Y = 72 - 48, F = 5 },
            ["chr 16.win"] = new ChrData { X = 251 - 192, Y = 73 - 48, F = 1 },
            ["chr 17.win"] = new ChrData { X = 236 - 192, Y = 65 - 48, F = 3 },
            ["chr 18.win"] = new ChrData { X = 236 - 192, Y = 65 - 48, F = 7 },
            ["chr 19.win"] = new ChrData { X = 251 - 192, Y = 60 - 48, F = 6 },
            ["chr 20.win"] = new ChrData { X = 219 - 192, Y = 72 - 48, F = 5 },
            ["chr 21.win"] = new ChrData { X = 263 - 192, Y = 75 - 48, F = 5 },
            ["chr 22.win"] = new ChrData { X = 247 - 192, Y = 58 - 48, F = 8 },
            ["chr 23.win"] = new ChrData { X = 253 - 192, Y = 71 - 48, F = 2 }
        };

        public static ChrData GetData(string key)
        {
            return _data.TryGetValue(key, out var val) ? val : null;
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

        private int face = 0;
        private int imageWidth = 0;
        private int imageHeight = 0;
        private Bitmap renderedBitmap = null;


        private void ShowPirate()
        {
            if (fileData == null || fileData.Length != 1522) return;



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
                                3 => colorIndex - 8,
                                _ => 0
                            };

                            int px = cx * 8 + i * 2;
                            int py = cy * 8 + row;

                            for (int dx = 0; dx < 2; dx++)
                            {
                                bmp.SetPixel(px + dx, py, getC64Color(paletteIndex));
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
                        string relativePath = Path.Combine(folder, Path.GetFileName(file));
                        listBox2.Items.Add(relativePath.Replace("\\", "/"));
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
            loadImage();
        }
        private void loadImage()
        {
            string selectedFile = listBox1.Items[listBox1.SelectedIndex].ToString();
            loadBinary(selectedFile);
            ShowPirate();
            if (checkBox2.Checked) {
                string fileName = selectedFile.Substring(selectedFile.LastIndexOf('/') + 1);
                var data = GetData(fileName);
                if (data != null)
                {
                    //Console.WriteLine($"X={data.X}, Y={data.Y}, F={data.F}");
                    selectedFile = "a\\face " + data.F.ToString();
                    loadBinary(selectedFile);
                    ShowFace(pictureBox1, data.X, data.Y - 8, face);
                }
            }
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

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFile = listBox2.Items[listBox2.SelectedIndex].ToString();
            loadBinary(selectedFile);
            ShowFace(pictureBox1, 76, 24, 0);
            //if (checkBox1.Checked) saveImage(selectedFile + "_EX.png");
        }

        private void ShowFace(PictureBox targetBox, int offsetX, int offsetY, int faceno)
        {
            if (fileData == null || fileData.Length != 1538) return;

            int[] palet = { 1, 1, 2, 0 }; // katman renkleri

            const int spriteWidth = 24;
            const int spriteHeight = 21;
            const int spriteSize = 63;
            const int spriteSpacing = 0;
            const int totalSprites = 24;

            //int faceno = 0;
            int layerno = 0;

            // Eğer targetBox'ta bir görüntü yoksa, yeni bir bitmap oluştur
            if (targetBox.Image == null)
            {
                targetBox.Image = new Bitmap(targetBox.Width, targetBox.Height);
            }

            Bitmap bmp = (Bitmap)targetBox.Image;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int spriteIndex = (4 * faceno); spriteIndex < totalSprites; spriteIndex++)
                {
                    int spriteOffset = 8 + spriteIndex * 64;

                    for (int row = 0; row < spriteHeight; row++)
                    {
                        int rowOffset = spriteOffset + row * 3;
                        if (rowOffset + 2 >= fileData.Length) continue;

                        byte b0 = fileData[rowOffset];
                        byte b1 = fileData[rowOffset + 1];
                        byte b2 = fileData[rowOffset + 2];

                        string bits = Convert.ToString(b0, 2).PadLeft(8, '0') +
                                      Convert.ToString(b1, 2).PadLeft(8, '0') +
                                      Convert.ToString(b2, 2).PadLeft(8, '0');

                        for (int x = 0; x < spriteWidth; x++)
                        {
                            if (bits[x] == '1')
                            {
                                //int y = faceno * (spriteHeight + spriteSpacing) + row;
                                int y = row;
                                int px = offsetX + x;
                                int py = offsetY + y;

                                if (px >= 0 && px < bmp.Width && py >= 0 && py < bmp.Height)
                                {
                                    bmp.SetPixel(px, py, getC64Color(palet[layerno]));
                                }
                            }
                        }
                    }

                    layerno++;
                    if (layerno == 4)
                    {
                        layerno = 0;
                        //faceno++;
                        break;
                    }
                }
            }

            targetBox.Invalidate();
        }

        Color getC64Color(int index)
        {
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

            if (index < 0 || index > 15)
                return Color.Transparent;

            return Color.FromArgb(
                c64Palette[index, 0],
                c64Palette[index, 1],
                c64Palette[index, 2]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            face++;
            if (face > 5) face = 0;
            loadImage();

        }
    }
}
