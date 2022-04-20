
using Domain.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Presistence.Configuration;


public class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder
         .ToTable("Posts", "Post");
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.CommentEnabled);
        builder.Property(e => e.CreateTimeUtc).HasColumnType("datetime");
        builder.Property(e => e.PubDateUtc).HasColumnType("datetime");
        builder.Property(e => e.LastModifiedUtc).HasColumnType("datetime");
        builder.Property(e => e.PostContent);
        builder.Property(e => e.Author).HasMaxLength(64);
        builder.Property(e => e.Slug).HasMaxLength(128);
        builder.Property(e => e.Title).HasMaxLength(128);
        builder.Property(e => e.OriginLink).HasMaxLength(256);
        builder.Property(e => e.HeroImageUrl).HasMaxLength(256);
    }
}
public class CategorysConfig : IEntityTypeConfiguration<Categorys>
{
    public void Configure(EntityTypeBuilder<Categorys> builder)
    {
        builder
        .ToTable("Categorys", "Post");
        builder.HasKey(e => e.CategoryId);
        builder.Property(e => e.DisplayName).HasMaxLength(64);
        builder.Property(e => e.Note).HasMaxLength(128);
    }
}


public class TagsConfig : IEntityTypeConfiguration<Tags>
{
    public void Configure(EntityTypeBuilder<Tags> builder)
    {
        builder
           .ToTable("Tags", "Post");
        builder.HasKey(e => e.TagId);
        builder.Property(e => e.DisplayName).HasMaxLength(64);
    }
}