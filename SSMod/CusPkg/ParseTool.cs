using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.UnrealTypes;
using UAssetAPI;

namespace SSMod.CusPkg
{
    internal class ParseTool
    {
        public (UAsset, List<Export>, ArrayPropertyData, ArrayPropertyData) parse_a_file(string source_file)
        {
            UAsset myAsset = new UAsset(source_file, EngineVersion.VER_UE4_24);
            var exports = myAsset.Exports;
            var main_export = (NormalExport)exports[0];
            var datas = main_export.Data;
            var scence_ind = ((ObjectPropertyData)datas[0]).Value.Index;
            var scence = (NormalExport)exports[scence_ind - 1];
            var scence_data = scence.Data;
            var object_bindings = (ArrayPropertyData)scence_data[1];
            var object_possessables = (ArrayPropertyData)scence_data[0];

            return (myAsset, exports, object_bindings, object_possessables);
        }
    }
}
