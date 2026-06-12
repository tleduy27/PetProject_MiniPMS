using FluentValidation;
using MediatR;
using System.Runtime.Intrinsics.X86;

namespace PetProject2026.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //B1: Lay danh sach validator ap dung cho request nay
            if (!_validators.Any())
            {
                return await next();
            }
            //B2: Chay tung validator
            var context = new ValidationContext<TRequest>(request);

            //Vi co nhieu validator cho cung 1 request nen ta chay het tat ca song song:
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            //B3: Goi loi va quyet dinh throw hay không
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
            if(failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
            return await next();
        }
        

    }
    
}
