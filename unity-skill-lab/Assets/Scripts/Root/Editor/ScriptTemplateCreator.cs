using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Root.Editor
{
    /*
     * [기능]
     * - 프로젝트 내부에서 관리되는 C# 스크립트 템플릿을 기반으로 새로운 스크립트를 생성하는 기능을 제공합니다.
     * - Unity의 `Assets/Create/C# Custom Script` 메뉴를 통해 실행할 수 있습니다.
     * - 특정 폴더를 선택한 경우, 해당 폴더에 스크립트가 생성되며, 선택하지 않은 경우 기본 경로(`Assets/Scripts/Root/`)에 생성됩니다.
     * - 동일한 이름의 스크립트가 존재하면 `NewScript`, `NewScript 1`, `NewScript 2`와 같은 방식으로 넘버링하여 중복을 방지합니다.
     * - 템플릿 파일(`Assets/Scripts/Root/Editor/DefaultScriptTemplate.txt`)을 기반으로 스크립트를 생성하며, 클래스명(`#SCRIPTNAME#`)을 입력한 파일명으로 자동 변경합니다.
     * - 템플릿 파일이 존재하지 않을 경우 오류 메시지를 출력하여 개발자가 즉시 문제를 인지할 수 있도록 합니다.
     *
     * [사용 방법]
     * - Unity Editor에서 `Assets` 폴더 내에서 우클릭 후 `Create` → `C# Custom Script`를 선택하면 실행됩니다.
     * - 파일명을 입력하면 해당 이름으로 새로운 C# 스크립트가 생성됩니다.
     *
     * [주의사항]
     * - 템플릿 파일(`DefaultScriptTemplate.txt`)이 존재해야 정상적으로 동작합니다.
     * - 파일명에는 공백 및 특수 문자가 포함될 수 없으며, 자동으로 변환됩니다.
     */
    public static class ScriptTemplateCreator
    {
        private const string TEMPLATE_PATH = "Assets/Scripts/Root/Editor/DefaultScriptTemplate.txt"; // 템플릿 파일 위치
        private const string SCRIPT_SAVE_PATH = "Assets/Scripts/Root/"; // 생성된 스크립트 저장 기본 경로
        private const string DEFAULT_SCRIPT_NAME = "NewScript"; // 기본 스크립트 이름

        /// <summary>
        /// 프로젝트 내부에서 관리되는 C# 스크립트 템플릿을 기반으로 새로운 스크립트를 생성합니다.
        /// </summary>
        /// <remarks>
        /// - Unity 메뉴(`Assets/Create/C# Custom Script`)에서 실행할 수 있습니다.
        /// - 특정 폴더를 선택하면 해당 폴더에, 선택하지 않으면 기본 경로에 저장됩니다.
        /// - 동일한 파일명이 존재할 경우 `NewScript`, `NewScript 1`, `NewScript 2`와 같은 방식으로 자동 넘버링됩니다.
        /// - 템플릿 파일이 존재하지 않으면 오류 메시지를 출력합니다.
        /// </remarks>
        /// <exception cref="System.IO.FileNotFoundException">템플릿 파일이 존재하지 않을 경우 발생</exception>
        [MenuItem("Assets/Create/C# Custom Script", false, 80)]
        public static void CreateCustomScript()
        {
            // 선택한 폴더 설정 (선택한 폴더가 없으면 기본 저장 경로 사용)
            string folderPath = SCRIPT_SAVE_PATH;
            if (Selection.activeObject != null)
            {
                string selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (Directory.Exists(selectedPath))
                {
                    folderPath = selectedPath + "/";
                }
            }

            // 사용자에게 파일 이름 입력 요청 (확장자는 제외)
            string scriptName = EditorUtility.SaveFilePanel("새 스크립트 생성", folderPath, DEFAULT_SCRIPT_NAME, "cs");
            if (string.IsNullOrEmpty(scriptName))
            {
                Debug.Log("스크립트 생성이 취소되었습니다.");
                return;
            }

            scriptName = Path.GetFileNameWithoutExtension(scriptName); // 확장자 제거

            // 파일 이름이 없으면 기본값 사용
            if (string.IsNullOrWhiteSpace(scriptName))
            {
                scriptName = DEFAULT_SCRIPT_NAME;
            }

            // C# 클래스명으로 유효한 이름으로 변경 (공백 및 특수 문자 제거)
            scriptName = Regex.Replace(scriptName, @"[^a-zA-Z0-9_]", ""); // 알파벳, 숫자, `_`만 허용
            scriptName = scriptName.Replace(" ", ""); // 공백 제거

            // 유니크한 파일명 생성 (이미 존재하는 경우 NewScript, NewScript 1, NewScript 2...)
            string uniquePath = AssetDatabase.GenerateUniqueAssetPath(folderPath + scriptName + ".cs");

            // 템플릿 파일 확인
            if (!File.Exists(TEMPLATE_PATH))
            {
                Debug.LogError($"템플릿 파일이 존재하지 않습니다: {TEMPLATE_PATH}");
                return;
            }

            // 템플릿 읽기 및 클래스명 대체
            string templateContent = File.ReadAllText(TEMPLATE_PATH);
            templateContent = templateContent.Replace("#SCRIPTNAME#", scriptName); // ✅ 입력한 파일명으로 클래스명 변경

            // 새 스크립트 파일 생성
            File.WriteAllText(uniquePath, templateContent);
            AssetDatabase.Refresh();

            Debug.Log($"새로운 스크립트 생성됨: {uniquePath}");
        }
    }
}
