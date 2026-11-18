using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[DefaultExecutionOrder(100)]
public class ControlInicio : MonoBehaviour
{

    public static bool EsperandoInicio { get; private set; }

   
    private static bool reinicioDirecto = false;

    public static void MarcarReinicioDirecto()
    {
        reinicioDirecto = true;
    }

    [Header("Textos")]
    [SerializeField] private string titulo = "Controles";
    [SerializeField, TextArea(3, 6)] private string instrucciones =
        "ESPACIO  -  Saltar\nF  -  Disparar\nA / D  -  Moverse";
    [SerializeField] private string textoBoton = "JUGAR";

    [Header("Teclas que inician el juego")]
    [SerializeField] private KeyCode[] teclasParaIniciar =
    {
        KeyCode.Space, KeyCode.F, KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D
    };

    private GameObject canvasInicio;
    private ControlJugador controlJugador;
    private SaltoJugador saltoJugador;

    private void Awake()
    {
        if (reinicioDirecto)
        {
            
            reinicioDirecto = false;
            EsperandoInicio = false;
            Time.timeScale = 1f;
            enabled = false;
            return;
        }

        EsperandoInicio = true;
        Time.timeScale = 0f;

        

        AsegurarEventSystem();
        CrearPantalla();
    }

    private void Start()
    {
        
        Time.timeScale = 0f;

       
        controlJugador = FindObjectOfType<ControlJugador>();
        saltoJugador = FindObjectOfType<SaltoJugador>();

        if (controlJugador != null) controlJugador.enabled = false;
        if (saltoJugador != null) saltoJugador.enabled = false;
    }

    private void OnDestroy()
    {
        EsperandoInicio = false;
    }

    private void Update()
    {
        if (!EsperandoInicio) return;

 
        Time.timeScale = 0f;

        foreach (KeyCode tecla in teclasParaIniciar)
        {
            if (Input.GetKeyDown(tecla))
            {
                Iniciar();
                return;
            }
        }
    }

    private void Iniciar()
    {
        if (!EsperandoInicio) return;

        EsperandoInicio = false;

        if (canvasInicio != null)
            Destroy(canvasInicio);

        Time.timeScale = 1f;

        StartCoroutine(ActivarJugador());
    }

    private IEnumerator ActivarJugador()
    {
        // Un frame de espera para que la tecla que inicio el juego
        // no cuente tambien como salto o disparo
        yield return null;

        if (controlJugador != null) controlJugador.enabled = true;
        if (saltoJugador != null) saltoJugador.enabled = true;
    }

    // ---------- Interfaz creada por codigo ----------

    private void AsegurarEventSystem()
    {
        if (FindObjectOfType<EventSystem>() != null) return;

        GameObject es = new GameObject("EventSystem");
        es.AddComponent<EventSystem>();
        es.AddComponent<StandaloneInputModule>();
    }

    private void CrearPantalla()
    {
        canvasInicio = new GameObject("CanvasInicio");

        Canvas canvas = canvasInicio.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler escalador = canvasInicio.AddComponent<CanvasScaler>();
        escalador.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escalador.referenceResolution = new Vector2(1920f, 1080f);
        escalador.matchWidthOrHeight = 0.5f;

        canvasInicio.AddComponent<GraphicRaycaster>();

        // Fondo oscuro que cubre toda la pantalla
        GameObject fondo = new GameObject("Fondo", typeof(RectTransform));
        fondo.transform.SetParent(canvasInicio.transform, false);

        RectTransform rectFondo = fondo.GetComponent<RectTransform>();
        rectFondo.anchorMin = Vector2.zero;
        rectFondo.anchorMax = Vector2.one;
        rectFondo.offsetMin = Vector2.zero;
        rectFondo.offsetMax = Vector2.zero;

        Image imagenFondo = fondo.AddComponent<Image>();
        imagenFondo.color = new Color(0f, 0f, 0f, 0.75f);

        CrearTexto("Titulo", fondo.transform, titulo, 84, FontStyle.Bold,
                   new Vector2(0f, 300f), new Vector2(1600f, 130f));

        CrearTexto("Instrucciones", fondo.transform, instrucciones, 52, FontStyle.Normal,
                   new Vector2(0f, 40f), new Vector2(1200f, 320f));

        CrearBoton(fondo.transform, textoBoton, new Vector2(0f, -230f));

        CrearTexto("Pista", fondo.transform, "o presiona cualquier tecla de juego", 34,
                   FontStyle.Italic, new Vector2(0f, -340f), new Vector2(1200f, 60f));
    }

    private Text CrearTexto(string nombre, Transform padre, string contenido, int tamano,
                            FontStyle estilo, Vector2 posicion, Vector2 tamanoRect)
    {
        GameObject obj = new GameObject(nombre, typeof(RectTransform));
        obj.transform.SetParent(padre, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = posicion;
        rect.sizeDelta = tamanoRect;

        Text texto = obj.AddComponent<Text>();
        texto.text = contenido;
        texto.font = Font.CreateDynamicFontFromOSFont(new string[] { "Arial", "Helvetica" }, tamano);
        texto.fontSize = tamano;
        texto.fontStyle = estilo;
        texto.alignment = TextAnchor.MiddleCenter;
        texto.lineSpacing = 1.3f;
        texto.color = Color.white;
        texto.horizontalOverflow = HorizontalWrapMode.Overflow;
        texto.verticalOverflow = VerticalWrapMode.Overflow;
        texto.raycastTarget = false;

        return texto;
    }

    private void CrearBoton(Transform padre, string contenido, Vector2 posicion)
    {
        GameObject obj = new GameObject("BotonJugar", typeof(RectTransform));
        obj.transform.SetParent(padre, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = posicion;
        rect.sizeDelta = new Vector2(360f, 100f);

        Image imagen = obj.AddComponent<Image>();
        imagen.color = new Color(0.2f, 0.7f, 0.3f);

        Button boton = obj.AddComponent<Button>();
        boton.targetGraphic = imagen;
        boton.onClick.AddListener(Iniciar);

        GameObject textoObj = new GameObject("Texto", typeof(RectTransform));
        textoObj.transform.SetParent(obj.transform, false);

        RectTransform rectTexto = textoObj.GetComponent<RectTransform>();
        rectTexto.anchorMin = Vector2.zero;
        rectTexto.anchorMax = Vector2.one;
        rectTexto.offsetMin = Vector2.zero;
        rectTexto.offsetMax = Vector2.zero;

        Text texto = textoObj.AddComponent<Text>();
        texto.text = contenido;
        texto.font = Font.CreateDynamicFontFromOSFont(new string[] { "Arial", "Helvetica" }, 56);
        texto.fontSize = 56;
        texto.fontStyle = FontStyle.Bold;
        texto.alignment = TextAnchor.MiddleCenter;
        texto.color = Color.white;
        texto.raycastTarget = false;
    }
}