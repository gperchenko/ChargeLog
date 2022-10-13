USE master;  
GO  

CREATE DATABASE    ChargeLog
    ON (FILENAME = 'D:\Databases\CharegLog_dat.mdf'),   
    (FILENAME = 'D:\Databases\CharegLog_log.ldf')   
    FOR ATTACH;  
GO