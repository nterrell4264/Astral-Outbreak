Map Editor Controls

	Click: Use Brush
	Scroll: Zoom In/Out
	WASD: Pan the camera
	Shift: Hold while panning the camera to speed it up
	Enter: Save the map
	UP/DOWN: Change Cursor Size
	LEFT/RIGHT: Change Cursor Value
		Note: Cursor Value is only important for items at this moment however, it may be expanded to represent enemy level as a bonus goal.

	Numbers Change Brush Type
		1: Erase
		2: Wall
		3: DemonEnemy
		4: SlugEnemy
		5: Player Starting position
			Note: This will remove the old player starting position as there can only be one of these
		6: Upgrade Item
			-Cursor Value 1: Dash
			-Cursor Value 2-5: Unimplemented Upgrades/Extra Upgrades 

	IMPORTANT: The map editing tool will NOT override the current map.
				If you want to override the map you MUST MANUALY DRAG THE FILE INTO THE OTHER PROJECT.
				This is intentional and acts as a failsafe against accidently deleting the entire map and saving.

	Note: The Map Editor will open up the currently saved LevelMap file if it is avaialable (this is the file saved by the editor).
			If it cannot find a LevelMap.dat, it eill just grab the current map from the main game instead.