using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class WebGetCardInfo : MonoBehaviour
{
    [Header("UI 연결 - 검색")]
    public TMP_InputField searchInputField;

    [Header("UI 연결 - 카드 슬롯(8개)")]
    public RawImage[] cardSlots = new RawImage[8];

    [Header("UI 연결 - 컨트롤 버튼")]
    public Button prevButton;
    public Button nextButton;

    [Header("UI 연결 - 확대 기능")]
    public GameObject zoomPanel;
    public RawImage bigCardImage;
    public TextMeshProUGUI flavorTextDisplay; // 부가 설명용 TMP
    public Button closePanelButton;

    [Header("UI 연결 - 마나 필터 버튼(8개)")]
    public Button[] manaButtons = new Button[8];

    [Header("API 설정")]
    public string clientId = "b7bbec6104e7459185fc2cd2637dcd22";
    public string clientSecret = "VRfPgBeQHgaaJlx7lvPsDLtylWcN9TqC";

    private string accessToken = "";
    private int currentPage = 1;
    private int totalPageCount = 0;
    private string currentSearchText = "";
    private int currentManaFilter = -1;

    void Start()
    {
        prevButton.onClick.AddListener(OnPrevButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
        closePanelButton.onClick.AddListener(() => zoomPanel.SetActive(false));

        if (searchInputField != null)
            searchInputField.onEndEdit.AddListener(OnSearchEndEdit);

        for (int i = 0; i < manaButtons.Length; i++)
        {
            int manaValue = i;
            manaButtons[i].onClick.AddListener(() => OnManaFilterClicked(manaValue));
        }

        prevButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        zoomPanel.SetActive(false);

        StartCoroutine(GetAccessTokenAndLoad());
    }

    void OnManaFilterClicked(int mana)
    {
        currentManaFilter = (currentManaFilter == mana) ? -1 : mana;
        currentPage = 1;
        StopAllCoroutines();
        StartCoroutine(LoadCards(currentPage, currentSearchText, currentManaFilter));
        UpdateManaButtonUI();
    }

    void OnSearchEndEdit(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || string.IsNullOrEmpty(text))
        {
            currentSearchText = text;
            currentPage = 1;
            StopAllCoroutines();
            StartCoroutine(LoadCards(currentPage, currentSearchText, currentManaFilter));
        }
    }

    IEnumerator GetAccessTokenAndLoad()
    {
        WWWForm form = new WWWForm();
        form.AddField("grant_type", "client_credentials");
        string auth = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

        using (UnityWebRequest request = UnityWebRequest.Post("https://oauth.battle.net/token", form))
        {
            request.SetRequestHeader("Authorization", "Basic " + auth);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                accessToken = JsonUtility.FromJson<TokenResponse>(request.downloadHandler.text).access_token;
                StartCoroutine(LoadCards(currentPage, "", -1));
            }
        }
    }

    IEnumerator LoadCards(int page, string searchQuery, int mana)
    {
        string encodedSearch = UnityWebRequest.EscapeURL(searchQuery);
        string url = $"https://kr.api.blizzard.com/hearthstone/cards?locale=ko_KR&set=legacy&class=neutral&pageSize=8&page={page}";

        if (!string.IsNullOrEmpty(searchQuery)) url += $"&textFilter={encodedSearch}";

        if (mana != -1)
        {
            if (mana >= 7)
            {
                url += $"&manaCost=7,8,9,10,12,20";
            }
            else
            {
                url += $"&manaCost={mana}";
            }
        }

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Bearer " + accessToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                CardListResponse response = JsonUtility.FromJson<CardListResponse>(request.downloadHandler.text);
                totalPageCount = response.pageCount;
                DisplayCards(response.cards);
                UpdateButtonStates();
            }
        }
    }

    void DisplayCards(List<CardData> cards)
    {
        List<CardData> validCards = new List<CardData>();
        foreach (var card in cards)
        {
            if (card != null && !string.IsNullOrEmpty(card.image))
                validCards.Add(card);
        }

        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].gameObject.SetActive(false);
            cardSlots[i].texture = null;
            Button btn = cardSlots[i].GetComponent<Button>();
            if (btn != null) btn.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < validCards.Count && i < cardSlots.Length; i++)
        {
            int index = i;
            // 데이터를 변수에 담아 클로저 문제를 확실히 방지합니다.
            CardData currentCard = validCards[index];

            StartCoroutine(DownloadTexture(currentCard.image, cardSlots[index]));

            Button btn = cardSlots[index].GetComponent<Button>();
            if (btn != null)
            {
                // 클릭 시 텍스트(flavorText)도 함께 넘겨줍니다.
                btn.onClick.AddListener(() => ShowZoomedImage(cardSlots[index].texture, currentCard.flavorText));
            }
        }
    }

    // 텍스트 매개변수 추가
    void ShowZoomedImage(Texture tex, string flavorText)
    {
        if (tex == null) return;
        bigCardImage.texture = tex;

        // 부가 설명 표시 (비어있으면 안내 문구)
        if (flavorTextDisplay != null)
        {
            flavorTextDisplay.text = string.IsNullOrEmpty(flavorText) ? "설명이 없는 카드입니다." : flavorText;
        }

        zoomPanel.SetActive(true);
    }

    IEnumerator DownloadTexture(string url, RawImage targetImage)
    {
        using (UnityWebRequest loader = UnityWebRequestTexture.GetTexture(url))
        {
            yield return loader.SendWebRequest();
            if (loader.result == UnityWebRequest.Result.Success)
            {
                targetImage.texture = DownloadHandlerTexture.GetContent(loader);
                targetImage.gameObject.SetActive(true);
            }
        }
    }

    void UpdateButtonStates()
    {
        prevButton.gameObject.SetActive(currentPage > 1);
        nextButton.gameObject.SetActive(currentPage < totalPageCount);
    }

    void UpdateManaButtonUI()
    {
        for (int i = 0; i < manaButtons.Length; i++)
        {
            ColorBlock cb = manaButtons[i].colors;
            cb.normalColor = (currentManaFilter == i) ? Color.yellow : Color.white;
            manaButtons[i].colors = cb;
        }
    }

    void OnPrevButtonClicked() { ChangePage(-1); }
    void OnNextButtonClicked() { ChangePage(1); }

    void ChangePage(int amount)
    {
        currentPage += amount;
        StartCoroutine(LoadCards(currentPage, currentSearchText, currentManaFilter));
    }
}

[System.Serializable] public class TokenResponse { public string access_token; }
[System.Serializable] public class CardListResponse { public List<CardData> cards; public int cardCount, pageCount, page; }

[System.Serializable]
public class CardData
{
    public int id;
    public string image;
    public string flavorText; // 부가 설명 필드 추가
}