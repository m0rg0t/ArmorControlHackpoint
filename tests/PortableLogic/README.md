# Portable source regression checks

Run with .NET SDK 10.0.x (validated with 10.0.401):

```sh
dotnet run --project tests/PortableLogic/PortableLogic.csproj --configuration Release
```

Links the actual Phone AccelerationItem and HitItem sources, compiled as C# 5. Exercises 11 checks: valid frames, malformed second/final axes, null/empty/short frames, later recovery, property notifications and the existing current-culture numeric behavior. The original source fails six checks; the patched source passes all 11. Parsing all three values before assigning prevents a malformed frame from mixing a new X/Y with old axes. This is not a thread-safety guarantee: notifications still occur in the original X/Y/Z order. The device protocol, current-culture parsing, ignored extra fields and nonfinite-value policy are intentionally unchanged; firmware/protocol validation is a separate task.

## Scope and dependencies

Original Windows/Phone 8.1 and Phone 8 targets, TCD.Controls 3.0.1, MVVM Light 5.0.1 and Newtonsoft.Json 6.0.5 are retained. The test host has no external NuGet packages; its NuGet.Config clears package sources. Minimal test-only MVVM Light notification and platform type adapters do not reproduce the historic implementations. No whole-app build, XAML binding execution, original binary/runtime compatibility, device, location service or Bluetooth operation was performed. All fixtures are synthetic; no live HTTP, credentials or user data are used.

Microsoft documents that Phone 8.x and Store 8/8.1 projects require historical tooling: https://learn.microsoft.com/en-us/visualstudio/releases/2022/port-migrate-and-upgrade-visual-studio-projects . Newtonsoft.Json 13.0.4 metadata reports computed Phone compatibility, but this is not a validated restore/build for these specific projects: https://www.nuget.org/packages/Newtonsoft.Json/13.0.4 . No blanket incompatibility claim is made. Per-target package asset resolution and a matching historical SDK baseline are required before replacing existing assemblies; no modern platform migration is included.

CI is read-only, uses pinned actions, disables persisted checkout credentials and executes this harness only. Green CI is not a complete historical application validation.
