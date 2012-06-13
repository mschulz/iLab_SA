echo LSS Tables: %1

isqlw -E -d %1 -i .\ProcessAgent\ProcessAgentTables.sql -o lssBuild0.log
isqlw -E -d %1 -i .\ProcessAgent\SetdefaultsPA.sql -o lssBuild1.log
isqlw -E -d %1 -i .\Scheduling\LSS_Tables.sql -o lssBuild2.log

