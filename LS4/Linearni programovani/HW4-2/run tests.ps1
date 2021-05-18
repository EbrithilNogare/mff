$timeout = 300 # seconds
Get-ChildItem ".\tests" -Filter *.txt | 
Foreach-Object {
	$job = Start-Job -ScriptBlock { 
		$FullName = $($args[0])
		$Now = Get-Date

		cat $FullName | python .\HW4.py > coloring.data
		$output = ..\glpsol.exe -m ./coloring.mod -d ./coloring.data | findstr "#OUTPUT: "
		
		$output = $output.Substring(9)
		$time = NEW-TIMESPAN –Start $Now –End (Get-Date)
		echo "$output`t$($time.Seconds + 60*$time.Minutes).$($time.Milliseconds) s"
	}  -ArgumentList $_.FullName
	Wait-Job -Job $job -Timeout $timeout | Out-Null
	Stop-Job -Job $job
	$result = Receive-Job -Job $job
	Remove-Job -Job $job
	if([string]::IsNullOrEmpty($result)){
		echo "$($_.Basename)`t?`t>$timeout s"
	} else {
		echo "$($_.Basename)`t$($result)"
	}
}