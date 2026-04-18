# ToggleableTCATO

A KeePass 2 plugin to toggle Two-Channel Auto-Type Obfuscation in the middle of auto-typing, served as a [TCATO Placeholder](https://keepass.info/plugins.html#tcatoplh) drop-in replacement.

# Why use this

If the original doesn't work for some reason (it doesn't work for me).

# Usage

Toggle TCATO by putting `{TCATO:true}` and `{TCATO:false}` in auto-type sequences. For example, this will auto-type password without TCATO:

```
{USERNAME}{TAB}{TCATO:false}{PASSWORD}{ENTER}
```

# Build

This plugin is built with .NET Framework 4.8.1.

Download portable release of [KeePass 2](https://keepass.info/download.html).

Create `Directory.Build.props` in `ToggleableTCATO` and put the portable KeePass directory in `<KeePassDir>`:

```xml
<Project>
  <PropertyGroup>
    <KeePassDir>Your/KeePass/Dir</KeePassDir>
  </PropertyGroup>
</Project>
```

```sh
dotnet build -c Release
```

To start debugging, build the DLL in Debug configuration. Copy it to KeePass plugin directory and attach `KeePass.exe`. Alternatively in Visual Studio, start the project with executable `KeePass.exe`.
