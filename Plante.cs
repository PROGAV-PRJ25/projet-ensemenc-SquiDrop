using System;
using System.Collections.Generic;



public abstract class Plante
{
    // Générateur partagé pour la maladie
    private static readonly Random _rng = new Random();
    private int joursSansMaladie = 0;
    private const int probMaladieInverse = 10;

    // — Propriétés / Attributs — 
    public string NomPlante { get; protected set; }
    public string Acronyme { get; protected set; }
    public int EspacePris { get; protected set; }
    public Terrain TerrainIdeal { get; protected set; }
    public List<Saison> SaisonCompatible { get; protected set; }

    public float HydratationCritique { get; protected set; } = 30f;
    public float LuminositeIdeale { get; protected set; } = 70f;
    public float TemperatureMinimale { get; protected set; }
    public float TemperatureMaximale { get; protected set; }
    public float VitesseDeshydratation { get; protected set; }

    public float HydratationActuelle { get; set; } = 100f;
    public float LuminositeActuelle { get; protected set; } = 50f;
    public float TemperatureActuelle { get; set; } = 15f;

    public Maladie? MaladieActuelle { get; protected set; } = null;
    public bool EstMorte { get; protected set; } = false;

    public float VitesseCroissance { get; protected set; }
    public float HauteurActuelle { get; protected set; } = 0f;
    public float HauteurMaximale { get; protected set; } = 1f;

    // — Constructeur — 
    protected Plante(
        string nomPlante,
        string acronyme,
        int espacePris,
        Terrain terrainIdeal,
        List<Saison> saisonCompatible,
        float vitesseDeshydratation,
        float temperatureMinimale,
        float temperatureMaximale,
        float vitesseCroissance,
        float hauteurMaximale
    )
    {
        NomPlante = nomPlante;
        Acronyme = acronyme;
        EspacePris = espacePris;
        TerrainIdeal = terrainIdeal;
        SaisonCompatible = saisonCompatible;

        VitesseDeshydratation = vitesseDeshydratation;
        TemperatureMinimale = temperatureMinimale;
        TemperatureMaximale = temperatureMaximale;

        VitesseCroissance = vitesseCroissance;
        HauteurMaximale = hauteurMaximale;
        HauteurActuelle = 0f;

        // États initiaux
        HydratationActuelle = 100f;
        LuminositeActuelle = LuminositeIdeale;
        TemperatureActuelle = (temperatureMinimale + temperatureMaximale) / 2f;
    }

    // — Méthodes d’aide pour l’extérieur (ex: Obstacle) — 
    public void ReduireHydratation(float montant)
    {
        HydratationActuelle = Math.Max(0f, HydratationActuelle - montant);
    }

    public void Tuer()
    {
        EstMorte = true;
    }

    // — Actions de base — 
    public virtual void Arroser()
    {
        HydratationActuelle = 100f;
    }

    public virtual void RecevoirLumiere(float intensite)
    {
        LuminositeActuelle = Math.Min(100f, LuminositeActuelle + intensite);
    }

    public virtual void SetTemperature(float temperature)
    {
        TemperatureActuelle = temperature;
    }

    // — Évaluation des 5 conditions — 
    public virtual float EvaluerConditions(bool espaceRespecte)
    {
        int defauts = 0;
        if (HydratationActuelle < HydratationCritique) defauts++;
        if (Math.Abs(LuminositeActuelle - LuminositeIdeale) >= 20f) defauts++;
        if (TemperatureActuelle < TemperatureMinimale
         || TemperatureActuelle > TemperatureMaximale) defauts++;
        if (MaladieActuelle != null) defauts++;
        if (!espaceRespecte) defauts++;
        return (float)defauts / 5f;
    }

    // — Mise à jour journalière — 
    public virtual void Update(
        float tempsEcouleEnJours,
        float temperatureDuJour,
        bool espaceRespecte,
        float coeffAbsorptionEau,
        float luminositeDuJour
    )
    {
        if (EstMorte) return;

        // 1) Température
        SetTemperature(temperatureDuJour);

        // 2) Maladie
        if (MaladieActuelle == null)
        {
            joursSansMaladie++;
            if (joursSansMaladie >= probMaladieInverse)
            {
                var cand = Maladie.GenererMaladieAleatoire();
                if (cand.SeDeclare())
                {
                    MaladieActuelle = cand;
                    joursSansMaladie = 0;
                }
            }
        }
        else
        {
            MaladieActuelle.AppliquerEffets(this);
        }

        // 3) Conditions
        float tauxNonOpt = EvaluerConditions(espaceRespecte);
        if (tauxNonOpt >= 0.6f)
        {
            Tuer();
            return;
        }
        float tauxOpt = 1f - tauxNonOpt;

        // 4) Croissance
        Pousser(tauxOpt);

        // 5) Hydratation (modulée par le sol)
        float perte = VitesseDeshydratation * tempsEcouleEnJours * (1f - coeffAbsorptionEau);
        ReduireHydratation(perte);

        // 6) Jauge de lumière persistante
        LuminositeActuelle = Math.Max(0f, LuminositeActuelle - 1f * tempsEcouleEnJours);
    }

    // — Croissance — 
    protected virtual void Pousser(float tauxSatisfaction)
    {
        float delta = VitesseCroissance * tauxSatisfaction;
        HauteurActuelle = Math.Min(HauteurMaximale, HauteurActuelle + delta);
    }
}

