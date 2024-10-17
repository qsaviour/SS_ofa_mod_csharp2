using Newtonsoft.Json.Linq;
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
            
            "LS_Sng026_Fine_Env.uasset",

            

            "LS_Sng026_Night_Env.uasset",
            "LS_Sng026_Night_Num1_Env.uasset",
            "LS_Sng026_Night_Num2_Env.uasset",
            "LS_Sng026_Night_Num3_Env.uasset",
            "LS_Sng026_Night_Num4_Env.uasset",
        };


        public void parse(string source_folder, string target_folder, JObject json_obj)
        {
            foreach (var ufile in ufiles)
            {
                Console.WriteLine("Processsing:" + ufile);
                var source_file = source_folder + "\\" + ufile;
                var target_file = target_folder + "\\" + ufile;

                //var res_asset = parse_a_file(source_file, json_obj);

                //res_asset.Write(target_file);
                Console.WriteLine("Write target_file:" + target_file);
            }
        }
    }
}
