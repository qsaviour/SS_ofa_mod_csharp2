using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.UnrealTypes;
using UAssetAPI;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes.EngineEnums;
using System.Net;

namespace SSMod.CusPkg
{
    public class ParseTool
    {

        public NormalExport get_movie_scene(ref UAsset myAsset)
        {
            List<Export> exports = myAsset.Exports;
            var main_export = (NormalExport)exports[0];
            var main_export_data = (List<PropertyData>)main_export.Data;
            foreach (var property in main_export_data)
            {
                if(property.Name.ToString() == "MovieScene")
                {
                    var property_ =  (ObjectPropertyData)property;
                    var index = property_.Value.Index;
                    var movie_scene = (NormalExport)exports[index - 1];
                    return movie_scene;
                }
            }
            Console.WriteLine("Source file not find movie scene:" + myAsset.FilePath);
            return null;
        }
        public List<Export> get_master_tracks(ref UAsset myAsset)
        {
            List<Export> res = new List<Export> ();
            var movie_scene = get_movie_scene(ref myAsset);
            List<Export> exports = myAsset.Exports;
            if (movie_scene == null) return res;
            foreach(var data in movie_scene.Data)
            {
                if(data.Name.ToString() == "MasterTracks")
                {
                    var master_track_inds1 = (PropertyData[])((ArrayPropertyData)data).Value;
                    foreach(var master_track_ind1 in master_track_inds1)
                    {
                        var master_track_ind1_ = int.Parse(master_track_ind1.RawValue.ToString());
                        var master_track1 = (NormalExport)exports[master_track_ind1_ - 1];
                        foreach(var master_track_data in master_track1.Data)
                        {
                            if(master_track_data.Name.ToString() == "Sections")
                            {
                                var master_track_data_ = (ArrayPropertyData)master_track_data;
                                var master_track_ind2 = int.Parse(master_track_data_.Value[0].ToString());
                                var master_track2 = (NormalExport)exports[master_track_ind2 - 1];
                                res.Add(master_track2);
                            }
                        }
                    }
                }
            }
            return res;
        }

        public List<Tuple<string, StructPropertyData>> get_layer1_tracks(ref UAsset myAsset)
        {
            var movie_scene = get_movie_scene(ref myAsset);
            if (movie_scene == null) return null;
            List<Tuple<string, StructPropertyData>> res = new List<Tuple<string, StructPropertyData>>();
            var data = movie_scene.Data.Find((e) => { return e.Name.ToString() == "ObjectBindings"; });
            var data_ = (ArrayPropertyData)data;
            foreach(var binding_data in data_.Value)
            {
                string name;
                var binding_data_ = (StructPropertyData)binding_data;
                var binding_value = binding_data_.Value.Find((e) => { return e.Name.ToString() == "BindingName"; });
                var binding_value_ = (StrPropertyData)binding_value;
                name = binding_value_.Value.ToString();
                var r = new Tuple<string, StructPropertyData> (name, binding_data_);
                res.Add(r);
            }
            return res;
        }

        public List<string> get_layer2_names(ref UAsset myAsset,ref StructPropertyData layer1)
        {
            var exports = (List<Export>)myAsset.Exports;
            var layer2_tracks_ind1 = layer1.Value.Find((e) => { return e.Name.ToString() == "Tracks"; });
            var layer2_tracks_ind1_ = (ArrayPropertyData)layer2_tracks_ind1;
            var results = new List<string>();
            foreach (var layer2_track_ind1 in layer2_tracks_ind1_.Value)
            {
                var layer2_track_ind1_ = (ObjectPropertyData)layer2_track_ind1;
                var layer2_track_ind2 = int.Parse(layer2_track_ind1_.Value.ToString());
                var layer2_track_ind2_export_ind1 = (NormalExport)exports[layer2_track_ind2 - 1];
                foreach(var layer2 in layer2_track_ind2_export_ind1.Data){
                    var name = layer2.RawValue.ToString();
                    results.Add(name);
                }
            }
            return results;
        }

        public List<NormalExport> get_scalar_layer2_exports(ref UAsset myAsset,string layer1_name,StructPropertyData layer1,string layer2_name,string section_name = "Sections",bool use_value=true,string export_name=null)
        {   
            var exports = (List<Export>)myAsset.Exports;
            var layer2_tracks_ind1 = layer1.Value.Find((e) => { return e.Name.ToString() == "Tracks"; });
            var layer2_tracks_ind1_ = (ArrayPropertyData)layer2_tracks_ind1;
            var results = new List<NormalExport>();
            foreach(var layer2_track_ind1 in layer2_tracks_ind1_.Value)
            {
                var layer2_track_ind1_ = (ObjectPropertyData)layer2_track_ind1;
                var layer2_track_ind2 = int.Parse(layer2_track_ind1_.Value.ToString());
                var layer2_track_ind2_export_ind1 = (NormalExport)exports[layer2_track_ind2-1];
                object el;
                if (use_value)
                {
                    el = layer2_track_ind2_export_ind1.Data.Find((e) => { return e.RawValue.ToString() == layer2_name; });
                }
                else
                {
                    el = layer2_track_ind2_export_ind1.Data.Find((e) => { return e.Name.ToString() == layer2_name; });
                }
                if (el != null)
                {
                    var section = layer2_track_ind2_export_ind1.Data.Find((e) => { return e.Name.ToString() == section_name; });
                    var section_ = (ArrayPropertyData)section;
                    var layer2_track_ind2_export_ind2 = section_.Value[0];
                    var layer2_track_ind2_export_ind2_ = (ObjectPropertyData)layer2_track_ind2_export_ind2;
                    var layer2_track_ind = layer2_track_ind2_export_ind2_.Value.Index;
                    var layer2_export = (NormalExport)exports[layer2_track_ind - 1];
                    if(export_name!=null && !layer2_export.ObjectName.ToString().Contains(export_name))
                    {

                    }
                    else
                    {
                        results.Add(layer2_export);
                    }
                }
            }
            if (results.Count <= 0)
            {
                Console.WriteLine($"!!not find <layer2>-{layer2_name} from <Layer1>-{layer1_name}");
            }
            return results;
        }


        public List<string> get_layer2_curve_names(string layer1_name, string layer2_name, NormalExport layer2)
        {
            var results = new List<string>();
            var layer2_datas = (List<PropertyData>)layer2.Data;
            foreach(var layer2_data in layer2_datas)
            {
                results.Add(layer2_data.Name.ToString());
            }
            return results;
        }

        public PropertyData get_scalar_layer2_curve(NormalExport layer2, string curve_name)
        {
            
            var layer2_datas = (List<PropertyData>)layer2.Data;
            var curve = layer2_datas.FindAll((e) => { return e.Name.ToString() == curve_name; })[0];
            return curve;
        }

        public PropertyData get_scalar_layer3_curve(NormalExport layer2, string curve1_name,string curve2_name)
        {
            var layer2_datas = (List<PropertyData>)layer2.Data;
            var curve1 = layer2_datas.FindAll((e) => { return e.Name.ToString() == curve1_name; })[0];
            var curve1_ = (StructPropertyData)curve1;
            var curve2 = curve1_.Value.Find((e) => { return e.Name.ToString() == curve2_name; });
            return curve2;
        }

        public void set_scalar_layer2_curve_float_values(ref UAsset myAsset,PropertyData curve_,List<float> frames,List<float> values,List<bool> is_fades,float off_value=0)
        {
            var curve = (StructPropertyData)curve_;
            ArrayPropertyData times_curve = null;
            ArrayPropertyData values_curve = null;
            foreach(var sub_curve in curve.Value)
            {
                if (sub_curve.Name.ToString() == "Times") { times_curve = (ArrayPropertyData)sub_curve; }
                if (sub_curve.Name.ToString() == "Values") { values_curve = (ArrayPropertyData)sub_curve;  }
            }
            if(times_curve == null || values_curve == null)
            {
                Console.WriteLine($"Write Curve Failed!");
            }
            PropertyData[] frames_stack = new PropertyData[frames.Count];
            PropertyData[] values_stack = new PropertyData[values.Count];

            for(int i=0;i<frames.Count; i++)
            {
                StructPropertyData time_struct = new StructPropertyData(new FName(myAsset, "Times"));
                time_struct.StructType = new FName(myAsset, "FrameNumber");

                FrameNumberPropertyData frame_time = new FrameNumberPropertyData(new FName(myAsset, "Times"));
                frame_time.Value = new FFrameNumber((int)frames[i]*400);
                time_struct.Value.Add(frame_time);
                frames_stack[i] = time_struct;

                StructPropertyData value_stuct = new StructPropertyData(new FName(myAsset, "Values"));
                value_stuct.StructType = new FName(myAsset, "MovieSceneFloatValue");
                MovieSceneFloatValuePropertyData value_time = new MovieSceneFloatValuePropertyData(new FName(myAsset, "Values"));
                value_time.Value = new FMovieSceneFloatValue();
                value_time.Value.Value = values[i]+ off_value;

                value_time.Value.Tangent = new FMovieSceneTangentData();
                if (is_fades!=null && is_fades[i])
                {
                    value_time.Value.InterpMode = ERichCurveInterpMode.RCIM_Cubic;
                }
                else
                {
                    value_time.Value.InterpMode = ERichCurveInterpMode.RCIM_Constant;
                }
                value_stuct.Value.Add(value_time);
                values_stack[i] = value_stuct;
            }
            times_curve.Value = frames_stack;
            values_curve.Value = values_stack;
        }

        public void set_scalar_layer2_curve_boolean_values(ref UAsset myAsset, PropertyData curve_, List<float> frames, List<bool> values)
        {
            var curve = (StructPropertyData)curve_;
            ArrayPropertyData times_curve = null;
            ArrayPropertyData values_curve = null;
            foreach (var sub_curve in curve.Value)
            {
                if (sub_curve.Name.ToString() == "Times") { times_curve = (ArrayPropertyData)sub_curve; }
                if (sub_curve.Name.ToString() == "Values") { values_curve = (ArrayPropertyData)sub_curve; }
            }
            if (times_curve == null || values_curve == null)
            {
                Console.WriteLine($"Write Curve Failed!");
            }
            PropertyData[] frames_stack = new PropertyData[frames.Count];
            PropertyData[] values_stack = new PropertyData[values.Count];

            for (int i = 0; i < frames.Count; i++)
            {
                StructPropertyData time_struct = new StructPropertyData(new FName(myAsset, "Times"));
                time_struct.StructType = new FName(myAsset, "FrameNumber");

                FrameNumberPropertyData frame_time = new FrameNumberPropertyData(new FName(myAsset, "Times"));
                frame_time.Value = new FFrameNumber((int)frames[i] * 400);
                time_struct.Value.Add(frame_time);
                frames_stack[i] = time_struct;

                BoolPropertyData value_stuct = new BoolPropertyData();
                value_stuct.Value = values[i];
                values_stack[i] = value_stuct;
            }
            times_curve.Value = frames_stack;
            values_curve.Value = values_stack;
        }
    }
}
