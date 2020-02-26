using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redukcja_kolorow
{
    public partial class Form1 : Form
    {
        #region init data
        Bitmap pictureTexture, bmpMain, bmpAfterConstruction, bmpAlongConstruction;
        Color[,] colors, bitmapColors, colorsSecondVersion;
        Octree tree, secondTree;
        int colorsNumber = 1;
        #endregion
        public Form1()
        {
            InitializeComponent();
            pictureTexture = new Bitmap("example.bmp");
            UploadBitmapData();
        }
        #region init functions
        private void UploadBitmapData()
        {
            using (pictureTexture)
            {
                bmpMain = new Bitmap(picMain.Width, picMain.Height);
                bmpAfterConstruction = new Bitmap(picAfterConstruction.Width, picAfterConstruction.Height);
                bmpAlongConstruction = new Bitmap(picAlongConstruction.Width, picAlongConstruction.Height);
                using (var g = Graphics.FromImage(bmpMain))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.DrawImage(pictureTexture, new Rectangle(Point.Empty, bmpMain.Size));
                    picMain.Image = bmpMain;
                }
                using (var g = Graphics.FromImage(bmpAfterConstruction))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.DrawImage(pictureTexture, new Rectangle(Point.Empty, bmpAfterConstruction.Size));
                    colors = GetColorsFromBitmap(bmpAfterConstruction, picAfterConstruction.Width, picAfterConstruction.Height);
                }
                using (var g = Graphics.FromImage(bmpAlongConstruction))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.DrawImage(pictureTexture, new Rectangle(Point.Empty, bmpAlongConstruction.Size));
                    colorsSecondVersion = GetColorsFromBitmap(bmpAlongConstruction, picAlongConstruction.Width, picAlongConstruction.Height);
                }
            }
            bitmapColors = (Color[,])colors.Clone();
        }

        private Color[,] GetColorsFromBitmap(Bitmap bmp, int x, int y)
        {
            Color[,] colors = new Color[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    colors[i, j] = bmp.GetPixel(i, j);
            return colors;
        }
        #endregion
        #region pictureBox functions
        private void picAfterConstruction_Paint(object sender, PaintEventArgs e)
        {
            Bitmap temporaryBitmap = CreateBitmap(picAfterConstruction.Width, picAfterConstruction.Height, colors);
            e.Graphics.DrawImage(temporaryBitmap, 0, 0);
        }

        private void picAlongConstruction_Paint(object sender, PaintEventArgs e)
        {
            Bitmap temporaryBitmap = CreateBitmap(picAfterConstruction.Width, picAlongConstruction.Height, colorsSecondVersion);
            e.Graphics.DrawImage(temporaryBitmap, 0, 0);
        }

        private Bitmap CreateBitmap(int sizeX, int sizeY, Color[,] colors)
        {
            Bitmap bitmap = new Bitmap(sizeX, sizeY);
            unsafe
            {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, sizeX, sizeY), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                int heightInPixels = bitmapData.Height;
                int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
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
                bitmap.UnlockBits(bitmapData);
            }
            return bitmap;
        }

        #endregion
        #region events
        private void trcMain_Scroll(object sender, EventArgs e)
        {
            colorsNumber = trcMain.Value;
            btnReduce.Text = "Reduce to: " + trcMain.Value.ToString();
        }

        private void btnChangeBitmap_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Bitmap original = new Bitmap(dlg.FileName);
                    Bitmap resized = new Bitmap(original, new Size(picMain.Width, picMain.Height));
                    pictureTexture = resized;
                    UploadBitmapData();
                    picMain.Invalidate();
                    picAfterConstruction.Invalidate();
                    picAlongConstruction.Invalidate();
                }
            }
            lblAfterConstruction.Text = "using: all colors";
            lblAlongConstruction.Text = "using: all colors";
        }

        private void btnReduce_Click(object sender, EventArgs e)
        {
            barAfterConstruction.Value = 0;
            barAlongConstruction.Value = 0;
            ReduceColors();
            InitSecondTree();
        }
        private void btnGenerateTexture_Click(object sender, EventArgs e)
        {
            pictureTexture = GenerateBitmap(picMain.Width, picMain.Height);
            UploadBitmapData();
            picMain.Invalidate();
            picAfterConstruction.Invalidate();
            picAlongConstruction.Invalidate();
            lblAfterConstruction.Text = "using: all colors";
            lblAlongConstruction.Text = "using: all colors";
        }

        private Bitmap GenerateBitmap(int x, int y)
        {
            float radius = 10;
            float R = (float)Math.Min(0.3 * x, 0.3 * y);
            Bitmap bitmap = new Bitmap(x, y);
            using (var g = Graphics.FromImage(bitmap))
            {
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    g.FillRectangle(brush, (int)(0.1 * x), (int)(0.1 * y), (int)(0.9 * x), (int)(0.9 * y));
                }
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    g.FillRectangle(brush, 0, 0, (int)(0.1 * x), y);
                    g.FillRectangle(brush, 0, 0, x, (int)(0.1 * y));
                    g.FillRectangle(brush, (int)(0.9 * x), 0, x, y);
                    g.FillRectangle(brush, 0, (int)(0.9 * y), x, y);
                }
                for (int i = 0; i < 18; i++)
                {
                    double angle = i * 2 * Math.PI / 18;
                    Color color = ColorFromHSV((double)i * 20, 1, 1);
                    using (SolidBrush brush = new SolidBrush(color))
                    {
                        float centerX = (float)(R * Math.Sin(angle) + 0.5 * x);
                        float centerY = (float)(R * Math.Cos(angle) + 0.5 * y);
                        g.FillEllipse(brush, centerX - radius, centerY - radius,
                      radius + radius, radius + radius);
                    }
                }
            }
            return bitmap;
        }
        #endregion
        #region calculating colors
        private void InitTree()
        {
            tree = new Octree();
            for (int i = 0; i < picAfterConstruction.Width; i++)
                for (int j = 0; j < picAfterConstruction.Height; j++)
                {
                    var a = new BinaryColor(bitmapColors[i, j]);
                    tree.InsertTree(a, 0);
                    barAfterConstruction.Increment(1);
                }
        }

        private void InitSecondTree()
        {
            secondTree = new Octree();
            for (int i = 0; i < picAlongConstruction.Width; i++)
                for (int j = 0; j < picAlongConstruction.Height; j++)
                {
                    var a = new BinaryColor(bitmapColors[i, j]);
                    secondTree.InsertTreeSecondVersion(a, 0, colorsNumber);
                    barAlongConstruction.Increment(1);
                }
            var b = secondTree.GetLeafParents();
            var c = secondTree.GetLeafCount(b);
            UpdateColors(secondTree, colorsSecondVersion, lblAlongConstruction, picAlongConstruction, barAlongConstruction);
        }

        private void ReduceColors()
        {
            InitTree();
            tree.Reduce(colorsNumber);
            UpdateColors(tree, colors, lblAfterConstruction, picAfterConstruction, barAfterConstruction);
        }

        private void UpdateColors(Octree tree, Color[,] colors, Label label, PictureBox picture, ProgressBar bar)
        {
            for (int i = 0; i < picture.Width; i++)
                for (int j = 0; j < picture.Height; j++)
                {
                    Color color = bitmapColors[i, j];
                    colors[i, j] = tree.GetColor(new BinaryColor(color));
                    bar.Increment(1);
                }
            var leafParents = tree.GetLeafParents();
            int leafsNumber = tree.GetLeafCount(leafParents);
            if (leafsNumber != 1)
                label.Text = $"using: {leafsNumber} colors";
            else
                label.Text = $"using: {leafsNumber} color";
            picture.Invalidate();
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            switch (hi)
            {
                case 0:
                    return Color.FromArgb(255, v, t, p);
                case 1:
                    return Color.FromArgb(255, q, v, p);
                case 2:
                    return Color.FromArgb(255, p, v, t);
                case 3:
                    return Color.FromArgb(255, p, q, v);
                case 4:
                    return Color.FromArgb(255, t, p, v);
                case 5:
                    return Color.FromArgb(255, v, p, q);
            }
            return Color.Black;
        }
        #endregion
    }
}
