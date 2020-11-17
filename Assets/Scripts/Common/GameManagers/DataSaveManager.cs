using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataSaveManager : MonoBehaviour
{
    //이하 코드는 서강 겜교의 '게임제작실습' 수업서 얻은 도관목 선배님의 코드를 그대로 적용했습니다.

    public static Dictionary<string, int> ownItemCount = new Dictionary<string, int>();

    List<string> readList;

    private void Awake()
    {
        ReadData("DB_Item.csv", ownItemCount);
    }
    public void ReadData(string _filename, Dictionary<string, int> _readDic)
    {
        string filepath = PathForDocumentsFile(_filename);

        if (File.Exists(filepath)) // 이 파일이 존재한다면
        {
            readList = ReadData_oldFile(filepath);
        }
        else // 파일이 존재하지 않다면(앱 설치 후 첫 실행 시에만 작동)
        {
            readList = ReadData_newFile(_filename);
        }

        for (int i = 0; i < readList.Count; i += 2)
        {
            _readDic.Add(readList[i].ToString(), int.Parse(readList[i + 1]));
        }
    }

    public static void WriteData(string _filename, Dictionary<string, int> _saveDic)
    {
        string path = PathForDocumentsFile(_filename);
        FileStream f = new FileStream(path, FileMode.Create, FileAccess.Write);

        StreamWriter writer = new StreamWriter(f);

        foreach (KeyValuePair<string, int> items in _saveDic)
            writer.WriteLine(items.Key + "," + items.Value);
        writer.Close();
        f.Close();
    }

    private static string PathForDocumentsFile(string _filename)
    {
        return Application.persistentDataPath + "/" + _filename;
    }

    private List<string> ReadData_oldFile(string _filePath)
    {
        FileStream fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
        StreamReader streamReader = new StreamReader(fileStream);

        string source = "";
        string[] divsource;
        List<string> divList = new List<string>();

        source = streamReader.ReadLine();

        while (source != null)
        {
            divsource = source.Split(',');
            divList.Add(divsource[0]);
            divList.Add(divsource[1]);
            source = streamReader.ReadLine();
        }

        streamReader.Close();
        fileStream.Close();

        return divList;
    }

    private List<string> ReadData_newFile(string _filename)
    {
        // LastIndexOf(char) : 뒤에서부터 검색하면서 첫 char 포함 뒤 문자열을 짤라준다.
        // SubString(index1, index2) : index1부터 index2의 직전 텍스트까지만 잘라서 반환한다.
        TextAsset data = Resources.Load("csvData/" + _filename.Substring(0, _filename.LastIndexOf('.')), typeof(TextAsset)) as TextAsset;
        if (data == null)
        {
            Debug.Log("3");
            return null;
        }
        StringReader stringReader = new StringReader(data.text);

        string source = "";
        string[] divsource;
        List<string> divList = new List<string>();

        source = stringReader.ReadLine();

        while (source != null)
        {
            divsource = source.Split(',');
            divList.Add(divsource[0]);
            divList.Add(divsource[1]);
            source = stringReader.ReadLine();
        }

        stringReader.Close();

        return divList;
    }
}
