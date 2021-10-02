namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Core.ViewModels.Categories;
    using FastFood.Core.ViewModels.Employees;
    using FastFood.Core.ViewModels.Items;
    using FastFood.Core.ViewModels.Orders;
    using FastFood.Models;
    using System;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            //Employee
            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(x=>x.PositionId,y=>y.MapFrom(s=>s.Id));

            this.CreateMap<RegisterEmployeeInputModel, Employee>();

            this.CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(x=>x.Position, y=>y.MapFrom(x=>x.Position.Name));

            //Category
            this.CreateMap<Category, CategoryAllViewModel>();

            //Item
            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(x=>x.CategoryId, y=>y.MapFrom(s=>s.Id));

            this.CreateMap<CreateItemInputModel, Item>();

            this.CreateMap<Item, ItemsAllViewModels>();

            //Order

            //Automapping collection items???
            this.CreateMap<CreateOrderInputModel, Order>()
                .ForMember(x=>x.DateTime, y=>y.MapFrom(s=>DateTime.Now));

            this.CreateMap<Order, OrderAllViewModel>()
                .ForMember(x=>x.Employee, y=>y.MapFrom(s=>s.Employee.Name))
                .ForMember(x=>x.OrderId, y=>y.MapFrom(s=>s.Id));

        }
    }
}
