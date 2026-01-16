using HSDRaw;
using HSDRaw.Melee;
using HSDRawViewer.Tools;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HSDRawViewer.Extensions;

namespace HSDRawViewer.ContextMenus
{
    public class SisNodeContextMenu : CommonContextMenu
    {
        public override Type[] SupportedTypes { get; } = new Type[] { typeof(SIS_SdData) };

        public class SisJson
        {
            public string Text
            {
                get => TextCode;
                set => TextCode = value;
            }

            [JsonIgnore]
            private string TextCode;
        }

        public SisNodeContextMenu() : base()
        {
            ToolStripMenuItem export = new("Export to Text");
            export.Click += (sender, args) =>
            {
                if (MainForm.SelectedDataNode.Accessor is SIS_SdData root)
                {
                    string file = Tools.FileIO.SaveFile("Text (*.txt)|*.txt");

                    if (file != null)
                    {
                        File.WriteAllText(file, JsonSerializer.Serialize(root.SISData.Select(e => new SisJson() { Text = e.TextCode }),
                            new JsonSerializerOptions()
                            {
                                WriteIndented = true
                            }));
                    }
                }
            };
            Items.Add(export);


            ToolStripMenuItem export_font = new("Export Character Data");
            export_font.Click += (sender, args) =>
            {
                if (MainForm.SelectedDataNode.Accessor is SIS_SdData root && root.Images != null)
                {
                    string file = Tools.FileIO.OpenFolder();

                    if (file != null)
                    {
                        var images = root.Images.Array;
                        var spacing = root.SpacingParams.Array;

                        for (int i = 0; i < images.Length; i++)
                        {
                            images[i].ToTObj().SaveImagePNG(Path.Combine(file, $"{i.ToString("D3")}_{spacing[i].Before}_{spacing[i].After}.png"));
                        }
                    }
                }
            };
            Items.Add(export_font);

            ToolStripMenuItem import_font = new("Import From CBFG");
            import_font.Click += (sender, args) =>
            {
                if (MainForm.SelectedDataNode.Accessor is SIS_SdData root && root.Images != null)
                {
                    string imgFile = FileIO.OpenFile("Bitmap (*.bmp)|*.bmp");
                    if (string.IsNullOrEmpty(imgFile))
                        return;

                    string fontFile = FileIO.OpenFile("Font Data (*.dat)|*.dat");
                    if (string.IsNullOrEmpty(fontFile))
                        return;

                    using (var s = new FileStream(fontFile, FileMode.Open))
                    using (var d = new BinaryReaderExt(s))
                    {
                        d.BigEndian = false;

                        var sheet_width = d.ReadInt32();
                        var sheet_height = d.ReadInt32();
                        var char_width = d.ReadInt32();
                        var char_height = d.ReadInt32();

                        if (char_width != 32 || char_height != 32)
                        {
                            MessageBox.Show("Only 32x32 character sizes are supported", "Invalid Character Sizes", MessageBoxButtons.OK);
                            return;
                        }

                        var stride = sheet_width / char_width;
                        var start_char = d.ReadChar();
                        d.Skip(start_char);
                        var chars = d.ReadBytes((int)(s.Length - d.Position));

                        // get spacing params
                        root.SpacingParams = new HSDArrayAccessor<SIS_Spacing>()
                        {
                            Array = chars.Select(e => new SIS_Spacing() { Before = 0, After = (byte)(32 - e) }).ToArray()
                        };

                        // extract and convert image data
                        using var image = Image.Load<Bgra32>(imgFile);
                        image.RemapChannels(GUI.Dialog.ChannelType.MIX, GUI.Dialog.ChannelType.MIX);
                        root.Images = new HSDArrayAccessor<SIS_Character>()
                        {
                            Array = chars.Select((e, i) =>
                            {
                                int x = i % stride;
                                int y = i / stride;

                                using var im = image.Clone(ctx =>
                                    ctx.Crop(new Rectangle(x * char_width, y * char_height, char_width, char_height))
                                );

                                var tobj = TOBJExtentions.ToTObj(im, HSDRaw.GX.GXTexFmt.I4, HSDRaw.GX.GXTlutFmt.RGB565);
                                var c = new SIS_Character();
                                c.FromTObj(tobj);

                                return c;
                            }).ToArray()
                        };
                    }

                    //foreach (var f in Directory.GetFiles(file))
                    //{
                    //    var fname = Path.GetFileName(f);

                    //    if (TryParseFilename(fname, out int index, out int before, out int after))
                    //    {

                    //    }
                    //}
                    //var images = root.Images.Array;
                    //var spacing = root.SpacingParams.Array;

                    //for (int i = 0; i < images.Length; i++)
                    //{
                    //    images[i].ToTObj().SaveImagePNG(Path.Combine(file, $"{i.ToString("D3")}_{spacing[i].Before}_{spacing[i].After}.png"));
                    //}
                }
            };
            Items.Add(import_font);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        bool TryParseFilename(string filename, out int a, out int b, out int c)
        {
            a = b = c = 0;

            var match = Regex.Match(
                filename,
                @"^(\d+)_(\d+)_(\d+)\.png$",
                RegexOptions.IgnoreCase
            );

            if (!match.Success)
                return false;

            a = int.Parse(match.Groups[1].Value);
            b = int.Parse(match.Groups[2].Value);
            c = int.Parse(match.Groups[3].Value);

            return true;
        }
    }
}
