1..5 | ForEach-Object -Parallel {
    $total = 50

    for ($i = 1; $i -lt $total; $i++) {
        Start-Sleep -Milliseconds (Get-Random -Maximum 300)

        $remaining = $total - $i
        $completed = $total - $remaining
        $percentage = $completed / $total * 100

        $record = @{
            Id       = $_
            Activity = "Doing something $_"
            Status   = "$percentage% Complete"
        }
        Write-Progress @record
    }

    Write-Progress -Activity "Doing something $_" -Id $_ -Completed
}
