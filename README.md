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

## Examples

Here are a few examples where PSGhost changes the implementation for certain host functions.
The scripts used to test these scenarios can be found in [Examples](./Examples/).

### Progress Records

A progress record with a percentage on builtin looks like

![Builtin Progress Percentage](https://s9.gifyu.com/images/prog-per-builtin.gif)

_Note: The stray line at the top is an artifact of asciinema_

This is `PSGhost`

![PSGhost Progress Percentage](https://s9.gifyu.com/images/prog-per-psghost.gif)

_Note: The bar and spinner are incorrectly generated in asciinema._

A progress record may also not have an explicit percentage indicator but rather embed it in the status description.
This is what this looks like on builtin

![Builtin No Percentage](https://s9.gifyu.com/images/prog-builtin.gif)

_Note: The stray line at the top is an artifact of asciinema_

This is `PSGhost`

![PSGhost No Percentage](https://s9.gifyu.com/images/prog-psghost.gif)

_Note: The bar and spinner are incorrectly generated in asciinema._

### Prompt For Choice

Here is what `$host.UI.PromptForChoice` looks like today

![Builtin Choice](https://s9.gifyu.com/images/choice-builtin.gif)

This is `PSGhost`

![PSGhost Choice](https://s9.gifyu.com/images/choice-psghost.gif)

It is also possible to have a prompt for multiple choices, this is the builtin version

![Builtin Choices](https://s9.gifyu.com/images/choices-builtin.gif)

This is `PSGhost`

![PSGhost Choices](https://s9.gifyu.com/images/choices-psghost.gif)

### Confirm

When `-Confirm` is used, it will bring up the choice dialog box.
This is what it looks like in builtin

![Builtin Confirm](https://s9.gifyu.com/images/confirm-builtin.gif)

This is `PSGhost`

![PSGhost Confirm](https://s9.gifyu.com/images/confirm-psghost.gif)

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
