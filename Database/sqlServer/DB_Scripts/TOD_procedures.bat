echo TOD Procedures: %1

isqlw -E -d %1 -i .\ProcessAgent\ProcessAgentProcedures.sql -o TODBuild2.log

