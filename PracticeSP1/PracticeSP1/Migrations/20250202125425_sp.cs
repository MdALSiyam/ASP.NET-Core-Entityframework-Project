using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PracticeSP1.Migrations
{
    /// <inheritdoc />
    public partial class sp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TYPE PModuleType AS TABLE(
                    ModuleName NVARCHAR(50),
                    Duration INT
                );
                GO
                CREATE OR ALTER PROC dbo.spInsertStudent
                @StudentName NVARCHAR(50),
                @Dob DATETIME,
                @MobileNo NVARCHAR(15),
                @IsEnrolled BIT,
                @RegistrationFee DECIMAL(18,2),
                @CourseId INT,
                @ImageUrl NVARCHAR(MAX),
                @Modules PModuleType READONLY
                AS
                BEGIN
                SET NOCOUNT ON;
                BEGIN TRY
                DECLARE @LocalTable TABLE
                (
	                ModuleName NVARCHAR(50),
	                Duration INT,
	                StudentId INT
                );
                DECLARE @StudentId INT;
                INSERT INTO dbo.Students
                (StudentName,Dob,MobileNo,IsEnrolled,RegistrationFee,ImageUrl,CourseId)
                VALUES
                (@StudentName,@Dob,@MobileNo,@IsEnrolled,@RegistrationFee,@ImageUrl,@CourseId);
                SET @StudentId=SCOPE_IDENTITY();
                INSERT INTO @LocalTable(ModuleName,Duration,StudentId)
                SELECT ModuleName, Duration, @StudentId FROM @Modules;
                INSERT INTO dbo.Modules(ModuleName,Duration,StudentId)
                SELECT ModuleName,Duration, @StudentId FROM @LocalTable;
                END TRY
                BEGIN CATCH 
		                --Handel EXP
	                THROW
                END CATCH
                END
            
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROC IF EXISTS dbo.spInsertStudent");
            migrationBuilder.Sql("DROP TYPE IF EXISTS PModuleType");
        }
    }
}
