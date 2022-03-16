using UnityEngine;
using System;
using System.Collections;
using System.Threading;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using UnityEditor;
using Color = SixLabors.ImageSharp.Color;
using Font = SixLabors.Fonts.Font;
using FontStyle = SixLabors.Fonts.FontStyle;
using Vector2 = System.Numerics.Vector2;
using IOPath = System.IO.Path;
using IODirectory = System.IO.Directory;
using Object = System.Object;

public class CurvingText : MonoBehaviour
{
    public Material Material;
    public String Text;
    private Texture2D tex;

    protected void Start() {
        StartCoroutine(Corr());
    }

    protected IEnumerator Corr() {
        while (true) {
            var newTex = new Texture2D(1024, 1024);
            ApplyText(Text, newTex);
            Material newMat = new Material(Material);
            newMat.SetTexture("_MainTex", newTex);
            GetComponent<Renderer>().sharedMaterial = newMat;
            Text += "A";

            yield return new WaitForSeconds(2);
        }
    }

    private void ApplyText(String text, Texture2D newTex) {
        Image<Rgba32> image = new Image<Rgba32>(1024, 1024);
        TextToImage.CreateImage(text, image);
        int size = image.Width;

        image.ProcessPixelRows(accessor => {
            for (int y = 0; y < accessor.Height; y++) {
                Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                for (int x = 0; x < pixelRow.Length; x++) {
                    ref Rgba32 pixel = ref pixelRow[x];
                    newTex.SetPixel(x, y, pixel.B == 0 ? UnityEngine.Color.black : UnityEngine.Color.white);
                }
            }
        });

        newTex.Apply();
    }

}

static class TextToImage
{
    public static void CreateImage(string text, Image<Rgba32> image) {
        FontFamily fontFamily = SystemFonts.Get("Arial");
        var font = new Font(fontFamily, 32);
        TextOptions textOptions = new TextOptions(font);
        IPathCollection glyphs = TextBuilder.GenerateGlyphs(text, textOptions);

        glyphs.GetImage(image);
    }

    public static void GetImage(this IPathCollection shape, Image<Rgba32> image) {
        shape = shape.Translate(-shape.Bounds.Location).Translate(new Vector2(300, 400));

        image.Mutate(i => i.Fill(Color.White));

        foreach (IPath s in shape) {
            image.Mutate(i => i.Fill(Color.Black, s));
        }
    }
}