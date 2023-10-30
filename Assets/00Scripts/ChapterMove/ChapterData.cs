using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chapter Data", menuName = "ScriptableObject/Chapter Data", order = int.MaxValue)]
public class ChapterData : ScriptableObject
{
	[Header("현재 / 다음 챕터")] 
	[SerializeField] private EChapterType curChapter;
	[SerializeField] private string nextChapterNameSceneName;
	public EChapterType CurChapter => curChapter;
	public string NextChapterName => nextChapterNameSceneName;

	[Header("진입 시 활성화할 컷씬")] 
	[SerializeField] private ECutSceneType cutSceneType;
	[SerializeField] private float fadeOutTime = 0.8f;
	public ECutSceneType CutSceneType => cutSceneType;
	public float FadeOutTime => fadeOutTime;

	[Header("오브젝트 투시 여부")] 
	[SerializeField] private bool isObjectPenetrate;
	public bool IsPenetrate => isObjectPenetrate;
	
}
