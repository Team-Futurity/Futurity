using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllerMethod
{
	// 시스템을 가동하기 위해서 필요한 메서드 (최초 실행시 사용)
	// 시스템 라이프를 활성화 뿐만 아니라, 최초 할당에 필요한 정보들을 이 곳에서 실행
	void Active();

	// 일시중지된 시스템을 재작동하게끔 만드는 메서드
	void Run();
	
	// 실행중인 시스템을 일시중지 하기 위해 사용되는 메서드
	void Stop();
}
