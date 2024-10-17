using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI.UnrealTypes;
using UAssetAPI;

namespace SSMod.CusPkg
{
    internal class EnvParser_Gimmick
    {
        public List<string> ufiles = new List<string>() {
            "LS_Sng026_Common_Gimmick.uasset",
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

                process_PinSpot(ref myAsset, json_obj);
                process_individuallyRatio(ref myAsset, json_obj);
                myAsset.Write(target_file);
                Console.WriteLine($"Write to {target_file}!");
            }
        }

        void process_PinSpot(ref UAsset myAsset, JObject json_obj)
        {
            var stage_colors = json_obj["stage_color"];
            List<float> frames = new List<float>() { };
            List<float> reds = new List<float>() { };
            List<float> greens = new List<float>() { };
            List<float> blues = new List<float>() { };
            List<float> alphas = new List<float>() { };
            List<bool> is_fades = new List<bool>() { };
            foreach (var stage_color in stage_colors)
            {
                float frame = (float)stage_color["frame"];
                float red = float.Parse(stage_color["color"][0].ToString());
                float green = float.Parse(stage_color["color"][1].ToString());
                float blue = float.Parse(stage_color["color"][2].ToString());
                bool is_fade = (bool)stage_color["is_fade"];
                frames.Add(frame);
                reds.Add(red);
                greens.Add(green);
                blues.Add(blue);
                is_fades.Add(is_fade);
            }
            ParseTool parse_tool = new ParseTool();
            var layer1s = parse_tool.get_layer1_tracks(ref myAsset);
            List<string> to_edit_names = new List<string>() {
                "BP_GLGroup_LaserLightA","BP_GLGroup_LaserLightB",
                "BP_GLGroup_LedFixA_A", "BP_GLGroup_LedFixA_B",
                "BP_GLGroup_LedLightA_A", "BP_GLGroup_LedLightA_B",
                "BP_GLGroup_SerchLightA_A", "BP_GLGroup_SerchLightA_B","BP_GLGroup_SerchLightA_C","BP_GLGroup_SerchLightA_D","BP_GLGroup_SerchLightA_E",
                "BP_GLGroup_SerchLightB_A", "BP_GLGroup_SerchLightB_B",
                "BP_GLGroup_SpotLightA_A","BP_GLGroup_SpotLightA_B","BP_GLGroup_SpotLightA_C","BP_GLGroup_SpotLightA_D",
            };
            foreach (var layer1 in layer1s)
            {
                var name = layer1.Item1;

                if (to_edit_names.Contains(name))
                {
                    //Console.WriteLine($"Is Writing :{name} .......");
                    var curve_name_main = "_Color";
                    var layer2_exports = parse_tool.get_scalar_layer2_exports(ref myAsset, layer1.Item1, layer1.Item2, curve_name_main);
                    foreach (var layer2_export in layer2_exports)
                    {
                        var red_curve = parse_tool.get_scalar_layer2_curves(name, curve_name_main, layer2_export, "RedCurve");
                        var green_curve = parse_tool.get_scalar_layer2_curves(name, curve_name_main, layer2_export, "GreenCurve");
                        var blue_curve = parse_tool.get_scalar_layer2_curves(name, curve_name_main, layer2_export, "BlueCurve");
                        var alpha_curve = parse_tool.get_scalar_layer2_curves(name, curve_name_main, layer2_export, "AlphaCurve");

                        parse_tool.set_scalar_layer2_curve_values(ref myAsset, red_curve[0], frames, reds, is_fades);
                        parse_tool.set_scalar_layer2_curve_values(ref myAsset, green_curve[0], frames, greens, is_fades);
                        parse_tool.set_scalar_layer2_curve_values(ref myAsset, blue_curve[0], frames, blues, is_fades);
                        parse_tool.set_scalar_layer2_curve_values(ref myAsset, alpha_curve[0], alphas, alphas, is_fades);
                        //parse_tool.set_scalar_layer2_curve_values(ref myAsset, red_curve[0], new List<float>{-180}, new List<float> { 1 }, is_fades);
                        //parse_tool.set_scalar_layer2_curve_values(ref myAsset, green_curve[0], new List<float> { -180 }, new List<float> { 0 }, is_fades);
                        //parse_tool.set_scalar_layer2_curve_values(ref myAsset, blue_curve[0], new List<float> { -180 }, new List<float> { 0 }, is_fades);
                        //parse_tool.set_scalar_layer2_curve_values(ref myAsset, alpha_curve[0], new List<float> { -180 }, new List<float> { 1 }, is_fades);
                    }
                }
            }
        }

        void process_individuallyRatio(ref UAsset myAsset, JObject json_obj)
        {
            List<float> indiviually_ratio = new List<float>() { };
        }
    }
}
