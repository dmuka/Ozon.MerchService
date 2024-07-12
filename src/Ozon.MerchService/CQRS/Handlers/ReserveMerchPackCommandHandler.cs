using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Events.Integration;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Services.Interfaces;

namespace Ozon.MerchService.CQRS.Handlers;

    public class ReserveMerchPackCommandHandler(
            IEmployeeRepository employeeRepository,
            IMerchPackRequestRepository merchPackRequestRepository,
            IStockGrpcService stockGrpcService) : IRequestHandler<ReserveMerchPackCommand, Status>
    {
        private const string HandlerName = nameof(ReserveMerchPackCommandHandler);
        
        public async Task<Status> Handle(ReserveMerchPackCommand request, CancellationToken token)
        {
            Employee employee;
            
            if (Equals(request.Status, Status.Created))
            {
                employee = await GetEmployee(request, token);
            }
            else
            {
                employee = request.Employee;
            }

            var canReceiveMerchPack = employee.CanReceiveMerchPack(request.MerchPackType);

            var merchPackRequestData = new MerchPackRequest(request.MerchPackType, employee, request.RequestType);

            if (!canReceiveMerchPack)
            {
                merchPackRequestData.SetStatusDeclined();
                
                return merchPackRequestData.Status;
            }

            var merchPackRequestId = Equals(request.Status, Status.Created)
                ? await merchPackRequestRepository.CreateAsync(merchPackRequestData, token)
                : request.Id;
            var merchPackRequest = MerchPackRequest.CreateInstance(merchPackRequestId, merchPackRequestData);
            
            if (await stockGrpcService.GetMerchPackItemsAvailability(merchPackRequest, token) 
                && await stockGrpcService.ReserveMerchPackItems(merchPackRequest, token))
            {
                merchPackRequest.SetStatusIssued();
                
                if (Equals(request.Status, Status.Created))
                {
                    
                }
                else
                {
                 //var employeeNotificationEvent = new MerchReplenishedEvent(employee.Email, request.MerchPackType); for queued employees                   
                }
                
                //await SendMessage(employee, merchPackRequest, token); Add kafka
            }
            else
            {
                merchPackRequest.SetStatusQuequed();
                
                var hrNotificationEvent = new MerchEndedEvent(employee.HrEmail, request.MerchPackType);
            }
            
            await merchPackRequestRepository.UpdateAsync(merchPackRequest, token);
            
            return merchPackRequest.Status;
        }

        private async Task<Employee> GetEmployee(ReserveMerchPackCommand request, CancellationToken token)
        {
            var employee = await employeeRepository.GetByEmailAsync(request.EmployeeEmail, token);

            if (employee is null)
            {
                var employeeData = new Employee(
                    new FullName(request.EmployeeFullName),
                    new Email(request.EmployeeEmail),
                    new Email(request.HrEmail),
                    request.EmployeeClothingSize);

                var employeeId = await employeeRepository.CreateAsync(employeeData, token);

                employee = Employee.CreateInstance(employeeId, employee);
            }

            return employee;
        }
    }