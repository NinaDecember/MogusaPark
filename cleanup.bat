@echo off
cd /d %~dp0

rem Libraryフォルダの削除
if exist Library (
    rd /s /q Library
    echo Libraryフォルダを削除しました。
) else (
    echo Libraryフォルダが存在しません。
)

rem objフォルダの削除
if exist obj (
    rd /s /q obj
    echo objフォルダを削除しました。
) else (
    echo objフォルダが存在しません。
)

rem Logsフォルダの削除
if exist Logs (
    rd /s /q Logs
    echo Logsフォルダを削除しました。
) else (
    echo Logsフォルダが存在しません。
)

rem Tempフォルダの削除
if exist Temp (
    rd /s /q Temp
    echo Tempフォルダを削除しました。
) else (
    echo Tempフォルダが存在しません。
)

rem _BackUpThisFolder_ButDontShipItWithYourGame を含むフォルダの削除
for /d %%d in (*_BackUpThisFolder_ButDontShipItWithYourGame*) do (
    rd /s /q "%%d"
    echo %%dフォルダを削除しました。
)

rem .apk拡張子のファイルを削除
for %%f in (*.apk) do (
    del /q "%%f"
    echo %%fファイルを削除しました。
)

echo Cleanup completed successfully.
pause
