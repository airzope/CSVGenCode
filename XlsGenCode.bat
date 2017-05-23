@echo off
cd ./Bin
CSVGenCode.exe "-i=../XLS/XLS1|../XLS/XLS2" -o=../CSV/ -xls2csv
cd ..
call CopyToProject.bat