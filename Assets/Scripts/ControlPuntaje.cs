using UnityEngine;
using UnityEngine.UI;

public class ControlPuntaje : MonoBehaviour
{
    
    public static ControlPuntaje Instancia { get; private set; }

    [Header("Interfaz (opcional)")]
    
    [SerializeField] private Text textoMonedas;
    [SerializeField] private Text textoPuntaje;

    [Header("Distancia")]
   
    [SerializeField] private bool elMundoSeMueve = true;
    [SerializeField] private float velocidadMundo = 4f;
    [SerializeField] private Transform jugador;

    [Header("Puntaje")]
    [SerializeField] private float puntosPorMetro = 1f;

    private int monedas = 0;
    private float distancia = 0f;
    private float ultimaZJugador;

    private int monedasMostradas = -1;
    private int puntajeMostrado = -1;

    private void Awake()
    {
        Instancia = this;

        monedas = 0;
        distancia = 0f;

        if (textoMonedas == null || textoPuntaje == null)
            CrearInterfaz();
    }

    private void Start()
    {
        if (!elMundoSeMueve)
        {
            if (jugador == null)
            {
                ControlJugador control = FindObjectOfType<ControlJugador>();

                if (control != null)
                    jugador = control.transform;
            }

            if (jugador != null)
                ultimaZJugador = jugador.position.z;
        }

        ActualizarTextos();
    }

    private void OnDestroy()
    {
        if (Instancia == this)
            Instancia = null;
    }

    private void Update()
    {
       
        if (elMundoSeMueve)
        {
            distancia += velocidadMundo * Time.deltaTime;
        }
        else if (jugador != null)
        {
            float avance = jugador.position.z - ultimaZJugador;

            if (avance > 0f)
                distancia += avance;

            ultimaZJugador = jugador.position.z;
        }

        ActualizarTextos();
    }

    public void AgregarMoneda()
    {
        monedas++;
        ActualizarTextos();
    }

    private void ActualizarTextos()
    {
        int puntaje = Mathf.FloorToInt(distancia * puntosPorMetro);

        if (monedas != monedasMostradas && textoMonedas != null)
        {
            textoMonedas.text = "Monedas: " + monedas;
            monedasMostradas = monedas;
        }

        if (puntaje != puntajeMostrado && textoPuntaje != null)
        {
            textoPuntaje.text = "Puntaje: " + puntaje;
            puntajeMostrado = puntaje;
        }
    }

   

    private void CrearInterfaz()
    {
        GameObject canvasObj = new GameObject("CanvasPuntaje");

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler escalador = canvasObj.AddComponent<CanvasScaler>();
        escalador.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escalador.referenceResolution = new Vector2(1920f, 1080f);
        escalador.matchWidthOrHeight = 0.5f;

        if (textoMonedas == null)
        {
            textoMonedas = CrearTexto(
                "TextoMonedas",
                canvasObj.transform,
                new Vector2(0f, 1f),          
                new Vector2(40f, -30f),
                TextAnchor.UpperLeft
            );
        }

        if (textoPuntaje == null)
        {
            textoPuntaje = CrearTexto(
                "TextoPuntaje",
                canvasObj.transform,
                new Vector2(1f, 1f),         
                new Vector2(-40f, -30f),
                TextAnchor.UpperRight
            );
        }
    }

    private Text CrearTexto(string nombre, Transform padre, Vector2 ancla,
                            Vector2 posicion, TextAnchor alineacion)
    {
        GameObject obj = new GameObject(nombre);
        obj.transform.SetParent(padre, false);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = ancla;
        rect.anchorMax = ancla;
        rect.pivot = ancla;
        rect.anchoredPosition = posicion;
        rect.sizeDelta = new Vector2(600f, 80f);

        Text texto = obj.AddComponent<Text>();
        texto.font = Font.CreateDynamicFontFromOSFont(
            new string[] { "Arial", "Helvetica" }, 48);
        texto.fontSize = 48;
        texto.fontStyle = FontStyle.Bold;
        texto.alignment = alineacion;
        texto.color = Color.white;
        texto.horizontalOverflow = HorizontalWrapMode.Overflow;
        texto.verticalOverflow = VerticalWrapMode.Overflow;
        texto.raycastTarget = false;

        Outline contorno = obj.AddComponent<Outline>();
        contorno.effectColor = Color.black;
        contorno.effectDistance = new Vector2(2f, -2f);

        return texto;
    }
}