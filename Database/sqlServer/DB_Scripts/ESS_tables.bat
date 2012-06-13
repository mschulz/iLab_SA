echo creating ESS Tables: %1

isqlw -E -d %1 -i .\ProcessAgent\ProcessAgentTables.sql -o essBuild0.log
isqlw -E -d %1 -i .\ProcessAgent\SetdefaultsPA.sql -o essBuild1.log
isqlw -E -d %1 -i .\ESS\Ess_tables.sql -o essBuild2.log

