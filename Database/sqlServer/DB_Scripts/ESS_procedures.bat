echo Creating StoredProcedures ESS: %1
isqlw -E -d %1 -i .\ProcessAgent\ProcessAgentProcedures.sql -o essBuild3.log
isqlw -E -d %1 -i .\ESS\ESS_Procedures.sql -o essBuild4.log
