using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;


UAsset myAsset = new UAsset("E:\\IMModels\\ModProject\\Dance\\Scripts\\cache\\lyrics\\MD_Lyric_Sng026.uasset", EngineVersion.VER_UE4_24);
DataTableExport lyric_export = (DataTableExport)myAsset.Exports[0];
var table_info = lyric_export.Table;
int row_num = table_info.Data.Count();

List<StructPropertyData> datas = table_info.Data;
datas.Clear();

string ofa_string = File.ReadAllText("E:\\IMModels\\ModProject\\Dance\\Song_ful\\cache\\lyrics\\lyrics.json");
JArray ofa_lyrics = (JArray)JsonConvert.DeserializeObject(ofa_string);
for (int i = 0; i < ofa_lyrics.Count(); i++)
{
    StructPropertyData data = new StructPropertyData(new FName(myAsset, (i + 1).ToString()));

    StrPropertyData d1 = new StrPropertyData(new FName(myAsset, "Lyric"));
    d1.Value = FString.FromString((string)ofa_lyrics[i][2], Encoding.Unicode);
    data.Value.Add(d1);

    FloatPropertyData d2 = new FloatPropertyData(new FName(myAsset, "DisplayTime"));
    d2.Value = (float)ofa_lyrics[i][0];
    data.Value.Add(d2);

    FloatPropertyData d3 = new FloatPropertyData(new FName(myAsset, "HiddenTime"));
    d3.Value = (float)ofa_lyrics[i][1];
    data.Value.Add(d3);

    table_info.Data.Add(data);
}
////StructPropertyData t = (StructPropertyData)table_info.Data[0];
////var x = t.Value;
////Console.WriteLine(x[0].Name);
////var xx = new StrPropertyData(new FName(myAsset, "Lyric"));
////xx.Value = FString.FromString("SDLFKJSDF", Encoding.Unicode); ;
////x[0] = xx;


//StrPropertyData d1 = new StrPropertyData(new FName(myAsset, 12));
//d1.Value = FString.FromString("SDLFKJSDF", Encoding.Unicode);
//FloatPropertyData d2 = new FloatPropertyData(new FName(myAsset, "2222"));
//d2.Value = 12.2f;
//StructPropertyData ff = new StructPropertyData(new FName(myAsset, "22"));


//ff.Value.Add(d1);
//ff.Value.Add(d2);

//table_info.Data.Add(ff);
string target_folder = "E:\\IMModels\\ModProject\\Dance\\Song_ful\\output\\MD\\Live\\Lyric\\MD_Lyric_Sng026.uasset";
myAsset.Write(target_folder);

Console.WriteLine($"Finished! Write to {target_folder}");