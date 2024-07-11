using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Events.Integration;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Services.Interfaces;

namespace Ozon.MerchService.CQRS.Handlers;

    public class ReserveMerchPackHandler(
            IEmployeeRepository employeeRepository,
            IMerchPackRequestRepository merchPackRequestRepository,
            IStockGrpcService stockGrpcService) : IRequestHandler<ReserveMerchPackCommand, Status>
    {
        private const string HandlerName = nameof(ReserveMerchPackHandler);
        
        public async Task<Status> Handle(ReserveMerchPackCommand request, CancellationToken token)
        {
            var employee = await employeeRepository.GetByEmailAsync(request.EmployeeEmail, token);

            if (employee is null)
            {
                var employeeData = new Employee(
                    new FullName(request.EmployeeFirstName, request.EmployeeLastName),
                    new Email(request.EmployeeEmail),
                    new Email(request.HrEmail),
                    request.EmployeeClothingSize);

                var employeeId = await employeeRepository.CreateAsync(employeeData, token);

                employee = Employee.CreateInstance(employeeId, employee);
            }
            
            var canReceiveMerchPack = employee.CanReceiveMerchPack(request.MerchPackType);

            var merchPackRequestData = new MerchPackRequest(request.MerchPackType, employee);

            if (!canReceiveMerchPack)
            {
                merchPackRequestData.SetStatusDeclined();
                
                return merchPackRequestData.Status;
            }

            var merchPackRequestId = await merchPackRequestRepository.CreateAsync(merchPackRequestData, token);
            var merchPackRequest = MerchPackRequest.CreateInstance(merchPackRequestId, merchPackRequestData);
            
            if (await stockGrpcService.ReserveMerchPackItems(merchPackRequest, token))
            {
                var employeeNotificationEvent = new MerchReplenishedEvent(employee.Email, request.MerchPackType);
                merchPackRequest.SetStatusIssued();
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
    }