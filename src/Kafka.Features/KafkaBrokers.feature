Feature: KafkaBrokers
	In order to avoid silly mistakes
	As an Kafka amateur
	I want to consume Kafka metadata


@FileProc_Kafka
Scenario: Able to produce and consume kafka - prmlinux02 - messages
  Given I have Random expressions
  When I send it to kafka prmlinux02.cloudapp.net:9092 server to TestMessage topic
  Then I should consume it in 10 seconds
# TestMessage


@FileProc_Kafka
Scenario: Able to produce and consume kafka - shortfusedev - messages
  Given I have Random expressions
  When I send it to kafka shortfusedev-dn9.westus.cloudapp.azure.com:9092 server to fusetest topic
  Then I should consume it in 10 seconds
  # test

# from zookeper 172.26.8.13:2181
@FileProc_Kafka
Scenario: Able to consume kafka - shortfusedev
  Given I have kafka brokers 
	| kafka broker                                     |
	| shortfusedev-dn9.westus.cloudapp.azure.com:9092  |
	| shortfusedev-dn8.westus.cloudapp.azure.com:9092  |
	| shortfusedev-dn10.westus.cloudapp.azure.com:9092 |
	| shortfusedev-dn11.westus.cloudapp.azure.com:9092 |	
  And I have kafka topics
	| topic        | info                       |
	| Immunization | FirstOffset: 129; Items: 3 |
	| Condition    | FirstOffset: 258; Items: 7 |
	#| DiagnosticReport    | FirstOffset: 61; Items: 2 (53, 10)   |
	#| Patient             | FirstOffset: 217; Items: 3 (198, 22) |
	#| FamilyMemberHistory | FirstOffset: 16; Items: 2            |
	#| Medication          | FirstOffset: 396; Items: 4           |
	#| AllergyIntolerance  | FirstOffset: 51; Items: 8            |
  When I call kafka server
  Then I should retrieve last 3 messages in 10 seconds
  #And  I should retrieve from kafka4net last 3 messages in 10 seconds
 
  

# from zookeeper prmlinux02.cloudapp.net:2181
@FileProc_Kafka
Scenario: Able to consume kafka - prmlinux02 
  Given I have kafka brokers 
	| kafka broker                 |
	| prmlinux02.cloudapp.net:9092 |	 
  And I have kafka topics
    | topic        | info         |
    | Practitioner | 140 messages |
    | Test_Claim   | 55 messages  |
    | TestMessage  | 70 messages  |	
    | Organization | 4 messages  v1 |
    | Claim        | 4 messages  v1 |
    | Coverage     | 2 messages  v1 |
    | Patient      | 2 messages  v1 |
	#connot combine 1 and 3???
  When I call kafka server
  Then I should retrieve last 3 messages in 30 seconds

#@FileProc_Kafka
#Scenario Outline: Able to consume kafka prmlinux02.cloudapp.net by topic
#  Given I have kafka brokers 
#	| kafka broker                 |
#	| prmlinux02.cloudapp.net:9092 |	 
#  And I have kafka <topic>    
#  When I call kafka server
#  Then I should retrieve last 2 messages in 3 seconds
#Examples:
#    | topic        | info        |
#    | Organization | 4 messages  |
#    | Claim        | 4 messages  |
#    | Coverage     | 2 messages  |
#    | Patient      | 2 messages  |
#    | Practitioner | 6 messages  |
#    | Test_Claim   | 16 messages |
#    | TestMessage  | 3 messages  |


  @FileProc_Json
  Scenario: Able to parse json message Patient
  Given I have json file  
  When I parse it 
  Then I should be find in file <messages\Patient\item_215.json> matching json values
    | jsonpath                  | value             |
    | name[0].family[0]         | LAMBERT           |
    | address[0].addressLine[0] | 7881 Metus Street |
    | address[0].addressLine[1] | none              |
    | address[0].addressLine[2] | null              |

 @FileProc_Json
  Scenario: Able to parse json message Condition
  Given I have json file  
  When I parse it 
  Then I should be find in file <messages\Condition\item_257.json> matching json values
    | jsonpath                   | value    |
    | code.coding[0].code        | V04.81   |
    | category.coding[0].code    | 55607006 |
    | category.coding[0].primary | True     |
    | onset.onsetDateTime.offset | 0        |
  