using System.Collections.Generic;
using UnityEngine;

public class AssetsTest : MonoBehaviour
{
    [SerializeField]
    public List<Texture2D> textures = new List<Texture2D>();

    public UnityEngine.UI.Image image = null;

    // Start is called before the first frame update
    void Start()
    {
        string folderPath = "E:\\UnityProject\\CardGameImg\\AssetBundles\\StandaloneWindows64";
        string[] fs = System.IO.Directory.GetFiles(@folderPath, "*.assetbundle", System.IO.SearchOption.TopDirectoryOnly);
        foreach (string file in fs)
        {
            Debug.Log("folderPath : " + file);
            /*AssetBundle asset_bundle = AssetBundle.LoadFromFile(file);
            foreach (var asset in asset_bundle.LoadAllAssets())
            {
                Debug.Log("assetNames : " + asset.name);
            }*/
        }

        /*AssetBundle bundle = AssetBundle.LoadFromFile("E:\\UnityProject\\CardGameImg\\AssetBundles\\StandaloneWindows64\\bs_bs01_70_text.assetbundle");
        foreach (var asset in bundle.LoadAllAssets())
        {
            Debug.Log("assetNames : " + asset.name);
            TextAsset textAsset = (TextAsset)asset;
            Debug.Log("textAsset : " + textAsset.text);
        }

        /*AssetBundle bundle = AssetBundle.LoadFromFile("E:\\UnityProject\\CardGameImg\\AssetBundles\\StandaloneWindows64\\bs_bs01_70_spriteatlas.assetbundle");
        var request = bundle.LoadAssetAsync<SpriteAtlas>("BS_BS01_70_SpriteAtlas");
        request.completed += (operation) => {
            var atlas = (SpriteAtlas)request.asset;
            Debug.Log("spriteCount : " + atlas.spriteCount);
            image.sprite = atlas.GetSprite("BS01-001");
            /*var sprites = new Sprite[atlas.spriteCount];
            atlas.GetSprites(sprites);
            foreach (var sprite in sprites)
            {
                Debug.Log("sprite.name : " + sprite.name);
                this.textures.Add(sprite.texture);
            }*/
        //};
    }
}
