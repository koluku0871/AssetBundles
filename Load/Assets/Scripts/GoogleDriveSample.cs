using System;
using System.Collections.Generic;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using TMPro;
using UnityEngine;
public class GoogleDriveSample : MonoBehaviour
{
    /// <summary>
    /// キーのJSONファイル
    /// </summary>
    private const string JSON_FILE = "E:\\UnityProject\\AssetBundles\\Assets\\Resources\\Google\\graphic-charter-448804-u7-70967cff7bfb.json";
    /// <summary>
    /// Google DriveのフォルダーID
    /// </summary>
    private const string GOOGLE_DRIVE_FOLDER_ID = "1iK8lamB0BIbImrTOtP9N49uHjCZcgZjC";
    /// <summary>
    /// ダウンロードするファイルID
    /// </summary>
    private const string DOWNLOAD_FILE_ID = "[ダウンロードするファイルID]";
    /// <summary>
    /// ダウンロードファイル保存パス
    /// </summary>
    private const string SAVE_PATH = @"C:\sample";
    /// <summary>
    /// Google Driveのサービス
    /// </summary>
    private DriveService _driveService;
    private void Start()
    {
        // 認証情報を取得
        GoogleCredential credential;
        using (var stream = new FileStream(JSON_FILE, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.ScopeConstants.Drive);
        }
        // Drive APIのサービスを作成
        _driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Google Drive Sample",
        });
        Debug.Log("Service:" + _driveService.Name);

        List();
    }
    /// <summary>
    /// ファイルの一覧表示
    /// </summary>
    public void List()
    {
        try
        {
            List(_driveService);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// ファイルの一覧表示
    /// </summary>
    /// <param name="service">Google Driveのサービス</param>
    private void List(DriveService service)
    {
        Debug.Log("List start");
        // フォルダ内を検索する
        var request = service.Files.List();
        request.Q = "'" + GOOGLE_DRIVE_FOLDER_ID + "' in parents";
        // files(*) だとすべての情報が取得できる
        request.Fields = "nextPageToken, files(id, name, size, createdTime)";
        var files = new List<Google.Apis.Drive.v3.Data.File>();
        do
        {
            var result = request.Execute();
            files.AddRange(result.Files);
            request.PageToken = result.NextPageToken;
        } while (!string.IsNullOrEmpty(request.PageToken));
        // 結果を出力する
        foreach (var file in files)
        {
            Debug.Log("Name: " + file.Name + " ID: " + file.Id +
                             " Size: " + file.Size + "byte CreatedTime: " + file.CreatedTimeDateTimeOffset);
        }
        Debug.Log("List end");
    }
    /// <summary>
    /// ファイルのダウンロード
    /// </summary>
    public void Download()
    {
        try
        {
            Download(_driveService);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// ファイルのダウンロード
    /// </summary>
    /// <param name="service">Google Driveのサービス</param>
    private void Download(DriveService service)
    {
        Debug.Log("Download start");
        // メタデータを取得
        var file = service.Files.Get(DOWNLOAD_FILE_ID).Execute();
        if (file == null)
        {
            Debug.Log("fileがnull");
            return;
        }
        Debug.Log("Name: " + file.Name + " ID: " + file.Id);
        // ダウンロード
        var request = service.Files.Get(DOWNLOAD_FILE_ID);
        var fileStream = new FileStream(Path.Combine(SAVE_PATH, file.Name), FileMode.Create, FileAccess.Write);
        request.Download(fileStream);
        fileStream.Close();
        Debug.Log("Download end");
    }
}