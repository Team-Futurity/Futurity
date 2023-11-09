public enum EnemyState : int
{
	Spawn,                  //����
	Idle,                   //���
	Default,

	MoveIdle,               //��� �� ���� �̵�
	Hitted,                 //�ǰ�
	Death,                  //���

	//Melee Default
	MDefaultChase,          //�߰�
	MDefaultAttack,         //����
	MDefaultAttack2nd,

	//Ranged Default
	RDefaultChase,
	RDefaultBackMove,
	RDefaultAttack,
	RDefaultDelay,

	//MinimalDefault
	MiniDefaultChase,
	MiniDefaultDelay,
	MiniDefaultAttack,
	MiniDefaultKnockback,

	//EliteDefault
	EliteDefaultChase,
	EliteMeleeAttack,
	EliteRangedAttack,

	//B_DF
	B_DFChase,
	B_DFAttack,

	//Tutorial
	TutorialIdle,

	//Production
	None,
	AutoMove,
}