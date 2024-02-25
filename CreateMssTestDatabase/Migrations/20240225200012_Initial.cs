using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CreateMssTestDatabase.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExactNumBigInt = table.Column<long>(type: "bigint", nullable: false),
                    ExactNumInt = table.Column<int>(type: "int", nullable: false),
                    ExactNumSmallInt = table.Column<short>(type: "smallint", nullable: false),
                    ExactNumTinyInt = table.Column<byte>(type: "tinyint", nullable: false),
                    ExactNumBit = table.Column<bool>(type: "bit", nullable: false),
                    ExactNumMoney = table.Column<decimal>(type: "money", nullable: false),
                    ExactNumSmallMoney = table.Column<decimal>(type: "smallmoney", nullable: false),
                    ExactNumDecimal = table.Column<decimal>(type: "decimal(28,3)", precision: 28, scale: 3, nullable: false),
                    ExactNumNumeric = table.Column<decimal>(type: "numeric(28,3)", precision: 28, scale: 3, nullable: false),
                    ApproxNumFloat = table.Column<double>(type: "float", nullable: false),
                    ApproxNumReal = table.Column<float>(type: "real", nullable: false),
                    DTDate = table.Column<DateTime>(type: "date", nullable: false),
                    DTDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    DTDateTime2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DTSmallDateTime = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    DTDateTimeOffset = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DTTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CharStrChar20 = table.Column<string>(type: "char(20)", unicode: false, maxLength: 20, nullable: true),
                    CharStrVarchar20 = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    CharStrVarchar10K = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    CharStrNChar20 = table.Column<string>(type: "nchar(20)", maxLength: 20, nullable: true),
                    CharStrNVarchar20 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CharStrNVarchar10K = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BinBinary5K = table.Column<byte[]>(type: "binary(5000)", maxLength: 5000, nullable: true),
                    BinVarbinary10K = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    OtherGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OtherXml = table.Column<string>(type: "xml", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    AuditId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EntityCompressed = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AuditStorePath = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "CacheCommands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Command = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Protocol = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CacheCommands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguredScopes",
                columns: table => new
                {
                    ScopeId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AllowUnrestrictedIntrospection = table.Column<bool>(type: "bit", nullable: false),
                    ClaimsRule = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Emphasize = table.Column<bool>(type: "bit", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    IncludeAllClaimsForUser = table.Column<bool>(type: "bit", nullable: false),
                    AllowedForTenantSpecificClients = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Required = table.Column<bool>(type: "bit", nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "bit", nullable: false),
                    IsStandardScope = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguredScopes", x => x.ScopeId);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguredTenants",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AllowPartialLogin = table.Column<bool>(type: "bit", nullable: false),
                    TenantName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguredTenants", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "ExternalPersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalPersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "ExternalUserProfiles",
                columns: table => new
                {
                    SubjectId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CompanyId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUserProfiles", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "IdpMetaData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: false),
                    Raw = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUsed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdpMetaData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    U4SessionId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IdpName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    SubjectId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Scopes = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => new { x.SubjectId, x.ClientId });
                });

            migrationBuilder.CreateTable(
                name: "PermissionsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SubjectId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Action = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Scopes = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionsHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "PurgeSemaphore",
                columns: table => new
                {
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurgeSemaphore", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "ScopeTitles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScopeId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScopeTitles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsageHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastUsed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScopeClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScopeId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    AlwaysIncludeInIdToken = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScopeClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScopeClaims_ConfiguredScopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "ConfiguredScopes",
                        principalColumn: "ScopeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScopeConsentOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScopeId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    RequireConsent = table.Column<bool>(type: "bit", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    LinkDescription = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScopeConsentOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScopeConsentOptions_ConfiguredScopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "ConfiguredScopes",
                        principalColumn: "ScopeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScopeSecrets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScopeId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Expiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScopeSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScopeSecrets_ConfiguredScopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "ConfiguredScopes",
                        principalColumn: "ScopeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguredClients",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    AccessTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    AccessTokenType = table.Column<int>(type: "int", nullable: false),
                    AllowAccessToAllScopes = table.Column<bool>(type: "bit", nullable: false),
                    AlwaysSendClientClaims = table.Column<bool>(type: "bit", nullable: false),
                    AuthorizationCodeLifetime = table.Column<int>(type: "int", nullable: false),
                    ClientUri = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Flow = table.Column<int>(type: "int", nullable: false),
                    IdentityTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    IncludeJwtId = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerTenant = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    PrefixClientClaims = table.Column<bool>(type: "bit", nullable: false),
                    RefreshTokenExpiration = table.Column<int>(type: "int", nullable: false),
                    RefreshTokenUsage = table.Column<int>(type: "int", nullable: false),
                    RequireConsent = table.Column<bool>(type: "bit", nullable: false),
                    SlidingRefreshTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguredClients", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_ConfiguredClients_ConfiguredTenants_OwnerTenant",
                        column: x => x.OwnerTenant,
                        principalTable: "ConfiguredTenants",
                        principalColumn: "TenantId");
                });

            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    DomainName = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Domains_ConfiguredTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "ConfiguredTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Idps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdpName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    Authority = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    IdpRegId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdpSecret = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Protocol = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NameClaimType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IncludeIdentityScopesInConsent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Idps_ConfiguredTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "ConfiguredTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalUserProfileId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    ValueType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalUserClaims_ExternalUserProfiles_ExternalUserProfileId",
                        column: x => x.ExternalUserProfileId,
                        principalTable: "ExternalUserProfiles",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginId = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginData_Logins_LoginId",
                        column: x => x.LoginId,
                        principalTable: "Logins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AllowedCorsOrigins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedCorsOrigins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllowedCorsOrigins_ConfiguredClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "ConfiguredClients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AllowedCustomGrantTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    GrantType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedCustomGrantTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllowedCustomGrantTypes_ConfiguredClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "ConfiguredClients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Issuer = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    OriginalIssuer = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    ValueType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientClaims_ConfiguredClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "ConfiguredClients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientScope",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    ScopeId = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientScope", x => new { x.ClientId, x.ScopeId });
                    table.ForeignKey(
                        name: "FK_ClientScope_ConfiguredClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "ConfiguredClients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientScope_ConfiguredScopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "ConfiguredScopes",
                        principalColumn: "ScopeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientSecrets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Expiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientSecrets_ConfiguredClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "ConfiguredClients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostLogoutRedirectUris",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLogoutRedirectUris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostLogoutRedirectUris_ConfiguredClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "ConfiguredClients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RedirectUris",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedirectUris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RedirectUris_ConfiguredClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "ConfiguredClients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyIdClaimTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdpId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyIdClaimTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyIdClaimTypes_Idps_IdpId",
                        column: x => x.IdpId,
                        principalTable: "Idps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenIdConnectOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdpId = table.Column<int>(type: "int", nullable: false),
                    ResponseType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Scope = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EndSessionEndpoint = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true),
                    AcrValues = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIdConnectOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIdConnectOptions_Idps_IdpId",
                        column: x => x.IdpId,
                        principalTable: "Idps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientClaimId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(2047)", maxLength: 2047, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimProperties_ClientClaims_ClientClaimId",
                        column: x => x.ClientClaimId,
                        principalTable: "ClientClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AllTypes",
                columns: new[] { "Id", "ApproxNumFloat", "ApproxNumReal", "BinBinary5K", "BinVarbinary10K", "CharStrChar20", "CharStrNChar20", "CharStrNVarchar10K", "CharStrNVarchar20", "CharStrVarchar10K", "CharStrVarchar20", "DTDate", "DTDateTime", "DTDateTime2", "DTDateTimeOffset", "DTSmallDateTime", "DTTime", "ExactNumBigInt", "ExactNumBit", "ExactNumDecimal", "ExactNumInt", "ExactNumMoney", "ExactNumNumeric", "ExactNumSmallInt", "ExactNumSmallMoney", "ExactNumTinyInt", "OtherGuid", "OtherXml" },
                values: new object[] { 1L, 1234567.1234500001, 123.123f, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135 }, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, "1234567890", "ﯵ1234567890", "ﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵﯵ", "123456789ﯵ1234567890", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "12345678901234567890", new DateTime(2023, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 31, 11, 12, 13, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 31, 11, 12, 13, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2023, 3, 31, 11, 12, 13, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), new DateTime(2023, 3, 31, 11, 12, 13, 0, DateTimeKind.Unspecified), new TimeSpan(1, 2, 3, 4, 0), 123456789012345L, true, 1234567890123456789.123456m, 123456789, 1234567.1234m, 1234567.12345m, (short)12345, 123.123m, (byte)123, new Guid("a17542d9-a61c-4e4c-8512-daffc1416142"), "<?xml version=\"1.0\" encoding=\"utf-8\" ?> <SomeTag>A value</SomeTag>" });

            migrationBuilder.InsertData(
                table: "ConfiguredScopes",
                columns: new[] { "ScopeId", "AllowUnrestrictedIntrospection", "AllowedForTenantSpecificClients", "ClaimsRule", "Description", "DisplayName", "Emphasize", "Enabled", "IncludeAllClaimsForUser", "IsStandardScope", "LastUpdate", "Required", "ShowInDiscoveryDocument", "Type", "UserId", "UserName" },
                values: new object[,]
                {
                    { "address", false, true, null, "", "Use your postal address", true, true, false, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 0, null, null },
                    { "email", false, true, null, "", "Use your email address", true, true, false, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 0, null, null },
                    { "offline_access", false, true, null, "Access to an application has limited lifetime. Letting the application remember you extends your access lifetime and for example allows you to login only once. The application may ask for permission on your behalf without prompting for permission (including when you are not present).", "Remember you (offline access)", true, true, false, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 1, null, null },
                    { "openid", false, true, null, "The application needs this to be able to securely identify you. If you do not grant this then you cannot use the application.", "Use your user identifier", true, true, false, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 0, null, null },
                    { "phone", false, true, null, "", "Use your phone number", true, true, false, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 0, null, null },
                    { "profile", false, true, null, "Your user profile information (first name, last name, etc.).", "Use your user information", true, true, false, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 0, null, null },
                    { "roles", false, true, null, "The application supports authorization wholly or in part based on the roles you have been assigned by your organization. Rights within the application might be limited if you fail to grant this.", "Access your externally assigned roles", true, true, false, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "ScopeClaims",
                columns: new[] { "Id", "AlwaysIncludeInIdToken", "Description", "Name", "ScopeId" },
                values: new object[,]
                {
                    { 1, true, null, "sub", "openid" },
                    { 2, true, null, "tenant", "openid" },
                    { 3, true, null, "company_id", "openid" },
                    { 4, false, null, "name", "profile" },
                    { 5, false, null, "family_name", "profile" },
                    { 6, false, null, "given_name", "profile" },
                    { 7, false, null, "middle_name", "profile" },
                    { 8, false, null, "nickname", "profile" },
                    { 9, false, null, "preferred_username", "profile" },
                    { 10, false, null, "profile", "profile" },
                    { 11, false, null, "picture", "profile" },
                    { 12, false, null, "website", "profile" },
                    { 13, false, null, "gender", "profile" },
                    { 14, false, null, "birthdate", "profile" },
                    { 15, false, null, "zoneinfo", "profile" },
                    { 16, false, null, "locale", "profile" },
                    { 17, false, null, "updated_at", "profile" },
                    { 18, false, null, "email", "email" },
                    { 19, false, null, "email_verified", "email" },
                    { 20, false, null, "phone_number", "phone" },
                    { 21, false, null, "phone_number_verified", "phone" },
                    { 22, false, null, "address", "address" },
                    { 23, false, "External role claims", "role", "roles" }
                });

            migrationBuilder.InsertData(
                table: "ScopeConsentOptions",
                columns: new[] { "Id", "Link", "LinkDescription", "RequireConsent", "ScopeId" },
                values: new object[,]
                {
                    { 1, null, null, true, "openId" },
                    { 2, null, null, true, "profile" },
                    { 3, null, null, true, "email" },
                    { 4, null, null, true, "offline_access" },
                    { 5, null, null, true, "phone" },
                    { 6, null, null, true, "address" },
                    { 7, null, null, true, "roles" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllowedCorsOrigins_ClientId",
                table: "AllowedCorsOrigins",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AllowedCustomGrantTypes_ClientId",
                table: "AllowedCustomGrantTypes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimProperties_ClientClaimId",
                table: "ClaimProperties",
                column: "ClientClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientClaims_ClientId",
                table: "ClientClaims",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientScope_ScopeId",
                table: "ClientScope",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSecrets_ClientId",
                table: "ClientSecrets",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyIdClaimTypes_IdpId",
                table: "CompanyIdClaimTypes",
                column: "IdpId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguredClients_OwnerTenant",
                table: "ConfiguredClients",
                column: "OwnerTenant");

            migrationBuilder.CreateIndex(
                name: "IX_Domains_TenantId",
                table: "Domains",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalUserClaims_ExternalUserProfileId",
                table: "ExternalUserClaims",
                column: "ExternalUserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Idps_TenantId",
                table: "Idps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginData_LoginId",
                table: "LoginData",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIdConnectOptions_IdpId",
                table: "OpenIdConnectOptions",
                column: "IdpId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostLogoutRedirectUris_ClientId",
                table: "PostLogoutRedirectUris",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_RedirectUris_ClientId",
                table: "RedirectUris",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeClaims_ScopeId",
                table: "ScopeClaims",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeConsentOptions_ScopeId",
                table: "ScopeConsentOptions",
                column: "ScopeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScopeSecrets_ScopeId",
                table: "ScopeSecrets",
                column: "ScopeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllowedCorsOrigins");

            migrationBuilder.DropTable(
                name: "AllowedCustomGrantTypes");

            migrationBuilder.DropTable(
                name: "AllTypes");

            migrationBuilder.DropTable(
                name: "Audits");

            migrationBuilder.DropTable(
                name: "CacheCommands");

            migrationBuilder.DropTable(
                name: "ClaimProperties");

            migrationBuilder.DropTable(
                name: "ClientScope");

            migrationBuilder.DropTable(
                name: "ClientSecrets");

            migrationBuilder.DropTable(
                name: "CompanyIdClaimTypes");

            migrationBuilder.DropTable(
                name: "Domains");

            migrationBuilder.DropTable(
                name: "ExternalPersistedGrants");

            migrationBuilder.DropTable(
                name: "ExternalUserClaims");

            migrationBuilder.DropTable(
                name: "IdpMetaData");

            migrationBuilder.DropTable(
                name: "LoginData");

            migrationBuilder.DropTable(
                name: "OpenIdConnectOptions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PermissionsHistory");

            migrationBuilder.DropTable(
                name: "PersistedGrants");

            migrationBuilder.DropTable(
                name: "PostLogoutRedirectUris");

            migrationBuilder.DropTable(
                name: "PurgeSemaphore");

            migrationBuilder.DropTable(
                name: "RedirectUris");

            migrationBuilder.DropTable(
                name: "ScopeClaims");

            migrationBuilder.DropTable(
                name: "ScopeConsentOptions");

            migrationBuilder.DropTable(
                name: "ScopeSecrets");

            migrationBuilder.DropTable(
                name: "ScopeTitles");

            migrationBuilder.DropTable(
                name: "UsageHistory");

            migrationBuilder.DropTable(
                name: "ClientClaims");

            migrationBuilder.DropTable(
                name: "ExternalUserProfiles");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Idps");

            migrationBuilder.DropTable(
                name: "ConfiguredScopes");

            migrationBuilder.DropTable(
                name: "ConfiguredClients");

            migrationBuilder.DropTable(
                name: "ConfiguredTenants");
        }
    }
}
