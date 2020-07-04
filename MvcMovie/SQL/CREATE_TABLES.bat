@rem
@rem <CREATE_TABLES.bat>
@rem

:BEGIN
	rem echo off
	set CRTBL=CREATE_TABLES.txt
	if exist %CRTBL% for /f "delims= " %%I in (%CRTBL%) do mysql -u root -pp@ssw0rd moviedb < %%I

	pause

:EXIT
	echo on
goto :EOF
