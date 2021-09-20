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
            if (_maze[i, j].Status == 1)
            {
                return true;
            }

            return false;
        }

        public bool IsMazeEnd(int i, int j)
        {
            if (_maze[i, j].Status == -1)
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
            List<KeyValuePair<int, int>> closeNeighbors = new List<KeyValuePair<int, int>>();

            // Voisin SI case étudié se trouve sur la ligne du dessus
            if (_maze[i, j].Abscisse == 0)
            {
                closeNeighbors.Add(new KeyValuePair<int, int>(i, (j + 1)));
                closeNeighbors.Add(new KeyValuePair<int, int>(i, (j - 1)));
                closeNeighbors.Add(new KeyValuePair<int, int>((i + 1), j));
            }

            // Voisin SI case étudié se trouve sur la colonne de gauche
            if (_maze[i, j].Ordonnee == 0)
            {
                closeNeighbors.Add(new KeyValuePair<int, int>(i, (j + 1)));
                closeNeighbors.Add(new KeyValuePair<int, int>((i - 1), j));
                closeNeighbors.Add(new KeyValuePair<int, int>((i + 1), j));
            }

            // Voisin SI case étudié se trouve sur la ligne du dessous
            if (_maze[i, j].Abscisse == _lineSize - 1)
            {
                closeNeighbors.Add(new KeyValuePair<int, int>(i, (j + 1)));
                closeNeighbors.Add(new KeyValuePair<int, int>(i, (j - 1)));
                closeNeighbors.Add(new KeyValuePair<int, int>((i - 1), j));
            }
            // Voisin SI case étudié se trouve sur la colonne de droite
            if (_maze[i, j].Ordonnee == _columnSize - 1)
            {
                closeNeighbors.Add(new KeyValuePair<int, int>(i, (j - 1)));
                closeNeighbors.Add(new KeyValuePair<int, int>((i - 1), j));
                closeNeighbors.Add(new KeyValuePair<int, int>((i + 1), j));
            }

            // Voisin SI case étudié se trouve dans le coin haut gauche
            if (_maze[i, j].Abscisse == 0 && _maze[i, j].Ordonnee == 0)
            {
                closeNeighbors.Add(new KeyValuePair<int, int>(i, (j + 1)));
                closeNeighbors.Add(new KeyValuePair<int, int>((i + 1), j));
            }

            // Voisin SI case étudié se trouve dans le coin bas gauche
            if (_maze[i, j].Abscisse == 0 && _maze[i, j].Ordonnee == _columnSize - 1)
            {
                closeNeighbors.Add(new KeyValuePair<int, int>(i, (j + 1)));
                closeNeighbors.Add(new KeyValuePair<int, int>((i - 1), j));
            }

            // Voisin SI case étudié se trouve dans le coin haut droite
            if (_maze[i, j].Abscisse == _lineSize - 1 && _maze[i, j].Ordonnee == 0)
            {
                closeNeighbors.Add(new KeyValuePair<int, int>(i, (j - 1)));
                closeNeighbors.Add(new KeyValuePair<int, int>((i + 1), j));
            }

            // Voisin SI case étudié se trouve dans le coin bas droite
            if (_maze[i, j].Abscisse == _lineSize - 1 && _maze[i, j].Ordonnee == _columnSize - 1)
            {
                closeNeighbors.Add(new KeyValuePair<int, int>(i, (j - 1)));
                closeNeighbors.Add(new KeyValuePair<int, int>((i - 1), j));
            }

            // Voisin SI case étudié ne se trouve sur aucune extrémité
            if (_maze[i, j].Abscisse == 0 || _maze[i, j].Ordonnee == 0 || _maze[i, j].Abscisse == _lineSize - 1 || _maze[i, j].Ordonnee == _columnSize - 1)
            {
                closeNeighbors.Add(new KeyValuePair<int, int>(i, (j + 1)));
                closeNeighbors.Add(new KeyValuePair<int, int>(i, (j - 1)));
                closeNeighbors.Add(new KeyValuePair<int, int>((i + 1), j));
                closeNeighbors.Add(new KeyValuePair<int, int>((i - 1), j));
            }
            return closeNeighbors;
        }


        public KeyValuePair<int, int> Generate()
        {
            Random r = new Random();
            KeyValuePair<int, int> cellule = new KeyValuePair<int, int>();
            cellule<int, int> = r.Next(_lineSize, _columnSize);
            _maze[i,j].IsVisited = true;

            return new KeyValuePair<int, int>();

            // 5.b. Toutes les cases du labyrinth seront visité car pour chaque case il visite la case voisine puis retourne en arrière et réitère la manoeuvre, toute les cellule seront ainsi visité jusqua la cellule sortie. 
        }

        /* 6.
         * │ = parois gauche/droite fermé
         * ─ = parois haut/bas fermé
         * ├ = double parois vertical fermé avec début d'angle droite
         * ┼ = double parois vertical fermé avec d'but d'angle droite et gauche
         * ┤ = double parois vertical fermé avec début d'angle gauche
         * ' ' = parois ouverte
         * ╷ = simple parois vertical
         * ┐ = simple parois vertical avec angle haut gauche
         * ┌ = simple parois vertical avec angle haut droite
         * ┬ = simple parois vertical avec angle haut droite et gauche
         * ┘ = simple parois vertical avec angle bas gauche
         * └ = simple parois vertical avec angle bas droite
         * ┴ = simple parois vertical avec angle bas droite et gauche
         * ╶ = demi parois horizontal droite (parois fermé avec angle droite)
         * ╴ = demi parois horizontal gauche (parois fermé avec angle gauche)
         */
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
