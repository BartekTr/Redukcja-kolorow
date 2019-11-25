using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redukcja_kolorow
{
    public partial class Form1 : Form
    {
        Bitmap pictureTexture, bmp2, bmp3;
        Color[,] colors;
        Octree tree;
        public Form1()
        {
            InitializeComponent();
            pictureTexture = new Bitmap("example2.bmp");
            using (pictureTexture)
            {
                bmp2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                bmp3 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                using (var g = Graphics.FromImage(bmp2))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.DrawImage(pictureTexture, new Rectangle(Point.Empty, bmp2.Size));
                    pictureBox1.Image = bmp2;
                }
                using (var g = Graphics.FromImage(bmp3))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.DrawImage(pictureTexture, new Rectangle(Point.Empty, bmp3.Size));
                    colors = GetColorsFromBitmap(bmp3, pictureBox2.Width, pictureBox2.Height);
                }
            }
            InitTree();
        }

        private void InitTree()
        {
            tree = new Octree();
            for (int i = 0; i < pictureBox2.Width; i++)
                for (int j = 0; j < pictureBox2.Height; j++)
                {
                    tree.InsertTree(new BinaryColor(colors[i, j]), 0);
                }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Bitmap temporaryBitmap = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            unsafe
            {
                BitmapData bitmapData = temporaryBitmap.LockBits(new Rectangle(0, 0, pictureBox2.Width, pictureBox2.Height), ImageLockMode.ReadWrite, temporaryBitmap.PixelFormat);
                int heightInPixels = bitmapData.Height;
                int bytesPerPixel = Bitmap.GetPixelFormatSize(temporaryBitmap.PixelFormat) / 8;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* firstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = firstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        currentLine[x] = colors[x / 4, y].B;
                        currentLine[x + 1] = colors[x / 4, y].G;
                        currentLine[x + 2] = colors[x / 4, y].R;
                        currentLine[x + 3] = colors[x / 4, y].A;
                    }
                });
                temporaryBitmap.UnlockBits(bitmapData);
            }
            e.Graphics.DrawImage(temporaryBitmap, 0, 0);

        }

        private Color[,] GetColorsFromBitmap(Bitmap bmp, int x, int y)
        {
            Color[,] colors = new Color[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    colors[i, j] = bmp.GetPixel(i, j);
            return colors;
        }

    }
}
