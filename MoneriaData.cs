[System.Serializable]
public class MoneriaData
{
    public MoneriaType moneria;
    public int fansParaIngresos;

    public MoneriaData(MoneriaType tipo, int fans)
    {
        moneria = tipo;
        fansParaIngresos = fans;
    }
}