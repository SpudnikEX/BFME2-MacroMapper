//----------------------------------------------------------
//	World Builder Grouping	
//----------------------------------------------------------
	Side					 = Men
	EditorSorting			 = UNIT
	ThreatLevel				 = 4.0
	ThingClass				 = CHARACTER_UNIT
	BuildCost				 = M_CAH_BUILDCOST			   
	BuildTime				 = M_CAH_BUILDTIME	
	ShockwaveResistance 			 = M_SHOCKWAVE_RESISTANCE_STRONG		   
	TransportSlotCount		 = M_TRANSPORTSLOTCOUNT_HERO

;//	DisplayMeleeDamage		 = M_FARAMIR_DAMAGE
;//	DisplayRangedDamage		 = M_FARAMIR_BOW_DAMAGE
	
	VisionRange				 = M_CREATE_A_HERO_VISION_RANGE
	ShroudClearingRange		 = M_SHROUD_CLEAR_CREATE_A_HERO

	BountyValue				 = M_GONDOR_FARAMIR_BOUNTY_VALUE
	DisplayName				 = OBJECT:CreateAHero
	RecruitText				 = CONTROLBAR:CreateAHeroRecruit
	ReviveText				 = CONTROLBAR:CreateAHeroRevive
	Hotkey					 = CONTROLBAR:CreateAHeroHotkey
	CrusherLevel			 = 0			//Can I crush anything?
	MountedCrusherLevel		 = 1	 //Crush	level when mounted.
	CrushableLevel			 = 2		  //What	am I?:		  0	= for infantry,	1 =	for	trees, 2 = cavalry/heroes
	MountedCrushableLevel	 = 2   //Crusable level when	mounted.

	CrushWeapon				 = HeroCrush  

	MinCrushVelocityPercent	 = 50 //	Has	to be moving at	at least 50% of	full speed.
	CrushDecelerationPercent = 30 //	Lose xx	percent	of max velocity	when crushing.
	
	CrushKnockback			 = 40
	CrushZFactor			 = 1.0

	CommandSet				 = CreateAHeroCommandSet
	CommandPoints			 = M_CREATE_A_HERO_COMMAND_POINT_COST
	
