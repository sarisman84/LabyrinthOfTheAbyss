# Input Manager

Handles all of the user's inputs as well as allows other systems to alter key binds.


Input Manager pseudo implementation:
```
class InputManager as Service
	keybindDataset as Keybind

	constructor
		fetch and initialize the current keybinds from a file

	getKeybind(key)
		return value of keybindDataset(key)
```

Key bind pseudo implementation:
```
struct Keybind
	rawInput as InputEvent
	icon as Sprite
```