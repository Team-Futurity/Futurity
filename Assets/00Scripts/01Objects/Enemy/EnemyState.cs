public enum EnemyState : int
{
	Spawn,                  //����
	Idle,                   //���
	Default,

	MoveIdle,               //��� �� ���� �̵�
	Hitted,                 //�ǰ�
	Death,                  //���

	ClusterSlow,
	ClusterChase,

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

	//Tutorial
	TutorialIdle,

}