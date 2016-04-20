@echo off

set fake_args=%*

cd "._fake"
call "paket.bat" "install"
cd ".."

"._fake\packages\FAKE\tools\FAKE.exe" "build.fsx" %fake_args%
