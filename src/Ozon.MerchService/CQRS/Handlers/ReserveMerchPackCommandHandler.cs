using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Events.Integration;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.MessageBroker.Interfaces;
using Ozon.MerchService.Infrastructure.Services.Interfaces;

namespace Ozon.MerchService.CQRS.Handlers;

    public class ReserveMerchPackCommandHandler(
            IEmployeeRepository employeeRepository,
            IMerchPackRequestRepository merchPackRequestRepository,
            IMerchPacksRepository merchPacksRepository,
            IStockGrpcService stockGrpcService,
            IMessageBroker broker,
            IUnitOfWork unitOfWork) : IRequestHandler<ReserveMerchPackCommand, RequestStatus>
    {
        private const string HandlerName = nameof(ReserveMerchPackCommandHandler);
        
        public async Task<RequestStatus> Handle(ReserveMerchPackCommand request, CancellationToken token)
        {
            await unitOfWork.StartTransaction(token);
            
            Employee employee;
            
            if (Equals(request.RequestStatus, RequestStatus.Created))
            {
                employee = await GetEmployee(request, token);
            }
            else
            {
                employee = request.Employee;
            }

            var canReceiveMerchPack = employee.CanReceiveMerchPack(request.MerchPackType);

            var merchPack = await merchPacksRepository.GetMerchPackByMerchType(request.MerchPackType, token);

            var merchPackRequestData = new MerchPackRequest(merchPack, merchPack.Items, employee, request.RequestType);

            if (!canReceiveMerchPack)
            {
                merchPackRequestData.SetStatusDeclined();
                
                return merchPackRequestData.RequestStatus;
            }
            
            stockGrpcService.SetItemsSkusInRequest(merchPackRequestData, request.EmployeeClothingSize, token);

            var merchPackRequestId = Equals(request.RequestStatus, RequestStatus.Created)
                ? await merchPackRequestRepository.CreateAsync(merchPackRequestData, token)
                : request.Id;
            var merchPackRequest = merchPackRequestData;
            
            if (!canReceiveMerchPack)
            {
                merchPackRequestData.SetStatusDeclined();
                
                await merchPackRequestRepository.UpdateAsync(merchPackRequest, token);
                
                return merchPackRequestData.RequestStatus;
            }
            
            if (await stockGrpcService.GetMerchPackItemsAvailability(merchPackRequest, token) 
                && await stockGrpcService.ReserveMerchPackItems(merchPackRequest, token))
            {
                if (Equals(request.RequestStatus, RequestStatus.Queued))
                {
                    var employeeNotificationEvent = new MerchReplenishedEvent(employee.Email, request.MerchPackType);

                    await broker.ProduceAsync(
                        "EmployeeNotificationEventTopic", 
                        request.Id.ToString(), 
                        employeeNotificationEvent, 
                        token);
                }
                
                merchPackRequest.SetStatusIssued();
            }
            else
            {
                merchPackRequest.SetStatusQueued();
                
                var hrNotificationEvent = new MerchEndedEvent(merchPackRequest.HrEmail, merchPackRequest.MerchPackType);

                await broker.ProduceAsync(
                    "EmployeeNotificationEventTopic", 
                    request.Id.ToString(),
                    hrNotificationEvent,
                    token);
            }
            
            var affectedRows = await merchPackRequestRepository.UpdateAsync(merchPackRequest, token);

            await unitOfWork.SaveChangesAsync(token);
            
            return merchPackRequest.RequestStatus;
        }

        private async Task<Employee> GetEmployee(ReserveMerchPackCommand request, CancellationToken token)
        {
            var employee = await employeeRepository.GetByEmailAsync(request.EmployeeEmail, token);

            if (employee is not null) return employee;
            var employeeData = new Employee(
                new FullName(request.EmployeeFullName),
                new Email(request.EmployeeEmail));

            var employeeId = await employeeRepository.CreateAsync(employeeData, token);

            employee = Employee.CreateInstance(employeeId, employee);

            return employee;
        }
    }