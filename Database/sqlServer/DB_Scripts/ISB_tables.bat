echo InteractiveSB Tables: %1

isqlw -E -d %1 -i .\ProcessAgent\ProcessAgentTables.sql -o isbBuild0.log
isqlw -E -d %1 -i .\ProcessAgent\SetdefaultsPA.sql -o isbBuild1.log
isqlw -E -d %1 -i .\ServiceBroker\TIssuerTables.sql -o isbBuild2.log
isqlw -E -d %1 -i .\ServiceBroker\ServiceBrokerCoreTables.sql -o isbBuild3.log
isqlw -E -d %1 -i .\ServiceBroker\ServiceBrokerCoreDefaultValues.sql -o isbBuild4.log
