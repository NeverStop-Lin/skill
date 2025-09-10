using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WeChatWASM;

public class Load : MonoBehaviour
{
    ConfigSelect ConfigSelect;

    // Start is called before the first frame update
    async void Start()
    {
        var query = WX.GetLaunchOptionsSync()?.query ?? new Dictionary<string, string>();
        ConfigSelect = await Addressables.LoadAssetAsync<ConfigSelect>("ConfigSelect").Task;
        ConfigSelectItem ConfigSelectItem = ConfigSelect.defaultConfig;

        Debug.Log("启动参数" + JsonConvert.SerializeObject(query));

        string configSelectName;
        if (query.TryGetValue("cs", out configSelectName))
        {
            var configSelectItem = ConfigSelect.otherConfig.Find((item) => item.name == configSelectName);
            if (configSelectItem != null)
            {
                ConfigSelectItem = configSelectItem;
            }
        }

        var cubeListName = ConfigSelectItem.LevelConfig.CubeListName;
        for (int i = 0; i < cubeListName.Count; i++)
        {
            await addCube(cubeListName[i], Vector3.left * (i - cubeListName.Count * 0.5f));
        }
    }

    public async Task addCube(string cubeName, Vector3 pos)
    {
        var cubeObject = await Addressables.InstantiateAsync(cubeName).Task;
        cubeObject.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
    }
}