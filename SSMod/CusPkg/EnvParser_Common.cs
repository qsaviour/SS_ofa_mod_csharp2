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

namespace SSMod.CusPkg
{
    internal class EnvParser_Common
    {
        public List<string> ufiles = new List<string>() {
            "LS_Sng026_Common_Env.uasset",
        };

        public UAsset parse_a_file(string source_file)
        {
            UAsset myAsset = new UAsset(source_file, EngineVersion.VER_UE4_24);
            var exports = myAsset.Exports;
            var main_export = (NormalExport)exports[0];
            var datas = main_export.Data;
            var scence_ind = ((ObjectPropertyData)datas[0]).Value.Index;
            var scence = (NormalExport)exports[scence_ind - 1];
            var scence_data = scence.Data;
            var object_bindings = (ArrayPropertyData)scence_data[1];
            foreach (StructPropertyData obj_binding in object_bindings.Value)
            {
                var v1 = (StrPropertyData)obj_binding.Value[1];
                string name = v1.ToString();
                if (name == "PostProcessVolume")
                {
                    // PostProcessVolume(obj_binding, ref exports, ref myAsset); 
                }

                else if (name == "ToonGlobalSettings") { }
                else if (name == "CharaLightTargetA") { }
                else if (name == "CharaLightTargetB") { }
                else if (name == "CharaLightTargetC") { }
                else if (name == "CharaLightTargetD") { }
                else if (name == "CharaLightTargetE") { }
                else if (name == "BP_LLGroup_Center") { }
                else if (name == "SharedBackScreenMonitorSettings1") { }
                else if (name == "BP_LiveDecalCircleActor") { }
                else if (name == "BP_LiveDecalCircleActor_01SOLO") { }
                else if (name == "BP_LiveDecalCircleActor_02DUO") { }
                else if (name == "BP_FoliageAudienceController") { }

                else if (name == "BP_LL_PinSpotA") { }
                else if (name == "BP_LL_PinSpotB") { }
                else if (name == "BP_LL_PinSpotC") { }
                else if (name == "BP_LL_PinSpotD") { }
                else if (name == "BP_LL_PinSpotE") { }

                else if (name == "BP_IMAL_Sky_ConstellationSphere_CloudySky") { }

            }


            return myAsset;
        }

        public void parse(string source_folder, string target_folder)
        {
            foreach (var ufile in ufiles)
            {
                Console.WriteLine("Processsing:" + ufile);
                var source_file = source_folder + "\\" + ufile;
                var target_file = target_folder + "\\" + ufile;

                var res_asset = parse_a_file(source_file);

                res_asset.Write(target_file);
                Console.WriteLine("Write target_file:" + target_file);
            }
        }

        void PostProcessVolume(StructPropertyData d, ref List<Export> exports, ref UAsset myAsset)
        {
            void parse_color() { }
            var tracks = (ArrayPropertyData)d.Value[2];
            foreach (ObjectPropertyData track in tracks.Value)
            {
                int track_ind = int.Parse(track.ToString());
                var export = (NormalExport)exports[track_ind - 1];
                var data = (List<PropertyData>)export.Data;
                string name = data[1].ToString();

                if (name == "ColorGain" && false)
                {
                    var sub_track_ref = (ArrayPropertyData)data[3];
                    var sub_track_ind = int.Parse(((ObjectPropertyData)sub_track_ref.Value[0]).Value.ToString());
                    var sub_track = (NormalExport)exports[sub_track_ind - 1];
                    var x = (StructPropertyData)sub_track.Data[0];
                    var y = (StructPropertyData)sub_track.Data[1];
                    var z = (StructPropertyData)sub_track.Data[2];
                    var w = (StructPropertyData)sub_track.Data[3];

                    foreach (var l in new StructPropertyData[] { x, y, z })
                    {
                        var times_stack = (ArrayPropertyData)l.Value[0];
                        var values_stack = (ArrayPropertyData)l.Value[1];

                        //var times = times_stack.Value;
                        //var values = values_stack.Value;

                        List<PropertyData> times_stack_tmp_list = new List<PropertyData>();
                        List<PropertyData> values_stack_tmp_list = new List<PropertyData>();
                        for (int i = 0; i < 500; i++)
                        {
                            int time = i * 400 * 30;
                            int value = (i % 2);

                            StructPropertyData time_struct = new StructPropertyData(new FName(myAsset, "Times"));
                            time_struct.StructType = new FName(myAsset, "FrameNumber");

                            FrameNumberPropertyData frame_time = new FrameNumberPropertyData(new FName(myAsset, "Times"));
                            frame_time.Value = new FFrameNumber(time);

                            time_struct.Value.Add(frame_time);
                            times_stack_tmp_list.Add(time_struct);


                            StructPropertyData value_stuct = new StructPropertyData(new FName(myAsset, "Values"));
                            value_stuct.StructType = new FName(myAsset, "MovieSceneFloatValue");
                            MovieSceneFloatValuePropertyData value_time = new MovieSceneFloatValuePropertyData(new FName(myAsset, "Values"));
                            //value_time.Value = new FMovieSceneFloatValue();
                            //value_time.Value.Value = value;
                            //value_time.Value.Tangent = new FMovieSceneTangentData();
                            //value_time.Value.padding = new byte[0];
                            //value_time.Value.Tangent.padding = new byte[0];

                            value_stuct.Value.Add(value_time);
                            values_stack_tmp_list.Add(value_stuct);

                        }

                        PropertyData[] time_tmp_array = new PropertyData[times_stack_tmp_list.Count];
                        PropertyData[] value_tmp_array = new PropertyData[times_stack_tmp_list.Count];

                        for (int i = 0; i < times_stack_tmp_list.Count; i++)
                        {
                            time_tmp_array[i] = times_stack_tmp_list[i];
                            value_tmp_array[i] = values_stack_tmp_list[i];
                        }

                        times_stack.Value = time_tmp_array;
                        values_stack.Value = value_tmp_array;
                    }

                }
                else if (name == "ColorContrast")
                {

                }
                else if (name == "Settings.AutoExposureMinBrightness")
                {

                }
                else if (name == "Settings.AutoExposureMaxBrightness")
                {

                }
                else if (name == "Settings.LensFlareIntensity")
                {

                }
                else if (name == "Settings.LensFlareBokehSize")
                {

                }
                else if (name == "Settings.LensFlareThreshold")
                {

                }
            }
        }
    }
}
