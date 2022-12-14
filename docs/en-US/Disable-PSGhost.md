---
external help file: PSGhost.dll-Help.xml
Module Name: PSGhost
online version: https://github.com/jborean93/PSGhost/blob/main/docs/en-US/Disable-PSGhost.md
schema: 2.0.0
---

# Disable-PSGhost

## SYNOPSIS
Disables PSGhosts custom host.

## SYNTAX

```
Disable-PSGhost [<CommonParameters>]
```

## DESCRIPTION
Disables the custom PowerShell host in the current session as enabled by [Enable-PSGhost](./Enable-PSGhost.md).
This will revert the host back to the original PSHost provided by PowerShell.

## EXAMPLES

### Example 1
```powershell
PS C:\> Disable-PSGhost
```

Disables the custom PowerShell host provided by this module.

## PARAMETERS

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None
## OUTPUTS

### None
## NOTES

## RELATED LINKS
