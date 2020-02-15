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

            GUILayout.Label("SkillLevelData");
            if (GUILayout.Button("CreateSkillLevelDataObjectList"))
            {
                CreateMasterBookDataObjectList<SkillLevelDataObjectList>();
            }
            
            if (GUILayout.Button("CreateSkillLevelData"))
            {
                var skillLevelData=CreateMasterBookData<SkillLevelData>();
                var skillLevelDataObjectList=LoadMasterBookDataObjectList<SkillLevelDataObjectList>();
                if (skillLevelDataObjectList.skillLevelDataList == null)
                {
                    skillLevelDataObjectList.skillLevelDataList = new List<SkillLevelData>();
                }
                skillLevelDataObjectList.skillLevelDataList.Add(skillLevelData);
            }
        }
        private static TObjectList LoadMasterBookDataObjectList<TObjectList>()where TObjectList : ScriptableObject
        {
            return AssetDatabase.LoadAssetAtPath<TObjectList>($"{MasterBookDirectoryPath}{typeof(TObjectList).Name}.asset");
        }

        private static void CreateMasterBookDataObjectList<TObjectList>()where TObjectList : ScriptableObject
        {
            TObjectList data = CreateInstance<TObjectList>();

            var path = AssetDatabase.GenerateUniqueAssetPath($"{MasterBookDirectoryPath}{ typeof(TObjectList).Name}.asset");
            AssetDatabase.CreateAsset(data, path);
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

