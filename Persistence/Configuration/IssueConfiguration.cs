using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class IssueConfiguration : EntityTypeConfiguration<Issue>
    {
        public IssueConfiguration() 
        {
            HasKey(x => x.Id);

            Property(x => x.Name).HasMaxLength(20).IsRequired();

            HasIndex(x => x.Name).IsUnique();

            Property(x => x.Priority).HasMaxLength(10).IsRequired();

            Property(x => x.Memo).HasMaxLength(128).IsOptional();

            Property(x => x.Description).HasMaxLength(300).IsOptional();

            Property(x => x.Solution).HasMaxLength(300).IsOptional();

            Property(x => x.LastModified).IsOptional();

            HasRequired(x => x.CreatedBy)
                .WithMany(x => x.IssuesCreated)
                .HasForeignKey(x => x.CreatedById)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.LastModifiedBy)
                .WithMany(x => x.IssuesModefied)
                .HasForeignKey(x => x.LastModifiedById)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Type)
                .WithMany(x => x.Issues)
                .HasForeignKey(x => x.TypeId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.SubType)
                .WithMany(x => x.SubTypes)
                .HasForeignKey(x => x.SubTypeId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Problem)
                .WithMany(x => x.Problems)
                .HasForeignKey(x => x.ProblemId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Status)
                .WithMany(x => x.Issues)
                .HasForeignKey(x => x.StatusId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Assigned)
                .WithMany(x => x.Issues)
                .HasForeignKey(x => x.AssignedId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Pos)
                .WithMany(x => x.Issues)
                .HasForeignKey(x => x.PosId)
                .WillCascadeOnDelete(false);


        }
    }
}
