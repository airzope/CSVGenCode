@echo off
cd ./Bin
CSVGenCode.exe -i=../XLS -o=../Input/ -xls2csv
CSVGenCode.exe -i=../Input -o=../Config/Cpp/Output -t=../Config/Cpp/Templet -r=../Config/Cpp/KeywordMapRule.txt