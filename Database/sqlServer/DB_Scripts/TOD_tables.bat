echo populating TOD %1

isqlw -E -d %1 -i .\ProcessAgent\ProcessAgentTables.sql -o TODBuild0.log
isqlw -E -d %1 -i .\ProcessAgent\SetdefaultsPA.sql -o TODBuild1.log
