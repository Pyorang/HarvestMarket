# HarvestMarket - Project Overview

## Purpose
HarvestMarket is a Unity-based mobile idle/clicker farming game.
Players click farms to earn resources (eggs, meat, milk), upgrade farms, and experience fever modes.
Backend uses Firebase (auth + data persistence) with local fallback (PlayerPrefs).

## Tech Stack
- **Engine**: Unity (C#)
- **Backend**: Firebase (Auth, Realtime DB or Firestore)
- **Libraries**: DOTween (animations), Addressables (asset loading), TextMesh Pro, LeanPool (object pooling)
- **Platform**: Mobile (likely Android — google-services.json present)

## Architecture Pattern
- **OutGame** features follow a **layered architecture**: `1.Repository → 2.Domain → 3.Manager → 4.UI`
- **InGame** features are more traditional Unity MonoBehaviour-based with Manager pattern
- Interfaces used for repository abstraction (IRepository, IAccountRepository, ICurrencyRepository, etc.)

## Folder Structure (Assets)
```
Assets/
├── 01. Scenes/          — Unity scenes (Lobby, InGame, FireBaseTutorial)
├── 02. Scripts/         — All game scripts
│   ├── Core/            — Shared systems (Audio, UI utils, GameManager, Firebase init)
│   ├── InGame/          — In-game logic (Farm, Animal, Fever, Feedback, Environment, UI)
│   └── OutGame/         — Out-game logic (Account, Currency, Upgrade, UserData)
├── 03. Prefabs/         — Game prefabs (animals, food items, UI elements)
├── 04. Imported Asset/  — Third-party assets
├── 05. Fonts/           — Font files
├── 06. Music/           — Audio files
├── Resources/           — DOTween settings, test configs
└── (other Unity folders: Plugins, Firebase, Settings, etc.)
```

## Scenes
- **Lobby**: Main menu / login
- **InGame**: Core gameplay
- **FireBaseTutorial**: Firebase testing scene

## Code Style
- C# with Unity conventions
- PascalCase for classes, methods, properties
- Interfaces prefixed with 'I' (IRepository, IAudioChannel, IClickFeedback, etc.)
- Numbered folder prefixes for ordering (01. Scenes, 02. Scripts, etc.)
- Layered folder numbering in OutGame (1.Repository, 2.Domain, 3.Manager, 4.UI)
