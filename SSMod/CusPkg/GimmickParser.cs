using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI.ExportTypes;
using UAssetAPI.UnrealTypes;
using UAssetAPI;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;

namespace SSMod.CusPkg
{
    internal class GimmickParser
    {
        public List<string> ufiles = new List<string>()
        {
            "LS_Sng026_Common_Gimmick.uasset",
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

                if (name == "BP_GLGroup_LedFixA_A") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_LedFixA_B") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_LedLightA_A") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_LedLightA_B") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_SerchLightA_A") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_SerchLightA_B") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_SerchLightA_C") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_SerchLightA_D") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }

                else if (name == "BP_GLGroup_SerchLightA_E") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_SerchLightB_A") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_SerchLightB_B") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_SpotLightA_A") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_SpotLightA_B") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_SpotLightA_C") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_SpotLightA_D") { BP_GLGroup_LedFixA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }

                else if (name == "BP_GLGroup_LaserLightA_A") { BP_GLGroup_LaserLightA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
                else if (name == "BP_GLGroup_LaserLightB_A") { BP_GLGroup_LaserLightA_A(i, object_bindings, object_possessables, ref exports, ref myAsset); }
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

        public void BP_GLGroup_LedFixA_A(int i, ArrayPropertyData bindings, ArrayPropertyData possessables, ref List<Export> exports, ref UAsset myAsset)
        {
            var light_binding = (StructPropertyData)bindings.Value[i];
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
        }

        public void BP_GLGroup_LaserLightA_A(int i, ArrayPropertyData bindings, ArrayPropertyData possessables, ref List<Export> exports, ref UAsset myAsset)
        {

        }
    }
}
