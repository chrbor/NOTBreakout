using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FoliageShaderScript : MonoBehaviour
{
    //[Tooltip("material zum update der rendertextur, welche die geschwindigkeit angibt")]
    //public Material velMat;
    [Tooltip("material zum update der rtex, welche die verschiebung angibt")]
    public Material mat;
    private Material renderMat;

    public CustomRenderTexture rTex;
    [Tooltip("Länge des Rechts-shift für die größe der Rendertextur"), Range(0, 3)]
    public int scaleReduce = 0;

    [Header("Eigenschaften:")]
    public Color defaultColor = new Color(.5f, .5f, .5f, .5f);
    public float stiffness = .1f, damping = .02f, speedDown = 20;
    [Header("Input:")]
    public float strength = .1f;
    public Vector2 inputScale = Vector2.one;
    [Header("Wind:"), Tooltip("xy: windpower, z: windfrequenz, w: noise-scale")]
    public Vector4 wind = new Vector4(0,0,1,5);

    int objDataID, playerDataID;


    // Start is called before the first frame update
    void Awake()
    {
        SpriteRenderer srenderer = GetComponent<SpriteRenderer>();

        //Erstelle neue Instanz des Materials:
        mat = new Material(mat);
        objDataID = mat.shader.GetPropertyNameId(mat.shader.FindPropertyIndex("_ObjData"));
        playerDataID = mat.shader.GetPropertyNameId(mat.shader.FindPropertyIndex("_PlayerData"));
        
        //Erstelle neue Instanz des Materials des Spriterenderers:
        renderMat = new Material(srenderer.sharedMaterial);//anstatt srenderer.material
        srenderer.material = renderMat;
        mat.SetVector("_SpriteScale", srenderer.sprite.rect.size / srenderer.sprite.pixelsPerUnit * transform.localScale * (1 << scaleReduce));

        mat.SetFloat("_SpeedDown", speedDown);
        mat.SetFloat("_Damping", damping);
        mat.SetFloat("_Stiffness", stiffness);

        mat.SetVector("_Scale", inputScale);



        //Erstelle rendertexture
        Sprite sprite = srenderer.sprite;
        rTex = new CustomRenderTexture((int)sprite.rect.width >> scaleReduce, (int)sprite.rect.height >> scaleReduce, RenderTextureFormat.DefaultHDR);
        rTex.material = mat;
        rTex.initializationMode = CustomRenderTextureUpdateMode.OnLoad;
        rTex.initializationSource = CustomRenderTextureInitializationSource.TextureAndColor;
        rTex.initializationColor = defaultColor;
        rTex.updateMode = CustomRenderTextureUpdateMode.Realtime;
        //rTex.updateMode = CustomRenderTextureUpdateMode.OnLoad;

        rTex.doubleBuffered = true;
        rTex.Create();

        //Füge die rTex dem Material hinzu:
        renderMat.SetTexture("_DisplTex", rTex);
    }

    Vector3 prevPos;
    // Update is called once per frame
    void Update()
    {
        mat.SetVector("_Wind", wind);

        mat.SetVector(objDataID, new Vector4(transform.position.x, transform.position.y, 0,0));

        Vector3 mouseDiff = (Input.mousePosition - prevPos) * strength;
        //if (mouseDiff.sqrMagnitude > 1f) mouseDiff = mouseDiff.normalized * 1f;

        //Update der Spieler-daten:
        Vector2 worldPos = TouchAndScreen.PixelToWorld(Input.mousePosition);
        mat.SetVector(playerDataID, new Vector4(worldPos.x, worldPos.y, mouseDiff.x, mouseDiff.y));


        //Debug.Log((Input.mousePosition - prevPos) * strength + "\n" + TouchAndScreen.PixelToWorld(Input.mousePosition));
        prevPos = Input.mousePosition;
    }
}
