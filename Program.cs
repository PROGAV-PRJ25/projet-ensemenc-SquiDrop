using System;

class Program
{
    static void Main()
    {
        // 1. Initialisation du plateau (5x5 par défaut)
        string[,] plateau = new string[5, 5];
        InitialiserPlateau(plateau);

        // 2. Création de la simulation (vitesse inutilisée dans ce cas)
        var gestionnaireTerrain = new Simulation(vitesse: 1.0);
        
        Console.WriteLine("🌱 POTAGER INTERACTIF 🌱");
        Console.WriteLine("Appuyez sur une touche pour commencer...");
        Console.ReadKey();

        // 3. Lancement
        gestionnaireTerrain.Simuler(plateau);
    }

    static void InitialiserPlateau(string[,] plateau)
    {
        // Remplit le plateau de terre vide ("E")
        for (int i = 0; i < plateau.GetLength(0); i++)
        {
            for (int j = 0; j < plateau.GetLength(1); j++)
            {
                plateau[i, j] = "E"; // E = Terre vide
            }
        }
    }
}