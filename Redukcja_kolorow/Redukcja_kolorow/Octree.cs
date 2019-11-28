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
            return Color.FromArgb(r[0], g[0], b[0]);
        }
    }
    public class Octree
    {
        public int sum;
        public bool isLeaf;
        public bool isLeafParent;
        public Color color;
        public Octree[] Next;
        public Octree parent;
        public Octree()
        {
            Next = new Octree[8];
            sum = 0;
            isLeaf = false;
            isLeafParent = false;
        }

        public void InsertTreeSecondVersion(BinaryColor color, int depth, int maxLeafNumber)
        {
            InsertTree(color, depth);
            Reduce(maxLeafNumber);
        }

        public void InsertTree(BinaryColor color, int depth)
        {
            if (depth >= 8)
            {
                this.color = color.GetColor();
                isLeaf = true;
                parent.isLeafParent = true;
                Next = null;
                sum++;
            }
            else
            {
                if (isLeaf)
                {
                    Color newColor = color.GetColor();
                    int R = this.color.R * sum + newColor.R;
                    int G = this.color.G * sum + newColor.G;
                    int B = this.color.B * sum + newColor.B;
                    sum++;
                    this.color = Color.FromArgb((int)((double)R / (double)sum), (int)((double)G / (double)sum), (int)((double)B / (double)sum));
                    return;
                }
                sum++;
                int depth2 = 8 - depth;
                if (Next[Branch(color.R[depth2], color.G[depth2], color.B[depth2])] == null)
                {
                    Next[Branch(color.R[depth2], color.G[depth2], color.B[depth2])] = new Octree();
                    Next[Branch(color.R[depth2], color.G[depth2], color.B[depth2])].parent = this;
                }
                Next[Branch(color.R[depth2], color.G[depth2], color.B[depth2])]
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

        public void Reduce(int count)
        {
            List<Octree> leafParents = GetLeafParents();
            var sortedList = new SortedList<int, Octree>(new DuplicateKeyComparer<int>());
            foreach (var item in leafParents)
                sortedList.Add(item.sum, item);
            int colorCount = GetLeafCount(leafParents);
            while (colorCount > count && sortedList.Count > 0)
            {
                var o = sortedList.ElementAt(0);
                sortedList.RemoveAt(0);
                if (sortedList.Count > 0)
                {
                    if (!o.Value.parent.isLeafParent)
                        sortedList.Add(o.Value.parent.sum, o.Value.parent);
                }
                int a = o.Value.ReduceNode();
                colorCount = colorCount - a + 1;
            }
        }
        //return old childs count
        private int ReduceNode()
        {
            int count = 0;
            int R = 0, G = 0, B = 0;
            for (int i = 0; i < 8; i++)
            {
                if (Next[i] == null)
                    continue;
                count++;
                R += Next[i].color.R * Next[i].sum;
                G += Next[i].color.G * Next[i].sum;
                B += Next[i].color.B * Next[i].sum;
                Next[i] = null;
            }
            color = Color.FromArgb((int)((double)R / (double)sum), (int)((double)G / (double)sum), (int)((double)B / (double)sum));
            isLeaf = true;
            isLeafParent = false;
            if (parent != null)
                parent.isLeafParent = true;
            return count;
        }
        private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);
                if (result == 0)
                    return 1;   // Handle equality as beeing greater
                else
                    return result;
            }
        }
        public List<Octree> GetLeafParents()
        {
            List<Octree> list = new List<Octree>();
            GetLeafParentsRecursion(list);
            return list;
        }
        private void GetLeafParentsRecursion(List<Octree> list)
        {
            if(isLeaf)
                return;
            else
            {
                if (isLeafParent)
                    list.Add(this);
                for (int i = 0; i < 8; i++)
                {
                    if (Next[i] != null)
                        Next[i].GetLeafParentsRecursion(list);
                }
            }
        }

        public int GetLeafCount(List<Octree> leafParents)
        {
            int count = 0;
            foreach (var el in leafParents)
            {
                for (int i = 0; i < 8; i++)
                    if (el.Next[i] != null)
                        if(el.Next[i].isLeaf)
                            count++;
            }
            return count;
        }
        public Color GetColor(BinaryColor color)
        {
            Octree tmp = this;
            int depth = 0;
            while (!tmp.isLeaf)
            {
                tmp = tmp.Next[Branch(color.R[8 - depth], color.G[8 - depth], color.B[8 - depth])];
                depth++;
            }
            return tmp.color;
        }

    }

}