# ToggleableTCATO

A Keepass plugin to toggle Two-Channel Auto-Type Obfuscation in the middle of auto-typing, served as a [TCATO Placeholder](https://keepass.info/plugins.html#tcatoplh) drop-in replacement.

# Why use this

If the original didn't work for some reason (it didn't work for me).

# Usage

Toggle TCATO by putting `{TCATO:true}` and `{TCATO:false}` in auto-type sequences. For example, this will auto-type password without TCATO:

```
{USERNAME}{TAB}{TCATO:false}{PASSWORD}{ENTER}
```

# Build

Create `Direcotry.Build.props` in the repo:

```xml
<Project>
  <PropertyGroup>
    <KeePassDir>Your/KeePass/Dir</KeePassDir>
  </PropertyGroup>
</Project>
```

```sh
dotnet build
```

To start debugging, build the DLL in Debug configuration. Copy it to KeePass plugin directory and attach `KeePass.exe`. Alternatively in Visual Studio, start the project with executable `KeePass.exe`.