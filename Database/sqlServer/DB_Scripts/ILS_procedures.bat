echo InteractiveLS Procedures: %1

isqlw -E -d %1 -i .\ProcessAgent\ProcessAgentProcedures.sql -o ilsBuild3.log
isqlw -E -d %1 -i .\LabServer\LabServer_Procedures.sql -o ilsBuild4.log
