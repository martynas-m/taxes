CREATE TABLE [dbo].[Taxes] (
    [Id]             INT        IDENTITY (1, 1) NOT NULL,
    [MunicipalityId] INT        NOT NULL,
    [TaxType]        TINYINT    NOT NULL,
    [Tax]            FLOAT (53) NOT NULL,
    [Start]          DATE       NOT NULL,
    [End]            DATE       NOT NULL,
    CONSTRAINT [PK_Tax] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TaxToMunicipality] FOREIGN KEY ([MunicipalityId]) REFERENCES [dbo].[Municipalities] ([id]),
    CONSTRAINT [UX_Tax] UNIQUE NONCLUSTERED ([MunicipalityId] ASC, [TaxType] ASC, [Start] ASC, [End] ASC)
);

