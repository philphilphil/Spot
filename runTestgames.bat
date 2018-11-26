git pull
dotnet publish -c Release -r win10-x64


::for /f %%x in ('wmic path win32_localtime get /format:list ^| findstr "="') do set %%x
::set today=%Day%_%Month%_%YEAR%-%HOUR%%MINUTE%
::break>chep_vs_sf_%today%.pgn

::neat workaround to get commitid into a var
git log -n1 --format="%%h" > tmpFile 
set /p commitId= < tmpFile 
del tmpFile 

break>Testgames\%commitId%_20games_sf0.pgn

::sf level has to be set manually in cutechess-gui, seems not to work from cli
cutechess-cli.exe -rounds 20 -pgnout Testgames\%commitId%_20games_sf0.pgn -recover -engine conf=CHEP tc=inf -engine conf=sf9 tc=40/60 -each proto=uci