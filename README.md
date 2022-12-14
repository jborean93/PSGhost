# PSGhost

[![Test workflow](https://github.com/jborean93/PSGhost/workflows/Test%20PSGhost/badge.svg)](https://github.com/jborean93/PSGhost/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/jborean93/PSGhost/branch/main/graph/badge.svg?token=b51IOhpLfQ)](https://codecov.io/gh/jborean93/PSGhost)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/jborean93/PSGhost/blob/main/LICENSE)

Enable a PowerShell host implementation that uses [Spectre.Console](https://spectreconsole.net/) for host methods like progress updates and choice selection.
This is a Proof of Concept module to see what is needed to utilise `Spectre.Console` in PowerShell.

See [about_PSGhost](docs/en-US/about_PSGhost.md) for more details.

## Requirements

These cmdlets have the following requirements

* PowerShell v7.2 or newer

## Installing

This module is meant to be a proof of concept and is not published to the PSGallery.
To try it out you can build the module locally and import it.
It requires the dotnet sdk to be available on the PATH.

```powershell
pwsh.exe -File build.ps1 -Configuration Debug -Task Build
Import-Module -Name ./output/PSGhost
Enable-PSGhost
```

## Contributing

Contributing is quite easy, fork this repo and submit a pull request with the changes.
To build this module run `.\build.ps1 -Task Build` in PowerShell.
To test a build run `.\build.ps1 -Task Test` in PowerShell.
This script will ensure all dependencies are installed before running the test suite.
