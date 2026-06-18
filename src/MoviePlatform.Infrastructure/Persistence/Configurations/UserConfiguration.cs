using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviePlatform.Domain.Users;
using MoviePlatform.Domain.Users.ValueObjects;

namespace MoviePlatform.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable(
			"users",
			table =>
			{
				table.HasCheckConstraint(
					"CK_users_name_not_empty",
					"length(name) > 0");

				table.HasCheckConstraint(
					"CK_users_email_not_empty",
					"length(email) > 0");

				table.HasCheckConstraint(
					"CK_users_password_hash_not_empty",
					"length(password_hash) > 0");
			});

		builder
            .HasIndex(user => user.Email)
            .IsUnique()
            .HasDatabaseName("IX_users_email_unique");

		builder.HasKey(user => user.Id);

		builder
			.Property(user => user.Id)
			.HasColumnName("user_id")
			.HasConversion(
				userId => userId.Value,
				value => UserId.FromPersistence(value));

		builder
			.Property(user => user.Name)
			.HasColumnName("name")
			.IsRequired()
			.HasMaxLength(UserConstants.Name.MaxLength)
			.HasConversion(
				name => name.Value,
				value => Name.FromPersistence(value));

		builder
			.Property(user => user.Email)
			.HasColumnName("email")
			.IsRequired()
			.HasMaxLength(UserConstants.Email.MaxLength)
			.HasConversion(
				email => email.Value,
				value => Email.FromPersistence(value));

		builder
			.Property(user => user.PasswordHash)
			.HasColumnName("password_hash")
			.IsRequired()
			.HasConversion(
				passwordHash => passwordHash.Value,
				value => PasswordHash.FromPersistence(value));

		builder
			.Property(user => user.CreatedAtUtc)
			.HasColumnName("created_at_utc")
			.IsRequired();
	}
}
