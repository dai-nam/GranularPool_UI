
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AudioclipLoader : MonoBehaviour
{
    public int width;
    public int height;
    Color waveformColor = Color.black;
    Color bgColor = Color.white;
    public float sat = .5f;
    public int rectWidth, rectHeight;

    Image img;
    [SerializeField] AudioClip clip;

    private void Awake()
    {
        img = GetComponentInParent<Image>();
        bgColor = img.color;
        Vector3[] corners = new Vector3[4];
        transform.parent.gameObject.GetComponent<RectTransform>().GetWorldCorners(corners); //lu, lo, ro, ru
        rectWidth = (int)(corners[2].x - corners[1].x);
        rectHeight = (int) (corners[1].y - corners[0].y);
    }

    void Start()
    {
        //  transform.GetComponent<RectTransform>().rect.Set(transform.parent.position.x, transform.parent.position.y, rectWidth, rectHeight);
        transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectWidth);
        transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectHeight);
        DrawWaveform();
    }

    private void DrawWaveform()
    {
        Texture2D texture = PaintWaveformSpectrum(clip, sat,  width, height, waveformColor, bgColor);
        img.overrideSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public Texture2D PaintWaveformSpectrum(AudioClip audio, float saturation, int width, int height, Color col, Color bgColor)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        float[] samples = new float[audio.samples];
        float[] waveform = new float[width];
        audio.GetData(samples, 0);
        int packSize = (audio.samples / width) + 1;
        int s = 0;
        for (int i = 0; i < audio.samples; i += packSize)
        {
            waveform[s] = Mathf.Abs(samples[i]);
            s++;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, bgColor);
            }
        }

        for (int x = 0; x < waveform.Length; x++)
        {
            for (int y = 0; y <= waveform[x] * ((float)height * .75f); y++)
            {
                tex.SetPixel(x, (height / 2) + y, col);
                tex.SetPixel(x, (height / 2) - y, col);
            }
        }
        tex.Apply();

        return tex;
    }

    private void OnValidate()
    {
       // DrawWaveform();
    }

}
