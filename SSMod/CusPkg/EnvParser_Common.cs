using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes;
using UAssetAPI;
using Newtonsoft.Json.Linq;
using static System.Collections.Specialized.BitVector32;
using System.Windows.Markup;

namespace SSMod.CusPkg
{
    internal class EnvParser_Common
    {
        public List<string> ufiles = new List<string>() {
            "LS_Sng026_Common_Env.uasset",
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
                myAsset.Write(target_file);
                Console.WriteLine($"Write to {target_file}!");
            }
        }

        void process_PinSpot(ref UAsset myAsset, JObject json_obj)
        {
            var stage_colors = json_obj["stage_color"];
            List<float> frames = new List<float>() {};
            List<float> reds = new List<float>() {};
            List<float> greens = new List<float>() {};
            List<float> blues = new List<float>() {};
            List<bool> is_fades = new List<bool>() {};
            List<float> brightnesses = new List<float>() { };
            List<float> individuallyRatios = new List<float>() { };
            foreach (var stage_color in stage_colors)
            {
                float frame = (float)stage_color["frame"];
                float red = float.Parse(stage_color["color"][0].ToString());
                float green = float.Parse(stage_color["color"][1].ToString());
                float blue = float.Parse(stage_color["color"][2].ToString());
                bool is_fade = (bool)stage_color["is_fade"];
                float brightness = (float)stage_color["brightness"];
                float individuallyRatio = (float)stage_color["individuallyRatio"];
                frames.Add(frame);
                reds.Add(red);
                greens.Add(green);
                blues.Add(blue); 
                is_fades.Add(is_fade);
                brightnesses.Add(brightness);
                individuallyRatios.Add(individuallyRatio);
            }
            ParseTool parse_tool = new ParseTool();
            var layer1s = parse_tool.get_layer1_tracks(ref myAsset);
            List<string> BP_PinSpotLights = new List<string>() { "BP_LL_PinSpotA", "BP_LL_PinSpotB", "BP_LL_PinSpotC", "BP_LL_PinSpotD", "BP_LL_PinSpotE", "BP_LLGroup_Center", };
            foreach (var layer1 in layer1s)
            {
                var name = layer1.Item1;
                
                if (BP_PinSpotLights.Contains(name)) {
                    //Console.WriteLine($"Is Writing :{name} .......");
                    var curve_name_main = "_Color";
                    var layer2_exports = parse_tool.get_scalar_layer2_exports(ref myAsset, layer1.Item1, layer1.Item2, curve_name_main);
                    foreach(var layer2_export in layer2_exports)
                    {
                        var red_curve = parse_tool.get_scalar_layer2_curve(layer2_export, "RedCurve");
                        var green_curve = parse_tool.get_scalar_layer2_curve(layer2_export, "GreenCurve");
                        var blue_curve = parse_tool.get_scalar_layer2_curve(layer2_export, "BlueCurve");

                        parse_tool.set_scalar_layer2_curve_float_values(ref myAsset, red_curve, frames, reds, is_fades);
                        parse_tool.set_scalar_layer2_curve_float_values(ref myAsset, green_curve, frames, greens, is_fades);
                        parse_tool.set_scalar_layer2_curve_float_values(ref myAsset, blue_curve, frames, blues, is_fades);
                    }

                    var layer2_spot_exports = parse_tool.get_scalar_layer2_exports(ref myAsset, layer1.Item1, layer1.Item2, "Sections", use_value:false,export_name:"LiveSpotLightColorSection");
                    foreach(var layer2_export in layer2_spot_exports)
                    {
                        var blink_curve = parse_tool.get_scalar_layer3_curve(layer2_export, "ChannelsSpot", "Blink");
                        if(blink_curve!=null)
                            parse_tool.set_scalar_layer2_curve_boolean_values(ref myAsset, blink_curve, new List<float>() { -180f }, new List<bool>() { false });

                        var intensity_rate_curve = parse_tool.get_scalar_layer3_curve(layer2_export, "ChannelsSpot", "IntensityRate");
                        if (intensity_rate_curve != null)
                        {
                            float light_off = 3f;
                            if (name == "BP_LLGroup_Center") light_off = 0;
                            parse_tool.set_scalar_layer2_curve_float_values(ref myAsset, intensity_rate_curve, frames, brightnesses, is_fades, light_off) ;
                        }
                    }
                }
            }
            List<string> ToonGlobalSettings = new List<string>() { "ToonGlobalSettings", };
            foreach (var layer1 in layer1s)
            {
                var name = layer1.Item1;
                if (ToonGlobalSettings.Contains(name))
                {
                    var layer2_exports = parse_tool.get_scalar_layer2_exports(ref myAsset, layer1.Item1, layer1.Item2, "individuallyRatio");
                    foreach(var layer2_export in layer2_exports)
                    {
                        var individuallyRatio_curve = parse_tool.get_scalar_layer2_curve(layer2_export, "FloatCurve");
                        parse_tool.set_scalar_layer2_curve_float_values(ref myAsset, individuallyRatio_curve, frames, individuallyRatios, is_fades);
                    }
                }
            }

            List<string> Brightness = new List<string>() { "PostProcessVolume", };
            foreach (var layer1 in layer1s)
            {
                var name = layer1.Item1;
                if (Brightness.Contains(name))
                {
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
                            parse_tool.set_scalar_layer2_curve_float_values(ref myAsset, max_brightness, new List<float>() { -180},new List<float>() { 1.0f}, is_fades);
                        }
                    }

                }
            }
        }
    }
}
