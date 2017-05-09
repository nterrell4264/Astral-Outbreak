Map Editor Controls

	Click: Use Brush
	Scroll: Zoom In/Out
	WASD: Pan the camera
	Left Shift: Hold while panning the camera to speed it up. Stacks with control.
	Left Ctrl: Hold while panning the camera to increase speed. Stacks with shift.
	Enter: Save the map
	UP/DOWN: Change Cursor Size
	LEFT/RIGHT: Change Cursor Value
	Left CTRL, Left Shift, Delete: New Map button

	Numbers Change Brush Type
		1: Erase
		2: Wall
			-Cursor Value 0: Normal Wall 
				Note:	Old maps for testing purposes have many tile value 0 walls. 
						Cursor value 0 is therefore reserved to prevent compatabitlity issues.
			-Cursor Value 1: Normal Wall
			-Cursor Value 2: Platform
			-Cursor Value 3: Boss Gate
			-Cursor Value 4: Fire Block
			-Cursor Value 5: Hidden Passageway
			-Cursor Value 6: Defaults to normal Wall
				Note:	Not all brushes require the same amount of options, 
						so naturally some values like CURSOR VALUE 6 WALL don't have any purpose.
		3: DemonEnemy
			-Cursor Value 0: Bat
			-Cursor Value 1-6: JackRabbitDemon
		4: SlugEnemy
			-Cursor Value 0: Pod
			-Cursor Value 1-6: Slug
		5: Player Starting position
			Note: This will remove the old player starting position as there can only be one of these.
		6: Upgrade Item
			-Cursor Value 0: Health Upgrade
			-Cursor Value 1: Dash
			-Cursor Value 2: BatShield
			-Cursor Value 3: Multi-Shot
			-Cursor Value 4-6: Item that does nothing
		7: Boss
			-Cursor Value 0: Turret
			-Cursor Value 1: Dashing JackRabbit
			-Cursor Value 2: Bat Boss
			-Cursor Value 3: Multi-Shot jumping JackRabbit
			-Cursor Value 4: Core
			-Cursor Value 6: Escape Pod

	IMPORTANT: The map editing tool will NOT override the current map.
				If you want to override the map you MUST MANUALY DRAG THE FILE INTO THE OTHER PROJECT.
				This is intentional and acts as a failsafe against accidently deleting the entire map and saving.

	Note: The Map Editor will open up the currently saved LevelMap file if it is avaialable (this is the file saved by the editor).
			If it cannot find a LevelMap.dat, it will just grab the current map from the main game instead.

	Note: The level editor will automatically make a backup whenever you close it or make a new map.