// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json.Linq;
using SSMod.CusPkg;

string name = "gmw";

JsonParser json_reader = new JsonParser();
JObject json_data = (JObject)json_reader.read_json($"F:\\IMModels\\ModProject\\Dance\\Song_{name}\\cache\\env\\ofa_env.json");
//string target_folder = $"F:\\IMModels\\ModProject\\Dance\\Dance_{name}\\Saved\\Cooked\\WindowsNoEditor\\Dance_{name}\\StarlitSeason\\Content\\Sequence\\Live\\Sng026"; // <<-- need modified
string target_folder = $"F:\\IMModels\\ModProject\\Dance\\Song_{name}\\output\\Sequence\\Live\\Sng026";
EnvParser_Common commonEnvParser = new EnvParser_Common();
EnvParser_Gimmick envParser_Gimmick = new EnvParser_Gimmick();
EnvParser_Club clubEnvParser = new EnvParser_Club();
EnvParser_Other otherEnvParser = new EnvParser_Other();
EnvParser_Stg stgEnvParser = new EnvParser_Stg();
EnvParser_Fx fxEnvParser = new EnvParser_Fx();
BpmParser bpmParser = new BpmParser();
CutoffParser cutoffParser = new CutoffParser();




string source_folder = "F:\\IMModels\\ModProject\\Dance\\Scripts\\cache\\env\\source";

//string target_folder = "E:\\IMModels\\ModProject\\Dance\\Dance_bnd\\Content\\Sequence\\Live\\Common\\tmp"; // <<-- need modified
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

Console.WriteLine("Done!");
//Console.ReadLine();