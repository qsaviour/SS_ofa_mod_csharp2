using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI.ExportTypes;
using UAssetAPI.UnrealTypes;
using UAssetAPI;

namespace SSMod.CusPkg
{
    internal class CutoffParser
    {
        public List<string> ufiles = new List<string>()
        {
            "LS_Sng026_Common_Cutoff.uasset",
            "LS_Sng026_Stg103_Cutoff.uasset",
            "LS_Sng026_Stg114_Cutoff.uasset",
            "LS_Sng026_Stg115_Cutoff.uasset",
            "LS_Sng026_Stg116_Cutoff.uasset",
            "LS_Sng026_Stg120_Cutoff.uasset",
            "LS_Sng026_Stg136_Cutoff.uasset",
            "LS_Sng026_Stg202_Cutoff.uasset",
            "LS_Sng026_Stg203_Cutoff.uasset",
            "LS_Sng026_Stg205_Cutoff.uasset",
            "LS_Sng026_Stg209_Num5_Cutoff.uasset",
            "LS_Sng026_Stg302_Cutoff.uasset",
            "LS_Sng026_Stg302_Num3_Cutoff.uasset",
            "LS_Sng026_Stg302_Num4_Cutoff.uasset",
            "LS_Sng026_Stg311_Cutoff.uasset",
            "LS_Sng026_Stg312_Cutoff.uasset",
            "LS_Sng026_Stg313_Cutoff.uasset",
        };

        public UAsset parse_a_file(string source_file)
        {
            UAsset myAsset = new UAsset(source_file, EngineVersion.VER_UE4_24);
            var exports = myAsset.Exports;
            var main_export = (NormalExport)exports[0];
            var datas = main_export.Data;
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
