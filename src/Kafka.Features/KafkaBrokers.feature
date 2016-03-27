Feature: KafkaBrokers
	In order to avoid silly mistakes
	As an Kafka amateur
	I want to consume Kafka metadata


@FileProc_Kafka
Scenario: Able to produce and consume kafka messages
Given I have expression Random GUID
When I send it to kafka prmlinux02.cloudapp.net:9092 server to TestMessage topic
Then I should consume it in 10 seconds

@FileProc_Kafka
Scenario: Able to consume kafka server
  Given I have kafka brokers 
	| zookeeper                    | kafka broker                                     |
	| 172.26.8.13:2181             | shortfusedev-dn8.westus.cloudapp.azure.com:9092  |
	| 172.26.8.13:2181             | shortfusedev-dn9.westus.cloudapp.azure.com:9092  |
	| 172.26.8.13:2181             | shortfusedev-dn10.westus.cloudapp.azure.com:9092 |
	| 172.26.8.13:2181             | shortfusedev-dn11.westus.cloudapp.azure.com:9092 |
	| prmlinux02.cloudapp.net:2181 | prmlinux02.cloudapp.net:9092                     |
	#combine brokers for the same zookeeper
  And I have kafka topics
	| zookeeper                    | topic              | info           |
	| prmlinux02.cloudapp.net:2181 | Test_Claim         | 16 messages    |
	#| prmlinux02.cloudapp.net:2181 | TestMessage        | 3 messages     |
	#| 172.26.8.13:2181             | AllergyIntolerance | 5 messages     |
	#| 172.26.8.13:2181             | Condition          | 32 messages from 226 to 257   |
	#| 172.26.8.13:2181             | Patient            | messages       |
	#| 172.26.8.13:2181             | test               | 0 messages     |
	#| 172.26.8.13:2181             | testXX             | does not exist |
  When I call kafka server
  Then I should retreive it in 10 seconds
