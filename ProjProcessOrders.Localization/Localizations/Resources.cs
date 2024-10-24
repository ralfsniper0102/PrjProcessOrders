using Microsoft.Extensions.Localization;

namespace ProjProcessOrders.Localization.Localizations
{
    public class Resources
    {
        private readonly IStringLocalizer<Resources> _localizer;

        public Resources(IStringLocalizer<Resources> localizer)
        {
            _localizer = localizer;
        }

        public string ProductAlreadyExists() => _localizer[nameof(ProductAlreadyExists)];
        public string ClientsNotExists() => _localizer[nameof(ClientsNotExists)];
        public string ClientNotExists() => _localizer[nameof(ClientNotExists)];
        public string ProductNotRegistered() => _localizer[nameof(ProductNotRegistered)];
        public string OrdersNotExists() => _localizer[nameof(OrdersNotExists)];





        //public string FieldNotRequired(string field) => string.Format(_localizer[nameof(FieldNotRequired)], field);
        public string InvalidCharacters() => _localizer[nameof(InvalidCharacters)];
        public string RequireField() => _localizer[nameof(RequireField)];
        public string InvalidField() => _localizer[nameof(InvalidField)];
        public string EmailAlreadyExists() => _localizer[nameof(EmailAlreadyExists)];
        public string InvalidPassword() => _localizer[nameof(InvalidPassword)];
        public string InvalidCredentials() => _localizer[nameof(InvalidCredentials)];
        public string InvalidCodeResetPassword() => _localizer[nameof(InvalidCodeResetPassword)];
        public string InvalidCode() => _localizer[nameof(InvalidCode)];
        public string InvalidRole() => _localizer[nameof(InvalidRole)];
        public string InvalidCPF() => _localizer[nameof(InvalidCPF)];
        public string InvalidCEP() => _localizer[nameof(InvalidCEP)];
        public string InvalidRoad() => _localizer[nameof(InvalidRoad)];
        public string InvalidComplement() => _localizer[nameof(InvalidComplement)];
        public string InvalidNeighborhood() => _localizer[nameof(InvalidNeighborhood)];
        public string InvalidPhone() => _localizer[nameof(InvalidPhone)];
        public string InvalidMobilePhone() => _localizer[nameof(InvalidMobilePhone)];
        public string NameAlreadyExists() => _localizer[nameof(NameAlreadyExists)];
        public string InvalidCity() => _localizer[nameof(InvalidCity)];
        public string InvalidState() => _localizer[nameof(InvalidState)];

        public string InvalidTypeEquipment() => _localizer[nameof(InvalidTypeEquipment)];
        public string InvalidBrand() => _localizer[nameof(InvalidBrand)];
        public string InvalidModel() => _localizer[nameof(InvalidModel)];
        public string InvalidProblems() => _localizer[nameof(InvalidProblems)];
        public string InvalidServiceToBeDone() => _localizer[nameof(InvalidServiceToBeDone)];
        public string EquipmentNotExists() => _localizer[nameof(EquipmentNotExists)];
        public string ImageNotFound() => _localizer[nameof(ImageNotFound)];
        public string RoleAlreadyExists() => _localizer[nameof(RoleAlreadyExists)];
    }
}
