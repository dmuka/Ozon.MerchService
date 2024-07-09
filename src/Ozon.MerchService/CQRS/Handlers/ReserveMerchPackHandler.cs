using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Services.Interfaces;

namespace Ozon.MerchService.CQRS.Handlers;

    public class ReserveMerchPackHandler(
            IEmployeeRepository employeeRepository,
            IMerchPacksRepository merchPacksRepository,
            IMerchPackRequestRepository merchPackRequestRepository,
            IStockGrpcService stockGrpcService) : IRequestHandler<ReserveMerchPackCommand>
    {
        private const string HandlerName = nameof(ReserveMerchPackHandler);
        
        public async Task Handle(ReserveMerchPackCommand request, CancellationToken token)
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
                merchPackRequestData.SetStatus(Status.Declined);
                
                return;
            }

            var merchPackRequestId = await merchPackRequestRepository.CreateAsync(merchPackRequestData, token);
            var merchPackRequest = MerchPackRequest.CreateInstance(merchPackRequestId, merchPackRequestData);
            
            if (await stockGrpcService.ReserveMerchPackItems(merchPackRequest, token))
            {
                //await SendMessage(employee, merchPackRequest, token); Add kafka
            }
            else
            {
                merchPackRequest.SetStatus(Status.Quequed);
            }
            
            await merchPackRequestRepository.UpdateAsync(merchPackRequest, token);
            
            return;
        }
    }