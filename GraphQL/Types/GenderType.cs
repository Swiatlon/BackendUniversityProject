using Domain.Enums;

namespace GraphQL.Types
{
    public class GenderType : EnumType<Gender>
    {
        protected override void Configure(IEnumTypeDescriptor<Gender> descriptor)
        {
            descriptor.Name("Gender");
            descriptor.Value(Gender.Female).Name("FEMALE");
            descriptor.Value(Gender.Male).Name("MALE");
        }
    }
}
