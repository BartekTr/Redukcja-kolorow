using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Redukcja_kolorow
{
    public class BinaryColor
    {
        public BitArray R;
        public BitArray G;
        public BitArray B;

        public BinaryColor(Color color)
        {
            R = new BitArray(BitConverter.GetBytes(color.R));
            G = new BitArray(BitConverter.GetBytes(color.G));
            B = new BitArray(BitConverter.GetBytes(color.B));
        }
        public Color GetColor()
        {
            var r = new int[1];
            var g = new int[1];
            var b = new int[1];
            R.CopyTo(r, 0);
            G.CopyTo(g, 0);
            B.CopyTo(b, 0);
            return Color.FromArgb(r[0],g[0],b[0]);
        }
    }
    public class Octree
    {
        public int color_count = 0;
        public Color color;
        public Octree[] Next;
        public Octree()
        {
            Next = new Octree[8];
        }
        public void InsertTree(BinaryColor color, int depth)
        {
            if (depth >= 8)
            {
                this.color = color.GetColor();
                color_count++;
                Next = null;
            }
            else
            {
                Next[Branch(color.R[depth], color.G[depth], color.B[depth])] = new Octree();
                Next[Branch(color.R[depth], color.G[depth], color.B[depth])]
                    .InsertTree(color, depth + 1);
            }
        }
        private int Branch(bool R, bool G, bool B)
        {
            int res = 0;
            res += R ? 1 : 0;
            res += G ? 2 : 0;
            res += B ? 4 : 0;
            return res;
        }
    }

}