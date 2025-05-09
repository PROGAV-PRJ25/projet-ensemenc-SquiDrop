public abstract class Maladie : Obstacle
{
    public float Gravite { get; protected set; }   // perte d’eau ou de vigueur

    protected Maladie(string nom, string desc, float proba, float gravite)
        : base(nom, desc, proba)
    {
        Gravite = gravite;
    }

    public override void AppliquerEffets(Plante p)
    {
        // Exemple : réduire l’hydratation
        p.ReduireHydratation(Gravite);
    }
    public static Maladie GenererMaladieAleatoire()
    {
        var liste = new List<Maladie>
        {
            new MaladieMildew(),
            new MaladieRouille(),
            // etc.
        };
        var idx = new Random().Next(liste.Count);
        return liste[idx];
    }
}