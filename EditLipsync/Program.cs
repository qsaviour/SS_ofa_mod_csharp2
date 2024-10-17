using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes;

string base_folder = "E:\\IMModels\\ModProject\\Dance\\Song_ful";

string cache_lip_folder = base_folder + "\\cache\\lip";
//string ofa_string = File.ReadAllText("E:\\IMModels\\ModProject\\Dance\\Song_Cng\\lip\\ss\\LipSync_Sng026_00_ofa_target");
string ofa_string = File.ReadAllText(cache_lip_folder + "\\LipSync_Sng026_00_ofa_target");
float[][] ofalips = JsonConvert.DeserializeObject<float[][]>(ofa_string);

UAsset myAsset = new UAsset("E:\\IMModels\\ModProject\\Dance\\Scripts\\cache\\lip\\ss\\LipSync_Sng026_00.uasset", EngineVersion.VER_UE4_24);
NormalExport myExport = (NormalExport)myAsset.Exports[0];
var property = (ArrayPropertyData)myExport.Data[0];

for (int i = 0; i < property.Value.Length; i++)
{
    var v = (StructPropertyData)property.Value[i];
    var vv = ((ArrayPropertyData)v.Value[0]).Value;
    for (int j = 0; j < vv.Length; j++)
    {
        var vvv = (FloatPropertyData)(vv[j]);
        if (i < ofalips.Length)
        {
            vvv.Value = ofalips[i][j];
        }
        else
        {
            vvv.Value = 0f;
        }
    }
}
var output_folder = base_folder + "\\output\\Animation\\Character\\Facial\\LipSync\\Song";
Directory.CreateDirectory(output_folder);
string output_file = output_folder + "\\LipSync_Sng026_00.uasset";
myAsset.Write(output_file);
Console.WriteLine($"Finished! Write to {output_file}");
Console.ReadLine();