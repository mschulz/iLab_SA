echo USS Tables: %1

isqlw -E -d %1 -i .\ProcessAgent\ProcessAgentTables.sql -o ussBuild0.log
isqlw -E -d %1 -i .\ProcessAgent\SetdefaultsPA.sql -o ussBuild1.log
isqlw -E -d %1 -i .\Scheduling\USS_Tables.sql -o ussBuild2.log

