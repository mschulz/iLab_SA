echo USS Procedures: %1

isqlw -E -d %1 -i .\ProcessAgent\ProcessAgentProcedures.sql -o ussBuild3.log
isqlw -E -d %1 -i .\Scheduling\USS_Procedures.sql -o ussBuild4.log

