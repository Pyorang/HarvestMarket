using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class WebGetImageText : MonoBehaviour
{
    public RawImage MyImage;

    // HTTP 프로톨을 이용해서 웹 서버에게 데이터 작업을 요청할 수 있다.
    // 작업 요청은 크~게 4가지 약속이 있다.
    // 1. 데이터 내놔     : GET
    // 2. 데이터 줄게     : POST
    // 3. 데이터 수정해줘  : PUT
    // 4. 데이터 삭제해줘  : DELETE

    private async void Start()
    {
        // URL이란 웹서버 어떤 "자원(텍스트 / 이미지 / 사운드 / 데이터 / API)"이 있는 위치를 가리키는 주소
        // URL 구성
        // 프로토콜 : http(s)://
        // 경로(주소) : placecats.com/neo_2/300/300 (함수 이름)
        // 쿼리 : ?fit=contain&position=right (함수 매개변수)
        //      - ?로 소직하고, &로 구분한다. (?키1=값1&키2=값2키3=값3)
        //      - fit=contain
        //      - position=right
        //      ㄴ 옵션인데.. 매번 다르므로 웹서버 개발자와 이야기를 잘 하거나 문서를 잘 봐야한다.

        await GetTextureAsync();
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

    private async UniTask GetTextureAsync()
    {
        var request = UnityWebRequestTexture.GetTexture("https://placecats.com/500/500");
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return;
        }

        Texture myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        MyImage.texture = myTexture;
    }
}

