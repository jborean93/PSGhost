Function Test-Function {
    [CmdletBinding(SupportsShouldProcess)]
    param ()

    if ($PSCmdlet.ShouldContinue("Should Test-Function continue", "Custom caption")) {
        "did continue"
    }
    else {
        "did not continue"
    }
}

Test-Function -Confirm
