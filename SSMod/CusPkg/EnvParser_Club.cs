using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes.EngineEnums;
using UAssetAPI.UnrealTypes;
using UAssetAPI;

namespace SSMod.CusPkg
{
    internal class EnvParser_Club
    {
        public List<string> ufiles = new List<string>() {
            "LS_Sng026_Club_Env.uasset",
            "LS_Sng026_Club_Num1_Env.uasset",
            "LS_Sng026_Club_Num2_Env.uasset",
            "LS_Sng026_Club_Num3_Env.uasset",
            "LS_Sng026_Club_Num4_Env.uasset",

        };
        public UAsset parse_a_file(string source_file)
        {
            ParseTool details_getter = new ParseTool();
            var (myAsset, exports, object_bindings, object_possessables) = details_getter.parse_a_file(source_file);
            //foreach (StructPropertyData obj_binding in object_bindings.Value)
            for (int i = 0; i < object_bindings.Value.Count(); i++)
            {
                var obj_binding = (StructPropertyData)object_bindings.Value[i];
                var v1 = (StrPropertyData)obj_binding.Value[1];
                string name = v1.ToString();
                if (name == "Stg111Ambient01") { Stg111Ambient01(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "Stg111Ambient02") { Stg111Ambient01(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "Lit_SupportPointLight_01") { Stg111Ambient01(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "Lit_SupportPointLight_02") { Stg111Ambient01(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "Lit_SupportPointLight_03") { Stg111Ambient01(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "Stg116PointLight") { Stg111Ambient01(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "Stg114_PointLight_001") { Stg111Ambient01(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "Stg136_PointLight_001") { Stg111Ambient01(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "ToonGlobalSettings")
                {
                    Console.WriteLine(source_file + " ||| " + name);
                }
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

        public void Stg111Ambient01(int i, ArrayPropertyData bindings, ArrayPropertyData possessables, ref List<Export> exports, ref UAsset myAsset)
        {
            return;
            var possesible = (StructPropertyData)possessables.Value[i];
            var guid = ((StructPropertyData)possesible.Value[1]).Value[0];
            int j;
            for (j = 0; j < possessables.Value.Count(); j++)
            {
                var possesible_ = (StructPropertyData)possessables.Value[j];
                var parent_guid = ((StructPropertyData)possesible_.Value[4]).Value[0];
                if (parent_guid.ToString() == guid.ToString())
                {
                    break;
                }
            }
            var light_binding = (StructPropertyData)bindings.Value[j];
            var light_tracks_refs = (ArrayPropertyData)light_binding.Value[2];

            int light_intensity_track_ref_ind = int.Parse(((ObjectPropertyData)light_tracks_refs.Value[0]).ToString());
            int light_color_track_ref_ind = int.Parse(((ObjectPropertyData)light_tracks_refs.Value[1]).ToString());

            var light_intensity_track_ref = (NormalExport)exports[light_intensity_track_ref_ind - 1];
            var light_color_track_ref = (NormalExport)exports[light_color_track_ref_ind - 1];

            var light_intensity_track_ref_ref_ind = int.Parse((((ObjectPropertyData)((ArrayPropertyData)light_intensity_track_ref.Data[2]).Value[0]).Value).ToString());
            var light_color_track_ref_ref_ind = int.Parse((((ObjectPropertyData)((ArrayPropertyData)light_color_track_ref.Data[2]).Value[0]).Value).ToString());

            var light_intensity_track = (NormalExport)exports[light_intensity_track_ref_ref_ind - 1];
            var light_color_track = (NormalExport)exports[light_color_track_ref_ref_ind - 1];

            var light_intensity_curve = (StructPropertyData)light_intensity_track.Data[0];
            var light_color_r_curve = (StructPropertyData)light_color_track.Data[0];
            var light_color_g_curve = (StructPropertyData)light_color_track.Data[1];
            var light_color_b_curve = (StructPropertyData)light_color_track.Data[2];
            var light_color_w_curve = (StructPropertyData)light_color_track.Data[3];

            test_mess_up(light_intensity_curve, ref myAsset, 20);
            test_mess_up(light_color_r_curve, ref myAsset);
            test_mess_up(light_color_g_curve, ref myAsset);
            test_mess_up(light_color_b_curve, ref myAsset);

            //int light_intensity_track_ind = int.Parse(((ArrayPropertyData)((StructPropertyData)light_intensity_track_ref.Data[2]).Value[0]).Value[0].ToString());
        }

        public void test_mess_up(StructPropertyData curve, ref UAsset myAsset, int max_v = 1)
        {
            var times = (ArrayPropertyData)curve.Value[0];
            var frames = (ArrayPropertyData)curve.Value[1];

            List<PropertyData> times_stack_tmp_list = new List<PropertyData>();
            List<PropertyData> values_stack_tmp_list = new List<PropertyData>();

            for (int i = 0; i < 500; i++)
            {
                int time = i * 400 * 30;
                int value = (i % 2) * max_v;

                StructPropertyData time_struct = new StructPropertyData(new FName(myAsset, "Times"));
                time_struct.StructType = new FName(myAsset, "FrameNumber");

                FrameNumberPropertyData frame_time = new FrameNumberPropertyData(new FName(myAsset, "Times"));
                frame_time.Value = new FFrameNumber(time);

                time_struct.Value.Add(frame_time);
                times_stack_tmp_list.Add(time_struct);


                StructPropertyData value_stuct = new StructPropertyData(new FName(myAsset, "Values"));
                value_stuct.StructType = new FName(myAsset, "MovieSceneFloatValue");
                MovieSceneFloatValuePropertyData value_time = new MovieSceneFloatValuePropertyData(new FName(myAsset, "Values"));

                value_time.Value = new FMovieSceneFloatValue(null);
                value_time.Value.InterpMode = ERichCurveInterpMode.RCIM_Constant;
                value_time.Value.Value = value;
                //value_time.Value.Tangent = new FMovieSceneTangentData(null);
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
            var a1 = frames.Value;
            var a2 = value_tmp_array;
            times.Value = time_tmp_array;
            frames.Value = value_tmp_array;
        }
    }
}
