using System;
using System.Collections.Generic;

namespace Labyrinth
{
    public class Maze
    {
        /// <summary>
        /// Grille permettant de représenter un matériau poreux
        /// Pour chaque élément, true case ouverte, false case bloquée
        /// </summary>
        private readonly Cell[,] _maze;

        private readonly int _lineSize;

        private readonly int _columnSize;

        /// <summary>
        /// Construction d'une grille de taille n * m
        /// </summary>
        /// <param name="size"></param>
        public Maze(int n, int m)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), n, "le nombre de lignes de la grille négatif ou null.");
            }

            if (m <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), n, "le nombre de colonnes de la grille négatif ou null.");
            }

            _lineSize = n;
            _columnSize = m;
            _maze = new Cell[n, m];
        }

        public bool IsOpen(int i, int j, int w)
        {
            return _maze[i, j].Walls[w];
        }

        public bool IsMazeStart(int i, int j)
        {
            if (_maze[i,j].Status == 1 )
            {
                return true;
            }

            return false;
        }

        public bool IsMazeEnd(int i, int j)
        {
            if (_maze[i,j].Status == -1)
            {
                return true;
            }

            return false;
        }

        public void Open(int i, int j, int w)
        {
            _maze[i, j].Walls[w] = true;
        }

        private List<KeyValuePair<int, int>> CloseNeighbors(int i, int j)
        {
            return null;
        }

        public KeyValuePair<int, int> Generate()
        {
            return new KeyValuePair<int, int>();
        }

        public string DisplayLine(int n)
        {
            return string.Empty;
        }

        public List<string> Display(int n)
        {
            return new List<string>();
        }
    }
}
