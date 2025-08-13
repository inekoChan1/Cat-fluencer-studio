using System.Collections.Generic;
using UnityEngine;

public enum MoneriaType
{
    PanzaArriba, Maullido, RonroneoIntenso, Amasar, DormirAlSol, EstiramientoElegante, TirarCosas, Blep,
    Caja, Jugueton, Disfraz, Caza, Guardian, Chapoteo
}

[System.Serializable]
public class Cat
{
    public string nombre;
    public string raza;

    [Header("Atributos del Gato")]
    public List<MoneriaType> moneriasInnatas = new List<MoneriaType>();

    [Header("Fans y Progreso")]
    public int fansDePuntuacion = 0;
    public List<MoneriaData> fansPorMoneria = new List<MoneriaData>();

    [Header("Niveles de Atributo")]
    public int nivelCarisma = 1;
    public int nivelPosado = 1;

    [Header("Estadísticas de Estado")]
    public float estres = 0f;
    public float gananciaEstresPorPenalizacion = 15f;

    public enum NivelMania { Never, Sometimes, Frequent, Addicted }
    [Header("Manías y Coste")]
    public NivelMania maniaPlatitoDeLeche;
    public NivelMania maniaLataDeAtun;
    public int costeDiario;
}