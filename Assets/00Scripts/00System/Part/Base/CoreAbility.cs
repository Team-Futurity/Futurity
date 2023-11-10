using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackCoreType
{
	NONE = 0,
	
	ADD_DAMAGE,
	ADD_ODD_STATE,
	
	MAX
}

public abstract class CoreAbility : MonoBehaviour
{
	// Core Type : Attack, Collider
	// Attack Type : Status or Damage

	// ���� �̻� : Buff System ���� Ȱ��

	// Attack
	// ���� ����, PlayerController Update�� �޾Ƽ�

	// Core 
	// Player Controller
	// - ���� ���� ���� - ����, �ο�
	// - ����� ���� ���� ���� ���� 

	// Collider
	// - �ݶ��̴� ���� ( ������, ���� ���ط�, ����, �ð� ) -? ���� ���� ����??
	
	// protected Buff

	public void ExecutePart(UnitBase enemy)
	{
		OnPartAbility(enemy);
	}

	protected abstract void OnPartAbility(UnitBase enemy);
}