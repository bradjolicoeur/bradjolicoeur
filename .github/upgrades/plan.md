# .NET Upgrade to .NET 10 Plan

This document outlines the strategy and steps for upgrading the solution from .NET 9 to .NET 10.

## Table of Contents

1.  [Executive Summary](#1-executive-summary)
2.  [Migration Strategy](#2-migration-strategy)
3.  [Detailed Dependency Analysis](#3-detailed-dependency-analysis)
4.  [Project-by-Project Plans](#4-project-by-project-plans)
5.  [Package Update Reference](#5-package-update-reference)
6.  [Risk Management](#6-risk-management)
7.  [Testing & Validation Strategy](#7-testing--validation-strategy)
8.  [Source Control Strategy](#8-source-control-strategy)
9.  [Success Criteria](#9-success-criteria)

## 1. Executive Summary

This plan details the upgrade of 2 projects to .NET 10. The solution is classified as **Simple** due to its small size (2 projects, ~1500 LOC) and clear dependency structure.

- **Selected Strategy**: **All-At-Once Strategy**. All projects will be upgraded simultaneously in a single, coordinated operation. This approach is fastest for small solutions and avoids the complexity of managing intermediate states.
- **Key Findings**:
  - **1 incompatible package**: `Microsoft.VisualStudio.Azure.Containers.Tools.Targets`.
  - **3 breaking API changes** (2 binary, 1 source).
  - **8 NuGet packages** require upgrades.
- **Iteration Strategy**: A fast-batch approach will be used, with all details generated in a few iterations.

## 2. Migration Strategy

### Approach: All-At-Once

The **All-At-Once** strategy has been selected for this upgrade. Both projects in the solution will be updated to target .NET 10 simultaneously.

#### Rationale:

- **Small Solution Size**: With only 2 projects, a simultaneous upgrade is efficient and manageable.
- **Simple Dependencies**: The linear dependency graph minimizes the risk of complex build failures.
- **Low Project Complexity**: Both projects are marked as "low difficulty" in the assessment.
- **Efficiency**: This approach provides the fastest path to completing the upgrade.

#### Execution Principles:

1.  **Atomic Operation**: All project file modifications, package updates, and initial compilation fixes will be treated as a single, atomic operation.
2.  **Unified Build**: The entire solution will be restored and built at once after the changes are applied.
3.  **Comprehensive Testing**: All tests will be run after the solution successfully builds on .NET 10.

## 3. Detailed Dependency Analysis

The solution has a simple, linear dependency structure.

- `bradjolicoeur.web.csproj` (entry point project) depends on:
  - `bradjolicoeur.core.csproj` (class library)

`bradjolicoeur.core.csproj` has no other project dependencies, making it the leaf node in the dependency tree. There are no circular dependencies.

Given the **All-at-Once** strategy, all projects will be upgraded simultaneously. The dependency order is noted for context but does not dictate a phased rollout.

## 4. Project-by-Project Plans

### Project: `bradjolicoeur.core.csproj`

- **Current State**: .NET 9, 7 packages, 292 LOC
- **Target State**: .NET 10
- **Migration Steps**:
    1.  Update `TargetFramework` in the project file to `net10.0`.
    2.  Update package references as specified in the [Package Update Reference](#5-package-update-reference).
    3.  Address **1 source incompatible API change**:
        - `M:System.Uri.EscapeUriString(System.String)`
    4.  Compile and address any build errors.
    5.  Run tests to validate against **588 behavioral changes**, primarily related to `System.Net.Http.HttpContent` and `System.Uri`.

### Project: `bradjolicoeur.web.csproj`

- **Current State**: .NET 9, 11 packages, 1180 LOC
- **Target State**: .NET 10
- **Migration Steps**:
    1.  Update `TargetFramework` in the project file to `net10.0`.
    2.  Update package references as specified in the [Package Update Reference](#5-package-update-reference).
    3.  Address **2 binary incompatible API changes**:
        - `T:Microsoft.Extensions.DependencyInjection.ServiceCollectionExtensions`
        - `M:Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions.Configure''1(Microsoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.Configuration.IConfiguration)`
    4.  Handle the **incompatible package**: `Microsoft.VisualStudio.Azure.Containers.Tools.Targets`. This may involve finding a replacement or removing it if it's no longer needed for .NET 10.
    5.  Remove the `Microsoft.AspNetCore.Razor.Language` package, as its functionality is now included in the .NET SDK.
    6.  Compile and address any build errors.
    7.  Run tests to validate against **7 behavioral changes**.
    8.  Update `Dockerfile` to use a .NET 10 base image and ensure the container builds successfully.

## 5. Package Update Reference

The following package updates will be performed as part of the migration.

| Package | Current Version | Suggested Version | Projects Affected | Reason |
| :--- | :---: | :---: | :--- | :--- |
| **Microsoft.AspNetCore.Components** | 9.0.0 | 10.0.1 | `bradjolicoeur.web` | Upgrade Recommended |
| **Microsoft.Extensions.ApiDescription.Client** | 9.0.0 | 10.0.1 | `bradjolicoeur.core` | Upgrade Recommended |
| **Microsoft.Extensions.Configuration.Abstractions** | 9.0.0 | 10.0.1 | `bradjolicoeur.core` | Upgrade Recommended |
| **Microsoft.Extensions.Configuration.UserSecrets** | 9.0.0 | 10.0.1 | `bradjolicoeur.core` | Upgrade Recommended |
| **Microsoft.Extensions.DependencyInjection** | 9.0.0 | 10.0.1 | `bradjolicoeur.web` | Upgrade Recommended |
| **Microsoft.VisualStudio.Web.CodeGeneration.Design** | 9.0.0 | 10.0.1 | `bradjolicoeur.web` | Upgrade Recommended |
| **Newtonsoft.Json** | 13.0.3 | 13.0.4 | `bradjolicoeur.core` | Upgrade Recommended |
| **System.Text.Json** | 9.0.0 | 10.0.1 | `bradjolicoeur.web` | Upgrade Recommended |
| **Microsoft.AspNetCore.Razor.Language** | 6.0.36 | (remove) | `bradjolicoeur.web` | Included in Framework |
| **Microsoft.VisualStudio.Azure.Containers.Tools.Targets** | 1.21.0 | (investigate) | `bradjolicoeur.web` | ?? Incompatible |

## 6. Risk Management

The overall risk for this migration is **Low**.

| Risk | Description | Mitigation |
| :--- | :--- | :--- |
| **Incompatible Package** | The `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` package is incompatible. This could affect Docker container tooling integration. | **Investigate**: Check for an updated version or alternative package compatible with .NET 10. If none exists, the package may need to be removed and container support reconfigured. |
| **Binary Incompatible APIs** | Two breaking API changes in `bradjolicoeur.web` will cause build failures until fixed. | **Code Modification**: The execution phase will include specific steps to refactor the code using the new, correct APIs for .NET 10. |
| **Behavioral Changes** | 595 behavioral changes, mostly in `bradjolicoeur.core`, will not cause build errors but could lead to subtle runtime bugs. | **Thorough Testing**: The testing phase must include validation of application logic that relies on `System.Uri` and `System.Net.Http.HttpContent` to ensure behavior remains correct. |

## 7. Testing & Validation Strategy

A two-level testing strategy will be employed.

1.  **Build Verification**: After the atomic upgrade operation, the primary goal is to ensure the entire solution builds successfully without any compilation errors.
2.  **Runtime Validation**: Once the solution builds, a full suite of tests (if available) should be executed. Special attention should be paid to areas affected by the behavioral changes noted in the risk assessment. Manual smoke testing of the web application is recommended to confirm key features are working as expected.

## 8. Source Control Strategy

All changes for this upgrade will be committed to the `upgrade-to-NET10` branch. A single, comprehensive commit will be made after the atomic upgrade is complete and the solution builds successfully. This ensures the main branch remains stable and the upgrade can be reviewed as a single, cohesive change.

## 9. Success Criteria

The migration will be considered complete when:
- All projects in the solution target `net10.0`.
- All required package updates have been applied.
- The solution builds without any errors.
- All automated tests pass.
- The `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` incompatibility has been resolved.
- The application starts and key functionality is verified via manual smoke testing.
- The application's Docker image builds and runs successfully.
