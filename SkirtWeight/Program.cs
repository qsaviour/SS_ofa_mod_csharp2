using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes;


var MASS = 0.10f; // <0.6   0.15f
var ForceGravityStartRotationDegree = 25f; //  >2  15f;
var CentrifugalForcePower = 4f; // >1.5  8f
var ForceGravityRotationLowerRate_mul = 0.5f; // not unique  0.5f

UAsset myAsset = new UAsset("F:\\IMModels\\ModProject\\Cloth\\cheerleader\\cheerleader\\Saved\\Cooked\\GPA_chr_body_cos021_a_01.uasset", EngineVersion.VER_UE4_24);
//string target_file = "E:\\IMModels\\ModProject\\naked_shirt\\naked_shirt\\Saved\\Cooked\\WindowsNoEditor\\naked_shirt\\StarlitSeason\\Content\\Model\\Character\\Body\\Cos\\chr_body_cos021\\Mesh\\GPA_chr_body_cos021_a_01.uasset";
string target_file = "F:\\IMModels\\ModProject\\Cloth\\cheerleader\\cheerleader\\Saved\\Cooked\\GPA_chr_body_cos021_a_01.uasset";



var exports = (NormalExport)myAsset.Exports[0];
var body_tables = (ArrayPropertyData)exports[1];
var constrain_tables = (ArrayPropertyData)exports[2];
var locators_tables = (ArrayPropertyData)exports[3];

for (int i = 0; i < body_tables.Value.Count(); i++)
{
    var body_table = (StructPropertyData)body_tables.Value[i];
    var bone_name = body_table.Value[0].ToString();
    if (bone_name.IndexOf("HipsE_skirt") != -1 && !bone_name.EndsWith("0"))
    {
        Console.WriteLine(bone_name);
        ((FloatPropertyData)body_table.Value[5]).Value = MASS;

    }
}

for (int i = 0; i < constrain_tables.Value.Count(); i++)
{
    var constrain_table = (StructPropertyData)constrain_tables.Value[i];
    var bone_name = constrain_table.Value[0].ToString();
    if (bone_name.IndexOf("HipsE_skirt") != -1 && !bone_name.EndsWith("0"))
    {
        Console.WriteLine(bone_name);
        ((FloatPropertyData)constrain_table.Value[12]).Value = ForceGravityStartRotationDegree;
        ((FloatPropertyData)constrain_table.Value[17]).Value = CentrifugalForcePower;
        ((FloatPropertyData)constrain_table.Value[11]).Value *= ForceGravityRotationLowerRate_mul;

    }
}



myAsset.Write(target_file);


