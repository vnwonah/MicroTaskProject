SET ANSI_NULLS ON  
GO  
SET QUOTED_IDENTIFIER ON  
GO  

CREATE PROCEDURE sp_UpdateRecordStatus 
@RecordId bigint,  
@RecordStatus int,
@Message nvarchar(500)
  
AS  
BEGIN  
    SET NOCOUNT ON;  
    UPDATE [dbo].[Records] 
    Set Status = @RecordStatus, Message = @Message  
    Where id = @RecordId  
END  
GO  