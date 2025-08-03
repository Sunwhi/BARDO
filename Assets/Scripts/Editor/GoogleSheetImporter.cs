using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class GoogleSheetImporter : EditorWindow
{
    private readonly string exportId = "2PACX-1vRhc6EN_BylM8-kME3E-2-1HJOv-Fue-OADnszUloNoF_2j7DrT1GVC2L81Aqvi-JT6hH8r_j5tvwr7";
    private List<string> sheetNames = new();
    private Dictionary<string, bool> sheetSelection = new();
    private Vector2 scroll;

    [MenuItem("Tools/Google Sheet Importer")]
    public static void ShowWindow()
    {
        GetWindow<GoogleSheetImporter>("Google Sheet Importer");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("시트 불러오기"))
        {
            _ = FetchSheetNames();
        }

        if (sheetNames.Count > 0)
        {
            EditorGUILayout.LabelField("변환할 시트를 선택하세요:");
            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var name in sheetNames)
            {
                if (!sheetSelection.ContainsKey(name)) sheetSelection[name] = false;
                sheetSelection[name] = EditorGUILayout.ToggleLeft(name, sheetSelection[name]);
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("선택된 시트 변환 및 저장"))
            {
                _ = DownloadAndConvertSheets();
            }
        }
    }


    #region Import Sheets
    private async Task FetchSheetNames()
    {
        string htmlUrl = $"https://docs.google.com/spreadsheets/d/e/{exportId}/pubhtml";
        UnityWebRequest req = UnityWebRequest.Get(htmlUrl);
        await req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("시트 목록 HTML 요청 실패");
            return;
        }

        string html = req.downloadHandler.text;
        var matches = Regex.Matches(html, @"items\.push\(\{name: \""([^\""]+)\""");

        sheetNames.Clear();
        sheetSelection.Clear();

        foreach (Match match in matches)
        {
            string sheetName = match.Groups[1].Value;
            sheetNames.Add(sheetName);
            sheetSelection[sheetName] = false;
        }

        if (sheetNames.Count == 0)
            Debug.LogWarning("시트가 0개로 감지됨. 웹 게시 설정을 다시 확인.");
    }

    private async Task DownloadAndConvertSheets()
    {
        foreach (var pair in sheetSelection)
        {
            if (!pair.Value) continue;
            string sheetName = pair.Key;
            string csvUrl = $"https://docs.google.com/spreadsheets/d/e/{exportId}/pub?output=csv&sheet={sheetName}";

            UnityWebRequest csvReq = UnityWebRequest.Get(csvUrl);
            await csvReq.SendWebRequest();

            if (csvReq.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{sheetName} 다운로드 실패");
                continue;
            }

            string csv = csvReq.downloadHandler.text;
            string json = sheetName == "QuestData"
                ? ConvertToStructuredQuestJson(csv)
                : ConvertCsvToPlainJson(csv);
            SaveJson(sheetName, json);
        }

        AssetDatabase.Refresh();
        Debug.Log("모든 선택된 시트 처리 완료");
    }
    
    private void SaveJson(string sheetName, string json)
    {
        string dirPath = Path.Combine(Application.dataPath, "Resources");
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
        dirPath = Path.Combine(dirPath, "Json");
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

        string path = Path.Combine(dirPath, sheetName + ".json");
        File.WriteAllText(path, json);
        Debug.Log($"{sheetName} 저장 완료: {path}");
    }
    #endregion

    #region Convert Zone
    private string ConvertCsvToPlainJson(string csvText)
    {
        var lines = csvText.Split('\n').Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        var headers = lines[0].Split(',');

        var dataList = new List<Dictionary<string, string>>();

        for (int i = 1; i < lines.Length; i++)
        {
            var row = lines[i].Split(',');
            if (row.Length != headers.Length) continue;

            var dict = new Dictionary<string, string>();
            for (int j = 0; j < headers.Length; j++)
            {
                dict[headers[j]] = row[j];
            }
            dataList.Add(dict);
        }

        return JsonConvert.SerializeObject(dataList, Formatting.Indented);
    }

    private string ConvertToStructuredQuestJson(string csvText)
    {
        var lines = csvText.Split('\n').Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        var header = lines[0].Split(',');

        int idxQuestID = System.Array.IndexOf(header, "QuestID");
        int idxQuestTitle = System.Array.IndexOf(header, "QuestTitle");
        int idxSubQuestID = System.Array.IndexOf(header, "SubQuestID");
        int idxSubQuestName = System.Array.IndexOf(header, "SubQuestName");

        var flatList = new List<(int questID, string questTitle, int subID, string subName)>();

        for (int i = 1; i < lines.Length; i++)
        {
            var tokens = lines[i].Split(',');
            if (tokens.Length < 4) continue;
            flatList.Add((
                int.Parse(tokens[idxQuestID]),
                tokens[idxQuestTitle],
                int.Parse(tokens[idxSubQuestID]),
                tokens[idxSubQuestName]
            ));
        }

        var grouped = flatList.GroupBy(e => e.questID);
        var questList = new List<QuestData>();

        foreach (var group in grouped)
        {
            var quest = new QuestData
            {
                QuestID = group.Key,
                QuestTitle = group.First().questTitle,
                SubQuests = group.Select(e => new SubQuestData
                {
                    SubQuestID = e.subID,
                    SubQuestName = e.subName,
                    isCompleted = false
                }).ToList()
            };
            questList.Add(quest);
        }

        return JsonConvert.SerializeObject(questList, Formatting.Indented);
    }
    #endregion
}