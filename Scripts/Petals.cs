using UnityEngine;

public enum Petal
{
    Site, Water, Energy, Equity, Health, Beauty, Materials
}

public static class Petals
{
    public readonly static int COUNT = 7;
    public static string[] PETAL_QUESTIONS = { "", "", "", "", "", "", "" };
    
    // Converts the string name of a petal into the enum type.
    // This is used when converting the petals from the stored comment responses.

    public static Petal StringToPetal(string name)
    {
        switch (name.ToLower())
        {
            case "site":        return Petal.Site;
            case "water":       return Petal.Water;
            case "energy":      return Petal.Energy;
            case "equity":      return Petal.Equity;
            case "health":      return Petal.Health;
            case "beauty":      return Petal.Beauty;
            case "materials":   return Petal.Materials;
            default:            return Petal.Site;
        }
    }

    // Gets the color that represents the given petal.

    public static Color PetalColor(Petal petal)
    {
        switch (petal)
        {
            case Petal.Site:        return RGB(196, 217,  48);
            case Petal.Water:       return RGB( 26, 187, 214);
            case Petal.Energy:      return RGB(255, 205,   7);
            case Petal.Equity:      return RGB(131, 166,  62);
            case Petal.Health:      return RGB(144, 215, 233);
            case Petal.Beauty:      return RGB(139,  91, 132);
            case Petal.Materials:   return RGB(242, 156,  54);
            default:                return Color.black;
        }
    }

    // Gets the description that explains the given petal.

    public static string PetalDescription(Petal petal)
    {
        switch (petal)
        {
            case Petal.Site:        return "to realign how people understand and relate to the natural environment that sustains us";
            case Petal.Water:       return "to realign how people use water and to redefine 'waste' in the built environment so that water is respected as a precious resource";
            case Petal.Energy:      return "to signal a new age of design, wherein the built environment relies solely on renewable forms of energy and operates year round in a safe, pollution-free manner";
            case Petal.Equity:      return "to transform developments to foster a true, inclusive sense of community that is just and equitable regardless of an individual’s background, age, class, race, gender or sexual orientation";
            case Petal.Health:      return "to focus on the most important environmental conditions that must be present to create robust, healthy spaces";
            case Petal.Beauty:      return "to recognize the need for beauty as a precursor to caring enough to preserve, conserve, and serve the greater good";
            case Petal.Materials:   return "to help create a materials economy that is non-toxic, ecologically restorative, transparent, and socially equitable";
            default:                return "";
        }
    }

    private static Color RGB(int r, int g, int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }
}