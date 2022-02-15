
using FluentValidation;
namespace WebApi.Models
{
    public class AccountLoginDto
    {
        /// <summary>
        /// 账号名称
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 账号密码
        /// </summary>
        public string AccountPasswd { get; set; }
    }

    /// <summary>
    ///验证
    /// </summary>
    public class AccountLoginDtoValidator : AbstractValidator<AccountLoginDto>, IValidator
    {
        public AccountLoginDtoValidator()
        {
            RuleFor(_ => _.AccountName).NotEmpty();
            RuleFor(_ => _.AccountPasswd).NotEmpty();
        }
    }

    public class AccountChangePassDto
    {
        public string AccountName { get; set; }

        public string AccountPasswd { get; set; }

        public string AccountChangePasswd { get; set; }
    }


    public class AccountChangePassDtoValidator : AbstractValidator<AccountChangePassDto>, IValidator
    {
        public AccountChangePassDtoValidator()
        {
            RuleFor(_ => _.AccountName).NotEmpty();
            RuleFor(_ => _.AccountPasswd).NotEmpty();
            RuleFor(_ => _.AccountChangePasswd).NotEmpty();
        }
    }
}
