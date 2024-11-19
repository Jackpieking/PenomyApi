using System;
using Microsoft.AspNetCore.Identity;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

/// <summary>
///     This UnitOfWork file is only contain the dependencies for other Repository to work with.
///     Please do not add any Feature or Common Repositories to this UnitOfWork file.
/// </summary>
public sealed partial class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private readonly Lazy<UserManager<PgUser>> _userManager;
    private readonly Lazy<RoleManager<PgRole>> _roleManager;
    private readonly Lazy<SignInManager<PgUser>> _signInManager;

    public UnitOfWork(
        AppDbContext dbContext,
        Lazy<UserManager<PgUser>> userManager,
        Lazy<RoleManager<PgRole>> roleManager,
        Lazy<SignInManager<PgUser>> signInManager
    )
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }
}
