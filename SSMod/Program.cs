// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json.Linq;
using SSMod.CusPkg;


JsonParser json_reader = new JsonParser();
JObject json_data = (JObject)json_reader.read_json("E:\\IMModels\\ModProject\\Dance\\Scripts\\cache\\env\\ofa_env.json");
EnvParser_Common commonEnvParser = new EnvParser_Common();
EnvParser_Club clubEnvParser = new EnvParser_Club();
EnvParser_Other otherEnvParser = new EnvParser_Other();
EnvParser_Stg stgEnvParser = new EnvParser_Stg();
BpmParser bpmParser = new BpmParser();
CutoffParser cutoffParser = new CutoffParser();
GimmickParser gimmickParser = new GimmickParser();


string source_folder = "E:\\IMModels\\ModProject\\Dance\\Scripts\\cache\\env\\source";
string target_folder = "E:\\IMModels\\ModProject\\Dance\\Song_bnd\\output\\Sequence\\Live\\Sng026"; // <<-- need modified
Directory.CreateDirectory(target_folder);
commonEnvParser.parse(source_folder, target_folder);
clubEnvParser.parse(source_folder, target_folder);
otherEnvParser.parse(source_folder, target_folder);
stgEnvParser.parse(source_folder, target_folder);
bpmParser.parse(source_folder, target_folder);
cutoffParser.parse(source_folder, target_folder);
gimmickParser.parse(source_folder, target_folder);

Console.WriteLine("Done!");
//Console.ReadLine();