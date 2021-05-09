Get-ChildItem ".\tests" -Filter *.txt | 
Foreach-Object {
	$Now = Get-Date
	cat $_.FullName |.\HW4.py > coloring.data
	$output = .\glpsol.exe -m ./coloring.mod -d ./coloring.data | findstr "#OUTPUT: "
	$output = $output.Substring(9)
	echo "$($_.Basename)`t$output`t$((NEW-TIMESPAN –Start $Now –End (Get-Date)).Seconds).$((NEW-TIMESPAN –Start $Now –End (Get-Date)).Milliseconds)s"
}