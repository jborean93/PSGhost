$choices = @(
    [System.Management.Automation.Host.ChoiceDescription]::new("Star Wars Episode I", "Midichlorians"),
    [System.Management.Automation.Host.ChoiceDescription]::new("Star Wars Episode II"),
    [System.Management.Automation.Host.ChoiceDescription]::new("Star Wars Episode III"),
    [System.Management.Automation.Host.ChoiceDescription]::new("Star Wars Episode IV", "OG"),
    [System.Management.Automation.Host.ChoiceDescription]::new("Star Wars Episode V"),
    [System.Management.Automation.Host.ChoiceDescription]::new("Star Wars Episode VI", "the one with the Ewoks"),
    [System.Management.Automation.Host.ChoiceDescription]::new("Star Wars Episode VII"),
    [System.Management.Automation.Host.ChoiceDescription]::new("Star Wars Episode VIII"),
    [System.Management.Automation.Host.ChoiceDescription]::new("Star Wars Episode IX", "Seriously?"),
    [System.Management.Automation.Host.ChoiceDescription]::new("Rogue One: A Star Wars Story"),
    [System.Management.Automation.Host.ChoiceDescription]::new("Solo: A Star Wars Story")
)
$idx = $host.UI.PromptForChoice("Favourite Star Wars Movie", "There's no right answer", $choices, 3)
"You chose $($choices[$idx].Label)"
