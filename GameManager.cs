using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // --- Recursos Globales y Tiempo ---
    [Header("Recursos y Tiempo")]
    public int catCoins = 0;
    public int diaActual = 1;
    public int horaActual = 8;
    [Space(10)]
    public int suministrosLeche = 25;
    public int suministrosAtunDeLujo = 25;

    // --- Configuración de Balance y Tiempo ---
    [Header("Configuración de Balance")]
    public float estresPorTareaSatisfecha = 10f;
    public int fansBaseParaIngresos = 10;
    public float segundosRealesPorHoraDeJuego = 1.0f;

    // --- Gestión de Gatos ---
    [Header("Equipo de Cat-fluencers")]
    public List<Cat> gatosReclutados = new List<Cat>();
    public List<Cat> gatosDisponiblesParaReclutar = new List<Cat>();

    // --- Listas y Variables del Sistema ---
    private List<Assignment> activeAssignments = new List<Assignment>();
    private float timer = 0f;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= segundosRealesPorHoraDeJuego)
        {
            timer = 0;
            horaActual++;

            if (horaActual >= 24)
            {
                horaActual = 0;
                diaActual++;
            }

            CheckForCompletedAssignments();
        }
    }

    // --- LÓGICA DE TAREAS ---

    public void StartPhotoSession(Cat selectedCat)
    {
        StartTask(selectedCat, TaskType.PhotoSession, 2);
    }

    public void StartVideoSession(Cat selectedCat)
    {
        StartTask(selectedCat, TaskType.VideoSession, 4);
    }

    private void StartTask(Cat cat, TaskType type, int duration)
    {
        int currentTotalHours = (diaActual * 24) + horaActual;
        int endTotalHours = currentTotalHours + duration;
        Assignment newAssignment = new Assignment(cat, type, endTotalHours);
        activeAssignments.Add(newAssignment);
        Debug.Log(cat.nombre + " ha empezado " + type + ". Terminará en el día " + (endTotalHours / 24) + " a las " + (endTotalHours % 24) + ":00.");
        UIManager.instance.CloseCatSelectionPanel();
    }

    void CheckForCompletedAssignments()
    {
        int currentTotalHours = (diaActual * 24) + horaActual;
        foreach (Assignment assignment in activeAssignments.ToList())
        {
            if (currentTotalHours >= assignment.endTotalHours)
            {
                CompleteAssignment(assignment);
            }
        }
    }

    void CompleteAssignment(Assignment assignment)
    {
        Cat cat = assignment.assignedCat;
        Debug.Log("¡" + cat.nombre + " ha terminado su tarea de " + assignment.taskType + "!");

        if (assignment.taskType == TaskType.PhotoSession)
        {
            int fansGained = (cat.nivelPosado * 8);
            cat.fansDePuntuacion += fansGained;
            Debug.Log(cat.nombre + " ha ganado " + fansGained + " fans de Puntuación.");
        }
        else if (assignment.taskType == TaskType.VideoSession)
        {
            float coinsGained = CalcularIngresosPorHora(cat) * 4;
            catCoins += (int)coinsGained;
            Debug.Log(cat.nombre + " ha ganado " + (int)coinsGained + " Cat-Coins.");
        }

        activeAssignments.Remove(assignment);
    }

    // --- FUNCIONES DE SOPORTE ---

    public bool IsCatBusy(Cat catToCheck)
    {
        return activeAssignments.Any(assignment => assignment.assignedCat == catToCheck);
    }

    public int GetTotalFansDePuntuacion()
    {
        return gatosReclutados.Sum(gato => gato.fansDePuntuacion);
    }

    public float CalcularIngresosPorHora(Cat gato)
    {
        int sumaFansIngresos = gato.fansPorMoneria.Sum(data => data.fansParaIngresos);
        float ingresos = (sumaFansIngresos * gato.nivelCarisma) * 0.25f;
        return ingresos;
    }

    public void ReclutarGato(Cat gatoAReclutar)
    {
        if (gatosDisponiblesParaReclutar.Contains(gatoAReclutar))
        {
            gatosDisponiblesParaReclutar.Remove(gatoAReclutar);
            gatosReclutados.Add(gatoAReclutar);
            InicializarFansDeIngresos(gatoAReclutar);
            Debug.Log("Has reclutado a: " + gatoAReclutar.nombre);
        }
    }

    private void InicializarFansDeIngresos(Cat gato)
    {
        gato.fansPorMoneria.Clear();
        int numMonerias = gato.moneriasInnatas.Count;
        if (numMonerias == 0) return;
        int fansPorMoneria = fansBaseParaIngresos / numMonerias;
        foreach (MoneriaType moneria in gato.moneriasInnatas)
        {
            gato.fansPorMoneria.Add(new MoneriaData(moneria, fansPorMoneria));
        }
    }

    public float CalcularGananciaDeEstres(Cat gato)
    {
        bool necesitaLeche = gato.maniaPlatitoDeLeche > Cat.NivelMania.Never;
        bool necesitaAtun = gato.maniaLataDeAtun > Cat.NivelMania.Never;
        if (!necesitaLeche && !necesitaAtun) { return estresPorTareaSatisfecha; }
        bool lecheSatisfecha = !necesitaLeche || suministrosLeche > 0;
        bool atunSatisfecho = !necesitaAtun || suministrosAtunDeLujo > 0;
        if (lecheSatisfecha && atunSatisfecho) { return estresPorTareaSatisfecha; }
        else { return gato.gananciaEstresPorPenalizacion; }
    }

    public void AplicarBonusDePromocion(MoneriaType moneriaPromocionada, int fansGanados)
    {
        // Lógica futura
    }
}