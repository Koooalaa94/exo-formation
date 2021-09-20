namespace Labyrinth
{
    public struct Cell
    {
        // Coordonnée d'une cellule
        public int Ordonnee;
        public int Abscisse;

        // 0 : Haut, 1 : bas, 2 : gauche, 3 : droite
        public bool[] Walls { get; set; }

        public bool IsVisited { get; set; }

        // 1.b. Représentation du status, une représentation par des entiers a été choisi car la cellule represente un état défini. 
        // 0 : cellule simple , 1 : entrée , -1 : sortie
        public int Status;
    }
}
