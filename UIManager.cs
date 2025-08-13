using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Barra de Recursos Superior")]
    public TextMeshProUGUI catCoinsText, dayText, fansText, atunText, lecheText, horaText;

    [Header("Paneles de Interfaz")]
    public GameObject catSelectionPanel;

    [Header("Plantillas y Contenedores")]
    public GameObject catButtonPrefab;
    public Transform catButtonContainer;

    private TaskType currentTaskToAssign;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Update()
    {
        if (GameManager.instance != null)
        {
            catCoinsText.text = "Cat-Coins: " + GameManager.instance.catCoins.ToString("D7");
            dayText.text = "Día: " + GameManager.instance.diaActual.ToString();
            fansText.text = "Fans: " + GameManager.instance.GetTotalFansDePuntuacion().ToString("D7");
            atunText.text = "Latas de atún: " + GameManager.instance.suministrosAtunDeLujo.ToString();
            lecheText.text = "Brick de leche: " + GameManager.instance.suministrosLeche.ToString();
            horaText.text = "Hora: " + GameManager.instance.horaActual.ToString("D2") + ":00";
        }
    }

    public void OpenCatSelectionPanel(TaskType taskType)
    {
        currentTaskToAssign = taskType;
        if (catSelectionPanel != null)
        {
            catSelectionPanel.SetActive(true);
            PopulateCatSelectionPanel();
        }
    }

    public void CloseCatSelectionPanel()
    {
        if (catSelectionPanel != null)
        {
            catSelectionPanel.SetActive(false);
        }
    }

    void PopulateCatSelectionPanel()
    {
        foreach (Transform child in catButtonContainer) { Destroy(child.gameObject); }

        foreach (Cat gato in GameManager.instance.gatosReclutados)
        {
            if (!GameManager.instance.IsCatBusy(gato))
            {
                GameObject buttonGO = Instantiate(catButtonPrefab, catButtonContainer);
                buttonGO.GetComponent<CatButton>().catData = gato;
                buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = gato.nombre;

                buttonGO.GetComponent<Button>().onClick.AddListener(() => {
                    if (currentTaskToAssign == TaskType.PhotoSession)
                    {
                        GameManager.instance.StartPhotoSession(buttonGO.GetComponent<CatButton>().catData);
                    }
                    else if (currentTaskToAssign == TaskType.VideoSession)
                    {
                        GameManager.instance.StartVideoSession(buttonGO.GetComponent<CatButton>().catData);
                    }
                });
            }
        }
    }

    // --- FUNCIÓN DE TEST DENTRO DE LA CLASE ---
    public void TestFunction()
    {
        Debug.Log("EL TEST FUNCIONA!");
    }

} 