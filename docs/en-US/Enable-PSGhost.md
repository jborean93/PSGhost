---
external help file: PSGhost.dll-Help.xml
Module Name: PSGhost
online version: https://github.com/jborean93/PSGhost/blob/main/docs/en-US/Enable-PSGhost.md
schema: 2.0.0
---

# Enable-PSGhost

## SYNOPSIS
Enables PSGhosts custom host.

## SYNTAX

```
Enable-PSGhost [<CommonParameters>]
```

## DESCRIPTION
Enables the custom PowerShell host in the current session.
This host implements a custom method for displaying progress output and choice selection using [Spectre.Console](https://spectreconsole.net/).

## EXAMPLES

### Example 1
```powershell
PS C:\> Enable-PSGhost
```

Enables the custom host for the current session globally.

## PARAMETERS

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### None
## NOTES
The host is enabled globally, use [Disable-PSGhost](./Disable-PSGhost.md) to disable the custom host.

## RELATED LINKS
