using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Text;

public class FontMakerWizard : ScriptableWizard
{
    public TextAsset xmlFile;
    public int leftPadding;
    public int rightPadding;
    public int topPadding;
    public int bottomPadding;
    public int advanceOffset;

    class Glyph
    {
        public char code;
        public Rect bound;
        public Rect interiorBound;
        public int baseline;
        public int spacingA;
        public int spacingB;
        public int spacingC;
    }

    private int lineHeight;
    private int baseline;
    private int textureWidth;
    private int textureHeight;
    private int glyphWidth;
    private int glyphHeight;
    private string texturePath;

    private List<Glyph> glyphList;

    //[MenuItem("Tools/Font/Convert FontMaker")]
    static void CreateWindow()
    {
        ScriptableWizard.DisplayWizard<FontMakerWizard>("Convert FontMaker Config File", "Convert");
    }

    void OnWizardUpdate()
    {
        helpString = "Specify font config file";

        isValid = (xmlFile != null);
    }

    void OnWizardCreate()
    {
        LoadGlyph();
        ExportBMFont();
    }

    void ExportBMFont()
    {
        StringBuilder sb = new StringBuilder();

        // common lineHeight=64 base=51 scaleW=512 scaleH=512 pages=1
        sb.AppendFormat("common lineHeight={0} base={1} scaleW={2} scaleH={3} pages=1", lineHeight, baseline, textureWidth, textureHeight);
        sb.AppendLine();

        // page id=0 file="textureName.png"
        string path = Path.GetFileName(texturePath);
        sb.AppendFormat("page id=0 file=\"{0}\"", path);
        sb.AppendLine();

        // char id=13 x=506 y=62 width=3 height=3 xoffset=-1 yoffset=50 xadvance=0 page=0 chnl=15
        foreach (var glyph in glyphList)
        {
            int x = (int)(glyph.bound.x + glyph.interiorBound.x) - leftPadding;
            int y = (int)(glyph.bound.y + glyph.interiorBound.y) - topPadding;
            int w = (int)glyph.interiorBound.width + (rightPadding + leftPadding);
            int h = (int)glyph.interiorBound.height + (topPadding + bottomPadding);

            int xOffset = glyph.spacingA;
            int yOffset = (int)glyph.interiorBound.y - topPadding;
            int xAdvance = glyph.spacingA + glyph.spacingB + glyph.spacingC + advanceOffset;

            sb.AppendFormat("char id={0} x={1} y={2} width={3} height={4} xoffset={5} yoffset={6} xadvance={7} page=0 chnl=15",
                (int)glyph.code, x, y, w, h, xOffset, yOffset, xAdvance);
            sb.AppendLine();
        }

        string xmlPath = AssetDatabase.GetAssetPath(xmlFile);
        string outputDirectory = Path.GetDirectoryName(xmlPath) + "/" +  Path.GetDirectoryName(texturePath);
        string fntPath = outputDirectory + "/" + Path.GetFileNameWithoutExtension(xmlPath) + ".fnt";

        Debug.Log("Write fnt file " + fntPath);

        // save fnt file
        File.WriteAllText(fntPath, sb.ToString(), Encoding.UTF8);

        // refresh database
        AssetDatabase.Refresh();
    }

    void LoadGlyph()
    {
        glyphList = new List<Glyph>();

        // load font config file
        using (Stream stream = new MemoryStream(xmlFile.bytes))
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                reader.MoveToContent();

                reader.ReadStartElement("fontconfig");
                {
                    // typeface
                    if (reader.Name == "font")
                    {
                        lineHeight = int.Parse(reader.GetAttribute("height"));

                        // move to next
                        reader.Read();
                    }

                    // texture info
                    if (reader.Name == "texture")
                    {
                        textureWidth = int.Parse(reader.GetAttribute("width"));
                        textureHeight = int.Parse(reader.GetAttribute("height"));

                        // move to next
                        reader.Read();
                    }

                    // texture info
                    reader.ReadToFollowing("size");
                    if (reader.Name == "size")
                    {
                        glyphWidth = int.Parse(reader.GetAttribute("width"));
                        glyphHeight = int.Parse(reader.GetAttribute("height"));

                        // move to next
                        reader.Read();
                    }

                    // images
                    reader.ReadToFollowing("images");
                    if (!reader.IsEmptyElement)
                    {
                        List<string> images = new List<string>();
                        reader.ReadStartElement("images");

                        if (reader.Name == "image")
                        {
                            do
                            {
                                string imagePath = reader.GetAttribute("path");
                                images.Add(imagePath);
                            }
                            while (reader.ReadToNextSibling("image"));
                        }
                        reader.ReadEndElement();

                        if (images.Count > 0)
                        {
                            // only handle one texture
                            texturePath = images[0];
                        }
                        else
                        {
                            Debug.LogError("image path missing");
                            return;
                        }
                    }
                    else
                    {
                        reader.Read();
                    }

                    // glyph
                    if (!reader.IsEmptyElement)
                    {
                        reader.ReadStartElement("glyphs");
                        if (reader.Name == "glyph")
                        {
                            do
                            {
                                char character = char.Parse(reader.GetAttribute("char"));
                                int pageIndex = int.Parse(reader.GetAttribute("page"));
                                int x = int.Parse(reader.GetAttribute("x"));
                                int y = int.Parse(reader.GetAttribute("y"));
                                int w = int.Parse(reader.GetAttribute("w"));
                                int h = int.Parse(reader.GetAttribute("h"));

                                int ix = int.Parse(reader.GetAttribute("ix"));
                                int iy = int.Parse(reader.GetAttribute("iy"));
                                int iw = int.Parse(reader.GetAttribute("iw"));
                                int ih = int.Parse(reader.GetAttribute("ih"));

                                Glyph glyph = new Glyph();
                                glyph.code = character;
                                glyph.bound = new Rect(x, y, w, h);
                                glyph.interiorBound = new Rect(ix, iy, iw, ih);
                                glyph.baseline = int.Parse(reader.GetAttribute("baseline"));
                                glyph.spacingA = int.Parse(reader.GetAttribute("spacingA"));
                                glyph.spacingB = int.Parse(reader.GetAttribute("spacingB"));
                                glyph.spacingC = int.Parse(reader.GetAttribute("spacingC"));

                                baseline = glyph.baseline;

                                glyphList.Add(glyph);
                            }
                            while (reader.ReadToNextSibling("glyph"));

                        }
                        reader.ReadEndElement();
                    }
                    else
                    {
                        reader.Read();
                    }
                }
                reader.ReadEndElement();
            }
        }
    }
}
