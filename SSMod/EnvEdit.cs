// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json.Linq;
using SSMod.CusPkg;


JsonParser json_reader = new JsonParser();
JObject json_data = (JObject)json_reader.read_json("E:\\IMModels\\ModProject\\Dance\\Song_Cng\\cache\\env\\ofa_env.json");
EnvParser_Common commonEnvParser = new EnvParser_Common();
EnvParser_Gimmick envParser_Gimmick = new EnvParser_Gimmick();
EnvParser_Club clubEnvParser = new EnvParser_Club();
EnvParser_Other otherEnvParser = new EnvParser_Other();
EnvParser_Stg stgEnvParser = new EnvParser_Stg();
EnvParser_Fx fxEnvParser = new EnvParser_Fx();
BpmParser bpmParser = new BpmParser();
CutoffParser cutoffParser = new CutoffParser();



string source_folder = "E:\\IMModels\\ModProject\\Dance\\Scripts\\cache\\env\\source";

//string target_folder = "E:\\IMModels\\ModProject\\Dance\\Dance_bnd\\Content\\Sequence\\Live\\Common\\tmp"; // <<-- need modified
string target_folder = "E:\\IMModels\\ModProject\\Dance\\Dance_cng\\Saved\\Cooked\\WindowsNoEditor\\Dance_cng\\StarlitSeason\\Content\\Sequence\\Live\\Sng026"; // <<-- need modified
//string target_folder = "E:\\IMModels\\ModProject\\Dance\\Dance_bnd\\Saved\\Cooked\\WindowsNoEditor\\Dance_bnd\\StarlitSeason\\Content\\Sequence\\Live\\Sng026";

Directory.CreateDirectory(target_folder);
commonEnvParser.parse(source_folder, target_folder, json_data);
envParser_Gimmick.parse(source_folder, target_folder, json_data);
clubEnvParser.parse(source_folder, target_folder, json_data);
//fxEnvParser.parse(source_folder, target_folder, json_data);
//otherEnvParser.parse(source_folder, target_folder, json_data);
//stgEnvParser.parse(source_folder, target_folder, json_data);
//bpmParser.parse(source_folder, target_folder, json_data);
//cutoffParser.parse(source_folder, target_folder, json_data);
//gimmickParser.parse(source_folder, target_folder, json_data);

Console.WriteLine("Done!");
//Console.ReadLine();