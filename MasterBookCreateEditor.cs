using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using OneProjest.MasterBook;
using System.IO;

namespace OneProjest.Editor
{
    public class MasterBookCreateEditor : EditorWindow
    {
        private const string MasterBookDirectoryPath = "Assets/Resources/MasterBook/";

        [MenuItem("Tools/MasterBookCreateEditor")]
        private static void ShowWindow()
        {
            GetWindow<MasterBookCreateEditor>();
        }

        private void OnGUI()
        {
            CreateDataButton<ReminisceneData, ReminisceneDataObjectList>();
            CreateDataButton<ReinforceItemData, ReinforceItemDataObjectList>();
            CreateDataButton<CharacterData, CharacterDataObjectList>();
            CreateDataButton<SkillData, SkillDataObjectList>();
            CreateDataButton<SkillLevelData, SkillLevelDataObjectList>();

        }

        private void CreateDataButton<TData, TDataObjectList>()
    where TData : ScriptableObject, IMasterBookData
    where TDataObjectList : DataObjectList<TData>
        {
            GUILayout.Label(typeof(TData).Name);
            if (GUILayout.Button($"Create{typeof(TDataObjectList).Name}"))
            {
                CreateMasterBookDataObjectList<TDataObjectList, TData>();
            }
            if (GUILayout.Button($"Create{typeof(TData).Name}"))
            {
                var data = CreateMasterBookData<TData>();
                var dataObjectList = LoadMasterBookDataObjectList<TDataObjectList, TData>();
                if (dataObjectList.dataObjectList == null)
                {
                    dataObjectList.dataObjectList = new List<TData>();
                }
                dataObjectList.dataObjectList.Add(data);
            }
        }

        private static TDataObjectList LoadMasterBookDataObjectList<TDataObjectList,TData>()
            where TDataObjectList : DataObjectList<TData> 
            where TData:ScriptableObject,IMasterBookData
        {
            return AssetDatabase.LoadAssetAtPath<TDataObjectList>($"{MasterBookDirectoryPath}{typeof(TDataObjectList).Name}.asset");
        }

        public static void CreateMasterBookDataObjectList<TDataObjectList,TData>()
            where TDataObjectList : DataObjectList<TData> 
            where TData:ScriptableObject,IMasterBookData
        {

            TDataObjectList dataObjectList = CreateInstance<TDataObjectList>();
            var path = AssetDatabase.GenerateUniqueAssetPath($"{MasterBookDirectoryPath}{typeof(TDataObjectList).Name}.asset");

            AssetDatabase.CreateAsset(dataObjectList, path);
            AssetDatabase.Refresh();
        }

        private static TData CreateMasterBookData<TData>() where TData : ScriptableObject, IMasterBookData
        {
            TData data = CreateInstance<TData>();
            var directoryPath = $"Assets/Resources/MasterBook/{typeof(TData).Name}/";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var findAssets = AssetDatabase.FindAssets("t:ScriptableObject", new string[] { directoryPath });

            data.Id = findAssets.Length+1;
            
            var dataObjectList = AssetDatabase.LoadAssetAtPath<ScriptableObject>($"{MasterBookDirectoryPath}");


            var path = AssetDatabase.GenerateUniqueAssetPath($"{directoryPath}{ findAssets.Length+1}.asset");
            AssetDatabase.CreateAsset(data, path);



            AssetDatabase.Refresh();
            return data;
        }
    }
}

