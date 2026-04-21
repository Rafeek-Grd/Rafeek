using MediatR;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.GPAHandlers.Commands.SimulateGPA
{
    public class SimulateGPACommandHandler : IRequestHandler<SimulateGPACommand, float>
    {
        private readonly IUnitOfWork _ctx;

        public SimulateGPACommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<float> Handle(SimulateGPACommand request, CancellationToken cancellationToken)
        {            
            var predictedCGPA = (request.ExpectedGPA + 3.0f) / 2.0f;

            var log = new GPASimulatorLog
            {
                StudentId = request.StudentId,
                ExpectedGPA = request.ExpectedGPA,
                PredictedCGPA = (float)Math.Round(predictedCGPA, 2)
            };

            _ctx.GPASimulatorLogRepository.Add(log);
            await _ctx.SaveChangesAsync(cancellationToken);

            return log.PredictedCGPA;
        }
    }
}
