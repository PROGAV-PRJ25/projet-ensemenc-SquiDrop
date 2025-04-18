public class Meteo
{
    public float QuantitePluie { get; } // Entre 0 et 1
    public float Luminosite { get; }    // Entre 0 et 1
    public float Temperature { get; }   // Température du jour
    public bool Intemperie { get; }
    public string Description { get; }

    public Meteo(float pluie, float luminosite, float temperature, bool intemperie)
    {
        QuantitePluie = Math.Clamp(pluie, 0, 1);  // Garantit que la pluie est toujours entre 0% et 100%
        Luminosite = Math.Clamp(luminosite, 0.1f, 1);   // Garantit que la luminosité est toujours entre 10% et 100%
        Temperature = temperature;
        Intemperie = intemperie;

        Description = GenererDescription();
    }

    private string GenererDescription()
    {
        string desc = "";
        
        // Description température
        if (Temperature < 10) desc = "❄️ Glacial";
        else if (Temperature > 35) desc = "🔥 Caniculaire";
        else desc = "🌡️ Normal";

        // Ajout précipitations
        if (QuantitePluie > 0.5f) desc += " + Pluie torrentielle 🌧️";
        else if (QuantitePluie > 0) desc += " + Bruine ☔";

        // Ajout intempéries
        if (Intemperie) desc += " + Orage ⚡";

        return desc;
    }
}