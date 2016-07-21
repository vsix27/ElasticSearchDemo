Feature: KafkaBrokers
	In order to avoid silly mistakes
	As an Kafka amateur
	I want to consume Kafka metadata

Background: file C:\Windows\System32\drivers\etc\host 
	should be updated (otherwise kafka brokers not visible) with
	(added to see Kafka brokers (note from Fitzgerald, Kirk) )
172.26.15.7 rhi-fuse-dev-cluster-kafka0.westus.cloudapp.azure.com rhi-fuse-dev-cluster-mn0.westus.cloudapp.azure.com
172.26.15.5 rhi-fuse-dev-cluster-kafka1.westus.cloudapp.azure.com rhi-fuse-dev-cluster-mn1.westus.cloudapp.azure.com
172.26.15.9 rhi-fuse-dev-cluster-kafka2.westus.cloudapp.azure.com rhi-fuse-dev-cluster-mn2.westus.cloudapp.azure.com
	
# test json path online
# http://www.jsonquerytool.com/


@FileProc_Kafka_produce
Scenario: Able to resolve kafka broker names to ip in host file
  Given I have brokers for kafka from AcceptanceTest:KafkaUri    
  When I read host file  
  Then It should contain
    | ip machine                                                                                                 |
    | 172.26.15.7 rhi-fuse-dev-cluster-kafka0.westus.cloudapp.azure.com rhi-fuse-dev-cluster-mn0.westus.cloudapp.azure.com |
    | 172.26.15.5 rhi-fuse-dev-cluster-kafka1.westus.cloudapp.azure.com rhi-fuse-dev-cluster-mn1.westus.cloudapp.azure.com |
    | 172.26.15.9 rhi-fuse-dev-cluster-kafka2.westus.cloudapp.azure.com rhi-fuse-dev-cluster-mn2.westus.cloudapp.azure.com |


@FileProc_Kafka_produce
Scenario: Able to produce and consume kafka to topic - junk
  Given I have brokers for kafka from AcceptanceTest:KafkaUri 	 
    And I have Random expressions
  When I send it to kafka junk topic
  Then I should consume it in 0 seconds

@FileProc_Kafka_consume
Scenario: Able to consume kafka - fusedev
  Given I have brokers for kafka from AcceptanceTest:KafkaUri 	 
  And I have topics for kafka 
	| topic               | info                               |
	| fusetest            | --                                 |
	| junk                | 4 messages                         |
	| Practitioner        | 10 messages                        |
	| Patient             | 4 messages                         |
	| Procedure           | 4 messages                         |
	| Organization        | 4 messages  v1                     |
	| Claim               | 4 messages  v1                     |
	| Coverage            | 2 messages  v1                     |
	| Location            | 2 messages  v1                     |
	| Logpoc              | 2 messages  v1                     |
	| Condition           | FirstOffset: 258; Items: 7         |
	| DiagnosticReport    | FirstOffset: 61; Items: 2 (53, 10) |
	| Encounter           | 2 messages  v1                     |
	| FamilyMemberHistory | FirstOffset: 16; Items: 2          |
	| Immunization        | FirstOffset: 129; Items: 3         |
	| Medication          | FirstOffset: 396; Items: 4         |
	| AllergyIntolerance  | FirstOffset: 51; Items: 8          |
	| mt-event-mex        |                                    |
	| mt-event-test       |                                    |
	| mt-reindex          |                                    |

  When I call kafka server
  Then I should retrieve last 2 messages in -1 seconds
# in log/output - for failed/timed out topics -search for '. The operation was canceled.' or '. The operation was canceled.'

@FileProc_Kafka_datafolder
Scenario: Able to get data folder
  Given I have brokers for kafka 
    | kafka broker      |
    | 172.26.15.7:19092 |
    #| 172.26.8.26:9092  |
  When I call kafka server
  Then data folder is created if missing

@FileProc_Kafka_consume
Scenario: Able to consume kafka listed topics from app.config
  Given I have brokers for kafka from AcceptanceTest:KafkaUri 	 
   And I have topic list for kafka dummy, test1, junk, fusetest, Claim, 
   And I have topic list for kafka Coverage, Immunization, Location,Logpoc
   And I have topic list for kafka Medication, Organization, Patient, Procedure, Practitioner
  When I call kafka server
  Then I should retrieve last 2 messages in -1 seconds

@FileProc_Kafka_consume
Scenario: Able to consume kafka config topics from app.config
  Given I have brokers for kafka from AcceptanceTest:KafkaUri 	 
    And I have topic list AcceptanceTest:KafkaTopicsAll for kafka
   Then I should retrieve last 2 messages in -1 seconds

@FileProc_Kafka_consume
Scenario Outline: Able to consume kafka topic from app.config
  Given I have brokers for kafka from AcceptanceTest:KafkaUri 	 
  And I have kafka <topic>    
  When I call kafka server
  Then I should retrieve last 2 messages in -1 seconds

Examples:
    | topic        |  
    | fusetest     |  
    | junk         |  
    | Practitioner |  
    | Location     |  
    | Patient      | 
    | Procedure    | 
    | Organization |  
    | Claim        |  
    | Coverage     |  

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
  