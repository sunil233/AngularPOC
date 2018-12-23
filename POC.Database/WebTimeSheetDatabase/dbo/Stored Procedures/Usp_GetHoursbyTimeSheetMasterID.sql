CREATE proc [dbo].[Usp_GetHoursbyTimeSheetMasterID]    
@TimeSheetMasterID int   ,
@ProjectID int 
as    
begin    
    
SELECT  TimeSheetID, comments, TimesheetStatus ,
      Hours 
  FROM [TimeSheetDetails]     
  where TimeSheetMasterID =@TimeSheetMasterID and ProjectID =@ProjectID
  
  union all
  
  SELECT   NULL,  null,null,
      SUM(Hours) 
  FROM [TimeSheetDetails]     
  where TimeSheetMasterID =@TimeSheetMasterID and ProjectID =@ProjectID 
end



/****** Object:  StoredProcedure [dbo].[Usp_ChangeTimesheetStatus]    Script Date: 9/25/2018 2:35:27 PM ******/
SET ANSI_NULLS ON
