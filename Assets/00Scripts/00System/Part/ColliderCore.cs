using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCore : CoreAbility
{
	// ����, ��Ÿ, Ÿ��
	// ���� : �ð� Ȯ�� ��, ���� �̻� �ο� -> Player
	// ��Ÿ : ����� ��� ������ �ڸ��� ���� ���� ����, ���� �̻� �ο� -> Death Monster

	// ���� & ��Ÿ
	public float colliderRadius = .0f;
	public float colliderExecuteCycle = .0f;
	
	protected override void OnPartAbility(Enemy enemy)
	{
		
	}

	private void ExploreEnemy()
	{
		
	}
}
