using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI;
using UAssetAPI.UnrealTypes;
using static System.Reflection.Metadata.BlobBuilder;

namespace SSMod.CusPkg
{
    internal class EnvParser_Other
    {
        public List<string> ufiles = new List<string>() {
            
            "LS_Sng026_Fine_Env.uasset",

            "LS_Sng026_Dusk_Env.uasset",
            "LS_Sng026_Night_Env.uasset",
            "LS_Sng026_Night_Num1_Env.uasset",
            "LS_Sng026_Night_Num2_Env.uasset",
            "LS_Sng026_Night_Num3_Env.uasset",
            "LS_Sng026_Night_Num4_Env.uasset",
        };


        public void parse(string source_folder, string target_folder, JObject json_obj)
        {
            foreach (var ufile in ufiles)
            {
                Console.WriteLine("Processsing:" + ufile);
                var source_file = source_folder + "\\" + ufile;
                var target_file = target_folder + "\\" + ufile;
                var myAsset = new UAsset(source_file, EngineVersion.VER_UE4_24);
                process_PinSpot(ref myAsset, json_obj);
                Console.WriteLine("Write target_file:" + target_file);
            }
        }

        void process_PinSpot(ref UAsset myAsset, JObject json_obj)
        {
            ParseTool parse_tool = new ParseTool();
            var layer1s = parse_tool.get_layer1_tracks(ref myAsset);
            List<string> Brightness = new List<string>() { "PostProcessVolume", };

            List<bool> is_fades = new List<bool>() { };
            var stage_colors = json_obj["stage_color"];
            foreach (var stage_color in stage_colors)
            {
                bool is_fade = (bool)stage_color["is_fade"];
                is_fades.Add(is_fade);
            }

            foreach (var layer1 in layer1s)
            {
                var name = layer1.Item1;
                if (Brightness.Contains(name))
                {
                    Console.WriteLine($"Is modifing :{name} .......{myAsset.FilePath}");
                    foreach (var adjust_name_ratio in new List<Tuple<string, float>> {
                        new Tuple<string,float>("AutoExposureMaxBrightness", 1f),new Tuple<string,float>("AutoExposureMinBrightness", 1f),
                    })
                    {
                        var adjust_name = adjust_name_ratio.Item1;
                        var ratio = adjust_name_ratio.Item2;
                        var layer2_exports = parse_tool.get_scalar_layer2_exports(ref myAsset, layer1.Item1, layer1.Item2, adjust_name);
                        foreach (var layer2_export in layer2_exports)
                        {
                            var max_brightness = parse_tool.get_scalar_layer2_curve(layer2_export, "FloatCurve");
                            parse_tool.set_scalar_layer2_curve_float_values(ref myAsset, max_brightness, new List<float>() { -180 }, new List<float>() { 1.0f }, is_fades);
                        }
                    }

                }
            }
        }
    }
}
