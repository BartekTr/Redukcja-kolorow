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
        Color[,] colors, bitmapColors, colorsSecondVersion;
        Octree tree, secondTree;
        int colorReduction = 1;
        public Form1()
        {
            InitializeComponent();
            pictureTexture = new Bitmap("example2.bmp");
            UploadBitmapData();
        }
        private void UploadBitmapData()
        {
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
                    colorsSecondVersion = GetColorsFromBitmap(bmp3, pictureBox3.Width, pictureBox3.Height);
                }
            }
            bitmapColors = (Color[,])colors.Clone();
        }

        private void InitTree()
        {
            tree = new Octree();
            for (int i = 0; i < pictureBox2.Width; i++)
                for (int j = 0; j < pictureBox2.Height; j++)
                {
                    var a = new BinaryColor(bitmapColors[i, j]);
                    tree.InsertTree(a, 0);
                    progressBar1.Increment(1);
                }
        }

        private void InitSecondTree()
        {
            secondTree = new Octree();
            for (int i = 0; i < pictureBox3.Width; i++)
                for (int j = 0; j < pictureBox3.Height; j++)
                {
                    var a = new BinaryColor(bitmapColors[i, j]);
                    secondTree.InsertTreeSecondVersion(a, 0,colorReduction);
                    progressBar2.Increment(1);
                }
            var b = secondTree.GetLeafParents();
            var c = secondTree.GetLeafCount(b);
            UpdateColorsSecondVersion(secondTree);
        }

        private void ReduceColors()
        {
            InitTree();
            tree.Reduce(colorReduction);
            UpdateColors(tree);
        }

        private void UpdateColors(Octree tree)
        {
            for (int i = 0; i < pictureBox2.Width; i++)
                for (int j = 0; j < pictureBox2.Height; j++)
                {
                    Color color = bitmapColors[i, j];
                    colors[i, j] = tree.GetColor(new BinaryColor(color));
                    progressBar1.Increment(1);
                }
            var leafParents = tree.GetLeafParents();
            int leafsNumber = tree.GetLeafCount(leafParents);
            if(leafsNumber != 1)
                label3.Text = $"using: {leafsNumber} colors";
            else
                label3.Text = $"using: {leafsNumber} color";
            pictureBox2.Invalidate();
        }
        private void UpdateColorsSecondVersion(Octree tree)
        {
            for (int i = 0; i < pictureBox3.Width; i++)
                for (int j = 0; j < pictureBox3.Height; j++)
                {
                    Color color = bitmapColors[i, j];
                    colorsSecondVersion[i, j] = secondTree.GetColor(new BinaryColor(color));
                    progressBar2.Increment(1);
                }
            var leafParents = secondTree.GetLeafParents();
            int leafsNumber = secondTree.GetLeafCount(leafParents);
            if (leafsNumber != 1)
                label4.Text = $"using: {leafsNumber} colors";
            else
                label4.Text = $"using: {leafsNumber} color";
            pictureBox3.Invalidate();
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            Bitmap temporaryBitmap = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            unsafe
            {
                BitmapData bitmapData = temporaryBitmap.LockBits(new Rectangle(0, 0, pictureBox3.Width, pictureBox3.Height), ImageLockMode.ReadWrite, temporaryBitmap.PixelFormat);
                int heightInPixels = bitmapData.Height;
                int bytesPerPixel = Bitmap.GetPixelFormatSize(temporaryBitmap.PixelFormat) / 8;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* firstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = firstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        currentLine[x] = colorsSecondVersion[x / 4, y].B;
                        currentLine[x + 1] = colorsSecondVersion[x / 4, y].G;
                        currentLine[x + 2] = colorsSecondVersion[x / 4, y].R;
                        currentLine[x + 3] = colorsSecondVersion[x / 4, y].A;
                    }
                });
                temporaryBitmap.UnlockBits(bitmapData);
            }
            e.Graphics.DrawImage(temporaryBitmap, 0, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp|jpg files (*.jpg)|*.jpg";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Bitmap original = new Bitmap(dlg.FileName);
                    Bitmap resized = new Bitmap(original, new Size(pictureBox1.Width, pictureBox1.Height));
                    pictureTexture = resized;
                    UploadBitmapData();
                    pictureBox1.Invalidate();
                    pictureBox2.Invalidate();
                    pictureBox3.Invalidate();
                }
            }
            label3.Text = "using: all colors";
            label4.Text = "using: all colors";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar2.Value = 0;
            ReduceColors();
            InitSecondTree();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            colorReduction = trackBar1.Value;
            button1.Text = "Reduce to: " + trackBar1.Value.ToString();
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
