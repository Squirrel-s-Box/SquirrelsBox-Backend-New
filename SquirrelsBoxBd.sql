CREATE TABLE [dbo].[access_sessions] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [username]         NVARCHAR (50)  NULL,
    [password]         VARBINARY (50) NULL,
    [code]             VARCHAR (200)  NOT NULL,
    [attempt]          INT            DEFAULT ((0)) NULL,
    [creation_date]    DATETIME       DEFAULT (getdate()) NULL,
    [last_update_date] DATETIME       NULL,
    [refresh_token]    VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    UNIQUE NONCLUSTERED ([code] ASC)
);

CREATE TABLE [dbo].[assigned_permissions] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [user_code]        NVARCHAR (255) NOT NULL,
    [permission_id]    INT            NOT NULL,
    [creation_date]    DATETIME       NULL,
    [last_update_date] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([permission_id]) REFERENCES [dbo].[permissions] ([id])
);

CREATE TABLE [dbo].[boxes] (
    [id]               INT           IDENTITY (1, 1) NOT NULL,
    [name]             NVARCHAR (60) NULL,
    [user_code_owner]  VARCHAR (40)  NOT NULL,
    [favourite]        BIT           DEFAULT ((0)) NULL,
    [creation_date]    DATETIME      DEFAULT (getdate()) NULL,
    [last_update_date] DATETIME      NULL,
    [active]           BIT           DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[boxes_sections_list] (
    [box_id]     INT NOT NULL,
    [section_id] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([box_id] ASC, [section_id] ASC),
    FOREIGN KEY ([box_id]) REFERENCES [dbo].[boxes] ([id]),
    FOREIGN KEY ([section_id]) REFERENCES [dbo].[sections] ([id])
);

CREATE TABLE [dbo].[devices_sessions] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [token]            NVARCHAR (255) NULL,
    [creation_date]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [last_update_date] DATETIME       NULL,
    [user_id]          INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([user_id]) REFERENCES [dbo].[access_sessions] ([id])
);

CREATE TABLE [dbo].[permissions] (
    [id]         INT           IDENTITY (1, 1) NOT NULL,
    [collection] NVARCHAR (60) NULL,
    [name]       NVARCHAR (60) NULL,
    [value]      NVARCHAR (60) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[sections] (
    [id]               INT           IDENTITY (1, 1) NOT NULL,
    [name]             NVARCHAR (60) NULL,
    [color]            VARCHAR (100) NULL,
    [creation_date]    DATETIME      DEFAULT (getdate()) NULL,
    [last_update_date] DATETIME      NULL,
    [active]           BIT           DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[sections_items_list] (
    [section_id] INT NOT NULL,
    [item_id]    INT NOT NULL,
    PRIMARY KEY CLUSTERED ([section_id] ASC, [item_id] ASC),
    FOREIGN KEY ([item_id]) REFERENCES [dbo].[items] ([id]),
    FOREIGN KEY ([section_id]) REFERENCES [dbo].[sections] ([id])
);

CREATE TABLE [dbo].[items] (
    [id]               INT           IDENTITY (1, 1) NOT NULL,
    [name]             NVARCHAR (60) NULL,
    [description]      NVARCHAR (60) NULL,
    [amount]           NVARCHAR (60) NULL,
    [item_photo]       NVARCHAR (60) NULL,
    [creation_date]    DATETIME      DEFAULT (getdate()) NULL,
    [last_update_date] DATETIME      NULL,
    [active]           BIT           CONSTRAINT [DEFAULT_items_active] DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[personalized_specs] (
    [id]               INT           IDENTITY (1, 1) NOT NULL,
    [item_id]          INT           NOT NULL,
    [header_name]      NVARCHAR (60) NULL,
    [value]            NVARCHAR (60) NULL,
    [value_type]       NVARCHAR (60) NULL,
    [creation_date]    DATETIME      DEFAULT (getdate()) NULL,
    [last_update_date] DATETIME      NULL,
    [active]           BIT           CONSTRAINT [DEFAULT_personalized_specs_active] DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([item_id]) REFERENCES [dbo].[items] ([id]) ON DELETE CASCADE
);

-- CREATE TABLE [dbo].[shared_boxes] (
--     [id]               INT          IDENTITY (1, 1) NOT NULL,
--     [box_id]           INT          NOT NULL,
--     [user_code_guest]  VARCHAR (40) NOT NULL,
--     [creation_date]    DATETIME     DEFAULT (getdate()) NULL,
--     [last_update_date] DATETIME     NULL,
--     [state]            BIT          NOT NULL,
--     PRIMARY KEY CLUSTERED ([id] ASC),
--     FOREIGN KEY ([box_id]) REFERENCES [dbo].[boxes] ([id])
-- );

-- CREATE TABLE [dbo].[users_data] (
--     [id]               INT           IDENTITY (1, 1) NOT NULL,
--     [username]         NVARCHAR (40) NULL,
--     [email]            VARCHAR (80)  NOT NULL,
--     [name]             VARCHAR (80)  NOT NULL,
--     [lastname]         VARCHAR (80)  NOT NULL,
--     [user_photo]       VARCHAR (MAX) NULL,
--     [user_code]        VARCHAR (40)  NOT NULL,
--     [creation_date]    DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
--     [last_update_date] DATETIME2 (7) NULL,
--     [state]            BIT           DEFAULT ((1)) NULL,
--     PRIMARY KEY CLUSTERED ([id] ASC)
-- );


-- For boxes
ALTER TABLE [dbo].[boxes_sections_list]
DROP CONSTRAINT FK_boxes_sections_list_boxes;

ALTER TABLE [dbo].[boxes_sections_list]
ADD CONSTRAINT FK_boxes_sections_list_boxes
FOREIGN KEY (box_id) REFERENCES [dbo].[boxes](id) ON DELETE CASCADE;

-- For sections
ALTER TABLE [dbo].[boxes_sections_list]
DROP CONSTRAINT FK_boxes_sections_list_sections;

ALTER TABLE [dbo].[boxes_sections_list]
ADD CONSTRAINT FK_boxes_sections_list_sections
FOREIGN KEY (section_id) REFERENCES [dbo].[sections](id) ON DELETE CASCADE;

ALTER TABLE [dbo].[sections_items_list]
DROP CONSTRAINT FK_sections_items_list_sections;

ALTER TABLE [dbo].[sections_items_list]
ADD CONSTRAINT FK_sections_items_list_sections
FOREIGN KEY (section_id) REFERENCES [dbo].[sections](id) ON DELETE CASCADE;

-- For items
ALTER TABLE [dbo].[sections_items_list]
DROP CONSTRAINT FK_sections_items_list_items;

ALTER TABLE [dbo].[sections_items_list]
ADD CONSTRAINT FK_sections_items_list_items
FOREIGN KEY (item_id) REFERENCES [dbo].[items](id) ON DELETE CASCADE;

-- ALTER TABLE [dbo].[personalized_specs_items_list]
-- DROP CONSTRAINT FK_personalized_specs_items_list_items;

-- ALTER TABLE [dbo].[personalized_specs_items_list]
-- ADD CONSTRAINT FK_personalized_specs_items_list_items
-- FOREIGN KEY (item_id) REFERENCES [dbo].[items](id) ON DELETE CASCADE;



