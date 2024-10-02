using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI;

namespace SSMod.CusPkg
{
    internal class EnvParser_Other
    {
        public List<string> ufiles = new List<string>() {
            "LS_Sng026_Dusk_Env.uasset",
            "LS_Sng026_Fine_Env.uasset",

            "LS_Sng026_Arena_Env.uasset",
            "LS_Sng026_Arena_Num1_Env.uasset",
            "LS_Sng026_Arena_Num2_Env.uasset",
            "LS_Sng026_Arena_Num3_Env.uasset",
            "LS_Sng026_Arena_Num4_Env.uasset",

            "LS_Sng026_Night_Env.uasset",
            "LS_Sng026_Night_Num1_Env.uasset",
            "LS_Sng026_Night_Num2_Env.uasset",
            "LS_Sng026_Night_Num3_Env.uasset",
            "LS_Sng026_Night_Num4_Env.uasset",
        };

        public UAsset parse_a_file(string source_file)
        {
            ParseTool details_getter = new ParseTool();
            var (myAsset, exports, object_bindings, object_possessables) = details_getter.parse_a_file(source_file);

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
    }
}
