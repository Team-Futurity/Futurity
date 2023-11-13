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
	D_BFChase,
	D_BFAttack,

	//M_JF
	M_JFChase,
	M_JFAttack,

	//Tutorial
	TutorialIdle,

	//Production
	None,
	AutoMove,
}