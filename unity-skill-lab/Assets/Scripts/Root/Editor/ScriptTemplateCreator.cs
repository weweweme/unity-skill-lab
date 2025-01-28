using System.IO;
using UnityEditor;
using UnityEngine;

namespace Root.Editor
{
    public static class ScriptTemplateCreator
    {
        private const string TEMPLATE_PATH = "Assets/Scripts/Root/Editor/DefaultScriptTemplate.txt"; // 템플릿 파일 위치
        private const string SCRIPT_SAVE_PATH = "Assets/Scripts/Root/"; // 생성된 스크립트 저장 기본 경로

        [MenuItem("Assets/Create/C# Custom Script", false, 80)]
        public static void CreateCustomScript()
        {
            // 선택한 폴더에 저장하도록 경로 설정
            string folderPath = SCRIPT_SAVE_PATH;
            if (Selection.activeObject != null)
            {
                string selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (Directory.Exists(selectedPath))
                {
                    folderPath = selectedPath + "/";
                }
            }

            // 파일명 설정 및 고유 경로 생성
            string scriptName = "NewScript";
            string uniquePath = AssetDatabase.GenerateUniqueAssetPath(folderPath + scriptName + ".cs");

            // 템플릿 파일이 존재하는지 확인
            if (!File.Exists(TEMPLATE_PATH))
            {
                Debug.LogError($"템플릿 파일이 존재하지 않습니다: {TEMPLATE_PATH}");
                return;
            }

            // 템플릿 읽기 및 클래스명 대체
            string templateContent = File.ReadAllText(TEMPLATE_PATH);
            templateContent = templateContent.Replace("#SCRIPTNAME#", Path.GetFileNameWithoutExtension(uniquePath));

            // 새 스크립트 파일 생성
            File.WriteAllText(uniquePath, templateContent);
            AssetDatabase.Refresh();

            Debug.Log($"새로운 스크립트 생성됨: {uniquePath}");
        }
    }
}
