USE [master]
GO
/****** Object:  Database [ZenChat]    Script Date: 15.04.2016 19:04:51 ******/
CREATE DATABASE [ZenChat]
GO
ALTER DATABASE [ZenChat] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ZenChat].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ZenChat] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ZenChat] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ZenChat] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ZenChat] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ZenChat] SET ARITHABORT OFF 
GO
ALTER DATABASE [ZenChat] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ZenChat] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ZenChat] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ZenChat] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ZenChat] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ZenChat] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ZenChat] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ZenChat] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ZenChat] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ZenChat] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ZenChat] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ZenChat] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ZenChat] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ZenChat] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [ZenChat] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ZenChat] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [ZenChat] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ZenChat] SET RECOVERY FULL 
GO
ALTER DATABASE [ZenChat] SET  MULTI_USER 
GO
ALTER DATABASE [ZenChat] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ZenChat] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ZenChat] SET QUERY_STORE = ON
GO
ALTER DATABASE [ZenChat] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO)
GO
USE [ZenChat]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [ZenChat]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_diagramobjects]    Script Date: 15.04.2016 19:04:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE FUNCTION [dbo].[fn_diagramobjects]() 
	RETURNS int
	WITH EXECUTE AS N'dbo'
	AS
	BEGIN
		declare @id_upgraddiagrams		int
		declare @id_sysdiagrams			int
		declare @id_helpdiagrams		int
		declare @id_helpdiagramdefinition	int
		declare @id_creatediagram	int
		declare @id_renamediagram	int
		declare @id_alterdiagram 	int 
		declare @id_dropdiagram		int
		declare @InstalledObjects	int

		select @InstalledObjects = 0

		select 	@id_upgraddiagrams = object_id(N'dbo.sp_upgraddiagrams'),
			@id_sysdiagrams = object_id(N'dbo.sysdiagrams'),
			@id_helpdiagrams = object_id(N'dbo.sp_helpdiagrams'),
			@id_helpdiagramdefinition = object_id(N'dbo.sp_helpdiagramdefinition'),
			@id_creatediagram = object_id(N'dbo.sp_creatediagram'),
			@id_renamediagram = object_id(N'dbo.sp_renamediagram'),
			@id_alterdiagram = object_id(N'dbo.sp_alterdiagram'), 
			@id_dropdiagram = object_id(N'dbo.sp_dropdiagram')

		if @id_upgraddiagrams is not null
			select @InstalledObjects = @InstalledObjects + 1
		if @id_sysdiagrams is not null
			select @InstalledObjects = @InstalledObjects + 2
		if @id_helpdiagrams is not null
			select @InstalledObjects = @InstalledObjects + 4
		if @id_helpdiagramdefinition is not null
			select @InstalledObjects = @InstalledObjects + 8
		if @id_creatediagram is not null
			select @InstalledObjects = @InstalledObjects + 16
		if @id_renamediagram is not null
			select @InstalledObjects = @InstalledObjects + 32
		if @id_alterdiagram  is not null
			select @InstalledObjects = @InstalledObjects + 64
		if @id_dropdiagram is not null
			select @InstalledObjects = @InstalledObjects + 128
		
		return @InstalledObjects 
	END
	
GO
/****** Object:  Table [dbo].[chatroom]    Script Date: 15.04.2016 19:04:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[chatroom](
	[id_chatroom] [int] IDENTITY(1,1) NOT NULL,
	[admin] [int] NOT NULL,
	[created] [datetime] NOT NULL,
	[topic] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_chatroom] PRIMARY KEY CLUSTERED 
(
	[id_chatroom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[chatroom_user]    Script Date: 15.04.2016 19:05:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[chatroom_user](
	[id_chatroom_user] [int] IDENTITY(1,1) NOT NULL,
	[fk_user] [int] NOT NULL,
	[fk_chatroom] [int] NOT NULL,
	[isMember] [bit] NOT NULL,
 CONSTRAINT [PK_chatroom_person] PRIMARY KEY CLUSTERED 
(
	[id_chatroom_user] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[friendship]    Script Date: 15.04.2016 19:05:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[friendship](
	[id_friendship] [int] IDENTITY(1,1) NOT NULL,
	[fk_user1] [int] NOT NULL,
	[fk_user2] [int] NOT NULL,
 CONSTRAINT [PK_friendship] PRIMARY KEY CLUSTERED 
(
	[id_friendship] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[message]    Script Date: 15.04.2016 19:05:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[message](
	[id_message] [int] IDENTITY(1,1) NOT NULL,
	[author] [int] NOT NULL,
	[message] [nvarchar](500) NOT NULL,
	[created] [datetime] NOT NULL,
 CONSTRAINT [PK_message] PRIMARY KEY CLUSTERED 
(
	[id_message] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[message_user]    Script Date: 15.04.2016 19:05:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[message_user](
	[id_message_user] [int] IDENTITY(1,1) NOT NULL,
	[fk_message] [int] NOT NULL,
	[fk_user] [int] NOT NULL,
	[fk_chatroom] [int] NULL,
	[wasRead] [bit] NOT NULL,
	[wasRecieved] [bit] NOT NULL,
 CONSTRAINT [PK_message_user] PRIMARY KEY CLUSTERED 
(
	[id_message_user] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[sysdiagrams]    Script Date: 15.04.2016 19:05:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sysdiagrams](
	[name] [sysname] NOT NULL,
	[principal_id] [int] NOT NULL,
	[diagram_id] [int] IDENTITY(1,1) NOT NULL,
	[version] [int] NULL,
	[definition] [varbinary](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[diagram_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
 CONSTRAINT [UK_principal_name] UNIQUE NONCLUSTERED 
(
	[principal_id] ASC,
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[user]    Script Date: 15.04.2016 19:05:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[id_user] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[phone] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[id_user] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
ALTER TABLE [dbo].[chatroom] ADD  CONSTRAINT [DF_chatroom_created]  DEFAULT (getdate()) FOR [created]
GO
ALTER TABLE [dbo].[chatroom_user] ADD  CONSTRAINT [DF_chatroom_user_isMember]  DEFAULT ((1)) FOR [isMember]
GO
ALTER TABLE [dbo].[message] ADD  CONSTRAINT [DF_message_created]  DEFAULT (getdate()) FOR [created]
GO
ALTER TABLE [dbo].[message_user] ADD  CONSTRAINT [DF_message_user_wasRead]  DEFAULT ((0)) FOR [wasRead]
GO
ALTER TABLE [dbo].[message_user] ADD  CONSTRAINT [DF_message_user_wasRecieved]  DEFAULT ((0)) FOR [wasRecieved]
GO
ALTER TABLE [dbo].[chatroom]  WITH CHECK ADD  CONSTRAINT [FK_chatroom_user] FOREIGN KEY([admin])
REFERENCES [dbo].[user] ([id_user])
GO
ALTER TABLE [dbo].[chatroom] CHECK CONSTRAINT [FK_chatroom_user]
GO
ALTER TABLE [dbo].[chatroom_user]  WITH CHECK ADD  CONSTRAINT [FK_chatroom_person_chatroom] FOREIGN KEY([fk_chatroom])
REFERENCES [dbo].[chatroom] ([id_chatroom])
GO
ALTER TABLE [dbo].[chatroom_user] CHECK CONSTRAINT [FK_chatroom_person_chatroom]
GO
ALTER TABLE [dbo].[chatroom_user]  WITH CHECK ADD  CONSTRAINT [FK_chatroom_person_user] FOREIGN KEY([fk_user])
REFERENCES [dbo].[user] ([id_user])
GO
ALTER TABLE [dbo].[chatroom_user] CHECK CONSTRAINT [FK_chatroom_person_user]
GO
ALTER TABLE [dbo].[friendship]  WITH CHECK ADD  CONSTRAINT [FK_friendship_user1] FOREIGN KEY([fk_user1])
REFERENCES [dbo].[user] ([id_user])
GO
ALTER TABLE [dbo].[friendship] CHECK CONSTRAINT [FK_friendship_user1]
GO
ALTER TABLE [dbo].[friendship]  WITH CHECK ADD  CONSTRAINT [FK_friendship_user2] FOREIGN KEY([fk_user2])
REFERENCES [dbo].[user] ([id_user])
GO
ALTER TABLE [dbo].[friendship] CHECK CONSTRAINT [FK_friendship_user2]
GO
ALTER TABLE [dbo].[message]  WITH CHECK ADD  CONSTRAINT [FK_message_user] FOREIGN KEY([author])
REFERENCES [dbo].[user] ([id_user])
GO
ALTER TABLE [dbo].[message] CHECK CONSTRAINT [FK_message_user]
GO
ALTER TABLE [dbo].[message_user]  WITH CHECK ADD  CONSTRAINT [FK_message_user_chatroom] FOREIGN KEY([fk_chatroom])
REFERENCES [dbo].[chatroom] ([id_chatroom])
GO
ALTER TABLE [dbo].[message_user] CHECK CONSTRAINT [FK_message_user_chatroom]
GO
ALTER TABLE [dbo].[message_user]  WITH CHECK ADD  CONSTRAINT [FK_message_user_message] FOREIGN KEY([fk_message])
REFERENCES [dbo].[message] ([id_message])
GO
ALTER TABLE [dbo].[message_user] CHECK CONSTRAINT [FK_message_user_message]
GO
ALTER TABLE [dbo].[message_user]  WITH CHECK ADD  CONSTRAINT [FK_message_user_user] FOREIGN KEY([fk_user])
REFERENCES [dbo].[user] ([id_user])
GO
ALTER TABLE [dbo].[message_user] CHECK CONSTRAINT [FK_message_user_user]
GO
/****** Object:  StoredProcedure [dbo].[sp_alterdiagram]    Script Date: 15.04.2016 19:05:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_alterdiagram]
	(
		@diagramname 	sysname,
		@owner_id	int	= null,
		@version 	int,
		@definition 	varbinary(max)
	)
	WITH EXECUTE AS 'dbo'
	AS
	BEGIN
		set nocount on
	
		declare @theId 			int
		declare @retval 		int
		declare @IsDbo 			int
		
		declare @UIDFound 		int
		declare @DiagId			int
		declare @ShouldChangeUID	int
	
		if(@diagramname is null)
		begin
			RAISERROR ('Invalid ARG', 16, 1)
			return -1
		end
	
		execute as caller;
		select @theId = DATABASE_PRINCIPAL_ID();	 
		select @IsDbo = IS_MEMBER(N'db_owner'); 
		if(@owner_id is null)
			select @owner_id = @theId;
		revert;
	
		select @ShouldChangeUID = 0
		select @DiagId = diagram_id, @UIDFound = principal_id from dbo.sysdiagrams where principal_id = @owner_id and name = @diagramname 
		
		if(@DiagId IS NULL or (@IsDbo = 0 and @theId <> @UIDFound))
		begin
			RAISERROR ('Diagram does not exist or you do not have permission.', 16, 1);
			return -3
		end
	
		if(@IsDbo <> 0)
		begin
			if(@UIDFound is null or USER_NAME(@UIDFound) is null) -- invalid principal_id
			begin
				select @ShouldChangeUID = 1 ;
			end
		end

		-- update dds data			
		update dbo.sysdiagrams set definition = @definition where diagram_id = @DiagId ;

		-- change owner
		if(@ShouldChangeUID = 1)
			update dbo.sysdiagrams set principal_id = @theId where diagram_id = @DiagId ;

		-- update dds version
		if(@version is not null)
			update dbo.sysdiagrams set version = @version where diagram_id = @DiagId ;

		return 0
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_creatediagram]    Script Date: 15.04.2016 19:05:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_creatediagram]
	(
		@diagramname 	sysname,
		@owner_id		int	= null, 	
		@version 		int,
		@definition 	varbinary(max)
	)
	WITH EXECUTE AS 'dbo'
	AS
	BEGIN
		set nocount on
	
		declare @theId int
		declare @retval int
		declare @IsDbo	int
		declare @userName sysname
		if(@version is null or @diagramname is null)
		begin
			RAISERROR (N'E_INVALIDARG', 16, 1);
			return -1
		end
	
		execute as caller;
		select @theId = DATABASE_PRINCIPAL_ID(); 
		select @IsDbo = IS_MEMBER(N'db_owner');
		revert; 
		
		if @owner_id is null
		begin
			select @owner_id = @theId;
		end
		else
		begin
			if @theId <> @owner_id
			begin
				if @IsDbo = 0
				begin
					RAISERROR (N'E_INVALIDARG', 16, 1);
					return -1
				end
				select @theId = @owner_id
			end
		end
		-- next 2 line only for test, will be removed after define name unique
		if EXISTS(select diagram_id from dbo.sysdiagrams where principal_id = @theId and name = @diagramname)
		begin
			RAISERROR ('The name is already used.', 16, 1);
			return -2
		end
	
		insert into dbo.sysdiagrams(name, principal_id , version, definition)
				VALUES(@diagramname, @theId, @version, @definition) ;
		
		select @retval = @@IDENTITY 
		return @retval
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_dropdiagram]    Script Date: 15.04.2016 19:05:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_dropdiagram]
	(
		@diagramname 	sysname,
		@owner_id	int	= null
	)
	WITH EXECUTE AS 'dbo'
	AS
	BEGIN
		set nocount on
		declare @theId 			int
		declare @IsDbo 			int
		
		declare @UIDFound 		int
		declare @DiagId			int
	
		if(@diagramname is null)
		begin
			RAISERROR ('Invalid value', 16, 1);
			return -1
		end
	
		EXECUTE AS CALLER;
		select @theId = DATABASE_PRINCIPAL_ID();
		select @IsDbo = IS_MEMBER(N'db_owner'); 
		if(@owner_id is null)
			select @owner_id = @theId;
		REVERT; 
		
		select @DiagId = diagram_id, @UIDFound = principal_id from dbo.sysdiagrams where principal_id = @owner_id and name = @diagramname 
		if(@DiagId IS NULL or (@IsDbo = 0 and @UIDFound <> @theId))
		begin
			RAISERROR ('Diagram does not exist or you do not have permission.', 16, 1)
			return -3
		end
	
		delete from dbo.sysdiagrams where diagram_id = @DiagId;
	
		return 0;
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_helpdiagramdefinition]    Script Date: 15.04.2016 19:05:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_helpdiagramdefinition]
	(
		@diagramname 	sysname,
		@owner_id	int	= null 		
	)
	WITH EXECUTE AS N'dbo'
	AS
	BEGIN
		set nocount on

		declare @theId 		int
		declare @IsDbo 		int
		declare @DiagId		int
		declare @UIDFound	int
	
		if(@diagramname is null)
		begin
			RAISERROR (N'E_INVALIDARG', 16, 1);
			return -1
		end
	
		execute as caller;
		select @theId = DATABASE_PRINCIPAL_ID();
		select @IsDbo = IS_MEMBER(N'db_owner');
		if(@owner_id is null)
			select @owner_id = @theId;
		revert; 
	
		select @DiagId = diagram_id, @UIDFound = principal_id from dbo.sysdiagrams where principal_id = @owner_id and name = @diagramname;
		if(@DiagId IS NULL or (@IsDbo = 0 and @UIDFound <> @theId ))
		begin
			RAISERROR ('Diagram does not exist or you do not have permission.', 16, 1);
			return -3
		end

		select version, definition FROM dbo.sysdiagrams where diagram_id = @DiagId ; 
		return 0
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_helpdiagrams]    Script Date: 15.04.2016 19:05:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_helpdiagrams]
	(
		@diagramname sysname = NULL,
		@owner_id int = NULL
	)
	WITH EXECUTE AS N'dbo'
	AS
	BEGIN
		DECLARE @user sysname
		DECLARE @dboLogin bit
		EXECUTE AS CALLER;
			SET @user = USER_NAME();
			SET @dboLogin = CONVERT(bit,IS_MEMBER('db_owner'));
		REVERT;
		SELECT
			[Database] = DB_NAME(),
			[Name] = name,
			[ID] = diagram_id,
			[Owner] = USER_NAME(principal_id),
			[OwnerID] = principal_id
		FROM
			sysdiagrams
		WHERE
			(@dboLogin = 1 OR USER_NAME(principal_id) = @user) AND
			(@diagramname IS NULL OR name = @diagramname) AND
			(@owner_id IS NULL OR principal_id = @owner_id)
		ORDER BY
			4, 5, 1
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_renamediagram]    Script Date: 15.04.2016 19:05:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_renamediagram]
	(
		@diagramname 		sysname,
		@owner_id		int	= null,
		@new_diagramname	sysname
	
	)
	WITH EXECUTE AS 'dbo'
	AS
	BEGIN
		set nocount on
		declare @theId 			int
		declare @IsDbo 			int
		
		declare @UIDFound 		int
		declare @DiagId			int
		declare @DiagIdTarg		int
		declare @u_name			sysname
		if((@diagramname is null) or (@new_diagramname is null))
		begin
			RAISERROR ('Invalid value', 16, 1);
			return -1
		end
	
		EXECUTE AS CALLER;
		select @theId = DATABASE_PRINCIPAL_ID();
		select @IsDbo = IS_MEMBER(N'db_owner'); 
		if(@owner_id is null)
			select @owner_id = @theId;
		REVERT;
	
		select @u_name = USER_NAME(@owner_id)
	
		select @DiagId = diagram_id, @UIDFound = principal_id from dbo.sysdiagrams where principal_id = @owner_id and name = @diagramname 
		if(@DiagId IS NULL or (@IsDbo = 0 and @UIDFound <> @theId))
		begin
			RAISERROR ('Diagram does not exist or you do not have permission.', 16, 1)
			return -3
		end
	
		-- if((@u_name is not null) and (@new_diagramname = @diagramname))	-- nothing will change
		--	return 0;
	
		if(@u_name is null)
			select @DiagIdTarg = diagram_id from dbo.sysdiagrams where principal_id = @theId and name = @new_diagramname
		else
			select @DiagIdTarg = diagram_id from dbo.sysdiagrams where principal_id = @owner_id and name = @new_diagramname
	
		if((@DiagIdTarg is not null) and  @DiagId <> @DiagIdTarg)
		begin
			RAISERROR ('The name is already used.', 16, 1);
			return -2
		end		
	
		if(@u_name is null)
			update dbo.sysdiagrams set [name] = @new_diagramname, principal_id = @theId where diagram_id = @DiagId
		else
			update dbo.sysdiagrams set [name] = @new_diagramname where diagram_id = @DiagId
		return 0
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_upgraddiagrams]    Script Date: 15.04.2016 19:05:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_upgraddiagrams]
	AS
	BEGIN
		IF OBJECT_ID(N'dbo.sysdiagrams') IS NOT NULL
			return 0;
	
		CREATE TABLE dbo.sysdiagrams
		(
			name sysname NOT NULL,
			principal_id int NOT NULL,	-- we may change it to varbinary(85)
			diagram_id int PRIMARY KEY IDENTITY,
			version int,
	
			definition varbinary(max)
			CONSTRAINT UK_principal_name UNIQUE
			(
				principal_id,
				name
			)
		);


		/* Add this if we need to have some form of extended properties for diagrams */
		/*
		IF OBJECT_ID(N'dbo.sysdiagram_properties') IS NULL
		BEGIN
			CREATE TABLE dbo.sysdiagram_properties
			(
				diagram_id int,
				name sysname,
				value varbinary(max) NOT NULL
			)
		END
		*/

		IF OBJECT_ID(N'dbo.dtproperties') IS NOT NULL
		begin
			insert into dbo.sysdiagrams
			(
				[name],
				[principal_id],
				[version],
				[definition]
			)
			select	 
				convert(sysname, dgnm.[uvalue]),
				DATABASE_PRINCIPAL_ID(N'dbo'),			-- will change to the sid of sa
				0,							-- zero for old format, dgdef.[version],
				dgdef.[lvalue]
			from dbo.[dtproperties] dgnm
				inner join dbo.[dtproperties] dggd on dggd.[property] = 'DtgSchemaGUID' and dggd.[objectid] = dgnm.[objectid]	
				inner join dbo.[dtproperties] dgdef on dgdef.[property] = 'DtgSchemaDATA' and dgdef.[objectid] = dgnm.[objectid]
				
			where dgnm.[property] = 'DtgSchemaNAME' and dggd.[uvalue] like N'_EA3E6268-D998-11CE-9454-00AA00A3F36E_' 
			return 2;
		end
		return 1;
	END
	
GO
USE [master]
GO
ALTER DATABASE [ZenChat] SET  READ_WRITE 
GO
