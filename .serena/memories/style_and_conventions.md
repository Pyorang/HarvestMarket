# Code Style & Conventions

## Naming
- **Classes/Methods/Properties**: PascalCase (e.g., `FarmManager`, `GetReward()`)
- **Interfaces**: 'I' prefix (e.g., `IRepository`, `IAccountRepository`, `IAudioChannel`)
- **Private fields**: likely camelCase or _camelCase (Unity convention)

## Architecture
- **OutGame features**: Strict layered pattern — `1.Repository / 2.Domain / 3.Manager / 4.UI`
  - Repository: Data access (Firebase or local)
  - Domain: Data models, specifications, business logic
  - Manager: Orchestration layer
  - UI: Presentation
- **InGame features**: Feature-based folders (Animal, Farm, Fever, etc.) with Manager pattern
- **Core**: Shared utilities, audio system, UI helpers

## Patterns Used
- Repository pattern with interface abstraction (dual implementation: Firebase + Local)
- Specification pattern (AccountEmailSpecification, AccountPasswordSpecification)
- Object pooling (TextFeedbackPool, LeanPool)
- Strategy/Interface pattern for feedback (IClickFeedback)
- Observer-like patterns for Fever system

## Task Completion
- No automated linting/formatting/testing pipeline found
- Test in Unity Play Mode
- Verify Firebase integration if backend changes are made
