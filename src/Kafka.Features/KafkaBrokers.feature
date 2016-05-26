Feature: KafkaBrokers
	In order to avoid silly mistakes
	As an Kafka amateur
	I want to consume Kafka metadata
	
# test json path online
# http://www.jsonquerytool.com/

@FileProc_Kafka_produce
Scenario: Able to produce and consume kafka - prmlinux02 - messages
  Given I have Random expressions
  When I send it to kafka prmlinux02.cloudapp.net:9092 server to TestMessage topic
  Then I should consume it in 10 seconds
# TestMessage

@FileProc_Kafka_produce
Scenario: Able to produce and consume kafka - 172.26.11.135 - messages
  Given I have Random expressions
  When I send it to kafka 172.26.11.135:9092 server to TestMessage topic
  Then I should consume it in 10 seconds

@FileProc_Kafka_produce
Scenario: Able to produce and consume kafka - shortfusedev-dn9.westus.cloudapp.azure.com:9092 - messages to Claim topic
  Given I have Random expressions
  When I send it to kafka shortfusedev-dn9.westus.cloudapp.azure.com:9092 server to Claim topic
  Then I should consume it in 10 seconds
  
@FileProc_Kafka_produce
Scenario Outline: Able to produce/consume kafka
  Given I have brokers for kafka  
	| kafka broker                                     |
	| shortfusedev-dn9.westus.cloudapp.azure.com:9092  |
	| shortfusedev-dn8.westus.cloudapp.azure.com:9092  |
	| shortfusedev-dn10.westus.cloudapp.azure.com:9092 |
	| shortfusedev-dn11.westus.cloudapp.azure.com:9092 |	
  And I have Random expressions
  When I send it to kafka 172.26.8.26:9092 server to <topic> topic
  Then I should consume it in 0 seconds

Examples:
  | topic        |
  | fusetest     |
  | fusetest2    |
  | TestMessage  |
  | Practitioner |
  | Claim        |
  | Coverage     |
  | Location     |

#fusetest, fusetest2, fusetopic2, test
@FileProc_Kafka_produce
Scenario: Able to produce and consume kafka - shortfusedev - messages to fusetest topic
  Given I have Random expressions
  When I send it to kafka shortfusedev-dn9.westus.cloudapp.azure.com:9092 server to fusetest topic
  Then I should consume it in 10 seconds
  # test


# from zookeper 172.26.8.13:2181
@FileProc_Kafka_consume
Scenario: Able to consume kafka - shortfusedev
  Given I have brokers for kafka  
	| kafka broker                                     |
	| shortfusedev-dn9.westus.cloudapp.azure.com:9092  |
	| shortfusedev-dn8.westus.cloudapp.azure.com:9092  |
	| shortfusedev-dn10.westus.cloudapp.azure.com:9092 |
	| shortfusedev-dn11.westus.cloudapp.azure.com:9092 |	
  And I have topics from kafka 
	| topic               | info                               |
	| fusetest            | 4 messages                         |
	| fusetest2           | 4 messages                         |
	| Practitioner        | 10 messages                        |
	| Patient             | 4 messages                         |
	| Procedure           | 4 messages                         |
	| Organization        | 4 messages  v1                     |
	| Claim               | 4 messages  v1                     |
	| Coverage            | 2 messages  v1                     |
	| Location            | 2 messages  v1                     |
	| Condition           | FirstOffset: 258; Items: 7         |
	| DiagnosticReport    | FirstOffset: 61; Items: 2 (53, 10) |
	| Encounter           | 2 messages  v1                     |
	| FamilyMemberHistory | FirstOffset: 16; Items: 2          |
	| Immunization        | FirstOffset: 129; Items: 3         |
	| Medication          | FirstOffset: 396; Items: 4         |
	| AllergyIntolerance  | FirstOffset: 51; Items: 8          |
  When I call kafka server
  Then I should retrieve last 3 messages in 10 seconds
  
	#| kafka broker                 |
	#| prmlinux02.cloudapp.net:9092 |	 
	#| 172.26.11.135:9092 |	 

@FileProc_Kafka_consume
Scenario: Able to get data folder
  Given I have brokers for kafka 
    | kafka broker     |
	| 172.26.8.26:9092 |
  When I call kafka server
  Then data folder is created if missing

@FileProc_Kafka_consume
Scenario: Able to consume kafka - 172.26.8.26-29 
  Given I have brokers for kafka 
	| kafka broker     |
	| 172.26.8.26:9092 |
	| 172.26.8.27:9092 |
	| 172.26.8.28:9092 |
	| 172.26.8.29:9092 |	 
  And I have topics for kafka
    | topic        | info           |
    | fusetest     | 4 messages     |
    | fusetest2    | 4 messages     |
    | Practitioner | 10 messages    |
    | Patient      | 4 messages     |
    | Procedure    | 4 messages     |
    | Organization | 4 messages  v1 |
    | Claim        | 4 messages  v1 |
    | Coverage     | 2 messages  v1 |
    | Location     | 2 messages  v1 |
    #| Test_Claim   | 55 messages  |
    #| TestMessage  | 70 messages  |	
	#connot combine 1 and 3???
  When I call kafka server
  Then I should retrieve last 3 messages in 10 seconds

@FileProc_Kafka_consume
Scenario Outline: Able to consume kafka topic
  Given I have brokers for kafka
    | kafka broker     |
	| 172.26.8.26:9092 |
	| 172.26.8.27:9092 |
	| 172.26.8.28:9092 |
	| 172.26.8.29:9092 |	 
  And I have kafka <topic>    
  When I call kafka server
  Then I should retrieve last 2 messages in 0 seconds

Examples:
    | topic        | info           |
    | fusetest     | A messages     |
    | fusetest2    | B messages     |
    | Practitioner | 10 messages    |
    | Location     | 2 messages  v1 |
    #| Patient      | 4 messages     |
    #| Procedure    | 4 messages     |
    #| Organization | 4 messages  v1 |
    #| Claim        | 4 messages  v1 |
    #| Coverage     | 2 messages  v1 |

#@FileProc_Kafka
#Scenario Outline: Able to consume kafka prmlinux02.cloudapp.net by topic
#  Given I have brokers for kafka 
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
    | jsonpath                                 | value    |
    | category.coding[?(@.code=='ccc')].system | xxx      |
    | code.coding[0].code                      | V04.81   |
    | category.coding[0].code                  | 55607006 |
    | category.coding[0].primary               | True     |
    | onset.onsetDateTime.offset               | 0        |
  

 @Xml_validation_XXE
Scenario: Able to parse xml message with XmlResolver
Given I have xml content 
When I load it 
Then It should be loaded as xmldoc.Load(xmlreader) loaded with DtdProcessing.Prohibit
 And It should be loaded as xmldoc.Load(string) with CustomUrlResovler
 And It should be loaded as xdocument.Load(xmlreader) with DtdProcessing.Prohibit
 And It should be deserialized as xmlSerializer.Deserialize(xmlreader) with DtdProcessing.Prohibit
 And xml file should be cleaned
  