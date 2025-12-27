$content = Get-Content "c:\src\bradjolicoeur\lighthouse-report\bradjolicoeur.com-20251227T104406.html" -Raw
if ($content -match 'window\.__LIGHTHOUSE_JSON__\s*=\s*({.*});') {
    $jsonStr = $matches[1]
    $json = $jsonStr | ConvertFrom-Json
    
    Write-Host "Category Scores:"
    $json.categories.PSObject.Properties | ForEach-Object {
        Write-Host "$($_.Name): $($_.Value.score)"
    }
    
    Write-Host "`nFailed Audits:"
    $json.audits.PSObject.Properties | ForEach-Object {
        $audit = $_.Value
        if ($audit.score -ne $null -and $audit.score -lt 1) {
            Write-Host "Audit: $($audit.id)"
            Write-Host "Score: $($audit.score)"
            Write-Host "Title: $($audit.title)"
            Write-Host "Description: $($audit.description)"
            Write-Host "DisplayValue: $($audit.displayValue)"
            Write-Host "--------------------------------------------------"
        }
    }
} else {
    Write-Host "Could not find Lighthouse JSON in the file."
}
