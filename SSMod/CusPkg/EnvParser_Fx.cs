using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes;
using UAssetAPI;

namespace SSMod.CusPkg
{
    internal class EnvParser_Fx
    {
        public List<string> ufiles = new List<string>() {
            "LS_Sng026_Common_Fx.uasset",
        };
        public void parse(string source_folder, string target_folder, JObject json_obj)
        {
            ParseTool parse_tool = new ParseTool();
            foreach (var ufile in ufiles)
            {
                //Console.WriteLine("Processsing:" + ufile);
                var source_file = source_folder + "\\" + ufile;
                var target_file = target_folder + "\\" + ufile;

                var myAsset = new UAsset(source_file, EngineVersion.VER_UE4_24);

                var layer1s = parse_tool.get_layer1_tracks(ref myAsset);

                foreach (var layer1 in layer1s)
                {
                    //Console.WriteLine(layer1.Item1);
                    var layer1_ = layer1.Item2;
                    if (layer1.Item1 == "PS_Eff_PaperShower")
                    {
                        List<string> layer2_names = parse_tool.get_layer2_names(ref myAsset, ref layer1_);
                        foreach (var e in layer2_names)
                        {
                            //Console.WriteLine($"\tLayer1-{layer1.Item1},{e}");
                        }
                        var curve_name = "ParticleSections";
                        var layer2_exports = parse_tool.get_scalar_layer2_exports(ref myAsset, layer1.Item1, layer1.Item2, curve_name, "ParticleSections",false);
                        foreach (var layer2_export in layer2_exports)
                        {
                            //parse_tool.get_scalar_layer2_curves(layer1.Item1, "color", layer2_export,"")
                            var layer2_curve_names = parse_tool.get_layer2_curve_names(layer1.Item1, curve_name, layer2_export);
                            foreach (var e in layer2_curve_names)
                            {
                                //Console.WriteLine($"\t\t{e}");
                            }
                            var rr = parse_tool.get_scalar_layer2_curves(layer1.Item1, layer2_export.ObjectName.ToString(), layer2_export, "ParticleKeys");
                         }
                    }
                }
                Console.WriteLine("Write target_file:" + target_file);
            }
        }
    }
}
