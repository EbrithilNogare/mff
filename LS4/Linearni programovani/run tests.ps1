Get-ChildItem ".\tests" -Filter *.txt | 
Foreach-Object {

	$Now = Get-Date

	cat $_.FullName |.\HW4.py > out.mod
	$output = .\glpsol.exe -m ./out.mod | findstr "result: "
	$output = $output.Substring(8)
	echo "$($_.Basename)`t$output`t$((NEW-TIMESPAN –Start $Now –End (Get-Date)).Seconds).$((NEW-TIMESPAN –Start $Now –End (Get-Date)).Milliseconds)s"

}