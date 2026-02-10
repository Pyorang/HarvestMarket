
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public class WebGetTextTest : MonoBehaviour
{
    // HTTP 프로톨을 이용해서 웹 서버에게 데이터 작업을 요청할 수 있다.
    // 작업 요청은 크~게 4가지 약속이 있다.
    // 1. 데이터 내놔     : GET
    // 2. 데이터 줄게     : POST
    // 3. 데이터 수정해줘  : PUT
    // 4. 데이터 삭제해줘  : DELETE

    private async void Start()
    {
        await GetTextAsync();
    }

    private async UniTask<string> GetWebText(string url)
    {
        var request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return string.Empty;
        }

        return request.downloadHandler.text;
    }

    private async UniTask GetTextAsync()
    {
        string url = "https://www.google.com/search?q=zutomayo&oq=&gs_lcrp=EgZjaHJvbWUqCQgAECMYJxjqAjIJCAAQIxgnGOoCMgkIARAjGCcY6gIyCQgCECMYJxjqAjIJCAMQIxgnGOoCMgkIBBAjGCcY6gIyCQgFECMYJxjqAjIJCAYQIxgnGOoCMgkIBxAjGCcY6gLSAQg1NzVqMGoxNagCCLACAfEFeKWoA1nX--Y&sourceid=chrome&ie=UTF-8";
        var request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return;
        }

        Debug.Log(request.downloadHandler.text);
    }
}