Create Procedure [dbo].[GetUserById]
    @id int
AS
SELECT
    [Id],
    [Name]
From
    [dbo].[User]
Where
    Id = @id
