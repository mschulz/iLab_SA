echo InteractiveLS Tables: %1

isqlw -E -d %1 -i .\ProcessAgent\ProcessAgentTables.sql -o ilsBuild0.log
isqlw -E -d %1 -i .\ProcessAgent\SetdefaultsPA.sql -o ilsBuild1.log
isqlw -E -d %1 -i .\LabServer\LabServer_Tables.sql -o ilsBuild2.log

