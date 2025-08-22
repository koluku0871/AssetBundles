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
    /// �L�[��JSON�t�@�C��
    /// </summary>
    private const string JSON_FILE = "E:\\UnityProject\\AssetBundles\\Assets\\Resources\\Google\\graphic-charter-448804-u7-70967cff7bfb.json";
    /// <summary>
    /// Google Drive�̃t�H���_�[ID
    /// </summary>
    private const string GOOGLE_DRIVE_FOLDER_ID = "1iK8lamB0BIbImrTOtP9N49uHjCZcgZjC";
    /// <summary>
    /// �_�E�����[�h����t�@�C��ID
    /// </summary>
    private const string DOWNLOAD_FILE_ID = "[�_�E�����[�h����t�@�C��ID]";
    /// <summary>
    /// �_�E�����[�h�t�@�C���ۑ��p�X
    /// </summary>
    private const string SAVE_PATH = @"C:\sample";
    /// <summary>
    /// Google Drive�̃T�[�r�X
    /// </summary>
    private DriveService _driveService;
    private void Start()
    {
        // �F�؏����擾
        GoogleCredential credential;
        using (var stream = new FileStream(JSON_FILE, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.ScopeConstants.Drive);
        }
        // Drive API�̃T�[�r�X���쐬
        _driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Google Drive Sample",
        });
        Debug.Log("Service:" + _driveService.Name);

        List();
    }
    /// <summary>
    /// �t�@�C���̈ꗗ�\��
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
    /// �t�@�C���̈ꗗ�\��
    /// </summary>
    /// <param name="service">Google Drive�̃T�[�r�X</param>
    private void List(DriveService service)
    {
        Debug.Log("List start");
        // �t�H���_������������
        var request = service.Files.List();
        request.Q = "'" + GOOGLE_DRIVE_FOLDER_ID + "' in parents";
        // files(*) ���Ƃ��ׂĂ̏�񂪎擾�ł���
        request.Fields = "nextPageToken, files(id, name, size, createdTime)";
        var files = new List<Google.Apis.Drive.v3.Data.File>();
        do
        {
            var result = request.Execute();
            files.AddRange(result.Files);
            request.PageToken = result.NextPageToken;
        } while (!string.IsNullOrEmpty(request.PageToken));
        // ���ʂ��o�͂���
        foreach (var file in files)
        {
            Debug.Log("Name: " + file.Name + " ID: " + file.Id +
                             " Size: " + file.Size + "byte CreatedTime: " + file.CreatedTimeDateTimeOffset);
        }
        Debug.Log("List end");
    }
    /// <summary>
    /// �t�@�C���̃_�E�����[�h
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
    /// �t�@�C���̃_�E�����[�h
    /// </summary>
    /// <param name="service">Google Drive�̃T�[�r�X</param>
    private void Download(DriveService service)
    {
        Debug.Log("Download start");
        // ���^�f�[�^���擾
        var file = service.Files.Get(DOWNLOAD_FILE_ID).Execute();
        if (file == null)
        {
            Debug.Log("file��null");
            return;
        }
        Debug.Log("Name: " + file.Name + " ID: " + file.Id);
        // �_�E�����[�h
        var request = service.Files.Get(DOWNLOAD_FILE_ID);
        var fileStream = new FileStream(Path.Combine(SAVE_PATH, file.Name), FileMode.Create, FileAccess.Write);
        request.Download(fileStream);
        fileStream.Close();
        Debug.Log("Download end");
    }
}