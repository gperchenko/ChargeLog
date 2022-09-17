USE master;  
GO  
CREATE DATABASE ChargeLog  
ON   
( NAME = ChargeLog_dat,  
    FILENAME = 'F:\Databases\CharegLog_dat.mdf'
)  
LOG ON  
( NAME = ChargeLog_log,  
    FILENAME = 'F:\Databases\CharegLog_log.ldf'
);  
GO  