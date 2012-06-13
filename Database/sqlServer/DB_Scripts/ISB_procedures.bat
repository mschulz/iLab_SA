echo iLabServiceBroker Procedures: %1

isqlw -E -d %1 -i .\ProcessAgent\ProcessAgentProcedures.sql -o isbBuild5.log -r
isqlw -E -d %1 -i .\ServiceBroker\TIssuerProcedures.sql -o isbBuild6.log -r
isqlw -E -d %1 -i .\ServiceBroker\ServiceBrokerCoreProcedures.sql -o isbBuild7.log -r

