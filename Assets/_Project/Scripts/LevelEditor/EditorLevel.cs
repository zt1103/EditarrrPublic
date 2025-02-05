using System;
using System.IO;
using LevelEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CorgiExtension
{
    public class EditorLevel : MonoBehaviour
    {
        public static event Action<string> OnEditorLevelUpload;
        public static event Action<string> OnLeaderboardRequest;

        public Text Title;
        public Text Creator;
        public RawImage ScreenshotImage;
        public Transform EditButton;
        public Transform UploadButton;
        public Transform DeleteButton;
        private string _code = "";

        public void DeleteLevel()
        {
            EditorLevelStorage.Instance.DeleteLevel(_code);
        }

        public void UploadLevel()
        {
            OnEditorLevelUpload?.Invoke(_code);
        }

        public void GoToEditorLevel()
        {
            CheckCodePreferences();
            LevelManager.Instance.GotoLevel("EditorCreate");
        }

        private void CheckCodePreferences()
        {
            if (_code.Length > 0)
            {
                PlayerPrefs.SetString("EditorCode", _code);
            }
            else
            {
                PlayerPrefs.SetString("EditorCode", "");
            }
        }

        public void GetLevelLeaderboard()
        {
            OnLeaderboardRequest?.Invoke(_code);
            EditorLevelStorage.Instance.GetLevelScores(_code);
        }

        public void GoToReplay()
        {
            CheckCodePreferences();
            LevelManager.Instance.GotoLevel("EditorReplay");
        }

        public void GoToLevel()
        {
            CheckCodePreferences();
            LevelManager.Instance.GotoLevel("EditorTest");
        }

        public void HideEditorTools()
        {
            if (EditButton)
            {
                EditButton.gameObject.SetActive(false);
            }

            if (UploadButton)
            {
                UploadButton.gameObject.SetActive(false);
            }
        }

        public void HideDeleteButton()
        {
            if (DeleteButton)
            {
                DeleteButton.gameObject.SetActive(false);
            }
        }

        public void SetTitle(string title)
        {
            Title.text = title.ToUpper();
            _code = title.ToLower();
        }

        public void SetCreator(string creator)
        {
            Creator.text = creator.ToUpper();
        }

        public void SetScreenshot(string code)
        {
            // create texture from image file
            string path = $"{EditorLevelStorage.ScreenshotStoragePath}{code}.png";

            bool isDistroLevel = false;
            if (!File.Exists(path) && File.Exists($"{EditorLevelStorage.DistroLevelStoragePath}{code}.png"))
            {
                path = $"{EditorLevelStorage.DistroLevelStoragePath}{code}.png";
                isDistroLevel = true;
            }

            if (isDistroLevel)
            {
                HideDeleteButton();
            }

            if (ScreenshotImage && File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);

                ScreenshotImage.texture = tex;
                ScreenshotImage.color = Color.white;
            }

        }
    }
}
