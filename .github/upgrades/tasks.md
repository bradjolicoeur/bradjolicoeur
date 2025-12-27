# .NET 10 Upgrade Tasks

## Overview

This document tracks the execution of the upgrade from .NET 9 to .NET 10. Both projects will be upgraded simultaneously in a single atomic operation, followed by testing and validation.

**Progress**: 0/2 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [▶] TASK-001: Atomic framework and dependency upgrade with compilation fixes
**References**: Plan §4 (Project-by-Project Plans), Plan §5 (Package Update Reference), Plan §6 (Risk Management)

- [✓] (1) Update `TargetFramework` to `net10.0` in both project files: `bradjolicoeur.core.csproj` and `bradjolicoeur.web.csproj`
- [✓] (2) Both projects target `net10.0` (**Verify**)
- [✓] (3) Update package references per Plan §5 Package Update Reference (8 packages: Microsoft.AspNetCore.Components 10.0.1, Microsoft.Extensions.ApiDescription.Client 10.0.1, Microsoft.Extensions.Configuration.Abstractions 10.0.1, Microsoft.Extensions.Configuration.UserSecrets 10.0.1, Microsoft.Extensions.DependencyInjection 10.0.1, Microsoft.VisualStudio.Web.CodeGeneration.Design 10.0.1, Newtonsoft.Json 13.0.4, System.Text.Json 10.0.1)
- [✓] (4) Remove `Microsoft.AspNetCore.Razor.Language` package from `bradjolicoeur.web.csproj` (included in framework)
- [✓] (5) Investigate and resolve `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` incompatibility per Plan §6 Risk Management (find compatible version, alternative, or remove)
- [✓] (6) All package references updated and incompatibility resolved (**Verify**)
- [▶] (7) Restore all dependencies with `dotnet restore`
- [ ] (8) All dependencies restored successfully (**Verify**)
- [✓] (9) Build solution and fix all compilation errors addressing breaking changes per Plan §4 (bradjolicoeur.core: System.Uri.EscapeUriString API change; bradjolicoeur.web: ServiceCollectionExtensions and OptionsConfigurationServiceCollectionExtensions API changes)
- [✓] (10) Solution builds with 0 errors (**Verify**)
- [✓] (11) Update `Dockerfile` to use .NET 10 base image
- [✓] (12) Docker image builds successfully (**Verify**)
- [▶] (13) Commit changes with message: "TASK-001: Complete atomic upgrade to .NET 10"

---

### [ ] TASK-002: Run full test suite and validate upgrade
**References**: Plan §7 (Testing & Validation Strategy), Plan §4 (Project-by-Project Plans - behavioral changes)

- [ ] (1) Run all automated tests (if test projects exist)
- [ ] (2) Fix any test failures related to behavioral changes per Plan §4 (595 behavioral changes: 588 in bradjolicoeur.core affecting System.Net.Http.HttpContent and System.Uri; 7 in bradjolicoeur.web)
- [ ] (3) Re-run tests after fixes
- [ ] (4) All tests pass with 0 failures (**Verify**)
- [ ] (5) Start application and verify it runs successfully
- [ ] (6) Application starts without errors (**Verify**)
- [ ] (7) Verify Docker container runs successfully
- [ ] (8) Docker container runs without errors (**Verify**)
- [ ] (9) Commit test fixes with message: "TASK-002: Complete testing and validation"

---










