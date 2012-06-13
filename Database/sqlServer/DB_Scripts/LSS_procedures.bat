echo LSS Procedures: %1

isqlw -E -d %1 -i .\ProcessAgent\ProcessAgentProcedures.sql -o lssBuild3.log
isqlw -E -d %1 -i .\Scheduling\LSS_Procedures.sql -o lssBuild4.log
