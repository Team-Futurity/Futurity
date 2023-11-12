using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingSystem : MonoBehaviour
{
	[SerializeField, Header("씬 Fade 시간")]
	private float fadeTime = 1f;

	[SerializeField, Header("로딩 아이콘")]
	private LoadingIconMove loadIcon;

	private string nextScene = "";

	public Image img;
	public TMP_Text cctvText;
	public TMP_Text newsText;

	private List<string> zz = new List<string>()
	{
		"[속보] 케트로 연구소에서 의문의 에너지 폭발, 긴급 대피 방송",
		"태엽시 번화가 주변으로 통제선 구축",
		"케트로 연구소장의 사건 관련 발언에 전국 '주목'",
		"대통령, 국가 비상사태 선언",
		"태엽시 혼란 상황에 전국 '사재기 조짐'",
		"태엽시 내부 군경 '몰살'... 유족들의 반발 거세져",
		"'피해자 수 계산 불가하다'... 이어지는 '추모 물결'",
		"각국에서 이어지는 도움의 손길... 미국 '군 투입 결정'",
		"의문의 폭력 조직에 의해 태엽시 '초토화'",
		"태엽시 상공에 의문의 존재 등장? 전문가의 의견 '환각 효과 확실'",
		"태엽시 참사... 골든타임 놓쳤다? 12시간이 지난 지금의 태엽시는...",
		"사상자, 실종자 사실상 계산 불가하다고 밝혀",
		"폭발 사유는 케트로 연구소의 '무리한 연구 행위'?",
		"강 의원의 '감정 로봇은 가족이다.' 발언에 반발 심해",
		"역대 최다 출산율에 산부인과 성황... 병원만 '싱글벙글'",
		"새롭게 발전해 나가는 과학 연구의 메카, 태엽시로 오세요!",
		"''K 에너지는 Kㅔ트로와 함께!' 케트로서 새로운 에너지 발견해",
		"집값 상승에 시민들 '허리 휜다' 이 의원 '정책 마련하겠다.'",
		"학군, 교통, 인프라... 세 가지 다 잡았다! '태엽시의 성공 역사'",
		"새롭게 떠오르는 게임 대기업 '퓨처리티'... 성공 사유는?",
	};

	public void SetLoadData(LoadingData data)
	{
		if (data == null) return;

		img.sprite = data.cctveImage;
		cctvText.text = data.cctvText;
		newsText.text = zz[Random.Range(0, zz.Count)];
	}

	public void SetNextScene(string sceneName)
	{
		nextScene = sceneName;

		UIManager.Instance.RemoveAllWindow();

		FadeManager.Instance.FadeOut(fadeTime, () =>
		{
			AudioManager.Instance.CleanUp();

			SceneLoader.Instance.updateProgress?.AddListener(loadIcon.MoveIcon);
			SceneLoader.Instance.LoadSceneAsync(nextScene);
		});
	}
}
