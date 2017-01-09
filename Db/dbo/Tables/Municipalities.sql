CREATE TABLE [dbo].[Municipalities] (
    [id]   INT            IDENTITY (1, 1) NOT NULL,
    [name] NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_municipality] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UX_municipality_name] UNIQUE NONCLUSTERED ([name] ASC)
);

