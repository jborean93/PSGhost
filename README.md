# PSGhost

[![Test workflow](https://github.com/jborean93/PSGhost/workflows/Test%20PSGhost/badge.svg)](https://github.com/jborean93/PSGhost/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/jborean93/PSGhost/branch/main/graph/badge.svg?token=b51IOhpLfQ)](https://codecov.io/gh/jborean93/PSGhost)
[![PowerShell Gallery](https://img.shields.io/powershellgallery/dt/PSGhost.svg)](https://www.powershellgallery.com/packages/PSGhost)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/jborean93/PSGhost/blob/main/LICENSE)

Enable a PowerShell host implementation that uses [Spectre.Console](https://spectreconsole.net/) for host methods like progress updates and choice selection.

See [about_PSGhost](docs/en-US/about_PSGhost.md) for more details.

## Requirements

These cmdlets have the following requirements

* PowerShell v7.2 or newer

## Installing

The easiest way to install this module is through
[PowerShellGet](https://docs.microsoft.com/en-us/powershell/gallery/overview).

You can install this module by running;

```powershell
# Install for only the current user
Install-Module -Name PSGhost -Scope CurrentUser

# Install for all users
Install-Module -Name PSGhost -Scope AllUsers
```

## Contributing

Contributing is quite easy, fork this repo and submit a pull request with the changes.
To build this module run `.\build.ps1 -Task Build` in PowerShell.
To test a build run `.\build.ps1 -Task Test` in PowerShell.
This script will ensure all dependencies are installed before running the test suite.