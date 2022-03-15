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
    private Mutex mutex = new Mutex();

    protected void Start() {
        StartCoroutine(Corr());
    }

    protected IEnumerator Corr() {
        while (true) {
            var thread = new System.Threading.Thread(new ThreadStart(MutexCheck));
            thread.Start();
            //TextToImage.CreateImage(Text, Material.name);
            //Material.SetTexture("_MainTex", tex);
            Text += "A";
            yield return new WaitForSeconds(1);
        }
    }

    private void MutexCheck() {
        if (mutex.WaitOne()) {
            TextToImage.CreateImage(Text, Material.name);
            mutex.ReleaseMutex();
        }
    }
}

static class TextToImage
{
    public static void CreateImage(string text, string name) {
        FontFamily fontFamily = SystemFonts.Get("Arial");
        var font = new Font(fontFamily, 32);
        TextOptions textOptions = new TextOptions(font);
        IPathCollection glyphs = TextBuilder.GenerateGlyphs(text, textOptions);

        glyphs.SaveImage(name);
    }

    public static void SaveImage(this IPathCollection shape, string name) {
        shape = shape.Translate(-shape.Bounds.Location).Translate(new Vector2(300, 400));

        string path = IOPath.GetFullPath(IOPath.Combine(
            "Assets", IOPath.Combine("Resources", IOPath.Combine("Textures", $"{name}.png")))
        );

        using (var img = new Image<Rgba32>(1024, 1024)) {
            img.Mutate(i => i.Fill(Color.White));

            foreach (IPath s in shape) {
                img.Mutate(i => i.Fill(Color.Black, s));
            }
            IODirectory.CreateDirectory(IOPath.GetDirectoryName(path));

            img.Save(path);
        }
    }
}