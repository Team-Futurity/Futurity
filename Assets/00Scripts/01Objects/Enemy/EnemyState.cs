public enum EnemyState : int
{
	Spawn,                  //스폰
	Idle,                   //대기
	Default,

	MoveIdle,               //대기 중 랜덤 이동
	Hitted,                 //피격
	Death,                  //사망

	ClusterSlow,
	ClusterChase,

	//Melee Default
	MDefaultChase,          //추격
	MDefaultAttack,         //공격
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