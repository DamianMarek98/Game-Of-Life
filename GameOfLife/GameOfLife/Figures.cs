using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    static class Figures
    {
        public static List<Tuple<int, int>> Tub()
        {
            List<Tuple<int, int>> pos = new List<Tuple<int, int>>();
            pos.Add(new Tuple<int, int>(1,0));
            pos.Add(new Tuple<int, int>(0,1));
            pos.Add(new Tuple<int, int>(-1,0));
            pos.Add(new Tuple<int, int>(0,-1));

            return pos;
        }

        public static List<Tuple<int, int>> Frog()
        {
            List<Tuple<int, int>> pos = new List<Tuple<int, int>>();
            pos.Add(new Tuple<int, int>(0, -1));
            pos.Add(new Tuple<int, int>(-1, 0));
            pos.Add(new Tuple<int, int>(-1, 1));
            pos.Add(new Tuple<int, int>(2, 0));
            pos.Add(new Tuple<int, int>(2, 1));
            pos.Add(new Tuple<int, int>(1, 2));

            return pos;
        }

        public static List<Tuple<int, int>> Glider()
        {
            List<Tuple<int, int>> pos = new List<Tuple<int, int>>();
            pos.Add(new Tuple<int, int>(-1, -1));
            pos.Add(new Tuple<int, int>(-1, 0));
            pos.Add(new Tuple<int, int>(-1, 1));
            pos.Add(new Tuple<int, int>(0, -1));
            pos.Add(new Tuple<int, int>(1, 0));

            return pos;
        }
    }
}
