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
            
            ParseTool parse_tool = new ParseTool();
            var layer1s = parse_tool.get_layer1_tracks(ref myAsset);
            Dictionary<string, string[]> edit_light_name_map = new Dictionary<string, string[]>()
            {
                { "BP_GLGroup_LaserLightA", new string[]{ "laser_light", "1" }},
                { "BP_GLGroup_LaserLightB", new string[]{ "laser_light", "2" }},

                { "BP_GLGroup_LedFixA_A", new string[]{ "fix_light", "1" }},
                { "BP_GLGroup_LedFixA_B", new string[]{ "fix_light", "2" }},

                { "BP_GLGroup_LedLightA_A", new string[]{ "fix_light", "3" }},
                { "BP_GLGroup_LedLightA_B", new string[]{ "fix_light", "4" }},

                { "BP_GLGroup_SerchLightA_A", new string[]{ "serch_light", "1" }},
                { "BP_GLGroup_SerchLightA_B", new string[]{ "serch_light", "2" }},
                { "BP_GLGroup_SerchLightA_C", new string[]{ "serch_light", "3" }},
                { "BP_GLGroup_SerchLightA_D", new string[]{ "serch_light", "2" }},
                { "BP_GLGroup_SerchLightA_E", new string[]{ "serch_light", "3" }},

                { "BP_GLGroup_SerchLightB_A", new string[]{ "serch_light", "4" }},
                { "BP_GLGroup_SerchLightB_B", new string[]{ "serch_light", "4" }},

                { "BP_GLGroup_SpotLightA_A", new string[]{ "spot_light", "1" }},
                { "BP_GLGroup_SpotLightA_B", new string[]{ "spot_light", "2" }},
                { "BP_GLGroup_SpotLightA_C", new string[]{ "spot_light", "3" }},
                { "BP_GLGroup_SpotLightA_D", new string[]{ "spot_light", "4" }},
            };
            foreach (var layer1 in layer1s)
            {
                var name = layer1.Item1;

                if (edit_light_name_map.Keys.Contains(name))
                {
                    var names = edit_light_name_map[name];
                    var x = json_obj[names[0]];
                    var gimmik_colors = json_obj[names[0]][names[1]];
                    List<float> frames = new List<float>() { };
                    List<float> reds = new List<float>() { };
                    List<float> greens = new List<float>() { };
                    List<float> blues = new List<float>() { };
                    List<float> alphas = new List<float>() { };
                    List<bool> is_fades = new List<bool>() { };
                    foreach (var gimmik_color in gimmik_colors)
                    {
                        float frame = (float)gimmik_color["frame"];
                        float red = float.Parse(gimmik_color["color"][0].ToString());
                        float green = float.Parse(gimmik_color["color"][1].ToString());
                        float blue = float.Parse(gimmik_color["color"][2].ToString());
                        var alpha = gimmik_color["alpha"];
                        var is_fade = gimmik_color["is_fade"];
                        frames.Add(frame);
                        reds.Add(red);
                        greens.Add(green);
                        blues.Add(blue);
                        if(alpha!=null)
                            alphas.Add((float)alpha);
                        if(is_fade != null)
                            is_fades.Add((bool)is_fade);
                    }

                    //Console.WriteLine($"Is Writing :{name} .......");
                    var curve_name_main = "_Color";
                    var layer2_exports = parse_tool.get_scalar_layer2_exports(ref myAsset, layer1.Item1, layer1.Item2, curve_name_main);
                    foreach (var layer2_export in layer2_exports)
                    {
                        var red_curve = parse_tool.get_scalar_layer2_curve( layer2_export, "RedCurve");
                        var green_curve = parse_tool.get_scalar_layer2_curve( layer2_export, "GreenCurve");
                        var blue_curve = parse_tool.get_scalar_layer2_curve( layer2_export, "BlueCurve");
                        var alpha_curve = parse_tool.get_scalar_layer2_curve( layer2_export, "AlphaCurve");

                        parse_tool.set_scalar_layer2_curve_float_values(ref myAsset, red_curve, frames, reds, is_fades.Count() == 0 ? null: is_fades) ;
                        parse_tool.set_scalar_layer2_curve_float_values(ref myAsset, green_curve, frames, greens, is_fades.Count() == 0 ? null : is_fades);
                        parse_tool.set_scalar_layer2_curve_float_values(ref myAsset, blue_curve, frames, blues, is_fades.Count() == 0 ? null : is_fades);
                        parse_tool.set_scalar_layer2_curve_float_values(ref myAsset, alpha_curve, alphas, alphas, is_fades.Count() == 0 ? null : is_fades);
                    }
                    var layer2_spot_exports = parse_tool.get_scalar_layer2_exports(ref myAsset, layer1.Item1, layer1.Item2, "Sections", use_value: false, export_name: "LiveGimmickLightColorSection");
                    foreach (var layer2_export in layer2_spot_exports)
                    {
                        var blink_curve = parse_tool.get_scalar_layer3_curve(layer2_export, "ChannelsGimmick", "Blink");
                        if (blink_curve != null)
                            parse_tool.set_scalar_layer2_curve_boolean_values(ref myAsset, blink_curve, new List<float>() { -180f }, new List<bool>() { false });

                        var intensity_rate_curve = parse_tool.get_scalar_layer3_curve(layer2_export, "ChannelsGimmick", "BeatRate");
                        if (blink_curve != null)
                            parse_tool.set_scalar_layer2_curve_float_values(ref myAsset, intensity_rate_curve, new List<float>() { -180f }, new List<float>() { 0.4f }, new List<bool>() { false });
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
