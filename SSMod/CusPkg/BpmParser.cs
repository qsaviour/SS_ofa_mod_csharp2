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
    internal class BpmParser
    {
        public List<string> ufiles = new List<string>()
        {
            "LS_Sng026_Common_Bpm.uasset",
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
