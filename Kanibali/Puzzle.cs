using System;
using System.Collections.Generic;
using System.Text;

namespace Detyra2
{
    class Puzzle
    {
        public int G, H, F;
        public Puzzle Parent;

        // G numri i nodes deri te currentNode
        // H shuma e kanibaleve dhe misioneve ne taget - shuma e kanibaleve dhe misioneve ne current node
        // F = G + H

        /*public int kanibaletMajtas;
        public int kanibaletDjathtas;*/

        public List<Kanibali> kanibaletMajtas;
        public List<Kanibali> kanibaletDjathtas;

        public int misionaretMajtas;
        public int misionaretDjathtas;

        public int anijaMajtas;
        public int anijaDjathtas;

        public Puzzle(List<Kanibali> _kanibaletMajtas, List<Kanibali> _kanibaletDjathtas
                    , int _misionaretMajtas, int _misionaretDjathtas,
                      int _anijaMajtas, int _anijaDjathtas)
        {
            kanibaletDjathtas = _kanibaletDjathtas;
            kanibaletMajtas = _kanibaletMajtas;

            misionaretDjathtas = _misionaretDjathtas;
            misionaretMajtas = _misionaretMajtas;

            anijaDjathtas = _anijaDjathtas;
            anijaMajtas = _anijaMajtas;
        }
    }
}
