USE master;  
GO  
CREATE DATABASE ChargeLog  
ON   
( NAME = ChargeLog_dat,  
    FILENAME = 'D:\Databases\CharegLog_dat.mdf'
)  
LOG ON  
( NAME = ChargeLog_log,  
    FILENAME = 'D:\Databases\CharegLog_log.ldf'
);  
GO  