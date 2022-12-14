1..5 | ForEach-Object -Parallel {
    $timer = [System.Diagnostics.Stopwatch]::StartNew()
    $total = 50

    for ($i = 1; $i -lt $total; $i++) {
        Start-Sleep -Milliseconds (Get-Random -Maximum 300)

        $remaining = $total - $i
        $completed = $total - $remaining
        $percentage = $completed / $total * 100
        $average = $timer.Elapsed.TotalSeconds / $completed

        $record = @{
            Id              = $_
            Activity        = "Doing something $_"
            PercentComplete = $percentage
            # I can't see how to change the spectre seconds remaining info
            # SecondsRemaining = $remaining * $average
        }
        Write-Progress @record
    }
}
