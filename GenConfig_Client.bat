Cd /d %~dp0
echo %CD%

@REM set WORKSPACE=.

@REM set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
@REM set CONF_ROOT=%WORKSPACE%\Config\Excel\GameConfig

@REM ::Client
@REM dotnet %LUBAN_DLL% ^
@REM     --customTemplateDir CustomTemplate ^
@REM     -t Client ^
@REM     -c cs-bin ^
@REM     -d bin ^
@REM     --conf %CONF_ROOT%\__luban__.conf ^
@REM     -x outputCodeDir=%WORKSPACE%\UnityProject\Assets\GameScripts\HotFix\GameProto\Generate\Config ^
@REM     -x bin.outputDataDir=%WORKSPACE%\Config\Generate\GameConfig\c ^
@REM     -x lineEnding=CRLF ^
    


@REM echo ==================== FuncConfig : GenClientFinish ====================

call ./Config/GameConfig/gen_code_bin_to_project_lazyload.bat

pause