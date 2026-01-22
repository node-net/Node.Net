# Implementation Plan: Logs Page

**Branch**: `003-logs-page` | **Date**: 2025-01-12 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/003-logs-page/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

Implement a reusable Logs Page component that provides a user interface for viewing, searching, filtering, and managing application log entries. The component integrates with Microsoft.Extensions.Logging to automatically capture application logs and stores them in a LiteDB database. The feature includes a Blazor Razor component for display, a service layer for CRUD operations and search, and configuration methods for integrating with Microsoft.Extensions.Logging. The implementation follows library-first design principles and supports multiple .NET target frameworks.

## Technical Context

**Language/Version**: C# (Latest language version)  
**Primary Dependencies**: 
- Microsoft.Extensions.Logging (for automatic log capture)
- LiteDB 5.0.17 (for log entry persistence - already in project)
- Microsoft.FluentUI.AspNetCore.Components 4.13.2 (for UI components - already in project, net8.0+ only)
- Serilog structured logging format compatibility (no direct dependency, format compatibility only)

**Storage**: LiteDB database file at `APPLICATION_DATA_DIRECTORY/log.db`  
**Testing**: NUnit (as per constitution)  
**Target Platform**: .NET multi-target (net48, net8.0, net8.0-windows)  
**Project Type**: Library component (reusable across applications)  
**Performance Goals**: 
- Page load: <2 seconds (SC-001)
- Search results: <1 second (SC-002)
- Support 1000+ log entries with pagination (SC-004)
- Capture 95%+ of Microsoft.Extensions.Logging messages (SC-005)

**Constraints**: 
- Must work in both server-side (ASP.NET Host) and client-side (WebAssembly) environments
- Component must be reusable and self-contained
- Must follow TDD (tests before implementation)
- Razor components excluded from net48 builds (Fluent UI requires .NET 6+)
- Must maintain API stability and versioning

**Scale/Scope**: 
- Single reusable component (`Node.Net/Components/Logs.razor`)
- Service layer (`Node.Net/Service/Logging/`)
- Integration with two example applications
- Support for structured logging data compatible with Serilog format

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

Verify compliance with Node.Net Constitution principles:

- **I. Library-First Design**: ✅ Feature is designed as reusable library component (`Node.Net/Components/Logs.razor` and `Node.Net/Service/Logging/`) with clear purpose - log viewing and management
- **II. Multi-Targeting Support**: ✅ Plan addresses net48, net8.0, and net8.0-windows targets appropriately. Razor components excluded from net48 (Fluent UI requires .NET 6+), service layer supports all targets
- **III. Test-First Development**: ✅ Test strategy defined - unit tests for service layer, component tests for UI, integration tests for Microsoft.Extensions.Logging integration
- **IV. API Stability & Versioning**: ✅ Public API changes documented - new public interfaces (`ILogService`) and component (`Logs.razor`) will be part of next MINOR version increment
- **V. Cross-Platform Compatibility**: ✅ Platform-specific code identified - LiteDB works cross-platform, no platform-specific code required for this feature

**Status**: [x] All principles satisfied | [ ] Violations documented below

## Project Structure

### Documentation (this feature)

```text
specs/003-logs-page/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
source/Node.Net/
├── Components/
│   └── Logs.razor        # Blazor component for displaying log entries (net8.0+ only)
├── Service/
│   └── Logging/
│       ├── LogEntry.cs           # C# class compatible with Serilog structured logging
│       ├── ILogService.cs        # Interface for CRUD operations and search
│       └── LogService.cs         # Concrete implementation backed by LiteDB
└── [existing structure...]

tests/Node.Net.Test/
├── Service/
│   └── Logging/
│       ├── LogEntry.Tests.cs
│       ├── LogService.Tests.cs
│       └── LogService.Integration.Tests.cs
└── Components/
    └── Logs.Tests.cs

examples/Node.Net.AspNet.Host/
└── Components/Pages/
    └── Logs.razor        # Integration page using Node.Net.Components.Log

examples/Node.Net.Wasm/
└── Pages/
    └── Logs.razor        # Integration page using Node.Net.Components.Log
```

**Structure Decision**: Following existing project patterns:
- Services organized under `source/Node.Net/Service/[Feature]/`
- Components organized under `source/Node.Net/Components/`
- Tests mirror source structure in `tests/Node.Net.Test/`
- Example applications integrate components in their respective page structures

## Phase Completion Status

### Phase 0: Outline & Research ✅

**Status**: Complete  
**Output**: `research.md`

**Research Topics Resolved**:
1. ✅ Serilog structured logging format compatibility
2. ✅ Microsoft.Extensions.Logging integration pattern
3. ✅ LiteDB schema and query patterns
4. ✅ Blazor component structure and Fluent UI integration
5. ✅ Editability and read-only enforcement
6. ✅ WebAssembly compatibility

All technical unknowns resolved. No NEEDS CLARIFICATION markers remain.

### Phase 1: Design & Contracts ✅

**Status**: Complete  
**Outputs**: 
- `data-model.md` - Entity definitions and data flow
- `contracts/ILogService.md` - Service interface specification
- `quickstart.md` - Integration scenarios and test cases

**Design Artifacts Created**:
1. ✅ LogEntry entity model with all required properties
2. ✅ ILogService interface contract with method specifications
3. ✅ Integration scenarios for all user stories
4. ✅ Agent context updated with new technologies

**Constitution Check Post-Design**: ✅ All principles still satisfied

### Phase 2: Task Generation

**Status**: Pending  
**Next Command**: `/speckit.tasks`  
**Output**: `tasks.md` - Actionable, dependency-ordered task list

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

No violations - all constitution principles satisfied.
