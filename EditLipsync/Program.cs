using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes;


string name = "fut";

string base_folder = $"F:\\IMModels\\ModProject\\Dance\\Song_{name}";
var output_folder = base_folder + "\\output\\Animation\\Character\\Facial\\LipSync\\Song"; // need folder


//string song_num = "021_00";
//string song_num = "021_01";
string song_num = "026_00";


string cache_lip_folder = base_folder + "\\cache\\lip";
//string ofa_string = File.ReadAllText("E:\\IMModels\\ModProject\\Dance\\Song_Cng\\lip\\ss\\LipSync_Sng026_00_ofa_target");

string ofa_string = File.ReadAllText(cache_lip_folder + "\\LipSync_Sng026_00_ofa_target");
Console.WriteLine($"Read from {cache_lip_folder + "\\LipSync_Sng026_00_ofa_target"}");
float[][] ofalips = JsonConvert.DeserializeObject<float[][]>(ofa_string);

UAsset myAsset = new UAsset("F:\\IMModels\\ModProject\\Dance\\Scripts\\cache\\lip\\ss\\LipSync_Sng"+ song_num + ".uasset", EngineVersion.VER_UE4_24);
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

Directory.CreateDirectory(output_folder);
string output_file = output_folder + "\\LipSync_Sng"+ song_num + ".uasset";
myAsset.Write(output_file);
Console.WriteLine($"Finished! Write to {output_file}");
Console.ReadLine();