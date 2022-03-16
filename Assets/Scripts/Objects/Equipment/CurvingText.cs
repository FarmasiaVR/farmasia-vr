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
using System.Diagnostics;

public class CurvingText : MonoBehaviour
{
    public Material Material;
    public string Text;

    private static readonly int w = 512;
    private static readonly int h = 512;
    private Image<Rgb24> image = new Image<Rgb24>(w, h);
    private byte[] pixelData = new byte[w * h * 3];

    private Texture2D tex;

    protected void Start() {
        tex = new Texture2D(w, h, TextureFormat.RGB24, false);
        StartCoroutine(Corr());
    }

    protected IEnumerator Corr() {
        while (true) {
            
            RunTask();

            yield return new WaitForSeconds(2);
        }
    }

    private async void RunTask() {
        var start = DateTime.UtcNow;

        await TextToImage.GenerateTextData(Text, image, pixelData);

        tex.SetPixelData(pixelData, 0, 0);
        tex.Apply();

        Material newMat = new Material(Material);
        newMat.SetTexture("_MainTex", tex);
        GetComponent<Renderer>().sharedMaterial = newMat;
        Text += "A";

        Logger.Print("Total: " + (DateTime.UtcNow - start).Milliseconds + " ms");
    }
}

static class TextToImage {

    static FontFamily fontFamily = SystemFonts.Get("Arial");
    static Font font = new Font(fontFamily, 16);
    static TextOptions textOptions = new TextOptions(font);

    public static System.Threading.Tasks.Task GenerateTextData(string text, Image<Rgb24> image, byte[] pixelData) {
        return System.Threading.Tasks.Task.Factory.StartNew(() => {

            CreateImage(text, image, pixelData);

        });
    }

    private static void CreateImage(string text, Image<Rgb24> image, in byte[] pixelData) {    
        IPathCollection glyphs = TextBuilder.GenerateGlyphs(text, textOptions);
        glyphs.Draw(image);

        Span<byte> pixelDataSpan = new Span<byte>(pixelData);

        image.CopyPixelDataTo(pixelDataSpan);

        System.Threading.Tasks.Task.Factory.StartNew(() => {
            Clear(image);
        });
    }

    private static void Clear(Image<Rgb24> image) {
        image.Mutate(img => img.Clear(Color.White));
    }

    private static void Draw(this IPathCollection shape, Image<Rgb24> image) {
        shape = shape.Translate(-shape.Bounds.Location).Translate(new Vector2(100, 200));

        foreach (IPath s in shape) {
            image.Mutate(i => i.Fill(Color.Black, s));
        }
    }
}