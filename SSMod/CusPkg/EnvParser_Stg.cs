using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI.ExportTypes;
using UAssetAPI.UnrealTypes;
using UAssetAPI;
using Newtonsoft.Json.Linq;

namespace SSMod.CusPkg
{
    internal class EnvParser_Stg
    {
        public List<string> ufiles = new List<string>()
        {
            "LS_Sng026_Stg101_Env.uasset",
            "LS_Sng026_Stg102_Env.uasset",
            "LS_Sng026_Stg103_Env.uasset",
            "LS_Sng026_Stg104_Env.uasset",
            "LS_Sng026_Stg111_Env.uasset",
            "LS_Sng026_Stg114_Env.uasset",
            "LS_Sng026_Stg114_Env.uasset",
            "LS_Sng026_Stg115_Env.uasset",
            "LS_Sng026_Stg120_Env.uasset",
            "LS_Sng026_Stg136_Env.uasset",
            "LS_Sng026_Stg201_Env.uasset",
            "LS_Sng026_Stg201_Num1_Env.uasset",
            "LS_Sng026_Stg201_Num2_Env.uasset",
            "LS_Sng026_Stg201_Num3_Env.uasset",
            "LS_Sng026_Stg201_Num4_Env.uasset",
            "LS_Sng026_Stg202_Env.uasset",
            "LS_Sng026_Stg203_Env.uasset",
            "LS_Sng026_Stg204_Env.uasset",
            "LS_Sng026_Stg205_Env.uasset",
            "LS_Sng026_Stg206_Env.uasset",
            "LS_Sng026_Stg207_Env.uasset",
            "LS_Sng026_Stg208_Env.uasset",
            "LS_Sng026_Stg209_Env.uasset",
            "LS_Sng026_Stg301_Env.uasset",
            "LS_Sng026_Stg302_Env.uasset",
            "LS_Sng026_Stg311_Env.uasset",
            "LS_Sng026_Stg312_Env.uasset",
            "LS_Sng026_Stg314_Env.uasset",
            "LS_Sng026_Stg315_Env.uasset",
        };
        public UAsset parse_a_file(string source_file, JObject json_obj)
        {
            UAsset myAsset = new UAsset(source_file, EngineVersion.VER_UE4_24);
            var exports = myAsset.Exports;
            var main_export = (NormalExport)exports[0];
            var datas = main_export.Data;
            return myAsset;
        }
        public void parse(string source_folder, string target_folder, JObject json_obj)
        {
            foreach (var ufile in ufiles)
            {
                Console.WriteLine("Processsing:" + ufile);
                var source_file = source_folder + "\\" + ufile;
                var target_file = target_folder + "\\" + ufile;

                var res_asset = parse_a_file(source_file, json_obj);

                res_asset.Write(target_file);
                Console.WriteLine("Write target_file:" + target_file);
            }
        }
    }
}
