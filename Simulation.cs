public class Simulation
{
    public double Vitesse { get; set; } // Inutilisé dans cette version

    public Simulation(double vitesse) => Vitesse = vitesse;

    public void Simuler(string[,] plateau)
    {
        int hauteur = plateau.GetLength(0);
        int largeur = plateau.GetLength(1);
        
        int posX = 0, posY = 0; // Position du curseur
        bool quitter = false;

        while (!quitter)
        {
            Console.Clear();
            Console.WriteLine("=== POTAGER ===");

            // Affiche le plateau avec curseur
            for (int y = 0; y < hauteur; y++)
            {
                for (int x = 0; x < largeur; x++)
                {
                    bool estCurseur = (y == posY && x == posX);
                    string caseActuelle = plateau[y, x];

                    Console.BackgroundColor = estCurseur ? ConsoleColor.White : ConsoleColor.Black;
                    Console.ForegroundColor = caseActuelle switch
                    {
                        "E" => ConsoleColor.DarkYellow,  // Terre vide
                        "P" => ConsoleColor.Green,       // Plante
                        _ => ConsoleColor.Gray
                    };

                    Console.Write($"[{caseActuelle}]");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            // Légende et commandes
            Console.WriteLine("\nLégende: [E] Terre vide | [P] Plante");
            Console.WriteLine("Flèches: Déplacer | Espace: Planter/Récolter | Q: Quitter");

            // Gestion des touches
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow when posY > 0: posY--; break;
                case ConsoleKey.DownArrow when posY < hauteur - 1: posY++; break;
                case ConsoleKey.LeftArrow when posX > 0: posX--; break;
                case ConsoleKey.RightArrow when posX < largeur - 1: posX++; break;
                case ConsoleKey.Spacebar:
                    plateau[posY, posX] = plateau[posY, posX] == "E" ? "P" : "E"; // Alterne entre plante et terre
                    break;
                case ConsoleKey.Q: quitter = true; break;
            }
        }
    }
}