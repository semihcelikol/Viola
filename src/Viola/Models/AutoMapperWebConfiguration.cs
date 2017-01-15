using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Viola.Models
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new CompanyProfile());
                cfg.AddProfile(new ProjectProfile());
                cfg.AddProfile(new TaskProfile());
                cfg.AddProfile(new EffortProfile());
                cfg.AddProfile(new UserProfile());
            });
        }
    }

    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyViewModel>();
        }
    }

    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectViewModel>();
        }
    }

    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Task, TaskViewModel>();
        }
    }

    public class EffortProfile : Profile
    {
        public EffortProfile()
        {
            CreateMap<Effort, EffortViewModel>();
        }
    }

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, RegisterViewModel>();
            CreateMap<User, EditViewModel>();
        }
    }
}