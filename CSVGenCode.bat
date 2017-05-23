@echo off
cd ./Bin
CSVGenCode.exe -i=../CSV -o=../Config/CS/Output -t=../Config/CS/Templet -r=../Config/CS/KeywordMapRule.txt
CSVGenCode.exe -i=../CSV -o=../Config/Cpp/Output -t=../Config/Cpp/Templet -r=../Config/Cpp/KeywordMapRule.txt
cd ..
call CopyToProject.bat